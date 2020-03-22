namespace WalletManager.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WalletManager.API.Models;
    using WalletManager.Data;

    public class WalletService : IWalletService
    {
        private readonly WalletManagerDBContext _db;
        private ICurrencyService _currencyService;

        public WalletService(WalletManagerDBContext db, ICurrencyService currencyService)
        {
            _db = db;
            _currencyService = currencyService;
    }

        private async Task<WalletCurrency> GetWalletCurrencyDependency(string userName, Guid walletId, Guid currencyId)
        {
            var wallet = await _db.Wallets.Where(w => w.User.Name == userName && w.Id == walletId).FirstOrDefaultAsync();

            if (wallet == null)
            {
                throw new WalletServiceException($"Wallet for {userName} with id = {walletId} is not found");
            }

            var currency = await _db.Currencies.Where(c => c.Id == currencyId).FirstOrDefaultAsync();

            if (currency == null)
            {
                throw new WalletServiceException($"Currency with id = {currencyId} is not found");
            }

            var walletCurrencyDependency = await _db.WalletCurrencies.Where(wc => wc.CurrencyId == currency.Id && wc.WalletId == wallet.Id).FirstOrDefaultAsync();

            if (walletCurrencyDependency == null)
            {
                walletCurrencyDependency = new WalletCurrency()
                {
                    Currency = currency,
                    CurrencyId = currency.Id,
                    Wallet = wallet,
                    WalletId = wallet.Id,
                    Amount = 0
                };

                _db.WalletCurrencies.Add(walletCurrencyDependency);

                await _db.SaveChangesAsync();

            }

            return walletCurrencyDependency;
        }

        public async Task FillUp(string userName, Guid walletId, Guid currencyId, decimal amount)
        {
            var walletCurrencyDependency = await GetWalletCurrencyDependency(userName, walletId, currencyId);

            walletCurrencyDependency.Amount += amount;

            await _db.SaveChangesAsync();
        }

        public async Task WithdrawCash(string userName, Guid walletId, Guid currencyId, decimal amount)
        {
            var walletCurrencyDependency = await GetWalletCurrencyDependency(userName, walletId, currencyId);

            if (walletCurrencyDependency.Amount - amount < 0)
            {
                throw new WalletServiceException($"Сan’t withdraw money from the wallet = {walletId} for {userName} because the wallet amount is less than {amount}");
            }

            walletCurrencyDependency.Amount -= amount;

            await _db.SaveChangesAsync();
        }

        public async Task<Currency> CreateCurrency(string currencyName)
        {
            var currency = new Currency() { 
                Id = Guid.NewGuid(), 
                Name = currencyName 
            };
            _db.Currencies.Add(currency);

            await _db.SaveChangesAsync();

            return currency;
        }

        public async Task Convert(string userName, Guid walletId, Guid inCurrencyId, Guid outCurrencyId, decimal amount)
        {
            var inCurrencyWalletDependency = await GetWalletCurrencyDependency(userName, walletId, inCurrencyId);
            var outCurrencyWalletDependency = await GetWalletCurrencyDependency(userName, walletId, outCurrencyId);

            decimal outAmount;

            try
            {
                outAmount = await _currencyService.ConvertCurrency(inCurrencyWalletDependency.Currency.Name, outCurrencyWalletDependency.Currency.Name, amount);
            }
            catch (CurrencyServiceException ex)
            {
                throw new WalletServiceException(ex.Message);
            }

            if (inCurrencyWalletDependency.Amount - amount < 0)
            {
                throw new WalletServiceException($"Сan’t withdraw money from the wallet = {walletId} for {userName} because the wallet amount is less than {amount}");
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    inCurrencyWalletDependency.Amount -= amount;
                    outCurrencyWalletDependency.Amount += outAmount;

                    await _db.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw new WalletServiceException("Transaction failed");
                }
            }
        }

        public async Task<IEnumerable<Account>> GetWalletStatement(string userName, Guid walletId)
        {
            return await _db.WalletCurrencies.Where(wc => wc.Wallet.Id == walletId && wc.Wallet.User.Name == userName)
                                             .Select(s => new Account() { Currency = s.Currency.Name, Amount = s.Amount })
                                             .ToListAsync();
        }

        public async Task<User> CreateUser(string userName)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = userName
            };
            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return user;
        }

        public async Task<Wallet> CreateWallet(string userName, Guid walletId)
        {
            var user = await _db.Users.Where(u => u.Name == userName).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new WalletServiceException($"{userName} is not found");
            }

            var wallet = await _db.Wallets.Where(w => w.User.Id == user.Id && w.Id == walletId).FirstOrDefaultAsync();

            if (wallet != null)
            {
                return wallet;
            }

            wallet = new Wallet()
            {
                Id = walletId,
                User = user
            };

            _db.Wallets.Add(wallet);

            await _db.SaveChangesAsync();

            return wallet;
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            return await _db.Currencies.ToListAsync();
        }

        public async Task<IEnumerable<Wallet>> GetUserWallets(string userName)
        {
            return await _db.Wallets.Where(w => w.User.Name == userName).ToListAsync();
        }
    }
}
