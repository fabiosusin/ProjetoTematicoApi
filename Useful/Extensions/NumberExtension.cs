using System;

namespace Useful.Extensions
{
    public static class NumberExtension
    {
        public static int GetDiff(int set, int set2) => set > set2 ? set - set2 : set2 - set;

    }
}
