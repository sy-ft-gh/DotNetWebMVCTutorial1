using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using System.Reflection;

using WebTutorial1.Controllers;
using WebTutorial1.Models;
using WebTutorial1.ViewModels;

namespace UnitTestProject1 {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestIndex() {
            // 参考：https://docs.microsoft.com/ja-jp/ef/ef6/fundamentals/testing/mocking

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
            var movieGenre = viewResult.ViewBag.movieGenre as SelectList;

            // ⑥movieGenreに重複が除去されたジャンルが含まれるか
            var dummyGenre = dummyData.ConvertAll(x => x.Genre).Distinct().ToList();
            dummyGenre.Sort((x, y) => x.CompareTo(y));
            var itemsGenre = (List<string>)movieGenre.Items;
            itemsGenre.Sort((x, y) => x.CompareTo(y));
            // ジャンル一覧の中身がすべて同じことで検証
            Assert.AreEqual(dummyGenre.Count(), itemsGenre.Count());
            Assert.IsTrue(dummyGenre.SequenceEqual(itemsGenre));
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
            // ※Queryableを正常に動作させるためSQLServerからではなく、ListからのLinqとしてDbSetを構築する
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
