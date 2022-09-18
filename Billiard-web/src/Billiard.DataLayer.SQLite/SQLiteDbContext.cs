using Billiard.DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using Billiard.Common.EFCoreToolkit;

namespace Billiard.DataLayer.SQLite
{
    public class SQLiteDbContext : ApplicationDbContext
    {
        public SQLiteDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddDateTimeOffsetConverter();
        }
    }
}