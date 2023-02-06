using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplicationForDidacticPurpose.BL.Interfaces;

namespace WebApplicationForDidacticPurpose.BL.Services
{
    public class Compare2StringsService : ICompare2StringsService
    {

        public int Calculate(string x, string y)
        {
            int[,] dp = new int[x.Length + 1, y.Length + 1];

            for (int i = 0; i <= x.Length; i++)
            {
                for (int j = 0; j <= y.Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else
                    {
                        dp[i, j] = Min(dp[i - 1, j - 1] + CostOfSubstitution(Convert.ToChar(x[i - 1]), Convert.ToChar(y[j - 1])),
                                dp[i - 1, j] + 1,
                                dp[i, j - 1] + 1);
                    }
                }
            }
            var bias = Math.Max(x.Length, y.Length) / 30.0;
            var stringsLengthDiference = dp[x.Length, y.Length];
            var result = (int)(100 / ((stringsLengthDiference + 1) / (bias + 1)));
            return result;
        }

        private static int CostOfSubstitution(char a, char b)
        {
            return a == b ? 0 : 1;
        }

        private static int Min(int distance1, int distance2, int distance3)
        {
            int[] numbers = { distance1, distance2, distance3 };
            var minimumValue = numbers.Min();
            if (numbers.Length.Equals(0))
            {
                return Int32.MaxValue;
            }
            return minimumValue;

        }
    }
}

