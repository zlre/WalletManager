namespace WalletManager.API.Services
{
    using System.Threading.Tasks;

    public interface ICurrencyService
    {
        Task<decimal> ConvertCurrency(string inCurrencyName, string outCurrencyName, decimal amount);
    }
}
