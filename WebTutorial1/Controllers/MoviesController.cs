﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTutorial1.Models;
using WebTutorial1.ViewModels;

namespace WebTutorial1.Controllers
{
    public class MoviesController : Controller
    {
        private MovieDBContext db = new MovieDBContext();

        // GET: Movies
        public ActionResult Index() {
            // List<Movie>を返却する。
            // return View(db.Movies.ToList());
            var model = new MoviesIndexViewModel(db.Movies.ToList());
            return View(model);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id) {
            /*
            if (id == null) {
                // 引数無しは400(BadRequestを送出)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // 対象IDでデータを１件選択
            Movie movie = db.Movies.Find(id);
            if (movie == null) {
                return HttpNotFound();
            }
            */
            // ヒットしなかった場合も含め該当がない場合はBadRequestを送出
            Movie movie;
            if (id == null || (movie = db.Movies.Find(id)) == null) {
                // ID=NULL、 dataのヒットなし→BadRequestを送出
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(new MovieEntry(movie));
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        // [HttpPost] = Postメソッドに限定
        // Postにする→HTTPS通信であれば送信データが暗号化される
        // ※GETのURLは暗号化されない
        [HttpPost]
        // [ValidateAntiForgeryToken] =  XSRF/CSRF 対策。パラメータにCSRFトークンが含まれる事をアノテーション
        // する事でトークンチェックが行われる
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate,Genre,Price")] MovieEntry movie) {
            // バインディング状態がOKなら登録
            if (ModelState.IsValid) {
                // リストに追加して変更を保存→INSERT＋COMMIT
                db.Movies.Add(movie.GetMovie());
                db.SaveChanges();
                // 登録成功→一覧画面へリダイレクト（アクションを指定）
                return RedirectToAction("Index");
            }
            // NGなら入力データで表示
            return View(movie);
        }

        // GET: Movies/Edit/5
        // 基底でGETメソッドに対応しているため省略している
        // [HttpGet]
        public ActionResult Edit(int? id) {
            /*
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null) {
                return HttpNotFound();
            }
            */
            // Movie情報を検索。取得できない場合はBadRequestとする
            Movie movie;
            if (id == null || (movie = db.Movies.Find(id)) == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new MovieEntry(movie));
        }

        // POST: Movies/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 をご覧ください。
        [HttpPost]
        // [ValidateAntiForgeryToken] =  XSRF/CSRF 対策。パラメータにCSRFトークンが含まれる事をアノテーション
        // する事でトークンチェックが行われる
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Price")] MovieEntry movie)　{
            if (ModelState.IsValid)　{
                Movie entry = movie.GetMovie();
                // 対象情報を変更状態にする
                db.Entry(entry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="id">対象のID</param>
        /// <returns>結果</returns>
        public ActionResult Delete(int? id) {
            /*
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null) {
                return HttpNotFound();
            }
            */
            // 対象IDで検索し、データが無い場合はBadRequestとする
            Movie movie;
            if (id == null || (movie = db.Movies.Find(id)) == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new MovieEntry(movie));
        }

        // POST: Movies/Delete/5
        // ActionNameアノテーションでメソッド名とは別にアクション名を設定
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            // 対象データが存在していた場合削除
            // 存在していな場合（入れ違い等）は削除済みとして
            // 正常終了とする。
            Movie movie = db.Movies.Find(id);
            if (movie != null) {
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
