using alwatnia.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alwatnia.Areas.Cpanel.ViewModels
{
    public class NewsViewModel
    {
        public NewsViewModel()
        {
            Companies = new List<int>();
        }

        public static NewsViewModel MapFromNews(News news, List<NewsCompany> newsCompanies)
        {
            return new NewsViewModel
            {
                Id = news.Id,
                Title = news.Title,
                Email = news.Email,
                Summary = news.Summary,
                Details = news.Details,
                Active = news.Active,
                Lang = news.Lang,
                Urgent = news.Urgent,
                Cdate = news.Cdate,
                Type = news.type,
                ReadCounter = news.ReadCounter,
                Photo = news.Photo,
                Companies = newsCompanies.Select(s => s.CompanyId).ToList()
            };
        }

        public int? Id { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public string Email { get; set; }

        [StringLength(4000)]
        public string Summary { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Details { get; set; }

        public string Photo { get; set; }

        public HttpPostedFileBase File { get; set; }

        public bool? Active { get; set; }

        public int? Lang { get; set; }

        public bool? Urgent { get; set; }

        public DateTime? Cdate { get; set; }

        public NewsType? Type { get; set; }

        public int? ReadCounter { get; set; }

        public List<int> Companies { get; set; }
    }
}