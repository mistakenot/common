using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Mistakenot.Common.Tests
{
    public class PersistentDictionaryTests
    {
        [Fact]
        public void PersistentDictionary_WritesSingleValue_Ok()
        {
            var value = DateTime.UtcNow.ToString();
            var dict = new PersistentDictionary<string, string>(
                "test_dir", 
                s => s,
                s => s,
                s => s);

            dict["test_key"] = value;

            var actual = dict["test_key"];

            Assert.Equal(value, actual);
        }
    }
}