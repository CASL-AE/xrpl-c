﻿
using Newtonsoft.Json;

namespace Xrpl.Client.Models.Ledger
{
    public class LOLedgerCurrentIndex
    {
        [JsonProperty("ledger_current_index")]
        public uint CurrentIndex { get; set; }
    }
}