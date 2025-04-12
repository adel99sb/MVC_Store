using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedAt { get; set; }  = DateTime.Now;
        public bool IsApproval { get; set; } = false;
        public string? Pic { get; set; }
        [NotMapped]
        public IFormFile? PicPath { get; set; }
    }
}
