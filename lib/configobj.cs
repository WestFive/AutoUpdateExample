using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTP.lib
{
   public  class configobj
    {
        private string nowversion;

        public string NowVersion
        {
            get { return nowversion; }
            set { nowversion = value; }
        }
    }
}
