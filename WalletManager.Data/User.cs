namespace WalletManager.Data
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Wallet> Wallets { get; set; }
    }
}
