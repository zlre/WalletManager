namespace WalletManager.Data
{
    using System;

    public class WalletCurrency
    {
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public Guid CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
