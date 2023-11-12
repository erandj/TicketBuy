using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    internal class ProductTable
    {
        private string typeCount;
        private string price;

        public string TypeCount { get => typeCount; set => typeCount = value; }
        public string Price { get => price; set => price = value; }
    }
}
