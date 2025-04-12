namespace Ali_Store.Data.Model
{
    public class Rate
    {
        public int Id { get; set; }
        public _User User { get; set; }
        public int UserId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public float RateTo5 { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set;} = DateTime.Now;
    }
}
