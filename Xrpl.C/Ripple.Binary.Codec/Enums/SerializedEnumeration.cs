﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Binary;

namespace Ripple.Binary.Codec.Enums
{
    public abstract class SerializedEnumeration<TEnum, TOrd> : Enumeration<TEnum> 
        where TEnum : SerializedEnumItem<TOrd>
        where TOrd : struct, IConvertible
    {
        protected SerializedEnumeration() => Width = Marshal.SizeOf(default(TOrd));

        public int Width { get;}

        public TEnum FromParser(BinaryParser parser, int? hint = null) => this[ReadOrdinal(parser)];

        public TEnum FromJson(JToken value) 
            => value.Type == JTokenType.String 
                ? this[value.ToString()] 
                : this[(int) value];

        public int ReadOrdinal(BinaryParser parser) => parser.Read(Width).Aggregate(0, (a, b) => (a >> 8) + b);
    }
}