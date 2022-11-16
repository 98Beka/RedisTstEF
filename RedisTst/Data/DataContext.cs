using Microsoft.EntityFrameworkCore;
using RedisTst.Models;

namespace RedisTst.Data {
    public class DataContext : DbContext {
        public DbSet<TstModel> TstModels { get; set; }
        public DataContext(DbContextOptions configuration) : base(configuration) { }

    }
}
