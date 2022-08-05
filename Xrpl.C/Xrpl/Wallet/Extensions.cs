﻿using Ripple.Binary.Codec.Hashing;
using Ripple.Binary.Codec.Util;

namespace Xrpl.Wallet
{
    internal static class Extensions
    {
        internal static byte[] Bytes(this HashPrefix hp) => Bits.GetBytes((uint)hp);
    }
}