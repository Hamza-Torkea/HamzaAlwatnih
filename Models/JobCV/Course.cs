using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models.JobCV
{
    public class Course
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Orginization { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Date { get; set; }

        public string Duration { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}