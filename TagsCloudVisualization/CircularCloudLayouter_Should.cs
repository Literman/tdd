using System;
using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

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

        //[Test]
        //public void ReturnCorrectRectangle_AfterSomePutting()
        //{
        //    cloud.PutNextRectangle(new Size(10, 10));
        //    cloud.PutNextRectangle(new Size(10, 10))
        //        .ShouldBeEquivalentTo(new Rectangle(-15, -1, 10, 10));
        //}

        [TestCase(2, 10, 10, TestName = "2 rectangles")]
        [TestCase(50, 20, 10, TestName = "50 rectangles")]
        [TestCase(100, 10, 20, TestName = "100 rectangles")]
        [TestCase(200, 30, 10, TestName = "200 rectangles"), Timeout(2500)]
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
    }
}