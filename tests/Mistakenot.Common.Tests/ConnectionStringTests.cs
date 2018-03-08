using System;
using Xunit;

namespace Mistakenot.Common.Tests
{
    public class ConnectionStringTests
    {
        [Fact]
        public void ConnectionString_FromUri_ParsesOk()
        {
            var uri = "postgres://postgres:test@localhost:5432/db";
            var actual = ConnectionString.FromPostgresUri(uri);
            var expected = "Host=localhost;Port=5432;Database=db;Username=postgres;Password=test;";
            
            Assert.Equal(expected, actual);
        }
    }
}
