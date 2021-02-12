using Microsoft.EntityFrameworkCore;
using PublicMessageStorytel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicMessageStorytel.UnitTests
{
    class PublicMessageContextMocker
    {
        public static PublicMessageContext GetPublicMessageContextForTests(string dbName)
        {
            // Create options for DbContext instance
            var options = new DbContextOptionsBuilder<PublicMessageContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            // Create instance of DbContext
            var dbContext = new PublicMessageContext(options);

            // Add entities in memory
            dbContext.Seed();

            return dbContext;
        }
    }
}
