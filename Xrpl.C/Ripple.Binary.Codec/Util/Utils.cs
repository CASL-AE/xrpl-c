﻿using System;
using System.Linq;

namespace Ripple.Binary.Codec.Util
{
    internal static class Utils
    {
        public static byte[] ZeroPad(byte[] arr, int len)
        {
            if (arr.Length > len)
            {
                throw new InvalidOperationException(
                    $"Existing length of {arr.Length} > " +
                    $"desired length of {len}");
            }
            if (arr.Length == len)
            {
                return arr;
            } 
            var extended = new byte[len];
            var startingAt = len - arr.Length;
            Array.Copy(arr, 0, extended, startingAt, arr.Length);
            return extended;
        }
    }

    /// <summary>
    ///     Converts base data types to an array of bytes, and an array of bytes to base
    ///     data types.
    ///     All info taken from the meta data of System.BitConverter. This implementation
    ///     allows for Endianness consideration.
    ///</summary>
    public static class Bits
    {
        /// <summary>
        ///     Indicates the byte order ("endianess") in which data is stored in this computer
        ///     architecture.
        ///</summary>
        /// 
        public static bool IsLittleEndian { get; set; } = false;

        /// <summary>
        ///     Converts the specified double-precision floating point number to a 64-bit
        ///     signed integer.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     A 64-bit signed integer whose value is equivalent to value.
        ///</summary>
        public static long DoubleToInt64Bits(double value) { throw new NotImplementedException(); }
        ///
        /// <summary>
        ///     Returns the specified Boolean value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     A Boolean value.
        ///
        /// Returns:
        ///     An array of bytes with length 1.
        ///</summary>
        public static byte[] GetBytes(bool value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        public static byte[] GetBytes(byte value) => new[] {value};

        ///
        /// <summary>
        ///     Returns the specified Unicode character value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     A character to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 2.
        ///</summary>
        public static byte[] GetBytes(char value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified double-precision floating point value as an array of
        ///     bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 8.
        ///</summary>
        public static byte[] GetBytes(double value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified single-precision floating point value as an array of
        ///     bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 4.
        ///</summary>
        public static byte[] GetBytes(float value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 32-bit signed integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 4.
        ///</summary>
        public static byte[] GetBytes(int value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 64-bit signed integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 8.
        ///</summary>
        public static byte[] GetBytes(long value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 16-bit signed integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 2.
        ///</summary>
        public static byte[] GetBytes(short value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 32-bit unsigned integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 4.
        ///</summary>
        
        public static byte[] GetBytes(uint value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 64-bit unsigned integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 8.
        ///</summary>
        
        public static byte[] GetBytes(ulong value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Returns the specified 16-bit unsigned integer value as an array of bytes.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     An array of bytes with length 2.
        ///</summary>
        public static byte[] GetBytes(ushort value) => IsLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();

        ///
        /// <summary>
        ///     Converts the specified 64-bit signed integer to a double-precision floating
        ///     point number.
        ///
        /// Parameters:
        ///   value:
        ///     The number to convert.
        ///
        /// Returns:
        ///     A double-precision floating point number whose value is equivalent to value.
        ///</summary>
        public static double Int64BitsToDouble(long value) { throw new NotImplementedException(); }
        ///
        /// <summary>
        ///     Returns a Boolean value converted from one byte at a specified position in
        ///     a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     true if the byte at startIndex in value is nonzero; otherwise, false.
        ///
        /// Exceptions:
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static bool ToBoolean(byte[] value, int startIndex) { throw new NotImplementedException(); }
        ///
        /// <summary>
        ///     Returns a Unicode character converted from two bytes at a specified position
        ///     in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A character formed by two bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex equals the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static char ToChar(byte[] value, int startIndex) { throw new NotImplementedException(); }
        ///
        /// <summary>
        ///     Returns a double-precision floating point number converted from eight bytes
        ///     at a specified position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A double precision floating point number formed by eight bytes beginning
        ///     at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 7, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static double ToDouble(byte[] value, int startIndex) { throw new NotImplementedException(); }
        ///
        /// <summary>
        ///     Returns a 16-bit signed integer converted from two bytes at a specified position
        ///     in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 16-bit signed integer formed by two bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex equals the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static short ToInt16(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToInt16(value, startIndex) : BitConverter.ToInt16(value.Reverse().ToArray(), value.Length - sizeof(Int16) - startIndex);

        ///
        /// <summary>
        ///     Returns a 32-bit signed integer converted from four bytes at a specified
        ///     position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 32-bit signed integer formed by four bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 3, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static int ToInt32(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToInt32(value, startIndex) : BitConverter.ToInt32(value.Reverse().ToArray(), value.Length - sizeof(Int32) - startIndex);

        ///
        /// <summary>
        ///     Returns a 64-bit signed integer converted from eight bytes at a specified
        ///     position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 64-bit signed integer formed by eight bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 7, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static long ToInt64(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToInt64(value, startIndex) : BitConverter.ToInt64(value.Reverse().ToArray(), value.Length - sizeof(Int64) - startIndex);

        ///
        /// <summary>
        ///     Returns a single-precision floating point number converted from four bytes
        ///     at a specified position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A single-precision floating point number formed by four bytes beginning at
        ///     startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 3, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static float ToSingle(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToSingle(value, startIndex) : BitConverter.ToSingle(value.Reverse().ToArray(), value.Length - sizeof(Single) - startIndex);

        ///
        /// <summary>
        ///     Converts the numeric value of each element of a specified array of bytes
        ///     to its equivalent hexadecimal string representation.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        /// Returns:
        ///     A System.String of hexadecimal pairs separated by hyphens, where each pair
        ///     represents the corresponding element in value; for example, "7F-2C-4A".
        ///
        /// Exceptions:
        ///   System.ArgumentNullException:
        ///     value is null.
        ///</summary>
        public static string ToString(byte[] value) => BitConverter.ToString(IsLittleEndian ? value : value.Reverse().ToArray());

        ///
        /// <summary>
        ///     Converts the numeric value of each element of a specified subarray of bytes
        ///     to its equivalent hexadecimal string representation.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A System.String of hexadecimal pairs separated by hyphens, where each pair
        ///     represents the corresponding element in a subarray of value; for example,
        ///     "7F-2C-4A".
        ///
        /// Exceptions:
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static string ToString(byte[] value, int startIndex) => BitConverter.ToString(IsLittleEndian ? value : value.Reverse().ToArray(), startIndex);

        ///
        /// <summary>
        ///     Converts the numeric value of each element of a specified subarray of bytes
        ///     to its equivalent hexadecimal string representation.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        ///   length:
        ///     The number of array elements in value to convert.
        ///
        /// Returns:
        ///     A System.String of hexadecimal pairs separated by hyphens, where each pair
        ///     represents the corresponding element in a subarray of value; for example,
        ///     "7F-2C-4A".
        ///
        /// Exceptions:
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex or length is less than zero.  -or- startIndex is greater than
        ///     zero and is greater than or equal to the length of value.
        ///
        ///   System.ArgumentException:
        ///     The combination of startIndex and length does not specify a position within
        ///     value; that is, the startIndex parameter is greater than the length of value
        ///     minus the length parameter.
        ///</summary>
        public static string ToString(byte[] value, int startIndex, int length) => BitConverter.ToString(IsLittleEndian ? value : value.Reverse().ToArray(), startIndex, length);

        ///
        /// <summary>
        ///     Returns a 16-bit unsigned integer converted from two bytes at a specified
        ///     position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     The array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 16-bit unsigned integer formed by two bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex equals the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static ushort ToUInt16(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToUInt16(value, startIndex) : BitConverter.ToUInt16(value.Reverse().ToArray(), value.Length - sizeof(UInt16) - startIndex);

        ///
        /// <summary>
        ///     Returns a 32-bit unsigned integer converted from four bytes at a specified
        ///     position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 32-bit unsigned integer formed by four bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 3, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static uint ToUInt32(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToUInt32(value, startIndex) : BitConverter.ToUInt32(value.Reverse().ToArray(), value.Length - sizeof(UInt32) - startIndex);

        ///
        /// <summary>
        ///     Returns a 64-bit unsigned integer converted from eight bytes at a specified
        ///     position in a byte array.
        ///
        /// Parameters:
        ///   value:
        ///     An array of bytes.
        ///
        ///   startIndex:
        ///     The starting position within value.
        ///
        /// Returns:
        ///     A 64-bit unsigned integer formed by the eight bytes beginning at startIndex.
        ///
        /// Exceptions:
        ///   System.ArgumentException:
        ///     startIndex is greater than or equal to the length of value minus 7, and is
        ///     less than or equal to the length of value minus 1.
        ///
        ///   System.ArgumentNullException:
        ///     value is null.
        ///
        ///   System.ArgumentOutOfRangeException:
        ///     startIndex is less than zero or greater than the length of value minus 1.
        ///</summary>
        public static ulong ToUInt64(byte[] value, int startIndex) => IsLittleEndian ? BitConverter.ToUInt64(value, startIndex) : BitConverter.ToUInt64(value.Reverse().ToArray(), value.Length - sizeof(UInt64) - startIndex);
    }
    public static class ChannelUtils
    {
        public static byte[] EncodeChannel(string channelHex, long amount)
        {
            byte[] HASH_CHANNEL_SIGN = { 0x43, 0x4C, 0x4D, 0x00 };
            var AMOUNT_HEX_ARRAY = Bits.GetBytes(amount);
            var CHANNEL_HEX_ARRAY = Convert.FromHexString(channelHex);

            var rv = new byte[HASH_CHANNEL_SIGN.Length + CHANNEL_HEX_ARRAY.Length + AMOUNT_HEX_ARRAY.Length];
            Buffer.BlockCopy(HASH_CHANNEL_SIGN, 0, rv, 0, HASH_CHANNEL_SIGN.Length);
            Buffer.BlockCopy(CHANNEL_HEX_ARRAY, 0, rv, HASH_CHANNEL_SIGN.Length, CHANNEL_HEX_ARRAY.Length);
            Buffer.BlockCopy(AMOUNT_HEX_ARRAY, 0, rv, HASH_CHANNEL_SIGN.Length + CHANNEL_HEX_ARRAY.Length, AMOUNT_HEX_ARRAY.Length);
            return rv;
        }
    }
}