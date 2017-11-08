using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace TagsCloudVisualization
{
    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        private CircularCloudLayouter cloud;
        private List<Rectangle> rectangles = new List<Rectangle>();

        [Test, OneTimeSetUp]
        public void DoesNotThrowException_AfterInit()
        {
            Assert.DoesNotThrow(() => new CircularCloudLayouter(Point.Empty));
        }

        [SetUp]
        public void SetUp()
        {
            cloud = new CircularCloudLayouter(Point.Empty);
            rectangles.Clear();
        }

        [TestCase(0, 10, TestName = "When width is zero")]
        [TestCase(10, 0, TestName = "When height is zero")]
        [TestCase(-10, 10, TestName = "When width is negative")]
        [TestCase(10, -10, TestName = "When height is negative")]
        public void ThrowException_OnNonPositiveSize(int width, int height)
        {
            var size = new Size(width, height);
            Assert.Throws<ArgumentException>(() => cloud.PutNextRectangle(size));
        }

        [TestCase(10, 10, 0, 0, TestName = "When width equals height")]
        [TestCase(20, 10, 5, 7, TestName = "When width greater than height")]
        [TestCase(10, 30, 20, 50, TestName = "When width less than height")]
        public void ReturnCorrectRectangle_AfterOnePutting(int width, int height, int cloudCenterX, int cloudCenterY)
        {
            var center = new Point(cloudCenterX, cloudCenterY);
            cloud = new CircularCloudLayouter(center);

            var expectedRectangle = new Rectangle(center.X - width / 2, center.Y - height / 2, width, height);
            var actualRectangle = cloud.PutNextRectangle(new Size(width, height));
            rectangles.Add(actualRectangle);

            actualRectangle.ShouldBeEquivalentTo(expectedRectangle);
        }

        private static readonly object[] RectanglesCases =
        {
            new object[] {new[] {(1, 2), (3, 4)}, new Rectangle(1, -2, 3, 4)},
            new object[] {new[] {(1, 2), (3, 4), (4, 2)}, new Rectangle(-4, -1, 4, 2)},
            new object[] {new[] {(1, 2), (3, 4), (4, 2), (5, 7), (10, 15)}, new Rectangle(4, -7, 10, 15)},
        };

        [TestCaseSource(nameof(RectanglesCases))]
        public void ReturnCorrectRectangle_AfterSeveralPutting((int x, int y)[] points, Rectangle expectedRectangle)
        {
            var actualRectangle = Rectangle.Empty;

            foreach (var point in points)
            {
                actualRectangle = cloud.PutNextRectangle(new Size(point.x, point.y));
                rectangles.Add(actualRectangle);
            }
            
            actualRectangle.ShouldBeEquivalentTo(expectedRectangle);
        }

        [TestCase(10, 10, 10, TestName = "10 rectangles")]
        [TestCase(100, 20, 10, TestName = "100 rectangles")]
        [TestCase(1000, 30, 10, TestName = "1000 rectangles"), Timeout(900)]
        public void ReturnFalseIntersaction_AfterPutting(int count, int width, int height)
        {
            var size = new Size(width, height);

            for (var i = 0; i < count; i++)
                rectangles.Add(cloud.PutNextRectangle(size));

            for (var i = 0; i < count; i++)
                for (var j = i + 1; j < count; j++)
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
        }

        [TestCase(10, 10, 10, TestName = "10 rectangles")]
        [TestCase(100, 20, 10, TestName = "100 rectangles")]
        [TestCase(1000, 30, 10, TestName = "1000 rectangles"), Timeout(700)]
        public void FormsCircle_WithRadius_LessThanExpected(int count, int width, int height)
        {
            var center = new Point(10, 10);
            cloud = new CircularCloudLayouter(center);
            var size = new Size(height, width);

            var area = 0;
            var cloudRadius = 0.0;
            for (var i = 0; i < count; i++)
            {
                var rectangle = cloud.PutNextRectangle(size);
                area += rectangle.Height * rectangle.Width;
                cloudRadius = Math.Max(cloudRadius, GetDistance(center, rectangle.Location));
            }
            var circleRadius = 1.5 * Math.Sqrt(area / Math.PI);

            cloudRadius.Should().BeLessThan(circleRadius);
        }

        private static double GetDistance(Point point1, Point point2)
        {
            var point = new Point(point1.X - point2.X, point1.Y - point2.Y);
            return Math.Sqrt(point.X * point.X + point.Y * point.Y);
        }

        [TearDown]
        public void TearDown()
        {
            var context = TestContext.CurrentContext;

            if (context.Result.Outcome.Status == TestStatus.Failed)
            {
                var path = $@"{context.TestDirectory}\{context.Test.Name}.bmp";

                CircularCloudDrawer.Save(path, Point.Empty, rectangles);
                Console.WriteLine("Tag cloud visualization saved to file " + path);
            }
        }
    }
}