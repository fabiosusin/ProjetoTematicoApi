using System;

namespace Useful.Extensions
{
    public static class NumberExtension
    {
        public static int GetDiff(int set, int set2) => set > set2 ? set - set2 : set2 - set;
        
        public static int RandomNumber(int lenght)
        {
            var min = "1";
            var max = "9";
            for (var i = 1; i < lenght; i++) { min += "0"; max += "9"; }

            return RandomNumber(int.Parse(min), int.Parse(max));
        }

        public static int RandomNumber(int min, int max) => new Random().Next(min, max);
    }
}
