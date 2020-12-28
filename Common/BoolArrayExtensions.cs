using System;

namespace AoC2019.Common
{
    public static class BoolArrayExtensions
    {
        public static short ConvertToShort(this bool[] bits)
        {
            if (bits == null)
            {
                return 0;
            }

            if (bits.Length > 16)
            {
                throw new ArgumentException("Bool array is too large for a short.");
            }

            short value = 0;
            for (var i = 16 - bits.Length; i < 16; i++)
            {
                if (bits[i - (16 - bits.Length)])
                {
                    value |= (short)(1 << (15 - i));
                }
            }

            return value;
        }

        public static int ConvertToInt32(this bool[] bits)
        {
            if (bits == null)
            {
                return 0;
            }

            if (bits.Length > 32)
            {
                throw new ArgumentException("Bool array is too large for an 32-bits int.");
            }

            int value = 0;
            for (var i = 32 - bits.Length; i < 32; i++)
            {
                if (bits[i - (32 - bits.Length)])
                {
                    value |= (int)(1 << (31 - i));
                }
            }

            return value;
        }
    }
}