using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SwitchingMultipleKeys.SqlServer
{
    public class SqlServerMultipleKeyContext : DbContext
    {
        public DbSet<SqlServerMultipleKeyInfo> MultipleKeyInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<SqlServerMultipleKeyInfo>();


            builder.Property(e => e.Data)
           //.HasJsonSerializeConversion();
           .HasConversion(c => JsonConvert.SerializeObject(c, new JsonSerializerSettings
           {
               TypeNameHandling = TypeNameHandling.All
           }), s => JsonConvert.DeserializeObject<IMultipleKeyEntity>(s, new JsonSerializerSettings
           {
               TypeNameHandling = TypeNameHandling.Auto
           }));
        }


    }
}
