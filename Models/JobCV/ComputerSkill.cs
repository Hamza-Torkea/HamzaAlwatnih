using System.ComponentModel.DataAnnotations;

namespace alwatnia.Models.JobCV
{
    public class ComputerSkill
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public SkillStatus Skill { get; set; }

        public int JobApplicationId { get; set; }

        public virtual JobApplication JobApplication { get; set; }
    }
}