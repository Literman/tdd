using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public static class CircularCloudDrawer
    {
        public static void Draw(Point center, List<Rectangle> rectangles, int width, int height)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                Draw(center, rectangles, bitmap);
                var form = new Form
                {
                    Width = width,
                    Height = height,
                    BackgroundImage = bitmap
                };
                form.ShowDialog();
            }
        }

        public static void Draw(string path, Point center, List<Rectangle> rectangles, int width, int height)
        {
            Draw(center, rectangles, width, height);
            Save(path, center, rectangles, width, height);
        }

        public static void Save(string path, Point center, List<Rectangle> rectangles, int width = 800, int height = 800)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                Draw(center, rectangles, bitmap);
                bitmap.Save(path);
            }
        }

        private static void Draw(Point center, List<Rectangle> rectangles, Bitmap bitmap)
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                var rectPen = new Pen(Color.Blue);
                g.DrawRectangle(new Pen(Color.Red), center.X, center.Y, 1, 1);

                foreach (var rectangle in rectangles)
                    g.DrawRectangle(rectPen, rectangle);
            }
        }
    }
}