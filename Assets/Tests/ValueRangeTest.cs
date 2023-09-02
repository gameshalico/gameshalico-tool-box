using NUnit.Framework;
using Shalico.ToolBox;

namespace Tests
{
    public class ValueRangeTest
    {
        [Test]
        public void TestContains()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.IsTrue(range.Contains(0));
            Assert.IsTrue(range.Contains(5));
            Assert.IsTrue(range.Contains(10));
            Assert.IsFalse(range.Contains(-1));
            Assert.IsFalse(range.Contains(11));
        }

        [Test]
        public void TestContainsRange()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.IsTrue(range.Contains(new ValueRange<int>(0, 10)));
            Assert.IsTrue(range.Contains(new ValueRange<int>(0, 5)));
            Assert.IsTrue(range.Contains(new ValueRange<int>(5, 10)));
            Assert.IsFalse(range.Contains(new ValueRange<int>(-1, 10)));
            Assert.IsFalse(range.Contains(new ValueRange<int>(0, 11)));
        }

        [Test]
        public void TestOverlaps()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.IsTrue(range.Overlaps(new ValueRange<int>(0, 10)));
            Assert.IsTrue(range.Overlaps(new ValueRange<int>(0, 5)));
            Assert.IsTrue(range.Overlaps(new ValueRange<int>(5, 10)));
            Assert.IsTrue(range.Overlaps(new ValueRange<int>(-1, 10)));
            Assert.IsTrue(range.Overlaps(new ValueRange<int>(0, 11)));
            Assert.IsFalse(range.Overlaps(new ValueRange<int>(-1, -1)));
            Assert.IsFalse(range.Overlaps(new ValueRange<int>(11, 11)));
        }

        [Test]
        public void TestClamp()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.AreEqual(0, range.Clamp(-1));
            Assert.AreEqual(0, range.Clamp(0));
            Assert.AreEqual(5, range.Clamp(5));
            Assert.AreEqual(10, range.Clamp(10));
            Assert.AreEqual(10, range.Clamp(11));
        }

        [Test]
        public void TestUnion()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Union(new ValueRange<int>(0, 10)));
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Union(new ValueRange<int>(0, 5)));
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Union(new ValueRange<int>(5, 10)));
            Assert.AreEqual(new ValueRange<int>(-1, 10), range.Union(new ValueRange<int>(-1, 10)));
            Assert.AreEqual(new ValueRange<int>(0, 11), range.Union(new ValueRange<int>(0, 11)));
            Assert.AreEqual(new ValueRange<int>(-1, 11), range.Union(new ValueRange<int>(-1, 11)));
        }

        [Test]
        public void TestIntersect()
        {
            var range = new ValueRange<int>(0, 10);
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Intersect(new ValueRange<int>(0, 10)));
            Assert.AreEqual(new ValueRange<int>(0, 5), range.Intersect(new ValueRange<int>(0, 5)));
            Assert.AreEqual(new ValueRange<int>(5, 10), range.Intersect(new ValueRange<int>(5, 10)));
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Intersect(new ValueRange<int>(-1, 10)));
            Assert.AreEqual(new ValueRange<int>(0, 10), range.Intersect(new ValueRange<int>(0, 11)));
            Assert.AreEqual(new ValueRange<int>(0, 0), range.Intersect(new ValueRange<int>(-1, -1)));
            Assert.AreEqual(new ValueRange<int>(0, 0), range.Intersect(new ValueRange<int>(11, 11)));
        }


        [Test]
        public void TestSubtract()
        {
            var range = new ValueRange<int>(0, 10);

            var a = range.Subtract(new ValueRange<int>(0, 10));
            Assert.AreEqual(0, a.Length);

            var b = range.Subtract(new ValueRange<int>(0, 5));
            Assert.AreEqual(1, b.Length);
            Assert.AreEqual(new ValueRange<int>(5, 10), b[0]);

            var c = range.Subtract(new ValueRange<int>(5, 10));
            Assert.AreEqual(1, c.Length);
            Assert.AreEqual(new ValueRange<int>(0, 5), c[0]);

            var d = range.Subtract(new ValueRange<int>(-1, 10));
            Assert.AreEqual(0, d.Length);

            var e = range.Subtract(new ValueRange<int>(0, 11));
            Assert.AreEqual(0, e.Length);

            var f = range.Subtract(new ValueRange<int>(-1, -1));
            Assert.AreEqual(1, f.Length);
            Assert.AreEqual(new ValueRange<int>(0, 10), f[0]);

            var g = range.Subtract(new ValueRange<int>(11, 11));
            Assert.AreEqual(1, g.Length);
            Assert.AreEqual(new ValueRange<int>(0, 10), g[0]);

            var h = range.Subtract(new ValueRange<int>(-1, 11));
            Assert.AreEqual(0, h.Length);

            var i = range.Subtract(new ValueRange<int>(-1, 5));
            Assert.AreEqual(1, i.Length);
            Assert.AreEqual(new ValueRange<int>(5, 10), i[0]);

            var j = range.Subtract(new ValueRange<int>(5, 11));
            Assert.AreEqual(1, j.Length);
            Assert.AreEqual(new ValueRange<int>(0, 5), j[0]);

            var k = range.Subtract(new ValueRange<int>(-1, 0));
            Assert.AreEqual(1, k.Length);
            Assert.AreEqual(new ValueRange<int>(0, 10), k[0]);

            var l = range.Subtract(new ValueRange<int>(10, 11));
            Assert.AreEqual(1, l.Length);
            Assert.AreEqual(new ValueRange<int>(0, 10), l[0]);

            var m = range.Subtract(new ValueRange<int>(1, 9));
            Assert.AreEqual(2, m.Length);
            Assert.AreEqual(new ValueRange<int>(0, 1), m[0]);
            Assert.AreEqual(new ValueRange<int>(9, 10), m[1]);
        }
    }
}