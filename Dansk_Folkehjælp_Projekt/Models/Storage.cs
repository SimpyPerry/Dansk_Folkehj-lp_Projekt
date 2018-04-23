using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dansk_Folkehjælp_Projekt.Models
{
    public class Storage
    {
        //Storage
        public string location { get; set; }

        //Bookcase
        public string bookcaseName { get; set; }

        //Box
        public string boxID { get; set; }

        //Item
        public string itemName { get; set; }
        public int amount { get; set; }
        public int minAmount { get; set; }
    }
}
