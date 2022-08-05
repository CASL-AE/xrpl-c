﻿using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;
using Ripple.Binary.Codec.Util;
using System.Diagnostics.Contracts;

namespace Ripple.Binary.Codec.Types
{
    public class Hash256 : Hash
    {
        public static readonly Hash256 Zero = new(new byte[32]);

        public Hash256(byte[] buffer) : base(buffer) => 
            Contract.Assert(buffer.Length == 32, "buffer should be 32 bytes");
        public static Hash256 FromJson(JToken token) => FromHex((string) token);

        public static Hash256 FromHex(string token) => new Hash256(B16.Decode(token));

        public static Hash256 FromParser(BinaryParser parser, int? hint = null) => new Hash256(parser.Read(32));

        public int Nibblet(int depth)
        {
            var byteIx = depth > 0 ? depth / 2 : 0;
            int b = Buffer[byteIx];
            if (depth % 2 == 0)
            {
                b = (b & 0xF0) >> 4;
            }
            else
            {
                b = b & 0x0F;
            }
            return b;
        }
    }
}