using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Models.ViewModels {
	public class ShoppingCartVM {
		public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
     
        public ShoppingCartVM()
        {
            ShoppingCartList = new List<ShoppingCart>();
            OrderHeader = new OrderHeader();
        }
    }
}
