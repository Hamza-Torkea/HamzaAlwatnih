using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace alwatnia.Models
{
    public class Job
    {
        public Job()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        public DateTime? EndDate { get; set; }


        public DateTime? FinishDate { get; set; }

        public string NewDate { get; set; }

        public bool? Active { get; set; }

        public int? Lang { get; set; }


        [StringLength(50)]
        public string City { get; set; }

        public bool? IsOpen { get; set; }

        public string Conditions { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}