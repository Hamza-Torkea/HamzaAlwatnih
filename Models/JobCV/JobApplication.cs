using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace alwatnia.Models.JobCV
{
    public class JobApplication
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; } 

        public string CountryOfBirth { get; set; }

        public string CityOfBirth { get; set; }

        public SocialStatus SocialStatus { get; set; }

        public string Nationality { get; set; }

        public int Gender { get; set; }

        public int? NumberOfDependents { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Cdate { get; set; }

        public string IdentityNo { get; set; }

        public DateTime? IdentitReleaseDate { get; set; }

        public string PlaceOfIssue { get; set; }

        public string Passport { get; set; }

        public DateTime? PassportReleaseDate { get; set; }

        public DateTime? PassportEndDate { get; set; }

        public string Mobile { get; set; }

        public string Tell { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        //other
        public bool HasCrime { get; set; }

        // relative in our company
        public bool HasRelativeInOurCompany { get; set; }

        public string CrimeDescription { get; set; }

        public bool HasDisable { get; set; }

        public string DisableDescription { get; set; }

        public string SalaryExpect { get; set; }

        public string WhenStartWorking { get; set; }

        public bool EmployeeNow { get; set; }

        public bool AskYourEmployer { get; set; }

        public string EmployerMobile { get; set; }

        /// <summary>
        ///     name of company & address & employer name
        /// </summary>
        public string EmployerDescription { get; set; }

        public string JobTitle { get; set; }

        public string JobDuration { get; set; }

        public string LeaveReason { get; set; }
        

        public bool Confirm { get; set; }

        public ICollection<Reference> References { get; set; }
        public ICollection<RelativeEmployer> RelativeEmployers { get; set; }
        public ICollection<Qualification> Qualifications { get; set; }
        public ICollection<LanguageSkill> LanguageSkills { get; set; }
        public ICollection<Employment> Employments { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<ComputerSkill> ComputerSkills { get; set; }

       
    }

    public enum SocialStatus
    {
        Single = 1,
        Married = 2,
        Widowed = 3,
        Divorcee = 4
    }

    public enum GenderType
    {
        Male = 1,
        Female = 2
    }
}