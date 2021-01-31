using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace WebTutorial1.Models {
    public class Movie {

        public int ID { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Genre { get; set; }

        public decimal? Price { get; set; }
    }
    // 当ファイルにDBContextが作成されているが、複数のPOCOを用いる場合
    // DBContextのみ独立したファイルに記載するのが適切
    public class MovieDBContext : DbContext {

        // 基底クラスのコンストラクタにに接続文字列を与える事で、Class名以外のNameを指定する事が可能
        // public MovieDBContext() : base("MovieDBContext") { }
        public DbSet<Movie> Movies { get; set; }
    }
}