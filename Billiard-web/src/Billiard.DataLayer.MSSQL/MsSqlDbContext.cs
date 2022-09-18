using Billiard.DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace Billiard.DataLayer.MSSQL
{
    public class MsSqlDbContext : ApplicationDbContext
    {
        public MsSqlDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}