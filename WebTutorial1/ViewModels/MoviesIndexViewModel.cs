using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using WebTutorial1.Models;

namespace WebTutorial1.ViewModels {
    /// <summary>
    /// Index画面用のビューモデル
    /// </summary>
    public class MoviesIndexViewModel {
        /// <summary>
        /// 登録済みMovie情報
        /// </summary>
        private List<Movie> origin_movies;

        /// <summary>
        /// 一覧表示するMovie情報
        /// </summary>
        private IEnumerable<MovieEntry> movies;

        /// <summary>
        /// Moviesリストを返却（表示用）
        /// </summary>
        public IEnumerable<MovieEntry> Movies { get => movies; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="movies">画面に一覧表示するMovieリスト</param>
        public MoviesIndexViewModel(List<Movie> movies) {
            this.origin_movies = movies;
            this.movies = from m in this.origin_movies
                          select (new MovieEntry(m));
        }

    }
}