using System.Reflection;
using System.Text;
using FlowWing.API.Controllers;
using FlowWing.API.Helpers;
using FlowWing.API.Middlewares;
using FlowWing.Business.Abstract;
using FlowWing.Business.Concrete;
using FlowWing.DataAccess;
using FlowWing.DataAccess.Abstract;
using FlowWing.DataAccess.Concrete;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FlowWing.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(c =>c.UseNpgsqlConnection(
                    "Server=localhost;Port=5432;Database=flowwing;User Id=postgres;Password=1234;\r\n")));
            
            services.AddHangfireServer();
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
            // Burada bağımlılıkları ekleyin
            //Add Cors and allow all the connections
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddScoped<DbContext,FlowWingDbContext>();
            services.AddScoped<ScheduledMailHelper>();
            services.AddScoped<EmailSenderService>();

            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<IEmailLogService, EmailLogManager>();

            services.AddScoped<IScheduledEmailRepository, ScheduledEmailRepository>();
            services.AddScoped<IScheduledEmailService, ScheduledEmailManager>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserManager>();
            
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IAttachmentService, AttachmentManager>();

            services.AddScoped<ILoggingRepository, LoggingRepository>();
            services.AddScoped<ILoggingService, LoggingManager>();


            services.AddDbContext<FlowWingDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            
            // JWT Authentication ekle
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:SecretKey").Value);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Swagger belgesi ekle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FlowWing API", 
                    Version = "v1",
                    Description = "FlowWing API",
                    Contact = new OpenApiContact
                    {
                        Name = "Kerem Mert Izmir",
                        Email = "keremmertizmir39@gmail.com"
                        
                    },
                });
                c.IncludeXmlComments(xmlPath);

                // JWT yetkilendirme ekle
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 
            //app.UseOpenApi();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "FlowWing Hangfire Dashboard"
            });
            
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlowWing API v1");
                
            });
            app.UseMiddleware<LoggingMiddleware>();
            app.UseAuthorization();
            app.UseWhen(context =>
                context.Request.Path.StartsWithSegments("/api/EmailLogs") ||
                context.Request.Path.StartsWithSegments("/api/ScheduledEmails"),
                builder =>
                {
                    builder.UseMiddleware<AuthorizationMiddleware>();
                }
            );

            
            app.UseMiddleware<EmailOwnershipMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
