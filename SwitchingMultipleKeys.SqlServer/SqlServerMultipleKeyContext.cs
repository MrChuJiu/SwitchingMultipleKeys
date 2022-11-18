using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SwitchingMultipleKeys.SqlServer
{
    public class SqlServerMultipleKeyContext: DbContext
    {
        public DbSet<SqlServerMultipleKeyInfo> MultipleKeyInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=(localdb)\mssqllocaldb;Database=EFQuerying;Trusted_Connection=True");

            optionsBuilder.UseSqlServer(
                @"Server=192.168.31.178; Database=EFQuerying; User=sa; Password=shinetech:123; Connection Timeout=600;MultipleActiveResultSets=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             var builder = modelBuilder.Entity<SqlServerMultipleKeyInfo>();


             builder.Property(e => e.Data)
                //.HasJsonSerializeConversion();
            .HasConversion(c=> JsonConvert.SerializeObject(c, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }), s=> JsonConvert.DeserializeObject<IMultipleKeyEntity>(s ,new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                }));
        }


    }
}
