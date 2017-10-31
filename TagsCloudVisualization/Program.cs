using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(400, 400);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            var rnd = new Random();

            for (var i = 0; i < 1000; i++)
                rectangles.Add(cloud.PutNextRectangle(
                    new Size(rnd.Next(5,10), rnd.Next(5,10))));

            CloudDrawer.Draw("1.bmp",center, rectangles, 800, 800);
        }
    }
}
