using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alwatnia.Models.JobCV
{
    public class Employment
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string JobTitle { get; set; }

        public string CompanyAddress { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ToDate { get; set; }

        public string Duration { get; set; }

        public string Salary { get; set; }

        public string Description { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}