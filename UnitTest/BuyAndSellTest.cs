using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace UnitTest
{
    public class BuyAndSellTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _testOutputHelper;

        public BuyAndSellTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _stocksService = new StocksService(new OrdersDbContext(new DbContextOptionsBuilder<OrdersDbContext>().Options));
        }

        #region CreateBuyOrder


        [Fact]
        public async Task BuyOrderRequest_AsNull()
        {
            //Arrange
            BuyOrderRequest? order_request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
               await _stocksService.CreateBuyOrder(order_request);
            });
        }


        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(0)] // passing parameters to the test method
        public async Task BuyOrderRequest_AsZero(uint orderQuantity)
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = orderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("1999-05-05")
            };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(order_request);
            });
        }

        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(100001)] // passing parameters to the test method
        public async Task BuyOrderRequest_AsMax(uint orderQuantity)
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = orderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("1999-05-05")
            };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(order_request);
            });
        }


        [Theory]
        [InlineData(0)]
        public async Task BuyOrderRequest_PriceLess(double orderPrice)
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = orderPrice,
                Quantity = 3
            };
            if (order_request.Price < 1)
            {
                await Assert.ThrowsAsync<ArgumentException>(async() =>
                {
                    await _stocksService.CreateBuyOrder(order_request);
                });
            }
        }

        [Theory]
        [InlineData(10001)]
        public async Task BuyOrderRequest_PriceMore(double orderPrice)
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = orderPrice,
                Quantity = 3
            };
            if (order_request.Price > 10000)
            {
                await Assert.ThrowsAsync<ArgumentException>(async() =>
                {
                    await _stocksService.CreateBuyOrder(order_request);
                });
            }
        }


        [Fact]
        public async Task BuyOrderRequest_EmptyStockSymbol()
        {
            BuyOrderRequest order_request = new BuyOrderRequest();
            if(string.IsNullOrEmpty(order_request.StockSymbol))
            {
                await Assert.ThrowsAsync<ArgumentException>(async() =>
                {
                    await _stocksService.CreateBuyOrder(order_request);
                });
            }
        }

        [Fact]
        public async Task BuyOrderRequest_OldDateTime()
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("1999-01-01"),
                Quantity = 2,
                Price = 100

            };
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(order_request);

            });
        }   

        [Fact]
        public async Task BuyOrderRequest_ProperValues()
        {
            BuyOrderRequest order_request = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2003-01-01"),
                Quantity= 2,
                Price= 100
            };

            BuyOrderResponse order_from_create = await _stocksService.CreateBuyOrder(order_request);

            List<BuyOrderResponse> order_list = await _stocksService.GetBuyOrders();

            //we check newly created Guid if it is created
            Assert.True(order_from_create.BuyOrderID != Guid.Empty);
            Assert.Contains(order_from_create, order_list);
        }


        [Fact] 
        public async Task GetAllBuyOrders_Empty()
        {
            List<BuyOrderResponse> empty_list = await _stocksService.GetBuyOrders();

            Assert.Empty(empty_list);
        }
        #endregion







        #region CreateSellOrder
        [Fact]
        public async Task SellOrderRequest_AsNull()
        {
            //Arrange
            SellOrderRequest? order_request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _stocksService.CreateSellOrder(order_request);
            });
        }


        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(0)] // passing parameters to the test method
        public async Task SellOrderRequest_QuantityLessThanMinimum(uint orderQuantity)
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = orderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("1999-05-05")
            };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(order_request);
            });
        }

        [Theory] // we use [Theory] instead of [Fact], so that we can pass parameters to the test method 
        [InlineData(100001)] // passing parameters to the test method
        public async Task SellOrderRequest_QuantityMoreThanMax(uint orderQuantity)
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = 1,
                Quantity = orderQuantity,
                DateAndTimeOfOrder = DateTime.Parse("1999-05-05")
            };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(order_request);
            });
        }


        [Theory]
        [InlineData(0)]
        public async Task SellOrderRequest_PriceLessThanMin(double orderPrice)
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = orderPrice,
                Quantity = 3
            };
            if (order_request.Price < 1)
            {
                await Assert.ThrowsAsync<ArgumentException>(async() =>
                {
                    await _stocksService.CreateSellOrder(order_request);
                });
            }
        }

        [Theory]
        [InlineData(10001)]
        public async Task SellOrderRequest_PriceMoreThanMax(double orderPrice)
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                Price = orderPrice,
                Quantity = 3
            };

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(order_request);
            });
        }


        [Fact]
        public async Task SellOrderRequest_EmptyStockSymbol()
        {
            SellOrderRequest order_request = new SellOrderRequest();
            if (string.IsNullOrEmpty(order_request.StockSymbol))
            {
                await Assert.ThrowsAsync<ArgumentException>(async() =>
                {
                    await _stocksService.CreateSellOrder(order_request);
                });
            }
        }

        [Fact]
        public async Task SellOrderRequest_OldDateTime()
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("1999-01-01"),
                Quantity = 2,
                Price = 100
            };

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(order_request);

            });
        }

        [Fact]
        public async Task SellOrderRequest_ProperValues()
        {
            SellOrderRequest order_request = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2003-01-01"),
                Quantity = 2,
                Price = 100
            };

            SellOrderResponse order_from_create = await _stocksService.CreateSellOrder(order_request);

            List<SellOrderResponse> order_list = await _stocksService.GetSellOrders();

            //we check newly created Guid if it is created
            Assert.True(order_from_create.SellOrderID != Guid.Empty);
            Assert.Contains(order_from_create, order_list);
        }

        [Fact]
        public async Task GetAllSellOrders_Empty()
        {
            List<SellOrderResponse> empty_list = await _stocksService.GetSellOrders();

            Assert.Empty(empty_list);
        }

        #endregion

    }
}