using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebTutorial1 {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            // リソースデータに直接アクセスされない為の記述
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
#if NO_ROUTE
            // ルートを使わない場合、この一行を追加
            // 意味：Attributeによるルートマップを使用する
            routes.MapMvcAttributeRoutes();
#else
            // MapRouteは宣言順が適用マッチ順
            // 具体的なものから記述するなど記述順の注意が必要
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 name: "Hello",
                 url: "{controller}/{action}/{name}/{id}"   
             );
#endif
        }
    }
}