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
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0}は{2}文字以上、{1}文字以内で入力してください。")]
        public string Title { get; set; }

        /// <summary>
        /// リリース日のメンバ変数。表示用・入力用でプロパティを分ける
        /// </summary>
        private DateTime? releaseDate;
        // リリース日の表示画面用アクセッサ(年月日表示)
        [DisplayName("リリース日")]
        //DataTypeはビューがレンダリングする際の書式設定へのヒントとなる（チェック処理とは結びつかない）
        //サーバのカルチャ設定で表示書式が決まる
        [DataType(DataType.Date)]
        //この例では表示用の書式を年月日の形式にしている。(デフォルトのカルチャではYYYY/MM/DD形式のため)
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}", ApplyFormatInEditMode = false)] //表示はyyyy年MM月dd日にしたい
        public DateTime? DisplayReleaseDate { get => this.releaseDate; }

        // リリース日の登録画面用アクセッサ(yyyy/MM/dd表示)
        [DisplayName("リリース日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get => this.releaseDate; set => this.releaseDate = value; }


        [DisplayName("ジャンル")]
        // 正規表現チェックA～Zの1回以上の繰り返しとアルファベット、シングルクォート、スペースの繰り返し
        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        // 必須入力である事(非nullであること)
        [Required]
        [StringLength(30, ErrorMessage = "{0}は{1}文字以内で入力してください。")]
        public string Genre { get; set; }

        [DisplayName("価格")]
        // 数値範囲で１～１００までに抑止する
        [Range(1, 100)]
        // 正規表現で記述する場合は以下(１～2桁の数字)
        //[RegularExpression(@"[0-9]{1,2}", ErrorMessage = "{0}には数字を指定してください。 。")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)] //表示はカンマ区切りにしたい
        public int? Price { get; set; }

        [DisplayName("評価")]
        public string Rating { get; set; }

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
            this.releaseDate = movie.ReleaseDate;
            this.Genre = movie.Genre;
            if (movie.Price != null)
                this.Price = decimal.ToInt32((decimal)movie.Price);
            else
                this.Price = null;
            this.Rating = movie.Rating;
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
            mv.ReleaseDate = this.ReleaseDate;
            mv.Genre = this.Genre;
            mv.Price = this.Price;
            mv.Rating = this.Rating;

            return mv;
        }
    }
}