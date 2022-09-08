using Billiard.Common.EFCoreToolkit;
using Billiard.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Billiard.DataLayer.SQLite;

public class SQLiteDbContext : ApplicationDbContext
{
    public SQLiteDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // NOTE: Add custom SQLite's settings here ...

        builder.AddDateTimeOffsetConverter();
        builder.SetCaseInsensitiveSearchesForSQLite();
    }
}