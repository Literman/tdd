using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(Point.Empty);
        }


        [Test]
        public void NotThrowException_AfterInit() => 
            new CircularCloudLayouter(Point.Empty);

        [TestCase(0, 10, TestName = "When width is zero")]
        [TestCase(10, 0, TestName = "When height is zero")]
        [TestCase(-10, 10, TestName = "When width is negative")]
        [TestCase(10, -10, TestName = "When height is negative")]
        public void ThrowException_OnNonPositiveSize(int width, int height)
        {
            Assert.Throws<ArgumentException>(() =>
                cloud.PutNextRectangle(new Size(width, height)));
        }

        [TestCase(10, 10, 0, 0, TestName = "When width equals height")]
        [TestCase(20, 10, 5, 7, TestName = "When width greater than height")]
        [TestCase(10, 30, 20, 50, TestName = "When width less than height")]
        public void ReturnCorrectRectangle_AfterOnePutting(int width, int height, int x, int y)
        {
            cloud = new CircularCloudLayouter(new Point(x, y));
            cloud.PutNextRectangle(new Size(width, height))
                .ShouldBeEquivalentTo(new Rectangle(x - width / 2, y - height / 2, width, height));
        }

        [TestCase(10, 10, 10, TestName = "10 rectangles")]
        [TestCase(100, 20, 10, TestName = "100 rectangles")]
        [TestCase(1000, 30, 10, TestName = "1000 rectangles")]
        [TestCase(3000, 10, 20, TestName = "3000 rectangles"), Timeout(4000)]
        public void ReturnFalseIntersaction_AfterPutting(int count, int width, int height)
        {
            var rectangles = new List<Rectangle>();
            var size = new Size(width, height);

            for (var i = 0; i < count; i++)
                rectangles.Add(cloud.PutNextRectangle(size));

            for (var i = 0; i < count; i++)
                for (var j = i + 1; j < count; j++)
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    var context = TestContext.CurrentContext;

        //    if (context.Result.Outcome.Status == TestStatus.Failed)
        //    {
        //        var path = $@"{context.TestDirectory}\{context.Test.Name}.bmp";

        //        //CloudDrawer.Draw(path, Point.Empty, rectangles);
        //        Console.WriteLine("Tag cloud visualization saved to file " + path);
        //    }
        //}
    }
}