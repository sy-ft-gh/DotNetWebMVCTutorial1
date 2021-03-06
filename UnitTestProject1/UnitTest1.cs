using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using System.Reflection;
using System.Threading.Tasks;

using WebTutorial1.Controllers;
using WebTutorial1.Models;
using WebTutorial1.ViewModels;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
        // 参考：https://docs.microsoft.com/ja-jp/ef/ef6/fundamentals/testing/mocking

        /// <summary>
        /// ジャンル一覧取得メソッドのテスト
        /// </summary>
        [TestMethod, TestCategory("Index")]
        public void TestgetGenreList() {
            // ダミー値で生成したDbContextを元にコントローラを生成
            var dummyData = createDummyMovieData();
            var mv = new MoviesController(createDummyContext(dummyData.AsQueryable()));
            // コントローラのPrivateObjectを生成する
            var po = new PrivateObject(mv);
            var movieGenre = (List<string>)po.Invoke("getGenreList");

            // ①movieGenreに重複が除去されたジャンルが含まれるか
            var dummyGenre = dummyData.ConvertAll(x => x.Genre).Distinct().ToList();
            dummyGenre.Sort((x, y) => x.CompareTo(y));
            movieGenre.Sort((x, y) => x.CompareTo(y));
            // ジャンル一覧の中身がすべて同じことで検証
            Assert.AreEqual(dummyGenre.Count(), movieGenre.Count());
            Assert.IsTrue(dummyGenre.SequenceEqual(movieGenre));
        }

        [TestMethod, TestCategory("Index")]
        public void TestIndex() {

            // ダミー値で生成したDbContextを元にコントローラを生成
            var dummyData = createDummyMovieData();
            var mv = new MoviesController(createDummyContext(dummyData.AsQueryable()));
            // Indexメソッドを実行

            // 引数＝ブランク
            var result = mv.Index(string.Empty, string.Empty);

            // ①ViewResult型の応答か
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;

            // ②ViewModelがモデルデータに設定されているか
            Assert.IsInstanceOfType(viewResult.Model, typeof(MoviesIndexViewModel));
            var model = viewResult.Model as MoviesIndexViewModel;

            // ③全件検索結果が含まれるか(絞り込み無しの為)格納件数を確認
            // 内部管理用
            // Reflectionを利用し、Privateのインスタンスフィールドから「origin_movies」を取り出す
            FieldInfo field = (model.GetType()).GetField("origin_movies", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Movie> origin_movies = (List<Movie>)(field.GetValue(model));
            Assert.AreEqual(origin_movies.Count(), dummyData.Count());
            // 表示用
            Assert.AreEqual(model.Movies.Count(), dummyData.Count());
            
            // ④ジャンル一覧フィールドが存在するか
            Assert.IsNotNull(viewResult.ViewBag.movieGenre);

            // ⑤ジャンル一覧の型をチェック（SelectListになっているか）
            Assert.IsInstanceOfType(viewResult.ViewBag.movieGenre, typeof(SelectList));

        }
        /// <summary>
        /// 同時実行テスト
        /// Indexを同時に実行した場合
        /// ※スレッドセーフか。他のセッションと影響がないか等確認する場合の手法
        /// </summary>
        [TestMethod]
        public void TestIndexMulti() {
            // ダミー値で生成したDbContextを元にコントローラを生成
            Func<MoviesController> createDummy = () => {
                var dummyData = createDummyMovieData();
                var mv = new MoviesController(createDummyContext(dummyData.AsQueryable()));
                return mv;
            };
            // 現在時の分から次の分までの時間を演算
            Func<int> timeSpan = () => {
                var interval = TimeSpan.FromMinutes(1);
                // 現在時の分から次の分までの時間を演算
                var dt1 = DateTime.Now;
                var dt2 = new DateTime((((dt1.Ticks + interval.Ticks) / interval.Ticks) - 1) * interval.Ticks, dt1.Kind);
                var ts = dt1 - dt2;
                // 時間間隔(ミリ秒)数を返却
                return ts.Milliseconds;
            };
            // MoviesControllerを生成しIndexメソッドの実行結果を返却するメソッドを作成
            // （検索結果は1件ずつ求まるものを設定)
            const string timeformat = "yyyy/MM/dd HH:mm:ss:fffffff"; // 複数回使用する固定文字は const宣言が望ましい
            const string title1 = "When Harry Met Sally";  //1スレッド目のMovieタイトル
            var exec1 = Task<(MoviesController, ActionResult)>.Run(async () => {
                // コントローラ生成
                var mv = createDummy();
                // Delayで待機(以降非同期処理)
                await Task.Delay(timeSpan());
                Console.WriteLine("exec1:" + DateTime.Now.ToString(timeformat));
                // Indexメソッドを実行
                return (mv, mv.Index("Romantic Comedy", title1));
            });
            const string title2 = "Rio Bravo";  //2スレッド目のMovieタイトル
            var exec2 = Task<(MoviesController, ActionResult)>.Run(async () => {
                // コントローラ生成
                var mv = createDummy();
                // Delayで待機(以降非同期処理)
                await Task.Delay(timeSpan());
                Console.WriteLine("exec2:" + DateTime.Now.ToString(timeformat));
                // Indexメソッドを実行
                return (mv, mv.Index("Western", title2));
            });

            // 終了まで1分待機
            Task.Run(async () => {
                await Task.Delay(1000);
                // 全てのタスクが終わるまで待機
                await Task.WhenAll(exec1, exec2);
            }).Wait(); // Task内部は非同期処理だが、Wait()で待つことで同期処理にする。
            // なぜ非同期→同期と複雑な処理を行う？
            // （Task.Delayが使用したいため)
            //    (待機で代表的なThread.Sleep()はスレッドの動きをとめてしまう!）

            // 1・2の結果を取得
            var (mv1, ar1) = exec1.Result;
            var model1 = (ar1 as ViewResult).Model as MoviesIndexViewModel;
            var (mv2, ar2) = exec2.Result;
            var model2 = (ar2 as ViewResult).Model as MoviesIndexViewModel;

            Assert.AreEqual(model1.Movies.Count(), 1);
            Assert.AreEqual(model1.Movies.ElementAt(0).Title, title1);

            Assert.AreEqual(model2.Movies.Count(), 1);
            Assert.AreEqual(model2.Movies.ElementAt(0).Title, title2);
        }
        /// <summary>
        /// Movieデータ（ダミー）の作成
        /// </summary>
        /// <returns>ダミー生成したMovieデータ</returns>
        private List<Movie> createDummyMovieData() {
            var dummyData = new List<Movie> {
                new Movie {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-1-11"),
                    Genre = "Romantic Comedy",
                    Rating = "PG",
                    Price = 7.99M
                },
                new Movie {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Rating = "PG",
                    Price = 8.99M
                },

                new Movie {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Rating = "PG",
                    Price = 9.99M
                },
                new Movie {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Rating = "PG",
                    Price = 3.99M
                }
            };
            return dummyData;
        }
        private MovieDBContext createDummyContext(IQueryable<Movie> dummyData) {
            // Mock用データ作成AsQueryable()でQueryableなオブジェクトとして生成

            // DbSetのMock
            var mockDbSet = new Mock<DbSet<Movie>>();
            // DbSetとテスト用データを紐付け
            // ※Queryableを正常に動作させるため、IQueryableからの継承部分を
            // DBSetの設定ではなく、Queryableの設定で構築する
            // →DBからのクエリではなく、dummyDataからのクエリにすげかわる
            mockDbSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(dummyData.Provider);
            mockDbSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(dummyData.Expression);
            mockDbSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(dummyData.ElementType);
            mockDbSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(dummyData.GetEnumerator());
            // DbContextのMockオブジェクトを作成
            var mockContext = new Mock<MovieDBContext>();
            // DbContextのMoviesオブジェクトを生成したListベースのオブジェクトで置き換える
            mockContext.Setup(c => c.Movies).Returns(mockDbSet.Object);
            // 作成したオブジェクトを返却
            return mockContext.Object;
        }
    }
}
