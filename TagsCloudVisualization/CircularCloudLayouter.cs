﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
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
            var rnd = new Random();
            for (var radius = 0; ; radius++)
            {
                //double i = 0; i < 2 * Math.PI; i += Math.PI / 30
                for (var i = rnd.Next(360); i < 360; i += 1) //todo +=1
                {
                    var rad = i / Math.PI * 180;
                    var newCenter = center + new Size((int)(radius * Math.Cos(i)), (int)(radius * Math.Sin(i)));
                    var rectangle = new Rectangle(newCenter - GetHalfSize(rectangleSize), rectangleSize);

                    if (!rectangles.Any(r => r.IntersectsWith(rectangle)))
                        return rectangle;
                }
            }
        }

        private bool Colision(Rectangle rectangle) => 
            rectangles.Any(r => r.IntersectsWith(rectangle));

        private static Size GetHalfSize(Size size) => new Size(size.Width / 2, size.Height / 2);
    }
}