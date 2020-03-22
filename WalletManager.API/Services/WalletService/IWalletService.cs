namespace WalletManager.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WalletManager.API.Models;
    using WalletManager.Data;

    public interface IWalletService 
    {
        Task FillUp(string userName, Guid walletId, Guid currencyId, decimal amount);

        Task WithdrawCash(string userName, Guid walletId, Guid currencyId, decimal amount);

        Task Convert(string userName, Guid walletId, Guid inCurrencyId, Guid outCurrencyId, decimal amount);

        Task<IEnumerable<Account>> GetWalletStatement(string userName, Guid walletId);

        Task<Currency> CreateCurrency(string currencyName);

        Task<IEnumerable<Currency>> GetCurrencies();

        Task<IEnumerable<Wallet>> GetUserWallets(string userName);

        Task<Wallet> CreateWallet(string userName, Guid walletId);

        Task<User> CreateUser(string userName);
    }
}
