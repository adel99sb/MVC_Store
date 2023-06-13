namespace Ali_Store.Data.Model
{
    public class Oreder
    {
        public int Id { get; set; }
        public _User User { get; set; }
        public Product Product { get; set; }
    }
}
