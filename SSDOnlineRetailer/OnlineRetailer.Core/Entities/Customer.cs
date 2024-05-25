namespace OnlineRetailer.Core
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public decimal Balance { get; set; }

        public string hashedPassword { get; set; }
    }
}
