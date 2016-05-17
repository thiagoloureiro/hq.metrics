﻿using System;
using System.Security.Cryptography;
using System.Threading;

namespace hq.metrics.Support
{
    /// <summary> Provides statistically relevant random number generation </summary>
    public static class Random
    {
        private static readonly ThreadLocal<RandomNumberGenerator> _random = new ThreadLocal<RandomNumberGenerator>(RandomNumberGenerator.Create);
        
        public static long NextLong()
        {
            var buffer = new byte[sizeof(long)];
            _random.Value.GetBytes(buffer);
            var value = BitConverter.ToInt64(buffer, 0);
            return value;
        }

        public static double NextDouble()
        {
            var l = NextLong();
            if(l == long.MinValue)
            {
                l = 0;
            }
            return (l + .0) / long.MaxValue;
        }
    }
}
