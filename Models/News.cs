using alwatnia.Areas.Cpanel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class News : IHasPaginator
    {
        public News()
        {
            Comments = new HashSet<Comment>();
        }

        public static News Create(NewsViewModel model)
        {
            return new News
            {
                Cdate = DateTime.Now,
                Title = model.Title,
                Email = model.Email,
                Summary = model.Summary,
                Details = model.Details,
                Active = model.Active,
                Urgent = model.Urgent,
                type = model.Type,
                ReadCounter = model.ReadCounter
            };
        }

        public void Edit(NewsViewModel model)
        {
            Title = model.Title;
            Email = model.Email;
            Summary = model.Summary;
            Details = model.Details;
            Active = model.Active;
            Lang = model.Lang ?? 1;
            Urgent = model.Urgent;
            Cdate = model.Cdate;
            type = model.Type;
            ReadCounter = model.ReadCounter;
        }

        public int Id { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public string Email { get; set; }

        [StringLength(4000)]
        public string Summary { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Details { get; set; }

        [StringLength(500)]
        public string Photo { get; set; }

        public bool? Active { get; set; }

        public int? Lang { get; set; }

        public bool? Urgent { get; set; }

        public DateTime? Cdate { get; set; }

        public NewsType? type { get; set; }

        public int? ReadCounter { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public enum NewsType
    {
        Social = 1
    }
}