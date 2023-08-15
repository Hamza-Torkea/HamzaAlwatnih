using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models
{
    public class Image
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string photo { get; set; }

        public int? companyId { get; set; }

        public int? PageId { get; set; }

        public int? AlbumId { get; set; }

        public virtual Album Album { get; set; }

        public virtual Company Company { get; set; }

        public virtual Page Page { get; set; }

    }
}