using System.Linq;
using PillarKata.VendingMachine;
using Xunit;

namespace PillarKata.VendingMachine.Tests
{
    public class AcceptCoinsTests
    {
        private const double Nickel = 5.0;
        private const double Dime = 2.27;
        private const double Quarter = 5.67;
        private const double Penny = 2.5;
        private const double MaliciousFakeCoin = 1.11;


        [Fact]
        public void ShouldShowDefaultDisplayWhenNothingHasYetHappened()
        {
            var sut = new VendingMachine();

            var display = sut.CheckDisplay();
            Assert.Equal("INSERT COINS", display.Message);
            Assert.Equal(0m, display.Amount);
        }

        [Fact]
        public void ShouldHaveNoCoinsInReturnWhenNothingHAsHappenedYet()
        {
            var sut = new VendingMachine();

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(0, coinReturn.Count);
        }

       
        [Theory]
        [InlineData(Nickel,.05)]    
        [InlineData(Dime,.10)]    
        [InlineData(Quarter,.25)]    
        [InlineData(Penny,0)]      
        [InlineData(MaliciousFakeCoin,0)]  
        public void ShouldAcceptNickelsDimesAndQuartersOnly(double weightInGrams, decimal expectedAmount)
        {
            var sut = new VendingMachine();

            sut.InsertCoin(new Coin(weightInGrams));

            var display = sut.CheckDisplay();

            Assert.Equal("INSERT COINS", display.Message);
            Assert.Equal(expectedAmount, display.Amount);
        }

        public void ShouldSumAmountWhenMultipleCoinsInserted()
        {
            var sut = new VendingMachine();
        }

        [Theory]
        [InlineData(Nickel)] 
        [InlineData(Dime)] 
        [InlineData(Quarter)] 
        public void ShouldNotReturnNickelsDimesAndQuarters(double weightInGrams)
        {
            var sut = new VendingMachine();

            var coinsInReturn = sut.GetCoinReturn();

            Assert.Equal(0, coinsInReturn.Count);
        }

        [Theory]
        [InlineData(Penny, true)] 
        [InlineData(MaliciousFakeCoin, true)] 
        public void ShouldReturnCoinsThatAreNotNickelsDimesOrQuarters(double weightInGrams, bool shouldReject)
        {
            var sut = new VendingMachine();

            var coin = new Coin(weightInGrams);
            sut.InsertCoin(coin);

            var coinReturn = sut.GetCoinReturn();
            Assert.Equal(1, coinReturn.Count);
            Assert.Equal(coin, coinReturn.First());
        }
    }
}
