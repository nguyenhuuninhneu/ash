using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.App_Start
{
    public class SubDomainRoute : Route
    {
        private readonly string[] namespaces;
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        public SubDomainRoute(string url, object defaults, string[] namespaces)
            : base(url, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            this.namespaces = namespaces;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            //HelperCache.ClearAllCache();
            var lstPageElement = _pageElementRepository.GetAll();
            var url = httpContext.Request.Headers["HOST"];
            var index = url.IndexOf(".");

            if (index < 0)
                return null;
            var routeData = base.GetRouteData(httpContext);
            if (routeData != null)
            {
                var objPageElement = lstPageElement.FirstOrDefault(g => g.Subdomain == url);
                if (objPageElement != null && objPageElement.Area != null)
                {
                    namespaces.SetValue("Web.Areas." + objPageElement.Area + ".Controllers", 0);
                    if (this.namespaces != null && this.namespaces.Length > 0)
                    {
                        routeData.DataTokens["Namespaces"] = this.namespaces;
                    }
                    routeData.Values["area"] = objPageElement.Area;
                    routeData.DataTokens["Area"] = objPageElement.Area;
                }
                else
                {
                    return null;
                }

                routeData.DataTokens["UseNamespaceFallback"] = bool.FalseString;
                return routeData;
            }
            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}