namespace alwatnia.Models.JobCV
{
    public class RelativeEmployer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string RelativeRelation { get; set; }

        public string JobTitle { get; set; }

        public string Unit { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}