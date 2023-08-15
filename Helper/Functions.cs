using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace alwatnia.Helper
{
    public static partial class Functions
    {
        public static string[] Extensions = ConfigurationManager.AppSettings["ImageFormats"].Split(',');

        public class PaginatedResult<T>
        {
            public int PageCounts { get; set; }

            public IEnumerable<T> Results { get; set; }

            public int TotalCount { get; set; }

            public string Entity { get; set; }

            public int CurrentPage { get; set; }

            public RouteValueDictionary QueryParams { get; set; }
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + " ...";
        }

        public static PaginatedResult<T> Paginate<T>(
            this IQueryable<T> query,
            int? pageNum,
            int pageSize = 12)
        {
            if (pageSize <= 0)
            {
                pageSize = DefaultPageSize;
            }

            //Total result count
            var rowsCount = query.Count();

            //If page number should be > 0 else set to first page
            if (!pageNum.HasValue || rowsCount <= pageSize || pageNum <= 0)
            {
                pageNum = 1;
            }

            //Calculate number of rows to skip on page size
            var excludedRows = (pageNum.Value - 1) * pageSize;

            return new PaginatedResult<T>
            {
                Results = query.Skip(excludedRows).Take(Math.Min(pageSize, rowsCount)).ToArray(),
                TotalCount = rowsCount,
                PageCounts = (int)Math.Ceiling((decimal)rowsCount / pageSize),
                CurrentPage = pageNum.Value
            };
        }

        public const int DefaultPageSize = 10;


        public static string getVideoId(string link)
        {
            try
            {
                return Regex.Match(link, "youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)").Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }

        public static string SerializeUrl(string title)
        {
            title = Regex.Replace(title, "[^0-9a-zA-Z\\._]", title);
            return title.Replace(" ", "_");
        }

        public static Tuple<bool, string> ValidateImage(HttpPostedFileBase file, bool restrict = true)
        {
            if (file == null)
            {
                return Tuple.Create(false, "There is no image");
            }

            if (!IsImage(file))
            {
                return Tuple.Create(false, "Accepted image formats are [" + string.Join(",", Extensions) + "]");
            }

            if (file.ContentLength <= 0)
            {
                return Tuple.Create(false, "File size can not be zero");
            }

            if (restrict)
            {
                var appSetting1 = ConfigurationManager.AppSettings["MaxImageSize"];
                var num = int.Parse(appSetting1.Replace("mb", "")) * 1048576;
                if (file.ContentLength > num)
                {
                    return Tuple.Create(false,
                        "Maximum allowed file size must be less than or equal to " + appSetting1);
                }

                var image = Image.FromStream(file.InputStream);
                var appSetting2 = ConfigurationManager.AppSettings["MinImageWidth"];
                var appSetting3 = ConfigurationManager.AppSettings["MinImageHeight"];
                int.Parse(appSetting2.Replace("px", ""));
                int.Parse(appSetting3.Replace("px", ""));
                image.Dispose();
                file.InputStream.Position = 0L;
            }
            return Tuple.Create(true, "");
        }

        public static bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            return Extensions.Any(item => file.FileName.ToLower().EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }


        public static int GetLanguage()
        {
            if (HttpContext.Current.Request.Cookies["lang"] != null)
            {
                return HttpContext.Current.Request.Cookies["lang"]?.Value == "en" ? (int)Languages.English : (int)Languages.Arabic;
            }

            return 1;
        }


        public static bool IsEnglish()
        {
            return GetLanguage() == (int)Languages.English;
        }

        public static string SaveTempFile(HttpPostedFileBase file, string path = "~/Temp", bool isImage = true)
        {
            var str = HttpContext.Current.Server.MapPath(path);
            var path2 = Guid.NewGuid() + ".jpeg";
            var saveTo = Path.Combine(str, path2);
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }

            if (isImage)
            {
                var memoryStream = new MemoryStream();
                file.InputStream.CopyTo(memoryStream);
                var array = memoryStream.ToArray();
                var image = Image.FromStream(file.InputStream);
                var width = 1200;
                var height = (int)(image.Height * width / (decimal)image.Width);
                GetCroppedImage(array, new Size(width, height), 0, 0, ImageFormat.Jpeg, saveTo);
                image.Dispose();
            }
            else
            {
                file.SaveAs(saveTo);
            }

            return path2;
        }

        public static string SaveTempFile(byte[] file, string path = "~/Temp")
        {
            var str = HttpContext.Current.Server.MapPath(path);
            var path2 = Guid.NewGuid() + ".jpeg";
            var saveTo = Path.Combine(str, path2);
            if (!Directory.Exists(str))
            {
                Directory.CreateDirectory(str);
            }

            Stream stream = new MemoryStream(file);
            var image = Image.FromStream(stream);
            var width = 1200;
            var height = (int)(image.Height * width / (decimal)image.Width);
            GetCroppedImage(file, new Size(width, height), 0, 0, ImageFormat.Jpeg, saveTo);
            image.Dispose();
            return path2;
        }

        public static byte[] GetCroppedImage(byte[] originalBytes, Size size, int x, int y, ImageFormat format,
            string saveTo = null)
        {
            using (var memoryStream1 = new MemoryStream(originalBytes))
            {
                using (var image = Image.FromStream(memoryStream1))
                {
                    var width1 = image.Width;
                    var height1 = image.Height;
                    var val2 = size.Width / (float)width1;
                    var num = Math.Max(size.Height / (float)height1, val2);
                    var width2 = (int)Math.Max(width1 * num, size.Width);
                    var height2 = (int)Math.Max(height1 * num, size.Height);
                    using (var bitmap1 = new Bitmap(width2, height2))
                    {
                        using (var graphics = Graphics.FromImage(bitmap1))
                        {
                            graphics.Clear(Color.White);
                            graphics.InterpolationMode = InterpolationMode.Default;
                            graphics.DrawImage(image, 0, 0, width2, height2);
                        }
                        x = Math.Max(x, (width2 - size.Width) / 2);
                        y = Math.Max(y, (height2 - size.Height) / 2);
                        var rect = new Rectangle(x, y, size.Width, size.Height);
                        using (var bitmap2 = bitmap1.Clone(rect, bitmap1.PixelFormat))
                        {
                            using (var memoryStream2 = new MemoryStream())
                            {
                                var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == format.Guid);
                                var encoderParams = new EncoderParameters(1);
                                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 30L);
                                bitmap2.Save(memoryStream2, encoder, encoderParams);
                                if (!string.IsNullOrEmpty(saveTo))
                                {
                                    bitmap2.Save(saveTo);
                                }

                                return memoryStream2.ToArray();
                            }
                        }
                    }
                }
            }
        }

        public static string GetAnonymousValue(object obj, string key)
        {
            if (obj == null)
            {
                return "";
            }

            var property = obj.GetType().GetProperty(key);
            return property != null ? property.GetValue(obj, null).ToString().ToLower() : "";
        }

        public static List<string> SaveMultiFile(HttpPostedFileBase[] file, string path = "~/Recources/Albums")
        {
            var stringList = new List<string>();
            foreach (var httpPostedFileBase in file)
            {
                var str = HttpContext.Current.Server.MapPath(path);
                var path2 = Guid.NewGuid() + ".jpeg";
                var saveTo = Path.Combine(str, path2);
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }

                var memoryStream = new MemoryStream();
                httpPostedFileBase.InputStream.CopyTo(memoryStream);
                var array = memoryStream.ToArray();
                var image = Image.FromStream(httpPostedFileBase.InputStream);
                var width = 1200;
                var height = (int)(image.Height * width / (decimal)image.Width);
                GetCroppedImage(array, new Size(width, height), 0, 0, ImageFormat.Jpeg, saveTo);
                image.Dispose();
                stringList.Add(path2);
            }
            return stringList;
        }

        public static int GetAnOtherDays(DateTime oldDate, DateTime newDate, int years)
        {
            var num = 0;
            oldDate = oldDate.AddYears(years);
            for (var dateTime = oldDate.AddMonths(1);
                dateTime.Month <= newDate.Month && dateTime.Year <= newDate.Year ||
                dateTime.Month > newDate.Month && dateTime.Year < newDate.Year;
                dateTime = oldDate.AddMonths(1))
            {
                num += (dateTime - oldDate).Days - 30;
                oldDate = oldDate.AddMonths(1);
            }
            return num;
        }

        public static int GetLeapDays(DateTime oldDate, DateTime newDate)
        {
            var num = 0;
            for (; oldDate.Year < newDate.Year; oldDate = oldDate.AddYears(1))
            {
                if (DateTime.IsLeapYear(oldDate.Year))
                {
                    ++num;
                }
            }

            return num;
        }
    }
}