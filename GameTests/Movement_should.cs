using System.Drawing;
using FluentAssertions;
using Game.Fighters;
using Game.GameInformation;
using NUnit.Framework;

namespace GameTests
{
    [TestFixture]
    public class Movement_should
    {
        [Test]
        public void TestWhenPossible()
        {
            const int width = 2000;
            var setting = new GameSettings(width, 2000);
            var first = new Paladin("paladin", new PointF(200, 1000));
            var second = new Skeleton("skeleton", new PointF(1000, 1000));
            const int delta = 10;
            float prevPos;

            for (; first.Body.X + first.Body.Width + delta < second.Body.X ;)
            {
                prevPos = first.Body.X;
                first.Move(delta, second);

                first.Body.X.Should().Be(prevPos + delta);
            }

        }

        [Test]
        public void TestWhenOpponentInFrontOf()
        {
            const int width = 2000;
            var setting = new GameSettings(width, 2000);
            var first = new Paladin("paladin", new PointF(200, 1000));
            var second = new Skeleton("skeleton", new PointF(1000, 1000));
            const int delta = 10;
            float prevPos;

            first.Move((int)(second.Body.X - first.Body.X - first.Body.Width - 1), second);
            for (var i = 0; i < delta * 10; i += delta)
            {
                prevPos = first.Body.X;
                first.Move(delta, second);

                first.Body.X.Should().Be(prevPos);
            }
        }

        [Test]
        public void TestWithScreenBorders()
        {
            const int width = 2000;
            var setting = new GameSettings(width, 2000);
            var first = new Paladin("paladin", new PointF(200, 1000));
            var second = new Skeleton("skeleton", new PointF(1000, 1000));
            const int delta = 10;
            float prevPos;

            first.Move((int)-first.Body.X + 1, second);
            prevPos = first.Body.X;
            first.Move(-delta, second);

            first.Body.X.Should().Be(prevPos);

            second.Move((int)(width - second.Body.X - second.Body.Width - 1), first);
            prevPos = second.Body.X;
            second.Move(delta, first);

            second.Body.X.Should().Be(prevPos);
        }
    }
}
