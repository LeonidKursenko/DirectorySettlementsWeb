using Xunit;
using DirectorySettlementsDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using DirectorySettlementsDAL.Data;
using DirectorySettlementsDALTests.Repositories;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDALTests.Helpers;
using System.Linq;
using DirectorySettlementsDAL.Exceptions;
using Xunit.Abstractions;

namespace DirectorySettlementsDAL.Repositories.Tests
{
    public class SettlementRepositoryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationContext _db;
        private IEnumerable<Settlement> _settlements;
        private SettlementRepository _repository;

        public SettlementRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _db = new InMemoryDbContextFactory().GetArticleDbContext();
            InitData();
            _repository = new SettlementRepository(_db);
        }

        #region Additional methods
        private void InitData()
        {
            _settlements = TestData.InitSettlementsData();
            _db.Settlements.AddRange(_settlements);
            _db.SaveChanges();
        }

        private void Show(string message)
        {
            _output.WriteLine(message);
        }

        [Fact]
        public void Dispose()
        {
            _db.Dispose();
        }
        #endregion

        #region Create tests
        [Theory]
        [InlineData("0400000000")]
        [InlineData("9000000000")]
        [InlineData("0130000000")]
        [InlineData("0112000000")]
        [InlineData("0111900000")]
        [InlineData("0110200000")]
        [InlineData("0110110000")]
        [InlineData("0110131000")]
        [InlineData("0110136200")]
        [InlineData("0110165602")]
        [InlineData("0110165612")]

        public void CreateTest(string te)
        {
            // Arange
            Settlement settlement = new Settlement { Te = te };
            // Act
            _repository.CreateAsync(settlement).Wait();
            _db.SaveChanges();
            // Assert
            Settlement settlementFromDb = _repository.GetAsync(te).GetAwaiter().GetResult();
            Assert.NotNull(settlementFromDb);
            Assert.Equal(settlement.Te, settlementFromDb.Te);
            Show($"Te='{settlementFromDb.Te}', ParentId='{settlementFromDb.ParentId}'");
        }

        [Theory]
        [InlineData("0410000000")]
        [InlineData("0410100000")]
        [InlineData("0410101000")]
        [InlineData("0410101100")]
        [InlineData("0410101001")]
        public void TryAddElementWithoutCorrectParentTest(string te)
        {
            // Arange
            Settlement settlement = new Settlement { Te = te };
            // Act and assert
            var ex = Assert.Throws<CreateOperationException>(() => _repository.CreateAsync(settlement).GetAwaiter().GetResult());
            Show(ex.Message);
        }

        [Theory]
        [InlineData("0100000000")]
        [InlineData("0111200000")]
        public void TryAddExistsSettlementTest(string te)
        {
            // Arange
            Settlement settlement = new Settlement { Te = te };
            // Assert
            var ex = Assert.Throws<CreateOperationException>(() => _repository.CreateAsync(settlement).GetAwaiter().GetResult());
            Show(ex.Message);
        }
        #endregion

        #region Delete tests
        [Theory]
        [InlineData("0110136900")]
        [InlineData("0110165601")]
        public void DeleteTest(string te)
        {
            // Arange
            var settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            // Act
            _repository.DeleteAsync(te).Wait();
            _db.SaveChanges();
            // Assert
            settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            Assert.Null(settlement);
        }

        [Theory]
        [InlineData("0000000000")]
        [InlineData("0110165602")]
        public void TryDeleteWithUnknownTeTest(string te)
        {
            // Arange
            var settlement = _repository.GetAsync(te);
            // Act and assert
            var exception = Assert.Throws<DeleteOperationException>(() => _repository.DeleteAsync(te).GetAwaiter().GetResult());
            Show(exception.Message);
        }

        [Theory]
        [InlineData("0110100000")]
        [InlineData("0100000000")]
        public void TryDeleteWithChildrenTest(string te)
        {
            // Arange
            var settlement = _repository.GetAsync(te);
            // Act and assert
            var exception = Assert.Throws<DeleteOperationException>(() => _repository.DeleteAsync(te).GetAwaiter().GetResult());
            Show(exception.Message);
        }

        [Theory]
        [InlineData("0110100000")]
        [InlineData("0100000000")]
        public void DeleteAllTest(string te)
        {
            // Arange
            var settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            string childTe = settlement.Children.FirstOrDefault().Te;
            // Act
            _repository.DeleteAllAsync(te).Wait();
            _db.SaveChanges();
            // Assert
            settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            Assert.Null(settlement);
            var child = _repository.GetAsync(childTe).GetAwaiter().GetResult();
            Assert.Null(child);
        }
        #endregion

        #region Get tests
        [Theory]
        [InlineData("0100000000")]
        [InlineData("0111200000")]
        public void GetTest(string te)
        {
            // Act
            Settlement settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            // Assert
            Assert.NotNull(settlement);
            Assert.Equal(te, settlement.Te);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("0000000000")]
        [InlineData("0110165602")]
        [InlineData("0400000000")]
        public void TryGetNotExistsSettlementTest(string te)
        {
            // Act
            Settlement settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            // Assert
            Assert.Null(settlement);
        }
        #endregion

        #region Find tests
        [Theory]
        [InlineData("АВТОНОМНА РЕСПУБЛІКА КРИМ/М.СІМФЕРОПОЛЬ")]
        public void FindTest(string name)
        {
            // Act
            var settlements = _repository.Find(s => s.Nu == name);
            // Assert
            Assert.NotNull(settlements);
            Assert.NotEmpty(settlements);
            Assert.Single(settlements);
            Assert.Equal(name, settlements.First().Nu);
        }

        [Theory]
        [InlineData("АВТОНОМНА")]
        public void TryFindNotExistsSettlementTest(string name)
        {
            // Act
            var settlements = _repository.Find(s => s.Nu == name);
            // Assert
            Assert.NotNull(settlements);
            Assert.Empty(settlements);
        }
        #endregion

        #region GetAll test
        [Fact()]
        public void GetAllTest()
        {
            // Act
            var settlements = _repository.GetAllAsync().GetAwaiter().GetResult();
            // Assert
            Assert.NotEmpty(settlements);
        }
        #endregion

        #region Update tests
        [Theory]
        [InlineData("0100000000")]
        [InlineData("0111200000")]
        public void UpdateTest(string te)
        {
            // Arange
            Settlement settlement = _repository.GetAsync(te).GetAwaiter().GetResult();
            string newNu = "NewName";
            string newNp = "M";
            settlement.Nu = newNu;
            settlement.Np = newNp;
            // Act
            _repository.UpdateAsync(settlement).Wait();
            Settlement settlementUpdated = _repository.GetAsync(te).GetAwaiter().GetResult();
            // Assert
            Assert.Equal(newNu, settlementUpdated.Nu);
            Assert.Equal(newNp, settlementUpdated.Np);
        }

        #endregion

        [Fact()]
        public void ClearTest()
        {
            // Arange
            int counter = _repository.GetAllAsync().GetAwaiter().GetResult().Count();
            // Act
            _repository.ClearAsync().GetAwaiter().GetResult();
            // Assert
            Assert.Empty(_repository.GetAllAsync().GetAwaiter().GetResult());
            Assert.NotEqual(counter, _repository.GetAllAsync().GetAwaiter().GetResult().Count());
        }
    }
}