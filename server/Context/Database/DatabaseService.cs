using Microsoft.EntityFrameworkCore;
using server.Context.Configuration;
using server.Models;

namespace server.Context.Database
{
    public class DatabaseService : DbContext
    {
        public DatabaseService(DbContextOptions<DatabaseService> options)
        : base(options) { }
        public DbSet<UserModel> Users {get;set;}
        public DbSet<EquipoModel> Equipos {get;set;}
        public DbSet<OficinaModel> Oficinas {get;set;}
        public DbSet<AAModel> Aires {get;set;}
        public DbSet<PcModel> Pcs {get;set;}
        public DbSet<ImpresoraModel> Impresoras {get;set;}
        public DbSet<MonitorModel> Monitores {get;set;}

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync()>0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityConfiguration(modelBuilder);
        }
        
        private void EntityConfiguration (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OficinaConfiguration());
            modelBuilder.ApplyConfiguration(new EquipoConfiguration());

            // Subtipos de Equipo
            modelBuilder.ApplyConfiguration(new PcConfiguration());
            modelBuilder.ApplyConfiguration(new AAModelConfiguration());
            modelBuilder.ApplyConfiguration(new ImpresoraConfiguration());
            modelBuilder.ApplyConfiguration(new MonitorConfiguration());
            modelBuilder.ApplyConfiguration(new AudiovisualConfiguration());
        }
    }
}