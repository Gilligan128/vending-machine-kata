using System.Linq;
using System.Runtime.InteropServices;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class AcceptCoinsTests
    {
        private readonly TestBuilder _testBuilder = new TestBuilder();
        private const double MaliciousFakeCoin = 1.11;

        [Fact]
        public void ShouldShowDefaultDisplayWhenNothingHasYetHappened()
        {
            var sut = _testBuilder.CreateVendingMachine();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display);
        }

        [Fact]
        public void ShouldHaveNoCoinsInReturnWhenNothingHAsHappenedYet()
        {
            var sut = _testBuilder.CreateVendingMachine();

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(0, coinReturn.Count);
        }
       
        [Theory]
        [InlineData(CoinWeights.Nickel,"$0.05")]    
        [InlineData(CoinWeights.Dime,"$0.10")]    
        [InlineData(CoinWeights.Quarter,"$0.25")]    
        [InlineData(CoinWeights.Penny,"INSERT COINS")]      
        [InlineData(MaliciousFakeCoin,"INSERT COINS")]  
        public void ShouldAcceptNickelsDimesAndQuartersOnly(double weightInGrams, string expectedAmount)
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(new Coin(weightInGrams));

            var display = sut.CheckDisplay();

            Assert.Equal(expectedAmount, display);
        }

        [Fact]
        public void ShouldSumAmountWhenMultipleCoinsInserted()
        {
            var sut = _testBuilder.CreateVendingMachine();

            sut.InsertCoin(new Coin(CoinWeights.Nickel));
            sut.InsertCoin(new Coin(CoinWeights.Nickel));
            sut.InsertCoin(new Coin(CoinWeights.Dime));
            sut.InsertCoin(new Coin(CoinWeights.Dime));
            sut.InsertCoin(new Coin(CoinWeights.Quarter));    
            sut.InsertCoin(new Coin(CoinWeights.Quarter));    
            sut.InsertCoin(new Coin(CoinWeights.Quarter));    

            var display = sut.CheckDisplay();
            Assert.Equal("$1.05", display);
        }

        [Theory]
        [InlineData(CoinWeights.Nickel)] 
        [InlineData(CoinWeights.Dime)] 
        [InlineData(CoinWeights.Quarter)] 
        public void ShouldNotReturnNickelsDimesAndQuarters(double weightInGrams)
        {
            var sut = _testBuilder.CreateVendingMachine();

            var coinsInReturn = sut.GetCoinReturn();

            Assert.Equal(0, coinsInReturn.Count);
        }

        [Theory]
        [InlineData(CoinWeights.Penny, true)] 
        [InlineData(MaliciousFakeCoin, true)] 
        public void ShouldReturnCoinsThatAreNotNickelsDimesOrQuarters(double weightInGrams, bool shouldReject)
        {
            var sut = _testBuilder.CreateVendingMachine();

            var coin = new Coin(weightInGrams);
            sut.InsertCoin(coin);

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(1, coinReturn.Count);
            Assert.Equal(coin, coinReturn.First());
        }

        [Fact]
        public void ShouldReturnMultipleCoinsWhenMultipleBadCoinsInserted()
        {
            var sut = _testBuilder.CreateVendingMachine();
            var firstCoin = new Coin(CoinWeights.Penny);
            var secondCoin = new Coin(MaliciousFakeCoin);

            sut.InsertCoin(firstCoin);
            sut.InsertCoin(secondCoin);

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(2, coinReturn.Count);
            Assert.Equal(firstCoin, coinReturn.First());
            Assert.Equal(secondCoin, coinReturn.Skip(1).First());
        }
    }
}
