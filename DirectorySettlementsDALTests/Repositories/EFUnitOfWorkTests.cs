using Xunit;
using DirectorySettlementsDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using DirectorySettlementsDAL.Data;
using Xunit.Abstractions;
using DirectorySettlementsDALTests.Repositories;
using DirectorySettlementsDAL.Interfaces;
using DirectorySettlementsDALTests.Helpers;
using System.Linq;

namespace DirectorySettlementsDAL.Repositories.Tests
{
    public class EFUnitOfWorkTests
    {
        private readonly ITestOutputHelper _output;
        private ApplicationContext _db;
        private IUnitOfWork _manager;

        public EFUnitOfWorkTests(ITestOutputHelper output)
        {
            _output = output;
            _db = new InMemoryDbContextFactory().GetArticleDbContext();
            _manager = new EFUnitOfWork(_db);
            InitData();
        }
        private void InitData()
        {
            var initialTable = TestData.InitialTableData();
            _db.InitialTable.AddRange(initialTable);
            _db.SaveChanges();
        }

        [Fact()]
        public void SaveTest()
        {
            // Arrange
            _manager.Fill();
            // Act
            _manager.Save();
            // Assert 
            int initialCounter = _db.InitialTable.Count();
            var settlements = _manager.Settlements.GetAll();
            Assert.NotNull(settlements);
            Assert.NotEmpty(settlements);
            Assert.Equal(initialCounter, settlements.Count());
            foreach (var settlement in settlements)
            {
                Assert.NotNull(settlement.Te);
                Assert.NotNull(settlement.Nu);
                if (settlement.ParentId != null)
                {
                    Assert.NotNull(settlement.Parent);
                }
                _output.WriteLine($"Te='{settlement.Te}', Np='{settlement.Np}', Nu='{settlement.Nu}', ParentId='{settlement.ParentId}', " +
                    $"Parent={settlement.Parent}, ChildrenCount={settlement.Children.Count()}");
            }
        }

        [Fact()]
        public void ClearTest()
        {
            // Arrange
            _manager.Fill();
            // Act
            _manager.Clear();
            // Assert 
            int initialCounter = _db.InitialTable.Count();
            var settlements = _manager.Settlements.GetAll();
            Assert.NotNull(settlements);
            Assert.Empty(settlements);
            Assert.NotEqual(initialCounter, settlements.Count());
        }

        private const string СonnectionString = "Server=.\\SqlExpress;Database=directorySettlementsDb;Trusted_Connection=True;MultipleActiveResultSets=true;";
        [Fact()]
        public void FillRealDbTest()
        {
            // Arrange
            ApplicationContext db = new ApplicationContext(СonnectionString);
            IUnitOfWork manager = new EFUnitOfWork(db);
            // Act
            manager.Fill();
            // Assert 
            int initialCounter = db.InitialTable.Count();
            var settlements = manager.Settlements.GetAll();
            Assert.NotNull(settlements);
            Assert.NotEmpty(settlements);
            Assert.Equal(initialCounter, settlements.Count());
            _output.WriteLine("Count=" + settlements.Count());
        }

        [Fact()]
        public void ClearRealDbTest()
        {
            // Arrange
            ApplicationContext db = new ApplicationContext(СonnectionString);
            IUnitOfWork manager = new EFUnitOfWork(db);
            // Act
            manager.Clear();
            // Assert 
            int initialCounter = db.InitialTable.Count();
            var settlements = manager.Settlements.GetAll();
            Assert.NotNull(settlements);
            Assert.Empty(settlements);
            Assert.NotEqual(initialCounter, settlements.Count());
            _output.WriteLine("Count=" + settlements.Count());
        }
    }
}