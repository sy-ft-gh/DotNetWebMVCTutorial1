namespace WebTutorial1.Migrations {
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebTutorial1.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebTutorial1.Models.MovieDBContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(WebTutorial1.Models.MovieDBContext context) {
            // DB・テーブルを作成後にマイグレーションでSeedを作成する場合は
            // 一度テーブルを削除後に実施する。

            // AddOrUpdate 条件のもと追加または更新を行う
            // →条件： i.Titleが指定され、このTitleを元に存在するかどうかを判定する
            //          ※IDは自動採番の為
            // 注意：TitleがDBとしては重複して登録できる。
            //       重複登録するとエラーが発生しSeedデータをマイグレーション時に更新できない。
            context.Movies.AddOrUpdate(i => i.Title,
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
           );
        }
    }
}
