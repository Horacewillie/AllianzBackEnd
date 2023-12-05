using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Domain.Config
{
    public class FlutterWaveConfig
    {
        public string BankTransferUrl { get; set; }

        public string ProviderApiKey { get; set; }

        public string MaxRetry { get; set; }
    }
}
