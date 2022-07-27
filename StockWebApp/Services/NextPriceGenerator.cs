namespace StockWebApp.Services
{
    public class NextPriceGenerator
    {
        private readonly Func<double> feed;

        public NextPriceGenerator(double startValue, Func<double> feed)
        {
            this.feed = feed;
            Price = startValue;
        }

        public double Force { get; set; } = 0;

        public double Price { get; private set; }

        public void Generate()
        {
            var price = Price;
            Price = price + feed() + Force;
        }
    }
}
