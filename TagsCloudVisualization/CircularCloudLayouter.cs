using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly IEnumerator<Point> centers;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            centers = GetCenter().GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Dimensions must be positive", nameof(rectangleSize));

            var rect = GetRectangle(rectangleSize);
            rectangles.Add(rect);
            return rect;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                centers.MoveNext();
                var newCenter = centers.Current - GetHalfSize(rectangleSize);
                rectangle = new Rectangle(newCenter, rectangleSize);
            } while (IsColision(rectangle));

            return rectangle;
        }

        private IEnumerable<Point> GetCenter()
        {
            for (var radius = 0; ; radius++)
                for (var i = 0.0; i < 2 * Math.PI; i += Math.PI / 180)
                    yield return center + new Size(
                                     (int)(radius * Math.Cos(i)), (int)(radius * Math.Sin(i)));
        }

        private bool IsColision(Rectangle rectangle) => 
            rectangles.Any(r => r.IntersectsWith(rectangle));

        private static Size GetHalfSize(Size size) => new Size(size.Width / 2, size.Height / 2);
    }
}