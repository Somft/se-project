using System;

namespace ExBook.Extensions
{
    public static class EnvironmentUtils
    {
        public static string GetMachineIdentifier()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}
