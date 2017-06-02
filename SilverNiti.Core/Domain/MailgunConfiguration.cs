using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverNiti.Core.Domain
{
   public class MailgunConfiguration
    {
        public string FromEmail { get; set; }
        public string AdminEmail { get; set; }
        public string Domain { get; set; }
        public string Key { get; set; }
    }
}
