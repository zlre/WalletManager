namespace WalletManager.API.Services
{
    using System;

    public class CurrencyServiceException : Exception
    {
        public CurrencyServiceException(string message) 
            : base(message)
        { 
        }
    }
}
