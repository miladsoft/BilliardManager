using Billiard.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Billiard.DataLayer.InMemoryDatabase
{
    public class InMemoryDatabaseContext : ApplicationDbContext
    {
        public InMemoryDatabaseContext(DbContextOptions options) : base(options)
        {
        }
    }
}