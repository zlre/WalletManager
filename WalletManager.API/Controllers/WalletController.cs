namespace WalletManager.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WalletManager.API.Models;
    using WalletManager.API.Services;
    using WalletManager.Data;

    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
             _walletService = walletService;
        }

        [HttpPost("FillUp")]
        public async Task<ActionResult> FillUp(string userName, Guid walletId, Guid currencyId, decimal amount)
        {
            try
            {
                await _walletService.FillUp(userName, walletId, currencyId, amount);
            }
            catch (WalletServiceException ex) 
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }        
        
        [HttpPost("Withdraw")]
        public async Task<ActionResult> Withdraw(string userName, Guid walletId, Guid currencyId, decimal amount)
        {
            try
            {
                await _walletService.WithdrawCash(userName, walletId, currencyId, amount);
            }
            catch (WalletServiceException ex) 
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpGet("WalletStatement")]
        public async Task<ActionResult<IEnumerable<Account>>> WalletStatement(string userName, Guid walletId)
        {
            return Ok(await _walletService.GetWalletStatement(userName, walletId));
        }

        [HttpGet("GetCurrencies")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            return Ok(await _walletService.GetCurrencies());
        }

        [HttpGet("GetUserWallets")]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetUserWallets(string userName)
        {
            return Ok(await _walletService.GetUserWallets(userName));
        }

        [HttpPost("CreateWallet")]
        public async Task<ActionResult<Wallet>> CreateWallet(string userName, Guid walletId)
        {
            try
            {
                return Ok(await _walletService.CreateWallet(userName, walletId));
            }
            catch (WalletServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }
                
        
        [HttpPost("CreateUser")]
        public async Task<ActionResult<Wallet>> CreateUser(string userName)
        {
            return Ok(await _walletService.CreateUser(userName));
        }

        [HttpPost("CreateCurrency")]
        public async Task<ActionResult<Currency>> CreateCurrency(string currencyName)
        {
            return await _walletService.CreateCurrency(currencyName);
        }


        [HttpPost("Convert")]
        public async Task<ActionResult> Convert(string userName, Guid walletId, Guid inCurrencyId, Guid outCurrencyId, decimal amount)
        {
            try
            {
                await _walletService.Convert(userName, walletId, inCurrencyId, outCurrencyId, amount);
            }
            catch (WalletServiceException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
