namespace WalletManager.API.Services
{
    using System;

    public class WalletServiceException : Exception
    {
        public WalletServiceException(string message) 
            : base(message)
        { 
        }
    }
}
