namespace StockWebApp.Services
{
    public class RandomPriceDeltaGenerator
    {
        private readonly Random random;

        public RandomPriceDeltaGenerator()
        {
            random = new Random();
        }

        /// <summary>
        /// Generates a value between -1.0 and 1.0
        /// </summary>
        public double Generate()
        {
            var source = random.NextDouble();
            return source * 2.0 - 1.0;
        }
    }
}
