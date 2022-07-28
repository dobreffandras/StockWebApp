using FluentAssertions;
using StockWebApp.Dtos;
using StockWebApp.Services;

namespace StockWebAppTest
{
    public class StockPriceServiceTests
    {
        private readonly StocksService sut;

        public StockPriceServiceTests()
        {
            sut = new StocksService();
        }

        [Fact]
        public void Generates_SameValues_For_Zero_Feed()
        {
            // Given
            var startDate = DateTime.Now;
            var startPrice = 152.34;

            // When
            var prices = sut.GeneratePrices(
                startPrice,
                startDate: startDate,
                endDate: startDate.AddDays(5),
                randomFeed: () => 0,
                forces: new [] { 0.0 });

            // Then
            var expected =
                new[]
                {
                    new StockPrice(startDate.AddDays(0), startPrice),
                    new StockPrice(startDate.AddDays(1), startPrice),
                    new StockPrice(startDate.AddDays(2), startPrice),
                    new StockPrice(startDate.AddDays(3), startPrice),
                    new StockPrice(startDate.AddDays(4), startPrice),
                };

            prices.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Generates_increasing_values_For_Constant_Feed()
        {
            // Given
            var startDate = DateTime.Now;
            var startPrice = 152.34;
            var constant = 0.03;

            // When
            var prices = sut.GeneratePrices(
                startPrice,
                startDate: startDate,
                endDate: startDate.AddDays(5),
                randomFeed: () => constant,
                forces: new[] { 0.0 });

            // Then
            var expected =
                new[]
                {
                    new StockPrice(startDate.AddDays(0), startPrice + 0 * constant),
                    new StockPrice(startDate.AddDays(1), startPrice + 1 * constant),
                    new StockPrice(startDate.AddDays(2), startPrice + 2 * constant),
                    new StockPrice(startDate.AddDays(3), startPrice + 3 * constant),
                    new StockPrice(startDate.AddDays(4), startPrice + 4 * constant),
                };

            prices.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Generates_increasing_values_For_variable_Feed()
        {
            // Given
            var startDate = DateTime.Now;
            var startPrice = 152.34;
            var feeds = new List<double> { 0.03, 0.01, -0.05, 0.01 };
            var feedEnumberator = feeds.GetEnumerator();

            // When
            var prices = sut.GeneratePrices(
                startPrice,
                startDate: startDate,
                endDate: startDate.AddDays(5),
                randomFeed: () =>
                {
                    feedEnumberator.MoveNext();
                    return feedEnumberator.Current;
                },
                forces: new[] { 0.0 });

            // Then
            var expected =
                new[]
                {
                    new StockPrice(startDate.AddDays(0), 152.34),
                    new StockPrice(startDate.AddDays(1), 152.37),
                    new StockPrice(startDate.AddDays(2), 152.38),
                    new StockPrice(startDate.AddDays(3), 152.33),
                    new StockPrice(startDate.AddDays(4), 152.34),
                };

            prices.Should().BeEquivalentTo(
                expected, 
                o => o.Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.001)).WhenTypeIs<double>());
        }

        [Fact]
        public void Generates_increasing_values_For_variable_Feed_Affected_by_Force()
        {
            // Given
            var startDate = DateTime.Now;
            var startPrice = 152.34;
            var feeds = new List<double> { 0.03, 0.01, -0.05, 0.01 };
            var feedEnumberator = feeds.GetEnumerator();

            // When
            var prices = sut.GeneratePrices(
                startPrice,
                startDate: startDate,
                endDate: startDate.AddDays(5),
                () =>
                {
                    feedEnumberator.MoveNext();
                    return feedEnumberator.Current;
                },
                forces: new[] {0.05});

            // Then
            var expected =
                new[]
                {
                    new StockPrice(startDate.AddDays(0), 152.34),
                    new StockPrice(startDate.AddDays(1), 152.42),
                    new StockPrice(startDate.AddDays(2), 152.48),
                    new StockPrice(startDate.AddDays(3), 152.48),
                    new StockPrice(startDate.AddDays(4), 152.54),
                };

            prices.Should().BeEquivalentTo(
                expected,
                o => o.Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.001)).WhenTypeIs<double>());
        }

        [Fact]
        public void Generates_values_For_variable_Feed_Affected_by_Variable_Forces()
        {
            // Given
            var startDate = DateTime.Now;
            var startPrice = 152.34;
            var feeds = new List<double> { 0.03, 0.01, -0.05, 0.01 };
            var feedEnumberator = feeds.GetEnumerator();

            // When
            var prices = sut.GeneratePrices(
                startPrice,
                startDate: startDate,
                endDate: startDate.AddDays(5),
                () =>
                {
                    feedEnumberator.MoveNext();
                    return feedEnumberator.Current;
                },
                forces: new[] { 0.05, -0.07 });

            // Then
            var expected =
                new[]
                {
                    new StockPrice(startDate.AddDays(0), 152.34), 
                    new StockPrice(startDate.AddDays(1), 152.42), // +0.05 +0.03
                    new StockPrice(startDate.AddDays(2), 152.48), // +0.05 +0.01
                    new StockPrice(startDate.AddDays(3), 152.36), // -0.07 -0.05
                    new StockPrice(startDate.AddDays(4), 152.30), // -0.07 +0.01
                };

            prices.Should().BeEquivalentTo(
                expected,
                o => o.Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.001)).WhenTypeIs<double>());
        }
    }
}