using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models.JobCV
{
    public class Qualification
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string University { get; set; }

        public string Specialization { get; set; }

        public string GraduationYear { get; set; }

        public int Grade { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}