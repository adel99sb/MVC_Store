using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Ali_Store.Data.Model
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public _User User { get; set; }
        public virtual List<CartItem> Items { get; set; } = new List<CartItem>();
        
        [NotMapped]
        public float TotalPrice => Items.Sum(item => item.Product?.NewPrice != null ? (float)item.Product.NewPrice : item.Product?.Price ?? 0);

        public int Quantity { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        
        public int Quantity { get; set; } = 1;
    }
} 