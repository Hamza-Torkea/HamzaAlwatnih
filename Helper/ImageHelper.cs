using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace alwatnia.Helper
{
    public static class ImageHelper
    {
        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (var memoryStream = new MemoryStream(content))
            {
                return CropImage(memoryStream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            using (var bitmap = new Bitmap(content))
            {
                var size = bitmap.Size;
                Convert.ToDouble(size.Width);
                size = bitmap.Size;
                Convert.ToDouble(size.Height);
                var srcRect = new Rectangle(x, y, width, height);
                using (var source = new Bitmap(srcRect.Width, srcRect.Height))
                {
                    using (var graphics = Graphics.FromImage(source))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(bitmap, new Rectangle(0, 0, source.Width, source.Height), srcRect,
                            GraphicsUnit.Pixel);
                        return GetBitmapBytes(source);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            var imageEncoder = ImageCodecInfo.GetImageEncoders()[4];
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
            using (var memoryStream = new MemoryStream())
            {
                source.Save(memoryStream, imageEncoder, encoderParams);
                var buffer = new byte[memoryStream.Length];
                memoryStream.Seek(0L, SeekOrigin.Begin);
                memoryStream.Read(buffer, 0, (int) memoryStream.Length);
                return buffer;
            }
        }

        public static void SaveFile(byte[] content, string path)
        {
            var fileFullPath = GetFileFullPath(path);
            if (!Directory.Exists(Path.GetDirectoryName(fileFullPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileFullPath));
            using (var fileStream = File.Create(fileFullPath))
            {
                fileStream.Write(content, 0, content.Length);
            }
        }

        public static string GetFileFullPath(string path)
        {
            var virtualPath = path.StartsWith("~") ? path : (path.StartsWith("/") ? "~" + path : path);
            return virtualPath.StartsWith("~") ? HostingEnvironment.MapPath(virtualPath) : virtualPath;
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
                    var val2 = size.Width / (float) width1;
                    var num = Math.Max(size.Height / (float) height1, val2);
                    var width2 = (int) Math.Max(width1 * num, size.Width);
                    var height2 = (int) Math.Max(height1 * num, size.Height);
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
                                    bitmap2.Save(saveTo);
                                return memoryStream2.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}