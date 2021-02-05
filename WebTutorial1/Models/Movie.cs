using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace WebTutorial1.Models {
    public class Movie {

        public int ID { get; set; }

        // 最大長の60文字がDB上の桁数になる（nvarcharで作成されるため）
        [StringLength(60)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        //Requiredアノテーション：not null制約をつける
        [Required]
        //最大文字数を指定
        [StringLength(30)]
        public string Genre { get; set; }

        public decimal? Price { get; set; }

        [StringLength(5)]
        public string Rating { get; set; }
    }
    // 当ファイルにDBContextが作成されているが、複数のPOCOを用いる場合
    // DBContextのみ独立したファイルに記載するのが適切
    public class MovieDBContext : DbContext {

        // 基底クラスのコンストラクタにに接続文字列を与える事で、Class名以外のNameを指定する事が可能
        // public MovieDBContext() : base("MovieDBContext") { }
        public DbSet<Movie> Movies { get; set; }
    }
}