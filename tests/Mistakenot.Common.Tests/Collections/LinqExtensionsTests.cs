using Xunit;
using Mistakenot.Common.Collections;
using System.Linq;

namespace Mistakenot.Common.Tests.Collections
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void LinqExtensions_DistinctBy_NonEmpty()
        {
            var items = new int [] {1,2,2,3,4,4,5};
            var actual = items.DistinctBy(i => i + 1);

            Assert.Equal(new [] {1,2,3,4,5}, actual);
        }

        [Fact]
        public void LinqExtensions_DistinctBy_Empty()
        {
            var items = new int [0];
            var actual = items.DistinctBy(i => i);

            Assert.Equal(new int [0], actual.ToArray());
        }

        [Fact]
        public void LinqExtensions_Except_Found()
        {
            var items = new [] {1,2,3};
            var actual = items.Except(1);

            Assert.Equal(new [] {2,3}, actual.ToArray());
        }

        [Fact]
        public void LinqExtensions_Except_NotFound()
        {
            var items = new [] {1,2,3};
            var actual = items.Except(4);

            Assert.Equal(new [] {1,2,3}, actual.ToArray());
        }

        [Fact]
        public void LinqExtensions_ExceptBy_Found()
        {
            var items = new [] { new TestClass {Id = 0}, new TestClass {Id = 1}, new TestClass{Id = 2}};
            var actual = items.ExceptBy(1, i => i.Id);

            Assert.Equal(2, actual.Count());
            Assert.Equal(new [] {0, 2}, actual.Select(a => a.Id).ToArray());
        }

        [Fact]
        public void LinqExtensions_ExceptBy_NotFound()
        {
            var items = new [] { new TestClass {Id = 0}, new TestClass {Id = 1}, new TestClass{Id = 2}};
            var actual = items.ExceptBy(3, i => i.Id);

            Assert.Equal(3, actual.Count());
            Assert.Equal(new [] {0,1,2}, actual.Select(a => a.Id).ToArray());
        }

        class TestClass
        {
            public int Id { get; set;}
        }
    }
}