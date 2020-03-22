namespace WalletManager.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Wallet
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [JsonIgnore]
        public ICollection<WalletCurrency> WalletCurrencies { get; set; }
    }
}
