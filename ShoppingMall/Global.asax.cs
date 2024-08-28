﻿
using Autofac;
using ShoppingMall.App_Code;
using ShoppingMall.Helper;
using ShoppingMall.Interface;
using ShoppingMall.Runtime;
using ShoppingMall.Views;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using ShoppingMall.Api.Admin;
using Autofac.Integration.WebApi;
using ShoppingMall.Api.Commodity;
using ShoppingMall.Api.Login;
using ShoppingMall.Api.Logout;
using ShoppingMall.Api.Member;
using ShoppingMall.Api.Menu;
using ShoppingMall.Api.Order;
using ShoppingMall.Api.Token;
using Autofac.Integration.Web;

namespace ShoppingMall
{
    public class WebApiApplication : HttpApplication, IContainerProviderAccessor
    {
        private IVersionListner _versionListner;
        private IDeleteOrder _deleteOrder;
        private TokenValidationHandler _tokenValidationHandler;

        private static IContainerProvider containerProvider;
        public IContainerProvider ContainerProvider { get { return containerProvider; } }

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 透過autofac注入services
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.RegisterWebApiModelBinderProvider();

            // 註冊類型
            #region
            builder.RegisterType<AdminProccess>().As<IAdmin>().InstancePerDependency();
            builder.RegisterType<CommodityProccess>().As<ICommodity>().InstancePerDependency();
            builder.RegisterType<LoginProccess>().As<ILogin>().InstancePerDependency();
            builder.RegisterType<Logout>().As<ILogout>().InstancePerDependency();
            builder.RegisterType<MemberProccess>().As<IMember>().InstancePerDependency();
            builder.RegisterType<MenuProccess>().As<IMenu>().InstancePerDependency();
            builder.RegisterType<OrderProccess>().As<IOrder>().InstancePerDependency();
            builder.RegisterType<TokenProccess>().As<IToken>().InstancePerDependency();

            builder.RegisterType<VersionListner>().As<IVersionListner>().SingleInstance();
            builder.RegisterType<DeleteOrder>().As<IDeleteOrder>().SingleInstance();

            builder.RegisterType<ContextHelper>().As<IContextHelper>().InstancePerDependency();
            builder.RegisterType<ConfigurationsHelper>().As<IConfigurationsHelper>().InstancePerDependency().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<LogHelper>().As<ILogHelper>().SingleInstance();
            builder.RegisterType<DbHelper>().As<IDbHelper>().SingleInstance();

            builder.RegisterType<TokenValidationHandler>().SingleInstance().InstancePerDependency().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<Tools>().As<ITools>().SingleInstance();
            #endregion

            IContainer container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            containerProvider = new ContainerProvider(container);

            // 初始化背景執行class
            InitializeRuntimeClasses(container);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            // 釋放版本監聽
            _versionListner.Dispose();
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest()) HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        private void InitializeRuntimeClasses(IContainer container)
        {
            _versionListner = container.Resolve<IVersionListner>();
            _deleteOrder = container.Resolve<IDeleteOrder>();
            _tokenValidationHandler = container.Resolve<TokenValidationHandler>();

            // 監聽檔案變化
            _versionListner.Initialize();

            // 定期執行刪除訂單
            _deleteOrder.DeleteOrderTimer();

            // Web API攔截
            GlobalConfiguration.Configuration.MessageHandlers.Add(_tokenValidationHandler);
        }
    }
}
