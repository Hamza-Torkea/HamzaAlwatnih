
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models
{
    [Table("Comment")]
    public class Comment
    {
        public int Id { get; set; }

        [Column(TypeName = "ntext")]
        public string comment_text { get; set; }

        [StringLength(500)]
        public string comment_auther { get; set; }

        public int? comment_parent { get; set; }

        public int? Status { get; set; }

        public DateTime? cdate { get; set; }

        public string comment_media { get; set; }

        public int? product_id { get; set; }

        public int? news_id { get; set; }

        public int? act_id { get; set; }

        public int? job_id { get; set; }

        public string mobile { get; set; }

        public string Email { get; set; }

        public string File { get; set; }

        public virtual Product Product { get; set; }

        public virtual Job Job { get; set; }

        public virtual News News { get; set; }

        public virtual Activity Activity { get; set; }
    }
}
