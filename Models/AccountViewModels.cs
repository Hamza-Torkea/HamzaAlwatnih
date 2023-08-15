using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    [Table("Activity")]
    public class Activity : IHasPaginator
    {
        public Activity()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Details { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        public DateTime? Cdate { get; set; }

        public int? Lang { get; set; }

        public string Photo { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}