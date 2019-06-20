namespace RepositoryTransactionApp
{
    public class Quote
    {
        public string ProductName { get; set; }

        public Quote(string productName)
        {
            ProductName = productName;
        }
    }

    public class QuoteCustomer
    {
        public string FullName { get; set; }

        public int QuoteId { get; set; }

        public QuoteCustomer(int quoteId, string fullName)
        {
            QuoteId = quoteId;
            FullName = fullName;
        }
    }
}
