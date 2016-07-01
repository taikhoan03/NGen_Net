using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace NexGen
{
    public class ImageTool
    {
        private static Image ImageResize(int newWidth, Image originalImage)
        {
            var OrginalSize_IMG = new Size(originalImage.Width,originalImage.Height);
            var percent = newWidth*1.0 / originalImage.Width;
            return originalImage.GetThumbnailImage(originalImage.Width, originalImage.Height, null, IntPtr.Zero);
            //return originalImage.GetThumbnailImage(newWidth,  Convert.ToInt32((double)originalImage.Height * percent), null, IntPtr.Zero);

        }
        public static Image LowQuality(Image img)
        {
            //return ImageResize(img.Width, img.Height, img);
            return ImageResize(900, img);
        }
    }
}
