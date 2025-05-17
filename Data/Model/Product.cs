using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Ali_Store.Data.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Model { get; set; }
        public string GoodFor { get; set; }
        public float Price { get; set; }

        public decimal? NewPrice { get; set; }
        public DateTime CreatedAt { get; set; }  = DateTime.Now;
        public bool? IsApproval { get; set; } = true;
        public bool? IsSall { get; set; } = false;
        public string? Pic { get; set; }
        public int Quantity { get; set; } = 1;
        public int AvailableQuantity { get; set; } = 1;
        
        [NotMapped]
        public IFormFile? PicPath { get; set; }
        
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }

        public ICollection<Rate> Rates { get; set; } = new List<Rate>();
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        [NotMapped]
        public double AverageRating { get; set; }
    }
}
