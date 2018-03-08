using System;
using System.Text;

namespace Mistakenot.Common
{
    public static class ConnectionString
    {
        public static string FromPostgresUri(string uriString)
        {
            var uri = new Uri(uriString);
            var builder = new StringBuilder();

            builder
                .Append($"Host={uri.Host};")
                .Append($"Port={uri.Port};")
                .Append($"Database={uri.LocalPath.Substring(1)};")
                .Append($"Username={uri.UserInfo.Split(':')[0]};")
                .Append($"Password={uri.UserInfo.Split(':')[1]};");

            return builder.ToString();
        }
    }
}
