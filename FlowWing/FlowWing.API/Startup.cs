using FlowWing.Business.Abstract;
using FlowWing.Business.Concrete;
using FlowWing.DataAccess;
using FlowWing.DataAccess.Abstract;
using FlowWing.DataAccess.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            // Burada bağımlılıkları ekleyin
        

            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IEmailLogRepository, EmailLogRepository>();
            services.AddScoped<IScheduledEmailRepository, ScheduledEmailRepository>();
            services.AddScoped<IRepeatingMailRepository, RepeatingMailRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IScheduledEmailService, ScheduledEmailManager>();
            services.AddScoped<IRepeatingMailService, RepeatingMailManager>();
            services.AddScoped<IEmailLogService, EmailLogManager>();
            //services.AddDbContext<FlowWingDbContext>(options =>options.UseNpgsql("Server=localhost;Port=5432;Database=flowwing;User Id=postgres;Password=1234;"));
            services.AddDbContext<FlowWingDbContext>(options =>options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = (doc =>
                {
                    doc.Info.Title = "FlowWing API Documentation";
                    doc.Info.Version = "1.0.0";
                    doc.Info.Contact = new NSwag.OpenApiContact()
                    {
                        Name = "Kerem Mert İzmir",
                        Email = "keremmertizmir39@gmail.com"
                    };
                });
            });
            //MVC kullanılacaksa aşağıdaki satır da eklenebilir:
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseAuthorization();
            app.UseRouting();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }
            );
        
        }
    }
}
