using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace NexGen.Common
{
    public class OCR_old
    {
        public static string OCRDrag(PictureBox pictureBox, Rectangle DragRect, OCRDocument Document, int Page, double percent)
        {
            try
            {
                XmlNode Pagenode = Document.Root.ChildNodes[Page];
                XmlNodeList LineNode = Pagenode.ChildNodes;
                List<XmlNode> Entities = new List<XmlNode>();
                foreach (XmlNode L in LineNode)
                    foreach (XmlNode N in L)
                        Entities.Add(N);
                Image IMG = pictureBox.Image;
                //var percent_ = pictureBox.Width*1.0 / orginalSize.Width;
                int X = Convert.ToInt32(1.0 * DragRect.X / percent);
                int Y = Convert.ToInt32(1.0 * DragRect.Y / percent);
                int W = Convert.ToInt32(1.0 * DragRect.Width / percent);
                int H = Convert.ToInt32(1.0 * DragRect.Height / percent);
                //int X = Convert.ToInt32(DragRect.X * pictureBox.Width * 1.0 / IMG.Width * percent);
                //int Y = Convert.ToInt32(DragRect.Y * pictureBox.Height * 1.0 / IMG.Height * percent);
                //int W = Convert.ToInt32(DragRect.Width * pictureBox.Width * 1.0 / IMG.Width * percent);
                //int H = Convert.ToInt32(DragRect.Height * pictureBox.Height * 1.0 / IMG.Height * percent);
                //int X = DragRect.X * IMG.Width / pictureBox.Width;
                //int Y = DragRect.Y * IMG.Height / pictureBox.Height;
                //int W = DragRect.Width * IMG.Width / pictureBox.Width;
                //int H = DragRect.Height * IMG.Height / pictureBox.Height;
                Rectangle R = new Rectangle(X, Y, W, H);

                String Result = "";
                foreach (XmlNode entity in Entities)
                {
                    if ((OCRDocument.GetBeginX(entity) < X + W) && (OCRDocument.GetEndX(entity) > X) && (OCRDocument.GetBeginY(entity) < Y + H) && (OCRDocument.GetEndY(entity) > Y))
                    {
                        Result += OCRDocument.GetContent(entity) + " ";
                    }
                }

                if (Result.Length > 0)
                    Result = Result.Remove(Result.Length - 1);
                return Result;
            }
            catch
            {
                return "";
            }
        }
    }
}