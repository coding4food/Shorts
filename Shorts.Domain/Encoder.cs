﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shorts.Domain
{
    public static class Encoder
    {
        private static readonly string symbols;

        static Encoder()
        {
            symbols = new string(Enumerable.Range(0, 10).Select(n => (char)('0' + n)).ToArray())
                + new string(Enumerable.Range(0, 26).Select(n => (char)('A' + n)).ToArray())
                + new string(Enumerable.Range(0, 26).Select(n => (char)('a' + n)).ToArray());
        }

        public static string Encode(long value)
        {
            var result = "";

            while (value > 0)
            {
                result = symbols[(int)(value % (long)symbols.Length)] + result;

                value /= (long)symbols.Length;
            }

            return result;
        }

        public static long Decode(string value)
        {
            long result = 0;

            foreach (var n in value.Select(c => symbols.IndexOf(c)))
            {
                result = result * symbols.Length + n;
            }

            return result;
        }
    }
}
