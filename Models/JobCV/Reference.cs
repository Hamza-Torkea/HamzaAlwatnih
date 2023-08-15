namespace alwatnia.Models.JobCV
{
    public class Reference
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string JobTitle { get; set; }

        public string Mobile { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}