using System.Collections.Generic;

namespace Ali_Store.Data.Model
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public float TotalPrice => Items.Sum(item => item.Product.Price);

        public int Quantity { get; set; }
    }

    public class CartItem
    {
        public Product Product { get; set; }
    }
} 