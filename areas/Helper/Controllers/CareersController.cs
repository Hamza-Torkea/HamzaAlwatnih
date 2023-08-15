using alwatnia.Models;
using alwatnia.Models.JobCV;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Resources;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace alwatnia.Areas.Helper.Controllers
{
    [Authorize(Roles = "Helper")]
    public class CareersController : Controller
    {
        public enum MStatus
        {
            success,
            danger
        }

        public ActionResult Index(string title, int? lang)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Jobs.Include("Comments").OrderByDescending(o => o.Id).ToList();
                if (!string.IsNullOrEmpty(title))
                {
                    list = list.Where(w => w.Title.Contains(title)).ToList();
                }

                return View(list);
            }
        }

        public ActionResult Comments(int? id, string comment_auther, string job)
        {
            using (var dataModel = new DataModel())
            {
                var list = dataModel.Comments.Where(w => w.job_id.HasValue)
                    .Include(w => w.Job).ToList();

                if (id.HasValue)
                {
                    list = list.Where(w =>
                    {
                        var jobId = w.job_id;
                        var nullable = id;
                        return jobId.GetValueOrDefault() == nullable.GetValueOrDefault() &&
                               jobId.HasValue == nullable.HasValue;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(comment_auther))
                {
                    list = list.Where(w => w.comment_auther.Contains(comment_auther)).ToList();
                }

                if (!string.IsNullOrEmpty(job))
                {
                    list = list.Where(w => w.Job.Title.Contains(job)).ToList();
                }

                return View(list);
            }
        }

        public ActionResult AddJob()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddJob(Job model)
        {
            using (var dataModel = new DataModel())
            {
                model.City = "";
                dataModel.Jobs.Add(model);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Careers");
            }
        }

        [HttpGet]
        public ActionResult DownloadCv(string name)
        {
            var path = "~/Recources/Uploads/" + name;
            var file = System.IO.File.ReadAllBytes(Server.MapPath(path));

            return this.File(file, "application/file", name);
        }

        public ActionResult RemoveJob(int id)
        {
            using (var dataModel = new DataModel())
            {
                var job = dataModel.Jobs.SingleOrDefault(e => e.Id == id);
                if (job == null)
                {
                    throw new NullReferenceException();
                }

                dataModel.Comments.RemoveRange(job.Comments);
                dataModel.Jobs.Remove(job);
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Index", "Careers");
            }
        }

        public ActionResult Contact()
        {
            using (var dataModel = new DataModel())
            {
                return View(dataModel.Contacts
                    .Where(t => t.Type == ContactType.Job)
                    .OrderByDescending(o => o.Id).ToList());
            }
        }

        private void CreatePersonalInfoSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.PersonalInformation);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var row2 = sheet.CreateRow(1);
            var row3 = sheet.CreateRow(2);
            var row4 = sheet.CreateRow(3);
            var row5 = sheet.CreateRow(4);
            var row6 = sheet.CreateRow(5);
            var row7 = sheet.CreateRow(6);
            var row9 = sheet.CreateRow(8);
            var row10 = sheet.CreateRow(9);
            var row11 = sheet.CreateRow(10);
            var row12 = sheet.CreateRow(11);

            #endregion

            #region TB#1

            row1.CreateCell(0).SetCellValue(Resource.FullName);
            row1.CreateCell(1).SetCellValue(app.FullName);

            row2.CreateCell(0).SetCellValue(Resource.DOB);
            row2.CreateCell(1).SetCellValue($"{app.DateOfBirth:D}");

            row3.CreateCell(0).SetCellValue(Resource.POB);
            row3.CreateCell(1).SetCellValue($"{app.CountryOfBirth}, {app.CityOfBirth}");

            row4.CreateCell(0).SetCellValue(Resource.SocialStatus);
            var socialStatus = string.Empty;
            switch (app.SocialStatus)
            {
                case SocialStatus.Single:
                    socialStatus = "أهزب";
                    break;
                case SocialStatus.Married:
                    socialStatus = "متزوج";
                    break;
                case SocialStatus.Widowed:
                    socialStatus = "أرمل ";
                    break;
                case SocialStatus.Divorcee:
                    socialStatus = "مطلق ";
                    break;
            }
            row4.CreateCell(1).SetCellValue(socialStatus);

            row5.CreateCell(0).SetCellValue(Resource.Nationality);
            row5.CreateCell(1).SetCellValue(app.Nationality);

            row6.CreateCell(0).SetCellValue(Resource.Gender);
            row6.CreateCell(1).SetCellValue(app.Gender == 1 ? "ذكر" : "آنثى");

            row7.CreateCell(0).SetCellValue(Resource.FamilyNo);
            row7.CreateCell(1).SetCellValue(app.NumberOfDependents ?? 0);

            #endregion

            #region TB#2

            row1.CreateCell(4).SetCellValue(Resource.IdNumber);
            row1.CreateCell(5).SetCellValue(app.IdentityNo);

            row2.CreateCell(4).SetCellValue(Resource.ReleaseDate);
            row2.CreateCell(5).SetCellValue(app.IdentitReleaseDate != null
                ? $"{app.IdentitReleaseDate:D}"
                : "");

            row3.CreateCell(4).SetCellValue(Resource.PlaceOfIssue);
            row3.CreateCell(5).SetCellValue(app.PlaceOfIssue);

            #endregion

            #region TB#3

            row5.CreateCell(0).SetCellValue(Resource.Passport);
            row5.CreateCell(1).SetCellValue(app.Passport);

            row6.CreateCell(0).SetCellValue(Resource.ReleaseDate);
            row6.CreateCell(1).SetCellValue(app.PassportReleaseDate != null
                ? $"{app.PassportReleaseDate:D}"
                : "");

            row7.CreateCell(0).SetCellValue(Resource.ExpirationDate);
            row7.CreateCell(1).SetCellValue(app.PassportEndDate != null
                ? $"{app.PassportEndDate:D}"
                : "");

            #endregion

            #region TB#4

            row9.CreateCell(0).SetCellValue(Resource.Mobile);
            row9.CreateCell(1).SetCellValue(app.Mobile);

            row10.CreateCell(0).SetCellValue(Resource.Email);
            row10.CreateCell(1).SetCellValue(app.Email);

            row11.CreateCell(0).SetCellValue(Resource.Phone);
            row11.CreateCell(1).SetCellValue(app.Tell);

            row12.CreateCell(0).SetCellValue(Resource.Address);
            row12.CreateCell(1).SetCellValue(app.Address);

            #endregion
        }

        private void CreateQualificationSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.Qualifications);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.FirstAcademic);
            row1.CreateCell(2).SetCellValue(Resource.AcademicInstatue);
            row1.CreateCell(3).SetCellValue(Resource.Specialization);
            row1.CreateCell(4).SetCellValue(Resource.GraduationYear);
            row1.CreateCell(5).SetCellValue(Resource.Grade);

            #endregion

            #region FillTB

            if (app.Qualifications.Any())
            {
                var i = 1;
                foreach (var qualification in app.Qualifications)
                {
                    var row = sheet.CreateRow(rowNumber);
                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(qualification.Name);
                    row.CreateCell(2).SetCellValue(qualification.University);
                    row.CreateCell(3).SetCellValue(qualification.Specialization);
                    row.CreateCell(4).SetCellValue(qualification.GraduationYear);
                    row.CreateCell(5).SetCellValue(qualification.Grade);

                    ++i;
                    ++rowNumber;
                }
            }

            #endregion
        }

        private void CreateLanguageSkillsSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.LanguageSkills);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.Lang);
            row1.CreateCell(2).SetCellValue(Resource.Conversation);
            row1.CreateCell(3).SetCellValue(Resource.Writing);
            row1.CreateCell(4).SetCellValue(Resource.Reading);

            #endregion

            #region FillTB

            var languageSkills = app.LanguageSkills
                .Where(t => !string.IsNullOrEmpty(t.Name))
                .ToList();
            if (languageSkills.Any())
            {
                var i = 1;
                foreach (var languageSkill in languageSkills)
                {
                    var row = sheet.CreateRow(rowNumber);
                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(languageSkill.Name);
                    row.CreateCell(2).SetCellValue(GetLevel(languageSkill.Conversation));
                    row.CreateCell(3).SetCellValue(GetLevel(languageSkill.Writing));
                    row.CreateCell(4).SetCellValue(GetLevel(languageSkill.Reading));

                    ++i;
                    ++rowNumber;
                }
            }

            #endregion
        }

        private string GetLevel(SkillStatus status)
        {
            switch (status)
            {
                case SkillStatus.Medium:
                    return "متوسط";
                case SkillStatus.Excellent:
                    return " جيد جدا";
                case SkillStatus.Good:
                    return "جيد";
                case SkillStatus.Beginner:
                    return "مبتدئ";
                default: return "";
            }
        }

        private void CreateComputerSkillsSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.ComputerSkills);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.Name);
            row1.CreateCell(2).SetCellValue(Resource.Level);

            #endregion

            #region FillTB

            var computerSkills = app.ComputerSkills
                .Where(t => !string.IsNullOrEmpty(t.Name))
                .ToList();
            if (computerSkills.Any())
            {
                var i = 1;
                foreach (var computerSkill in computerSkills)
                {
                    var row = sheet.CreateRow(rowNumber);
                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(computerSkill.Name);
                    row.CreateCell(2).SetCellValue(GetLevel(computerSkill.Skill));

                    ++i;
                    ++rowNumber;
                }
            }

            #endregion
        }

        private void CreateTrainingSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.Training);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.Address);
            row1.CreateCell(2).SetCellValue(Resource.Organization);
            row1.CreateCell(3).SetCellValue(Resource.Course);
            row1.CreateCell(4).SetCellValue(Resource.Date);
            row1.CreateCell(5).SetCellValue(Resource.Duration);

            #endregion

            #region FillTB

            var courses = app.Courses
                .Where(t => !string.IsNullOrEmpty(t.Name))
                .ToList();
            if (courses.Any())
            {
                var i = 1;
                foreach (var course in courses)
                {
                    var row = sheet.CreateRow(rowNumber);
                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(course.Address);
                    row.CreateCell(2).SetCellValue(course.Orginization);
                    row.CreateCell(3).SetCellValue(course.Name);
                    row.CreateCell(4).SetCellValue((DateTime)course.Date);
                    row.CreateCell(5).SetCellValue(course.Duration);

                    ++i;
                    ++rowNumber;
                }
            }

            #endregion
        }

        private void CreateEmploymentHistorySheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.EmploymentHistory);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.Duration);
            row1.CreateCell(2).SetCellValue(Resource.FromDate);
            row1.CreateCell(3).SetCellValue(Resource.ToDate);
            row1.CreateCell(4).SetCellValue(Resource.Salary);
            row1.CreateCell(5).SetCellValue(Resource.JobTitle);
            row1.CreateCell(6).SetCellValue(Resource.CompanyAddressAndDetails);
            row1.CreateCell(7).SetCellValue(Resource.JobDes);

            #endregion

            #region FillTB

            if (app.Employments.Any())
            {
                var i = 1;
                foreach (var employment in app.Employments)
                {
                    var row = sheet.CreateRow(rowNumber);

                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(employment.Duration);
                    row.CreateCell(2).SetCellValue((DateTime)employment.FromDate);
                    row.CreateCell(3).SetCellValue((DateTime)employment.ToDate);
                    row.CreateCell(4).SetCellValue(employment.Salary);
                    row.CreateCell(5).SetCellValue(employment.JobTitle);
                    row.CreateCell(6).SetCellValue(employment.CompanyAddress);
                    row.CreateCell(7).SetCellValue(employment.Description);

                    ++i;
                    ++rowNumber;
                }
            }

            #endregion
        }

        private void CreateOtherSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.Other);

            #region InitRows

            var rowNumber = 0;

            #endregion

            #region FillTB

            var row1 = sheet.CreateRow(rowNumber);
            row1.CreateCell(0).SetCellValue(Resource.Conduct);
            row1.CreateCell(1).SetCellValue(app.HasCrime ? "نعم" : "لا");

            ++rowNumber;

            if (app.HasCrime)
            {
                var row2 = sheet.CreateRow(rowNumber);
                row2.CreateCell(0).SetCellValue(Resource.Details);
                row2.CreateCell(1).SetCellValue(app.CrimeDescription);

                ++rowNumber;
            }

            var row3 = sheet.CreateRow(rowNumber);
            row3.CreateCell(0).SetCellValue(Resource.Health);
            row3.CreateCell(1).SetCellValue(app.HasDisable ? "نعم" : "لا");

            ++rowNumber;

            if (app.HasDisable)
            {
                var row2 = sheet.CreateRow(rowNumber);
                row2.CreateCell(0).SetCellValue(Resource.Details);
                row2.CreateCell(1).SetCellValue(app.DisableDescription);

                ++rowNumber;
            }

            var row4 = sheet.CreateRow(rowNumber);
            row4.CreateCell(0).SetCellValue(Resource.CurrentEmployementStatus);
            row4.CreateCell(1).SetCellValue(app.EmployeeNow ? "نعم" : "لا");

            ++rowNumber;

            var row5 = sheet.CreateRow(rowNumber);
            row5.CreateCell(0).SetCellValue(Resource.CanContactCompany);
            row5.CreateCell(1).SetCellValue(app.AskYourEmployer ? "نعم" : "لا");

            ++rowNumber;

            var row6 = sheet.CreateRow(rowNumber);
            row6.CreateCell(0).SetCellValue(Resource.Telephone);
            row6.CreateCell(1).SetCellValue(app.EmployerMobile);

            ++rowNumber;

            var row7 = sheet.CreateRow(rowNumber);
            row7.CreateCell(0).SetCellValue(Resource.CompanyAddressAndDetails);
            row7.CreateCell(1).SetCellValue(app.EmployerDescription);

            ++rowNumber;

            var row8 = sheet.CreateRow(rowNumber);
            row8.CreateCell(0).SetCellValue(Resource.StartWorking);
            row8.CreateCell(1).SetCellValue(app.WhenStartWorking);

            ++rowNumber;

            var row9 = sheet.CreateRow(rowNumber);
            row9.CreateCell(0).SetCellValue(Resource.ExpectedSalary);
            row9.CreateCell(1).SetCellValue(app.SalaryExpect);

            ++rowNumber;

            var row10 = sheet.CreateRow(rowNumber);
            row10.CreateCell(0).SetCellValue(Resource.RelativeWorker);
            row10.CreateCell(1).SetCellValue(app.HasRelativeInOurCompany ? "نعم" : "لا");


            if (app.HasRelativeInOurCompany)
            {
                ++rowNumber;
                ++rowNumber;

                #region InitRows

                var row11 = sheet.CreateRow(rowNumber);

                #endregion

                #region InitHeader

                row11.CreateCell(0).SetCellValue("#");
                row11.CreateCell(1).SetCellValue(Resource.Unit);
                row11.CreateCell(2).SetCellValue(Resource.EmployeeName);
                row11.CreateCell(3).SetCellValue(Resource.RelativeRelation);
                row11.CreateCell(4).SetCellValue(Resource.Job);

                ++rowNumber;

                #endregion

                #region FillRelativesTB

                var relativeEmployers = app.RelativeEmployers
                    .ToList();
                if (relativeEmployers.Any())
                {
                    var i = 1;
                    foreach (var relativeEmployer in relativeEmployers)
                    {
                        var row = sheet.CreateRow(rowNumber);
                        row.CreateCell(0).SetCellValue(i);
                        row.CreateCell(1).SetCellValue(relativeEmployer.Unit);
                        row.CreateCell(2).SetCellValue(relativeEmployer.Name);
                        row.CreateCell(3).SetCellValue(relativeEmployer.RelativeRelation);
                        row.CreateCell(4).SetCellValue(relativeEmployer.JobTitle);

                        ++i;
                        ++rowNumber;
                    }
                }

                #endregion
            }

            #endregion
        }

        private void CreateRefPersonsSheet(IWorkbook wb, JobApplication app)
        {
            var sheet = wb.CreateSheet(Resource.RefPersons);

            #region InitRows

            var row1 = sheet.CreateRow(0);
            var rowNumber = 1;

            #endregion

            #region InitHeader

            row1.CreateCell(0).SetCellValue("#");
            row1.CreateCell(1).SetCellValue(Resource.Name);
            row1.CreateCell(2).SetCellValue(Resource.Job);
            row1.CreateCell(3).SetCellValue(Resource.Address);
            row1.CreateCell(4).SetCellValue(Resource.Phone);

            #endregion

            #region FillTB

            if (app.References.Any())
            {
                var i = 1;
                foreach (var employment in app.References)
                {
                    var row = sheet.CreateRow(rowNumber);

                    row.CreateCell(0).SetCellValue(i);
                    row.CreateCell(1).SetCellValue(employment.Name);
                    row.CreateCell(2).SetCellValue(employment.JobTitle);
                    row.CreateCell(3).SetCellValue(employment.Address);
                    row.CreateCell(4).SetCellValue(employment.Mobile);

                    ++i;
                    ++rowNumber;
                }
            }

            ++rowNumber;

            var agrenmentRow = sheet.CreateRow(rowNumber);
            agrenmentRow.CreateCell(0).SetCellValue(Resource.ApplicationAgreenment);

            ++rowNumber;

            var answerRow = sheet.CreateRow(rowNumber);
            answerRow.CreateCell(0).SetCellValue(app.Confirm ? "نعم" : "لا");

            #endregion
        }

        public async Task<ActionResult> DownloadCv(int id)
        {
            var dataModel = new DataModel();
            var app = await dataModel.JobApplication
                .Include(t => t.Employments)
                .Include(t => t.ComputerSkills)
                .Include(t => t.Employments)
                .Include(t => t.Courses)
                .Include(t => t.LanguageSkills)
                .Include(t => t.Qualifications)
                .Include(t => t.References)
                .Include(t => t.RelativeEmployers)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (app == null)
            {
                throw new NullReferenceException();
            }

            var wb = new XSSFWorkbook();

            CreatePersonalInfoSheet(wb, app);
            CreateQualificationSheet(wb, app);
            CreateLanguageSkillsSheet(wb, app);
            CreateComputerSkillsSheet(wb, app);
            CreateTrainingSheet(wb, app);
            CreateEmploymentHistorySheet(wb, app);
            CreateOtherSheet(wb, app);
            CreateRefPersonsSheet(wb, app);

            using (var memoryStream = new MemoryStream()) //creating memoryStream
            {
                wb.Write(memoryStream);
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{app.FullName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }

        public async Task<ActionResult> CVsApplications()
        {
            using (var dataModel = new DataModel())
            {
                var titleName = Request.QueryString["title"];
                var email = Request.QueryString["email"];
                var gender = Request.QueryString["gender"];
                var nationality = Request.QueryString["nationality"];
                var identityNo = Request.QueryString["IdentityNo"];
                var mobile = Request.QueryString["mobile"];
                var placeOfBirth = Request.QueryString["PlaceOfBirth"];
                var name = Request.QueryString["nlevel"];
                var university = Request.QueryString["University"];
                var specialization = Request.QueryString["Specialization"];
                var graduationYear = Request.QueryString["GraduationYear"];

                var list = dataModel.JobApplication
                    .AsQueryable()
                    .Include(e => e.Qualifications);

                if (!string.IsNullOrEmpty(titleName))
                {
                    list = list.Where(w => w.FullName.Contains(titleName));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    list = list.Where(w => w.Email.Contains(email));
                }

                if (!string.IsNullOrEmpty(nationality))
                {
                    list = list.Where(w => w.Nationality.Contains(nationality));
                }

                if (!string.IsNullOrEmpty(identityNo))
                {
                    list = list.Where(w => w.IdentityNo.Contains(identityNo));
                }

                if (!string.IsNullOrEmpty(mobile))
                {
                    list = list.Where(w => w.Mobile.Contains(mobile));
                }

                if (!string.IsNullOrEmpty(placeOfBirth))
                {
                    list = list.Where(w => w.CountryOfBirth.Contains(placeOfBirth) ||
                                           w.CityOfBirth.Contains(placeOfBirth));
                }

                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(w => w.Qualifications.Any(c => c.Name.Contains(name)));
                }

                if (!string.IsNullOrEmpty(university))
                {
                    list = list.Where(w => w.Qualifications.Any(e => e.University.Contains(university)));
                }

                if (!string.IsNullOrEmpty(specialization))
                {
                    list = list.Where(w => w.Qualifications.Any(e => e.Specialization.Contains(specialization)));
                }

                if (!string.IsNullOrEmpty(graduationYear))
                {
                    list = list.Where(w => w.Qualifications.Any(e => e.GraduationYear.Contains(graduationYear)));
                }

                if (!string.IsNullOrEmpty(gender))
                {
                    list = list.Where(w => w.Gender == int.Parse(gender));
                }

                return View(await list.ToListAsync());
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveContact(int id)
        {
            using (var dataModel = new DataModel())
            {
                var contact = await dataModel.Contacts
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (contact != null)
                {
                    dataModel.Contacts.Remove(contact);
                    await dataModel.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Contact));
            }
        }

        public async Task<ActionResult> RemoveApplication(int id)
        {
            using (var dataModel = new DataModel())
            {
                var jobApplication = await dataModel.JobApplication
                    .FirstOrDefaultAsync(e => e.Id == id);
                if (jobApplication != null)
                {
                    dataModel.JobApplication.Remove(jobApplication);
                    await dataModel.SaveChangesAsync();
                }

                return RedirectToAction(nameof(CVsApplications));
            }
        }

        public async Task<ActionResult> RemoveComment(int id)
        {
            using (var dataModel = new DataModel())
            {
                var comment = await dataModel.Comments
                    .FirstOrDefaultAsync(e => e.Id == id);
                var jobId = comment.job_id;
                if (comment != null)
                {
                    dataModel.Comments.Remove(comment);
                    await dataModel.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Comments), new { id = jobId });
            }
        }

        public class newlist
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Nationality { get; set; }

            public string IdentityNo { get; set; }

            public string Mobile { get; set; }
            public string PlaceOfBirth { get; set; }

            public int Gender { get; set; }
            public int Id { get; set; }

            public string Name { get; set; }

            public string University { get; set; }

            public string Specialization { get; set; }

            public string GraduationYear { get; set; }

        }

        public ActionResult Details(int? id)
        {
            var dataModel = new DataModel();
            var data = dataModel.JobApplication
                .Include(t => t.Employments)
                .Include(t => t.ComputerSkills)
                .Include(t => t.Employments)
                .Include(t => t.Courses)
                .Include(t => t.LanguageSkills)
                .Include(t => t.Qualifications)
                .Include(t => t.References)
                .Include(t => t.RelativeEmployers)
                .FirstOrDefault(w => w.Id == id);

            return View(data);
        }

        public ActionResult Done(int? id)
        {
            using (var dataModel = new DataModel())
            {
                var contact = dataModel.Contacts.Find((object)id);
                contact.IsRead = true;
                dataModel.Entry(contact).State = (EntityState)16;
                dataModel.SaveChanges();
                return getMessage(MStatus.success, "", "Contact", "Careers");
            }
        }

        public ActionResult getMessage(Enum status, string meesage, string action, string controller)
        {
            TempData["Message"] = string.IsNullOrEmpty(meesage) ? "تمت العملية بنجاح" : meesage;
            TempData["Status"] = status;

            return RedirectToAction(action, controller);
        }
    }
}