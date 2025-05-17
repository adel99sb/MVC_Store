using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Ali_Store.Data.Model
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public bool IsMain { get; set; } = false;
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
} 