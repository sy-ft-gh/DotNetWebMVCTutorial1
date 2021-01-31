using System;

//DisplayName属性を使用する場合にインポートする  
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using WebTutorial1.Models;

namespace WebTutorial1.ViewModels {
    /// <summary>
    /// Movie情報入力・表示用クラス
    /// </summary>
    public class MovieEntry {
        [DisplayName("ID")]
        public string ID { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>

        // DisplayName:Viewでの表示名として扱われる
        [DisplayName("タイトル")]
        // 必須入力のチェックを付加。ErrorMessageはエラー時のメッセージ {0} でDisplayNameが入る
        [Required(ErrorMessage = "{0}は必須です。")]
        // 文字列蝶チェック 第一引数は文字数 [1]に文字数が入る
        [StringLength(30, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Title { get; set; }

        [DisplayName("リリース日")]
        //
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}", ApplyFormatInEditMode = false)] //表示はyyyy年MM月dd日にしたい
        public string ReleaseDate { get; set; }

        [DisplayName("ジャンル")]
        [StringLength(30, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Genre { get; set; }

        [DisplayName("価格")]
        // 正規表現で半角数字のみに抑止する
        [RegularExpression(@"[0-9]+", ErrorMessage = "フィールド{0}には数字を指定してください。 。")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)] //表示はカンマ区切りにしたい
        public int? Price { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MovieEntry() { }
        /// <summary>
        /// コンストラクタ（Movie情報を元に）
        /// </summary>
        /// <param name="movie">元とするMovie情報</param>
        public MovieEntry(Movie movie) {
            this.ID = movie.ID.ToString();
            this.Title =  movie.Title;
            this.ReleaseDate = movie.ReleaseDate?.ToLongDateString();
            this.Genre = movie.Genre;
            if (movie.Price != null)
                this.Price = decimal.ToInt32((decimal)movie.Price);
            else
                this.Price = null;
        }
        /// <summary>
        /// MpvieEntryとMovieの変換
        /// </summary>
        /// <returns>変換後のMovie</returns>
        public Movie GetMovie() {
            Movie mv = new Movie();
            int testInt = 0;
            mv.ID = int.TryParse(this.ID, out testInt) ? testInt : 0;
            mv.Title = this.Title;
            DateTime testDate;
            if (DateTime.TryParse(this.ReleaseDate, out testDate))
                mv.ReleaseDate = testDate;
            else
                mv.ReleaseDate = null;
            mv.Genre = this.Genre;
            mv.Price = this.Price;
            return mv;
        }
    }
}