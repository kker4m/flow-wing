using Autofac;
using FlowWing.Business.Abstract;
using FlowWing.Business.Concrete;
using FlowWing.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowWing.Business
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            // Sınıfların kaydı
            builder.RegisterType<EmailLog>();
            builder.RegisterType<User>();
            builder.RegisterType<ScheduledEmail>();

            // Servislerin bağlanması
            builder.RegisterType<EmailLogManager>().As<IEmailLogService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<ScheduledEmailManager>().As<IScheduledEmailService>();
            builder.RegisterType<RepeatingMailManager>().As<IRepeatingMailService>();


            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var emailLogger = scope.Resolve<IEmailLogService>();
                var userService = scope.Resolve<IUserService>();
                var scheduledEmailService = scope.Resolve<IScheduledEmailService>();
                var repeatingMailService = scope.Resolve<IRepeatingMailService>();
            }
        }
    }
}
