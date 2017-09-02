using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikeHelper
{
    public class MikeCypher
    {
        public static string Crypt(int skip, string str)
        {
            string lhs = "";
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                for (int i = 0; i < str.Length; ++i)
                {
                    char ch = str[i];
                    int num = ch;
                    if (num >= 32 && num <= 126)
                        ch = (char)((uint)Repeat((num - 32 + skip), 95) + 32);

                    lhs += ch;
                }
            }
            return lhs;
        }

        // Mathf.Repeat from UnityEngine
        private static int Repeat(int i, int j)
        {
            return (j + i % j) % j;
        }
    }
}
