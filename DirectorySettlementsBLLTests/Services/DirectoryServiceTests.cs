using Xunit;
using DirectorySettlementsBLL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using DirectorySettlementsBLLTests.Helpers;
using DirectorySettlementsDAL.Interfaces;
using Moq;
using DirectorySettlementsBLL.Interfaces;
using System.Linq;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDALTests.Repositories;
using DirectorySettlementsDAL.Repositories;
using DirectorySettlementsBLL.DTO;
using DirectorySettlementsBLL.BusinessModels;

namespace DirectorySettlementsBLL.Services.Tests
{
    public class DirectoryServiceTests
    {
        private readonly ITestOutputHelper _output;
        private ApplicationContext _db;
        private IUnitOfWork _manager;
        private IDirectoryService _directoryService;

        public DirectoryServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _db = new InMemoryDbContextFactory().GetArticleDbContext();
            _manager = new EFUnitOfWork(_db);
            _manager.Settlements.AddRangeAsync(TestData.EmitData()).GetAwaiter().GetResult();

            //var mock = new Mock<IUnitOfWork>();
            //mock.Setup(manager => manager.Settlements.GetAllAsync().GetAwaiter().GetResult())
            //    .Returns(TestData.EmitData());
            _directoryService = new DirectoryService(_manager);
            
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("АВТОНОМ", "")]
        [InlineData("", "Р")]
        [InlineData("КИЇВСЬКИЙ", "")]
        [InlineData("ГРЕСІВСЬКИЙ", "")]
        [InlineData("АЕРОФЛОТСЬКИЙ", "Р")]
        [InlineData("АЕРОФЛОТСЬКИЙ", "Т")]
        [InlineData("АЕРОФЛОТСЬКИЙ", "")]
        public void FilterAsyncTest(string name, string type)
        {
            // Act
            var results = _directoryService.FilterAsync(
                new FilterOptions { Name = name, SettlementType = type }).GetAwaiter().GetResult();
            // Assert
            Assert.NotNull(results);
            ShowAll(results);
        }

        [Fact()]
        public void GetAllAsyncTest()
        {
            // Act
            var results  = _directoryService.GetAllAsync().GetAwaiter().GetResult();
            // Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Theory]
        [InlineData("0110100000")]
        [InlineData("0100000000")]
        public void GetAsyncTest(string te)
        {
            // Act
            var result = _directoryService.GetAsync(te).GetAwaiter().GetResult();
            // Assert
            Assert.NotNull(result);
            _output.WriteLine($"Te={result.Te}, Name={result.Nu}, ChildCount={result.Children.Count()}");
        }

        [Theory]
        [InlineData("0110100000")]
        [InlineData("0100000000")]
        public void GetChildrenAsyncTest(string te)
        {
            // Act
            var results = _directoryService.GetChildrenAsync(te).GetAwaiter().GetResult();
            // Assert
            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Show(results);
        }

        private void Show(IEnumerable<SettlementDTO> results)
        {
            foreach (var result in results)
            {
                _output.WriteLine($"Te={result.Te}, Name={result.Nu}, ChildCount={result.Children.Count()}");
            }
        }

        private void ShowAll(IEnumerable<SettlementDTO> results)
        {
            foreach (var result in results)
            {
                _output.WriteLine($"Te={result.Te}, Name={result.Nu}, Type={result.Np}, ChildCount={result.Children.Count()}");
                ShowAll(result.Children);
            }
        }
    }
}