using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTutorial1.Controllers {

    // HTTP リクエスト・レスポンスについての解説 MDN
    //  https://developer.mozilla.org/ja/docs/Web/HTTP/Messages
    public class HelloWorldController : Controller {
        private int AccCnt { get; set; }

        // GET: /HelloWorld/
        /*
        public string Index() {
            // "This is my <b>default</b> action..."の文字列がResponseBodyとして返却される
            return "This is my <b>default</b> action...";
        }
        */
        public ActionResult Index() {
            return View();
        }
        // GET: /HelloWorld/ Welcome
        //public string Welcome(string name, int? numTimes) { // 文字列を応答する場合
        public ActionResult Welcome(string name, int? numTimes) { // ActionResultを応答する場合
            // URL1: https://localhost:44310/HelloWorld/Welcome
            // URL2: https://localhost:44310/HelloWorld/Welcome?name=SCOTT&numtimes=2
            // URL3: https://localhost:44310/HelloWorld/Welcome?name=SCO<>TT&numtimes=2

            // HttpUtility.HtmlEncode: HTMLエンコード機能 「<」 → 「&lt;」など
            // return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is: " + numTimes);

            // ViewBagを使用したデータ授受
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;
            if (numTimes == -1) {
                // 404 応答の送出
                return HttpNotFound();
            } else {
                // Viewの応答(200 OK)
                return View();
            }
        }
        public string Welcome2(string name, int ID = 1) {
            // URLパターン「{controller}/{action}/{id}」のidは優先してパラメータとなる
            // ※パラメータとURLだと、URLが優先
            // URL1: https://localhost:44310/HelloWorld/Welcome2/2?name=Scott

            // Helloルート有効
            // URL2: https://localhost:44310/HelloWorld/Welcome2/Scott/4

            return HttpUtility.HtmlEncode("Hello " + name + ", ID: " + ID);
        }
    }
}