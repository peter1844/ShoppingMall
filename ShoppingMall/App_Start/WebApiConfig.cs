using Autofac;
using System.Web.Http;

namespace ShoppingMall
{
    public static class WebApiConfig
    {
        public static string UrlPrefix { get { return "api"; } }
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(HttpConfiguration config)
        {
            // Web API 路由
            config.MapHttpAttributeRoutes();

            // Web API 攔截
            IContainer container = WebApiApplication.Container;
            TokenValidationHandler tokenValidationHandler = container.Resolve<TokenValidationHandler>();
            config.MessageHandlers.Add(tokenValidationHandler);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
