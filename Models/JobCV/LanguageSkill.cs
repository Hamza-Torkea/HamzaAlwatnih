using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models.JobCV
{
    public class LanguageSkill
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public SkillStatus Conversation { get; set; }

        public SkillStatus Writing { get; set; }

        public SkillStatus Reading { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }

    public enum SkillStatus
    {
        Excellent = 1,
        Good = 2,
        Medium = 3,
        Beginner = 4
    }
}