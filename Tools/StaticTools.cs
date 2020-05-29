using System;

namespace Factor.Tools
{
    public static class StaticTools
    {
        public static long GenerateCode()
        {
            Random random = new Random();
            return random.Next(1111, 9999);
        }
    }
}
