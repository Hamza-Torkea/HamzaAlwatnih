using System;
using System.IO;
using System.Web.Mvc;

namespace alwatnia.Helper
{
    public class ImageResult : ActionResult
    {
        public ImageResult(Stream imageStream, string contentType)
        {
            ImageStream = imageStream ?? throw new ArgumentNullException(nameof(imageStream));
            ContentType = contentType ?? throw new ArgumentNullException(nameof(contentType));
        }

        public Stream ImageStream { get; }

        public string ContentType { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            var buffer = new byte[1];
            while (true)
            {
                var count = ImageStream.Read(buffer, 0, buffer.Length);
                if (count != 0)
                    response.OutputStream.Write(buffer, 0, count);
                else
                    break;
            }
            response.End();
        }
    }
}