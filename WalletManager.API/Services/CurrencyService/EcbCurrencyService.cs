namespace WalletManager.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;
    using WalletManager.API.Models;

    public class EcbCurrencyService : ICurrencyService
    {
        const string EcbUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

        public Task<decimal> ConvertCurrency(string inCurrencyName, string outCurrencyName, decimal amount)
        {
            return Task.Run(() => {
                var curriencies = GetCurrencies();

                decimal inCurrencyValue = curriencies.Where(p => p.Currency == inCurrencyName).Select(p => p.Value).FirstOrDefault();
                decimal outCurrencyValue = curriencies.Where(p => p.Currency == outCurrencyName).Select(p => p.Value).FirstOrDefault();

                if (inCurrencyValue == 0 || outCurrencyValue == 0)
                {
                    throw new CurrencyServiceException($"Can`t convert {inCurrencyName} to {outCurrencyName}");
                }

                return amount * outCurrencyValue / inCurrencyValue;
            });
        }

        private IEnumerable<Rate> GetCurrencies() 
        {
            var doc = new XmlDocument();
            doc.Load(EcbUrl);

            XmlNodeList nodes = doc.SelectNodes("//*[@currency]");

            var result = new List<Rate>();

            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    var value = Decimal.Parse(node.Attributes["rate"].Value, NumberStyles.Any, new CultureInfo("en-Us"));
                    result.Add(new Rate()
                    {
                        Currency = node.Attributes["currency"].Value,
                        Value = value
                    });
                }
            }

            return result;
        }
    }
}
