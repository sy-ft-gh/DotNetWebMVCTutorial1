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
            // DB�E�e�[�u�����쐬��Ƀ}�C�O���[�V������Seed���쐬����ꍇ��
            // ��x�e�[�u�����폜��Ɏ��{����B

            // AddOrUpdate �����̂��ƒǉ��܂��͍X�V���s��
            // �������F i.Title���w�肳��A����Title�����ɑ��݂��邩�ǂ����𔻒肷��
            //          ��ID�͎����̔Ԃ̈�
            // ���ӁFTitle��DB�Ƃ��Ă͏d�����ēo�^�ł���B
            //       �d���o�^����ƃG���[��������Seed�f�[�^���}�C�O���[�V�������ɍX�V�ł��Ȃ��B
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
