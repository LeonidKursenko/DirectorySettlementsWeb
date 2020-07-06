using Xunit;
using DirectorySettlementsDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using DirectorySettlementsDAL.Data;
using Xunit.Abstractions;
using DirectorySettlementsDAL.Entities;
using DirectorySettlementsDALTests.Helpers;
using DirectorySettlementsDALTests.Repositories;

namespace DirectorySettlementsDAL.Repositories.Tests
{
    public class InitialRepositoryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationContext _db;
        private IEnumerable<InitialTable> _initialTable;
        private InitialRepository _repository;

        public InitialRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _db = new InMemoryDbContextFactory().GetArticleDbContext();
            InitData();
            _repository = new InitialRepository(_db);
        }

        private void InitData()
        {
            _initialTable = TestData.InitialTableData();
            _db.InitialTable.AddRange(_initialTable);
            _db.SaveChanges();
        }

        [Fact()]
        public void GetAllTest()
        {
            // Arrange
            // Act
            var data = _repository.GetAll();
            // Assert
            Assert.NotNull(data);
            Assert.NotEmpty(data);

        }
    }
}