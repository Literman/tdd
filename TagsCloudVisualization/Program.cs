using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var center = new Point(400, 400);
            var cloud = new CircularCloudLayouter(center);
            var rectangles = new List<Rectangle>();
            for (var i = 0; i < 150; i++)
            {
                rectangles.Add(cloud.PutNextRectangle(new Size(30, 30)));
                //rectangles.Add(cloud.PutNextRectangle(new Size(20, 10)));
            }

            CloudDrawer.Draw("1.bmp",center, rectangles, 800, 800);
        }
    }
}
