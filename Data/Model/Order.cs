namespace Ali_Store.Data.Model
{
    public enum OrderStatus
    {
        Active,
        Canceled,
        Completed
    }

    public class Order
    {
        public int Id { get; set; }
        public _User User { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public int NumberOfItems { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public int CancellationHours { get; set; } = 24; // Default 24 hours for cancellation
        public OrderStatus Status { get; set; } = OrderStatus.Active;
        
        // Helper property to calculate if the order is still within cancellation window
        public bool CanBeCanceled => (DateTime.Now - Date).TotalHours < CancellationHours && Status == OrderStatus.Active;
    }
}
