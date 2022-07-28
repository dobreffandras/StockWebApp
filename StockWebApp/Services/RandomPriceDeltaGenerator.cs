namespace StockWebApp.Services
{
    public class RandomPriceDeltaGenerator
    {
        private readonly Random random;

        public RandomPriceDeltaGenerator()
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
