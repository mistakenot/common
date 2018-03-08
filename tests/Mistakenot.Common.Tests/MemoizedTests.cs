using System;
using Xunit;

namespace Mistakenot.Common.Tests
{
    public class MemoizedTests
    {
        private bool _firstTime;
        private Func<object, string> _getString;

        public MemoizedTests()
        {
            _firstTime = true;
            _getString = (parameter) =>
            {
                if (_firstTime)
                {
                    _firstTime = false;
                    return "memoized " + parameter;
                }
                
                throw new Exception("Function not memoized");
            };
        }

        [Fact]
        public void Memoized_Sync_Ok()
        {
            Assert.True(_firstTime);

            // Write to the cache
            var memoized = _getString.Memoized();
            AssertMemoized(memoized);
        }

        [Fact]
        public void Memoized_Async_Ok()
        {
            Assert.True(_firstTime);

            var memoized = _getString.Memoized(threadSafe: true);
            AssertMemoized(memoized);
        }

        [Fact]
        public void Memoized_TwoParamaters()
        {
            Assert.True(_firstTime);

            Func<int, int, int> multiply = (x, y) => 
            {
                if (_firstTime)
                {
                    _firstTime = false;
                    return x * y;
                }

                throw new Exception("Function not memoized");
            };

            var memoized = multiply.Memoized();
            var _ = memoized(2, 3);
            var actual = memoized(2, 3);
            Assert.Equal(2*3, actual);
        }

        private void AssertMemoized(Func<object, string> memoized)
        {
            // Write to the cache
            var _ = memoized("ok");
            Assert.False(_firstTime);

            var actual = memoized("ok");
            Assert.Equal("memoized ok", actual);
        }
    }
}