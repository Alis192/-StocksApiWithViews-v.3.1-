using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using AutoFixture;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using Serilog;
using Microsoft.Extensions.Logging;

namespace UnitTest
{
    public class BuyAndSellTest
    {
        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreaterService _stocksCreaterService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;
        private readonly IStocksRepository _stocksRepository; //defines mocked methods 
        private readonly Mock<IStocksRepository> _stocksRepositoryMock; //helps us to define dummy implementation of a method
    

        public BuyAndSellTest(ITestOutputHelper testOutputHelper)
        {
            var diagnosticConextMock = new Mock<IDiagnosticContext>();
            var loggerMock = new Mock<ILogger<StocksCreaterService>>();


            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>(); 
            _stocksRepository = _stocksRepositoryMock.Object; //we are creating fake repository object

            _stocksGetterService = new StocksGetterService(_stocksRepository, loggerMock.Object, diagnosticConextMock.Object);
            _stocksCreaterService = new StocksCreaterService(_stocksRepository, loggerMock.Object, diagnosticConextMock.Object);
        }

        #region CreateBuyOrder


        [Fact]
        public async Task BuyOrderRequest_OrderIsNull()
        {
            //Arrange
            BuyOrderRequest? order_request = null;

            //Assert

            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }


        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(0)] // passing parameters to the test method
        public async Task BuyOrderRequest_OrderIsZero(uint orderQuantity)
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, orderQuantity)
                .Create();

            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(100001)] // passing parameters to the test method
        public async Task BuyOrderRequest_OrderIsMoreThanMax(uint orderQuantity)
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, orderQuantity)
                .Create();

            // Act and Assert
            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory]
        [InlineData(0)]
        public async Task BuyOrderRequest_NoPriceValue(double orderPrice)
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, orderPrice)
                .Create();



            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task BuyOrderRequest_PriceMoreThanLimit(double orderPrice)
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, orderPrice)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task BuyOrderRequest_EmptyStockSymbol()
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task BuyOrderRequest_OldDateTime()
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-01-01"))
                .Create();

            var action = async () =>
            {
                await _stocksCreaterService.CreateBuyOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task BuyOrderRequest_FullOrderValues_ToBeSuccessful()
        {
            BuyOrderRequest order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2003-01-01"))
                .Create();

            BuyOrder buy_order = order_request.ToBuyOrder();
            BuyOrderResponse buy_order_expected = buy_order.ToBuyOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buy_order);

            BuyOrderResponse order_from_create = await _stocksCreaterService.CreateBuyOrder(order_request);
            buy_order_expected.BuyOrderID = order_from_create.BuyOrderID;

            //we check newly created Guid if it is created

            order_from_create.Should().NotBe(Guid.Empty);
            order_from_create.Should().Be(buy_order_expected);
        }


        [Fact]
        public async Task GetAllBuyOrders_ToBeEmptyList()
        {
            var orders = new List<BuyOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(orders);

            List<BuyOrderResponse> empty_list = await _stocksGetterService.GetBuyOrders();

            empty_list.Should().BeNullOrEmpty();

        }
        #endregion




        #region CreateSellOrder
        [Fact]
        public async Task SellOrderRequest_ToBeNull()
        {
            //Arrange
            SellOrderRequest? order_request = null;

            //Assert
            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();

        }


        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(0)] // passing parameters to the test method
        public async Task SellOrderRequest_LessThanLimit_ShouldThrowArgumentException(uint orderQuantity)
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, orderQuantity)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(100001)] // passing parameters to the test method
        public async Task SellOrderRequest_MoreThanLimit_ShouldThrowArgumentException(uint orderQuantity)
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, orderQuantity)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Theory]
        [InlineData(0)]
        public async Task SellOrderRequest_PriceLessThanMin_ShouldThrowArgumentException(double orderPrice)
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, orderPrice)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task SellOrderRequest_PriceMoreThanMax_ShouldThrowArgumentException(double orderPrice)
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, orderPrice)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task SellOrderRequest_EmptyStockSymbol()
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();


            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task SellOrderRequest_OldDateTime_ShouldBeArgumentException()
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-01-01"))
                .Create();



            var action = async () =>
            {
                await _stocksCreaterService.CreateSellOrder(order_request);
            };

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task SellOrderRequest_ProperValues_ShouldBeSuccessful()
        {
            SellOrderRequest order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2003-01-01"))
                .Create();

            SellOrder sell_order = order_request.ToSellOrder();
            SellOrderResponse sell_order_expected = sell_order.ToSellOrderResponse();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sell_order);

            SellOrderResponse order_from_create = await _stocksCreaterService.CreateSellOrder(order_request);
            sell_order_expected.SellOrderID = order_from_create.SellOrderID;

            order_from_create.Should().NotBe(Guid.Empty);
            order_from_create.Should().Be(sell_order_expected);

            ////we check newly created Guid if it is created
            //Assert.True(order_from_create.SellOrderID != Guid.Empty);
            //Assert.Contains(order_from_create, order_list);
        }

        [Fact]
        public async Task GetAllSellOrders_Empty_ShouldBeEmpty()
        {
            var orders = new List<SellOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(orders);

            List<SellOrderResponse> empty_list = await _stocksGetterService.GetSellOrders();

            empty_list.Should().BeEmpty();  
        }

        #endregion

    }
}