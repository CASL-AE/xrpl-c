﻿using System.Linq;
using Newtonsoft.Json.Linq;
using Ripple.Binary.Codec.Types;

namespace Ripple.Binary.Codec.ShaMapTree
{
    internal class Versioner
    {
        private int _value;
        public int IncrementAndGet() => ++_value;
    }

    public class ShaMap : ShaMapInner
    {
        private Versioner _copies;
        public ShaMap() : base(0)
        {
            // This way we can copy the first to the second,
            // copy the second, then copy the first again ;)
            _copies = new Versioner();
        }
        protected ShaMap(bool isCopy, int depth) : base(isCopy, depth, 0)
        {

        }
        protected internal override ShaMapInner MakeInnerOfSameClass(int depth) => new ShaMap(true, depth);

        public virtual ShaMap Copy()
        {
            Version = _copies.IncrementAndGet();
            var copy = (ShaMap)Copy(_copies.IncrementAndGet());
            copy._copies = _copies;
            return copy;
        }
    }

    public class AccountState : ShaMap
    {
        protected AccountState(bool isCopy, int depth) : base(isCopy, depth)
        {
        }

        public AccountState()
        {
            
        }

        public bool Add(LedgerEntry entry) => AddItem(entry.Index(), entry);

        public bool Update(LedgerEntry readLedgerEntry) => UpdateItem(readLedgerEntry.Index(), readLedgerEntry);

        protected internal override ShaMapInner MakeInnerOfSameClass(int depth) => new AccountState(true, depth);

        public new AccountState Copy() => (AccountState) base.Copy();

        public static AccountState FromJson(JToken jToken, bool normalise=false)
        {
            var map = new AccountState();
            var items = from JObject entry in jToken
                where normalise == false ||
                      LedgerEntryType.FromJson(entry["LedgerEntryType"]) !=
                      LedgerEntryType.LedgerHashes
                select new LedgerEntry(entry);
            foreach (var ledgerEntry in items)
            {
                map.AddItem(ledgerEntry.Index(), ledgerEntry);
            }
            return map;
        }
    }

    public class TransactionTree : ShaMap
    {
        public bool Add(TransactionResult tx) => AddItem(tx.Hash(), tx);
    }
}