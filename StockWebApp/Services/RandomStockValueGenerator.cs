namespace StockWebApp.Services
{
    public class RandomStockValueGenerator
    {
        private readonly Random random;

        public RandomStockValueGenerator()
        {
            random = new Random();
        }

        public double Generate()
        {
            var source = random.NextDouble();
            return source * 2.0 - 1.0;
        }
    }
}
