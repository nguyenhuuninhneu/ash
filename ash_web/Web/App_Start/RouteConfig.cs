using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("search", "pages/search.html", new { controller = "Home", action = "Search" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("home", "home", new { controller = "Home", action = "Index", id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("danhmuc_chitiet", "pages/category/{id}/{title}.html", new { controller = "Category", action = "Index", id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("tintuc_chitiet", "pages/detail/{id}/{title}.html", new { controller = "News", action = "Detail",catid=0, id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("tintuc_chitiet1", "pages/detail/{catid}/{id}/{title}.html", new { controller = "News", action = "Detail", catid = UrlParameter.Optional, id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("topic_chitiet", "pages/topic/detail/{id}/{title}.html", new { controller = "Topic", action = "Detail", id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("eMagazine_chitiet", "pages/eMagazine/detail/{id}/{title}.html", new { controller = "eMagazine", action = "Detail", id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("anh_danhsach", "pages/images.html", new { controller = "Image", action = "CateImg"}, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("anh_chitiet", "pages/images/{id}/{title}.html", new { controller = "Image", action = "DetailImg", id = UrlParameter.Optional, zoneid = "" }, namespaces: new[] { "Web.Controllers" });


            routes.MapRoute("vanban_chuyenmuc", "pages/vanban.html", new { controller = "VanBanFE", action = "GetAllNewsByCat", catid = UrlParameter.Optional }, namespaces: new[] { "CMS.Controllers" });

            routes.MapRoute("vanban_chitiet", "pages/vanban/{id}/{title}.html", new { controller = "VanBanFE", action = "Details", id = UrlParameter.Optional }, namespaces: new[] { "CMS.Controllers" });

            routes.MapRoute("list-tthc", "pages/services/thu-tuc-hanh-chinh.html", new { controller = "TTHC", action = "GetALL" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("detail-tthc", "pages/service/{id}/{title}.html", new { controller = "TTHC", action = "Detail", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("list-tthc-donvi", "pages/services/{catid}/{title}.html", new { controller = "TTHC", action = "GetALL", catid = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" }); 

            routes.MapRoute("category-gallery", "pages/gallery/thu-vien-anh.html", new { controller = "Gallery", action = "Index"}, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("gallery", "pages/gallery/{catid}/{title}.html", new { controller = "Gallery", action = "Index", catid = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("sitemap", "pages/so-do-cong.html", new { controller = "SiteMap", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("hoi-dap", "pages/gui-cau-hoi.html", new { controller = "qa", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("hoi-dap-add", "pages/gui-cau-hoi/add.html", new { controller = "qa", action = "SendQuestion" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("dong-gop-y-kien", "pages/dong-gop-y-kien.html", new { controller = "NewsPaperFE", action = "ListDataFE" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("dong-gop", "pages/dong-gop-y-kien/{id}.html", new { controller = "NewsPaperFE", action = "Detail" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("lien-he", "pages/lien-he.html", new { controller = "contact", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("video", "pages/video.html", new { controller = "video", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("video_detail", "video/{id}/{title}.html", new { controller = "video", action = "Detail", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("video_category", "video/category/{catid}/{title}.html", new { controller = "video", action = "Detail", catid = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("tap-chi", "pages/tap-chi.html", new { controller = "magazine", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("eMagazine", "pages/eMagazine.html", new { controller = "eMagazine", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("truy-na", "pages/doi-tuong-truy-na.html", new { controller = "truyna", action = "Index" }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute("truy-na-chitiet", "pages/doi-tuong/{id}/{title}.html", new { controller = "truyna", action = "Details", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("to-giac", "pages/to-giac-toi-pham/{id}.html", new { controller = "ToGiac", action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web.Controllers" }
            );
        }
    }
}