using System;

namespace Mistakenot.Common
{
    public static class ConnectionString
    {
        public static string FromPostgresUri(string uriString)
        {
            var uri = new Uri(uriString);
            return $"Host={uri.Host};Port={uri.Port};Database={uri.LocalPath.Substring(1)};Username={uri.UserInfo.Split(':')[0]};Password={uri.UserInfo.Split(':')[1]}";

        }
    }
}
