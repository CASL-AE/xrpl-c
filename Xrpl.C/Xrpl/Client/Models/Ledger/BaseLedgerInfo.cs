﻿using Newtonsoft.Json;

namespace Xrpl.Client.Models.Ledger
{
    public class LOBaseLedger
    {
       [JsonProperty("ledger_hash")]
        public string LedgerHash { get; set; }

        [JsonProperty("ledger_index")] 
        public uint LedgerIndex { get; set; }
    }
}
