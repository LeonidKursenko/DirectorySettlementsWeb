using DirectorySettlementsDAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDALTests.Repositories
{
    public class InMemoryDbContextFactory
    {
        public ApplicationContext GetArticleDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                            .UseInMemoryDatabase(databaseName: "InMemoryApplicationDatabase")
                            .Options;
            var dbContext = new ApplicationContext(options);
            dbContext.Database.EnsureDeleted();
            return dbContext;
        }
    }
}
