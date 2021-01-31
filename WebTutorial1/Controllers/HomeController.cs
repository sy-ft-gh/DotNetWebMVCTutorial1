using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTutorial1.Controllers {


#if NO_ROUTE
    [RoutePrefix("Home")]　// ← 【1】
#endif
    public class HomeController : Controller {
#if NO_ROUTE
        [Route("~/")]　// 「/」で受け取れる
        [Route("Index")]　 // Indexとマッピング
#endif
        public ActionResult Index() {
            return View();
        }

#if NO_ROUTE
        /// <summary>
        /// Aboutとマッピング
        /// id という引数を受け取る場合、Route属性に{id}を追加
        　　　　 /// idがNull許容の場合、文字列でも{id?}のように「?」が必要
        /// </summary>
        [Route("About")]
        [Route("About/{id}")]
#endif
        public ActionResult About(string id) {
            ViewBag.Message = "Your application description page.";

            return View();
        }

#if NO_ROUTE
        /// <summary>
        /// Contactとマッピング
        /// id という引数を受け取る場合、Route属性に{id}を追加
        　　　　 /// idに「:int」とつけることで、int型しか受け付けないように制約がかけられる
        /// </summary>
        [Route("Contact")]
        [Route("Contact/{id:int}")] 
#endif
        public ActionResult Contact(int? id) {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}