using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetailer.Core.Entities
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
    }
}
