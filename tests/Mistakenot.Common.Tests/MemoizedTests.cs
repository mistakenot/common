﻿using System;
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
                else
                {
                    return "not ok";
                }
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