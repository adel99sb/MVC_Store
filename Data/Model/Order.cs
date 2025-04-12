namespace Ali_Store.Data.Model
{
    public class Order
    {
        public int Id { get; set; }
        public _User User { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public int NumberOfItems { get; set; }        
    }
}
