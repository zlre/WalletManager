namespace WalletManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Currency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<WalletCurrency> WalletCurrencies { get; set; }
    }
}
