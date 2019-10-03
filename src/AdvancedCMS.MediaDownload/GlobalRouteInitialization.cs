using System.Web.Mvc;
using System.Web.Routing;
using EPiServer;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace AdvancedCMS.MediaDownload
{
    [InitializableModule]
    public class GlobalRouteInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
        }

        public void Initialize(InitializationEngine context)
        {
            Global.RoutesRegistrating += Global_RoutesRegistrating;
        }

        public void Uninitialize(InitializationEngine context)
        {
            Global.RoutesRegistrating -= Global_RoutesRegistrating;
        }

        private void Global_RoutesRegistrating(object sender, RouteRegistrationEventArgs e)
        {
            var routeValues = new RouteValueDictionary();
            routeValues.Add("controller", "FolderDownload");
            routeValues.Add("action", "Index");
            routeValues.Add("contentFolderIds", UrlParameter.Optional);

            const string baseUrl = "cms-content-folder-download";

            var route = new Route(baseUrl + "/{contentFolderIds}", routeValues, new MvcRouteHandler());
            string[] allowedMethods = { "GET" };
            var methodConstraints = new HttpMethodConstraint(allowedMethods);
            route.Constraints = new RouteValueDictionary { { "httpMethod", methodConstraints } };

            e.Routes.Add(route);
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
