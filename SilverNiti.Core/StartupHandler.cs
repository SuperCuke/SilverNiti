using System;
using Umbraco.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ozzy.Server.Configuration;
using Umbraco.Web.Mvc;
using Ozzy.Umbraco;
using Ozzy.Server;
using System.Web;
using Umbraco.Web;
using System.Web.Http;
using SilverNiti.Core.Saga;
using Ozzy;
using SilverNiti.Core.LightInject;
using SilverNiti.Core.LightInject.Mvc;
using SilverNiti.Core.LightInject.WebApi;
using SilverNiti.Core.LightInject.Web;
using SilverNiti.Core.LightInject.Microsoft.DependencyInjection;
using SilverNiti.Core.Controllers;

namespace SilverNiti.Core
{
    public class StartupHandler : IApplicationEventHandler
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static OzzyNode OzzyNode { get; set; }
        public static IServiceProvider ServiceProvider { get; set; }
        internal static ServiceContainer Container { get; set; }

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            //no op
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(SilverNitiPageController));
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(HttpRuntime.AppDomainAppPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);

            if (environmentName == "Development")
            {
                builder.AddJsonFile($"appsettings.{System.Environment.MachineName}.json", optional: true);
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton<ISerializer, ContractlessMessagePackSerializer>();
            services
            .AddOzzyDomain<SilverNitiDb>(options =>
            {
                options.UseInMemoryFastChannel();
                options.AddSagaProcessor<ContactFormMessageSaga>();
            })
            .UseEntityFramework((options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SilverNitiDb"));
            }));

            services.ConfigureOzzyNode<SilverNitiDb>()
                .UseEFBackgroundTaskService<SilverNitiDb>();

            var container = new ServiceContainer();
            container.RegisterControllers();
            container.RegisterApiControllers();
            container.RegisterControllers(typeof(UmbracoApplication).Assembly);
            container.RegisterApiControllers(typeof(UmbracoApplication).Assembly);
            container.EnablePerWebRequestScope();
            container.EnableMvc();
            container.EnableWebApi(GlobalConfiguration.Configuration);
            ServiceProvider = container.CreateServiceProvider(services);
            OzzyNode = ServiceProvider.GetService<OzzyNode>();
            var starter = new OzzyStarter(OzzyNode);
            container.BeginScope();
            starter.Start();
        }
    }
}
