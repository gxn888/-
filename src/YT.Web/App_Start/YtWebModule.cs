using System.Reflection;
using System.Web;
using System.Web.Routing;
using Abp;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.IO;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Security;
using YT.Web.Routing;
using YT.WebApi;

namespace YT.Web
{
    /// <summary>
    /// Web module of the application.
    /// This is the most top and entrance module that depends on others.
    /// </summary>
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(AbpZeroOwinModule),
        typeof(YtDataModule),
        typeof(YtApplicationModule),
        typeof(YtWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency can be removed if not using Redis cache
        typeof(AbpHangfireModule))] //AbpHangfireModule dependency can be removed if not using Hangfire
    public class AbpZeroTemplateWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Use database for language management
         //   Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.Modules.AbpWeb().AntiForgery.IsEnabled = false;
            //Configure navigation/menu
            //  Configuration.Navigation.Providers.Add<AppNavigationProvider>();//SPA!
            //  Configuration.Navigation.Providers.Add<FrontEndNavigationProvider>();
            //   Configuration.Navigation.Providers.Add<MpaNavigationProvider>();//MPA!

            //  Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = WebUrlService.WebSiteRootAddress;

            //Uncomment these lines to use HangFire as background job manager.
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //Configuration.Caching.UseRedis();
        }

        public override void Initialize()
        {
            //Dependency Injection
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient()
                );

            //Areas
          //  AreaRegistration.RegisterAllAreas();

            //Routes
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Bundling
         //   BundleTable.Bundles.IgnoreList.Clear();
          //  CommonBundleConfig.RegisterBundles(BundleTable.Bundles);
         //   AppBundleConfig.RegisterBundles(BundleTable.Bundles);//SPA!
         //   FrontEndBundleConfig.RegisterBundles(BundleTable.Bundles);
          //  MpaBundleConfig.RegisterBundles(BundleTable.Bundles);//MPA!
        }

        //public override void PostInitialize()
        //{
        //    var server = HttpContext.Current.Server;
        //    var appFolders = IocManager.Resolve<AppFolders>();

        //    appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
        //    appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");
        //    appFolders.WebLogsFolder = server.MapPath("~/App_Data/Logs");
            
        //    try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
        //}
    }
}
