namespace Shop.DTO.Order
{
    public class OrderGetDTO
    {
        public Guid Id { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPhoto { get; set; }
        public string? ProductPrice { get; set; }
        public string? Status { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Address { get; set; }
    }
}
