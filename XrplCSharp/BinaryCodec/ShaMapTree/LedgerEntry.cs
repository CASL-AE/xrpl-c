﻿using Xrpl.BinaryCodecLib.Binary;
using Xrpl.BinaryCodecLib.Enums;
using Xrpl.BinaryCodecLib.Hashing;
using Xrpl.BinaryCodecLib.Types;

namespace Xrpl.BinaryCodecLib.ShaMapTree
{
    public class LedgerEntry : IShaMapItem<LedgerEntry>
    {
        public readonly StObject Entry;

        public LedgerEntry(StObject entry)
        {
            Entry = entry;
        }

        public void ToBytes(BytesList sink)
        {
            Entry.ToBytes(sink);
        }

        public IShaMapItem<LedgerEntry> Copy()
        {
            return this;
        }

        public LedgerEntry Value()
        {
            return this;
        }

        public HashPrefix Prefix()
        {
            return HashPrefix.LeafNode;
        }

        public Hash256 Index()
        {
            return (Hash256)Entry[Field.index];
        }
    }
    public static class LedgerEntryReader
    {
        public static LedgerEntry ReadLedgerEntry(this StReader reader)
        {
            var index = reader.ReadHash256();
            var obj = reader.ReadVlStObject();
            obj[Field.index] = index;
            return new LedgerEntry(obj);
        }
    }
}
