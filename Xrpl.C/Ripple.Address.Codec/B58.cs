using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Ripple.Address.Codec
{
    public class B58
    {
        private char[] _mAlphabet;

        private int[] _mIndexes;

        public B58(string alphabet)
        {
            SetAlphabet(alphabet);
            BuildIndexes();
        }

        public byte[] Decode(string input, Version version)
        {
            var buffer = DecodeAndCheck(input);
            return ExtractPayload(version, buffer);
        }

        private static byte[] ExtractPayload(Version version, byte[] buffer)
        {
            var expectedPayloadLen = version.ExpectedLength;
            var expectedVerLen = version.VersionBytes.Length;
            var expectedTotLen = expectedPayloadLen + expectedVerLen;
            var payloadEnd = buffer.Length - 4;
            if (expectedTotLen == payloadEnd)
            {
                var actualVersion = CopyOfRange(buffer, 0, expectedVerLen);
                if (ArrayEquals(actualVersion, version.VersionBytes))
                {
                    var payload = CopyOfRange(buffer, actualVersion.Length, payloadEnd);
                    return payload;
                }
                throw new EncodingFormatException("Version invalid");
            }
            throw new EncodingFormatException(
                $"Expected version + payload length was {expectedTotLen} " +
                                $"but actual length was {payloadEnd}");
        }

        public Decoded Decode(string input, Versions versions)
        {
            var buffer = DecodeAndCheck(input);
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < versions.VersionsArray.Length; i++)
            {
                var versionName = versions.NamesArray[i];
                var version = versions.VersionsArray[i];
                var expectedPayloadLen = version.ExpectedLength;
                var expectedVerLen = version.VersionBytes.Length;
                var expectedTotLen = expectedPayloadLen + expectedVerLen;
                var payloadEnd = buffer.Length - 4;
                if (expectedTotLen != payloadEnd) continue;
                var actualVersion = CopyOfRange(buffer, 0, expectedVerLen);
                if (!ArrayEquals(actualVersion, version.VersionBytes)) continue;
                var payload = CopyOfRange(buffer, actualVersion.Length, payloadEnd);
                return new Decoded(version.VersionBytes, payload, versionName);
            }
            throw new EncodingFormatException("No version matched amongst " +
                                $"{string.Join(", ", versions.NamesArray)}");
        }

        public byte[] Decode(string input)
        {
            if (input.Length == 0)
            {
                return Array.Empty<byte>();
            }
            var input58 = new byte[input.Length];
            // Transform the String to a base58 byte sequence
            for (var i = 0; i < input.Length; ++i)
            {
                var c = input[i];

                var digit58 = -1;
                if (c >= 0 && c < 128)
                {
                    digit58 = _mIndexes[c];
                }
                if (digit58 < 0)
                {
                    throw new EncodingFormatException("Illegal character " + c + " at " + i);
                }

                input58[i] = (byte)digit58;
            }
            // Count leading zeroes
            var zeroCount = 0;
            while (zeroCount < input58.Length && input58[zeroCount] == 0)
            {
                ++zeroCount;
            }
            // The encoding
            var temp = new byte[input.Length];
            var j = temp.Length;

            var startAt = zeroCount;
            while (startAt < input58.Length)
            {
                var mod = DivMod256(input58, startAt);
                if (input58[startAt] == 0)
                {
                    ++startAt;
                }

                temp[--j] = mod;
            }
            // Do no add extra leading zeroes, move j to first non null byte.
            while (j < temp.Length && temp[j] == 0)
            {
                ++j;
            }

            return CopyOfRange(temp, j - zeroCount, temp.Length);
        }

        public string Encode(byte[] buffer, Version version)
        {
            CheckLength(version, buffer);
            return EncodeToStringChecked(buffer, version.VersionBytes);
        }

        public string Encode(byte[] buffer, string versionName, Versions versions)
        {
            return Encode(buffer, versions.Find(versionName));
        }

        /// <summary>
        /// Encodes the given bytes in base58. No checksum is appended.
        /// </summary>
        public byte[] EncodeToBytes(byte[] input)
        {
            if (input.Length == 0)
            {
                return Array.Empty<byte>();
            }
            input = CopyOfRange(input, 0, input.Length);
            // Count leading zeroes.
            var zeroCount = 0;
            while (zeroCount < input.Length && input[zeroCount] == 0)
            {
                ++zeroCount;
            }
            // The actual encoding.
            var temp = new byte[input.Length * 2];
            var j = temp.Length;

            var startAt = zeroCount;
            while (startAt < input.Length)
            {
                var mod = DivMod58(input, startAt);
                if (input[startAt] == 0)
                {
                    ++startAt;
                }
                temp[--j] = (byte)_mAlphabet[mod];
            }

            // Strip extra '1' if there are some after decoding.
            while (j < temp.Length && temp[j] == _mAlphabet[0])
            {
                ++j;
            }
            // Add as many leading '1' as there were leading zeros.
            while (--zeroCount >= 0)
            {
                temp[--j] = (byte)_mAlphabet[0];
            }

            var output = CopyOfRange(temp, j, temp.Length);
            return output;
        }

        public byte[] EncodeToBytesChecked(byte[] input, int version) => EncodeToBytesChecked(input, new[] { (byte)version });

        public byte[] EncodeToBytesChecked(byte[] input, byte[] version)
        {
            var buffer = new byte[input.Length + version.Length];
            Array.Copy(version, 0, buffer, 0, version.Length);
            Array.Copy(input, 0, buffer, version.Length, input.Length);
            var checkSum = CopyOfRange(HashUtils.DoubleDigest(buffer), 0, 4);
            var output = new byte[buffer.Length + checkSum.Length];
            Array.Copy(buffer, 0, output, 0, buffer.Length);
            Array.Copy(checkSum, 0, output, buffer.Length, checkSum.Length);
            return EncodeToBytes(output);
        }

        public string EncodeToString(byte[] input)
        {
            var output = EncodeToBytes(input);
            return Encoding.ASCII.GetString(output);
        }

        public string EncodeToStringChecked(byte[] input, int version) => EncodeToStringChecked(input, new[] { (byte)version });

        public string EncodeToStringChecked(byte[] input, byte[] version) => Encoding.ASCII.GetString(EncodeToBytesChecked(input, version));

        public byte[] FindPrefix(int payLoadLength, string desiredPrefix)
        {
            var totalLength = payLoadLength + 4; // for the checksum
            var chars = Math.Log(Math.Pow(256, totalLength)) / Math.Log(58);
            var requiredChars = (int)Math.Ceiling(chars + 0.2D);
            // Mess with this to see stability tests fail
            var charPos = (_mAlphabet.Length / 2) - 1;
            var padding = _mAlphabet[(charPos)];
            var template = desiredPrefix + Repeat(requiredChars, padding);
            var decoded = Decode(template);
            return CopyOfRange(decoded, 0, decoded.Length - totalLength);
        }

        public bool IsValid(string input, Version version)
        {
            try
            {
                Decode(input, version);
                return true;
            }
            catch (EncodingFormatException)
            {
                return false;
            }
        }

        public bool IsValid(string input, Versions version)
        {
            try
            {
                Decode(input, version);
                return true;
            }
            catch (EncodingFormatException)
            {
                return false;
            }
        }

        internal static bool ArrayEquals(
            IReadOnlyCollection<byte> a,
            IReadOnlyList<byte> b)
        {
            if (a.Count != b.Count) return false;
            return !a.Where((t, i) => t != b[i]).Any();
        }

        private static void CheckLength(Version version, byte[] buffer)
        {
            if (version.ExpectedLength != buffer.Length)
            {
                throw new EncodingFormatException("version has expected " +
                                    $"length of {version.ExpectedLength}");
            }
        }

        private static byte[] CopyOfRange(byte[] source, int from_, int to)
        {
            var range = new byte[to - from_];
            Array.Copy(source, from_, range, 0, range.Length);
            return range;
        }

        //
        // number -> number / 256, returns number % 256
        //
        private static byte DivMod256(IList<byte> number58, int startAt)
        {
            var remainder = 0;
            for (var i = startAt; i < number58.Count; i++)
            {
                var digit58 = number58[i] & 0xFF;
                var temp = remainder * 58 + digit58;

                number58[i] = (byte)(temp / 256);

                remainder = temp % 256;
            }

            return (byte)remainder;
        }

        //
        // number -> number / 58, returns number % 58
        //
        private static byte DivMod58(IList<byte> number, int startAt)
        {
            var remainder = 0;
            for (var i = startAt; i < number.Count; i++)
            {
                var digit256 = number[i] & 0xFF;
                var temp = remainder * 256 + digit256;

                number[i] = (byte)(temp / 58);

                remainder = temp % 58;
            }

            return (byte)remainder;
        }

        private static string Repeat(int times, char repeated)
        {
            var chars = new char[times];
            for (var i = 0; i < times; i++)
            {
                chars[i] = repeated;
            }
            return new string(chars);
        }

        private void BuildIndexes()
        {
            _mIndexes = new int[128];

            for (var i = 0; i < _mIndexes.Length; i++)
            {
                _mIndexes[i] = -1;
            }
            for (var i = 0; i < _mAlphabet.Length; i++)
            {
                _mIndexes[_mAlphabet[i]] = i;
            }
        }

        private byte[] DecodeAndCheck(string input)
        {
            var buffer = Decode(input);
            if (buffer.Length < 4)
            {
                throw new EncodingFormatException("Input too short");
            }

            var toHash = CopyOfRange(buffer, 0, buffer.Length - 4);
            var hashed = CopyOfRange(HashUtils.DoubleDigest(toHash), 0, 4);
            var checksum = CopyOfRange(buffer, buffer.Length - 4, buffer.Length);

            if (!ArrayEquals(checksum, hashed))
            {
                throw new EncodingFormatException("Checksum does not validate");
            }
            return buffer;
        }

        private void SetAlphabet(string alphabet)=> _mAlphabet = alphabet.ToCharArray();

        public class Decoded
        {
            public readonly byte[] Payload;
            public readonly string Type;
            public readonly byte[] VersionBytes;

            public Decoded(byte[] version, byte[] payload, string typeName)
            {
                VersionBytes = version;
                Payload = payload;
                Type = typeName;
            }
        }

        public class Version
        {
            public int ExpectedLength;
            public byte[] VersionBytes;
            public Version(byte[] versionBytes, int expectedLength)
            {
                VersionBytes = versionBytes;
                ExpectedLength = expectedLength;
            }

            public static Version With(byte versionByte, int expectedLength) => With(new []{ versionByte}, expectedLength);

            public static Version With(byte[] versionBytes, int expectedLength) => new(versionBytes, expectedLength);
        }

        public class Versions
        {
            public string[] NamesArray;
            public Version[] VersionsArray;
            public Versions(Version[] versionsArray, string[] namesArray)
            {
                VersionsArray = versionsArray;
                NamesArray = namesArray;
            }

            public static Versions With(string typeName, Version version) => new(new[] { version }, new[] { typeName });

            public Versions And(string typeName, Version version) =>
                new(
                    new[] { version }.Concat(VersionsArray).ToArray(),
                    new[] { typeName }.Concat(NamesArray).ToArray());

            public Version Find(string sought)
            {
                for (var i = 0; i < NamesArray.Length; i++)
                {
                    var versionName = NamesArray[i];
                    if (sought == versionName)
                    {
                        return VersionsArray[i];
                    }
                }
                throw new InvalidOperationException($"Can't find version with name {sought}");
            }
        }
    }

    [Serializable]
    public class EncodingFormatException : FormatException
    {
        public EncodingFormatException()
        {
        }

        public EncodingFormatException(string message) : base(message)
        {
        }

        public EncodingFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EncodingFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    internal class HashUtils
    {
        internal static byte[] DoubleDigest(byte[] buffer) => Sha256(Sha256(buffer));

        internal static byte[] Sha256(byte[] buffer)
        {
            var hash = SHA256.Create();
            return hash.ComputeHash(buffer);
        }
    }
}