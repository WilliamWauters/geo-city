﻿using geocity.domain.Entities;
using geocity.domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace geocity.infrastructure
{
    public class GeoCityDbContext : DbContext
    {
        public GeoCityDbContext(DbContextOptions<GeoCityDbContext> options) : base(options) {}
        public DbSet<City> Cities { get; set; }
        public DbSet<Itinary> Itinaries { get; set; }
        public DbSet<ItinaryPointOfCrossing> ItinaryPointOfCrossings { get; set; }
        public DbSet<PointOfCrossing> PointOfCrossing { get; set; }
        public DbSet<ItinaryPointOfInterest> ItinaryPointOfInterests { get; set; }
        public DbSet<PointOfInterest> PointOfInterest { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripUser> TripUsers { get; set; }
        public DbSet<TripUserFavorite> TripUserFavorites { get; set; }
        public DbSet<TripUserRating> TripUserRatings { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            // HANDLE POSTION OF POI AND POC WHEN CREATED
            var historyAddedPoints = ChangeTracker
               .Entries()
               .Where(e => (e.Entity is ItinaryPointOfCrossing || e.Entity is ItinaryPointOfInterest) && (e.State == EntityState.Added));

            foreach (var points in historyAddedPoints)
            {
                var ItinaryId = points.Entity is ItinaryPointOfCrossing ? ((ItinaryPointOfCrossing)points.Entity).ItinaryId : ((ItinaryPointOfInterest)points.Entity).ItinaryId;

                var count = ItinaryPointOfCrossings.Where(x => x.ItinaryId == ItinaryId).Count() + ItinaryPointOfInterests.Where(x => x.ItinaryId == ItinaryId).Count();  

                var POCMaxPosition = ItinaryPointOfCrossings.Where(x => x.ItinaryId == ItinaryId).Max(x => (int?)x.Position) ?? 0;
                var POIMaxPosition = ItinaryPointOfInterests.Where(x => x.ItinaryId == ItinaryId).Max(x => (int?)x.Position) ?? 0;

                var position = 0;

                if (count != 0)
                {
                    position = POCMaxPosition > POIMaxPosition ? POCMaxPosition + 1 : POIMaxPosition + 1;
                } 


                if (points.Entity is ItinaryPointOfCrossing)
                {
                    ((ItinaryPointOfCrossing)points.Entity).Position = position;
                } 
                else
                {
                    ((ItinaryPointOfInterest)points.Entity).Position = position;
                }
            }

            // HANDLE POSTION OF POI AND POC WHEN DELETED
            var historyDeletedPoints = ChangeTracker
               .Entries()
               .Where(e => (e.Entity is ItinaryPointOfCrossing || e.Entity is ItinaryPointOfInterest) && (e.State == EntityState.Deleted));

            foreach (var points in historyDeletedPoints)
            {
                // GET ALL THE POINTS
                var ItinaryId = points.Entity is ItinaryPointOfCrossing ? ((ItinaryPointOfCrossing)points.Entity).ItinaryId : ((ItinaryPointOfInterest)points.Entity).ItinaryId;
                var POCS = ItinaryPointOfCrossings.Where(x => x.ItinaryId == ItinaryId).ToList();
                var POIS = ItinaryPointOfInterests.Where(x => x.ItinaryId == ItinaryId).ToList();

                // GET THE POSITION OF THE DELETED POINT
                var position = 0;
                if (points.Entity is ItinaryPointOfCrossing)
                {
                    position = ((ItinaryPointOfCrossing)points.Entity).Position;
                }
                else
                {
                    position = ((ItinaryPointOfInterest)points.Entity).Position;
                }


                // FROM THE DELETED PONT DECREMENT BY ONE ALL THE POINTS
                foreach (var poc in POCS)
                {
                    if (poc.Position > position)
                    {
                        poc.Position = poc.Position - 1;
                    }
                }

                foreach (var poi in POIS)
                {
                    if (poi.Position > position)
                    {
                        poi.Position = poi.Position - 1;
                    }
                }
            }

            return await base.SaveChangesAsync(); ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .Property(p => p.Latitude).HasPrecision(38, 18);

            modelBuilder.Entity<City>()
                .Property(p => p.Longitude).HasPrecision(38, 18);

            modelBuilder.Entity<PointOfCrossing>()
                .Property(p => p.Latitude).HasPrecision(38, 18);

            modelBuilder.Entity<PointOfCrossing>()
                .Property(p => p.Longitude).HasPrecision(38, 18);

            modelBuilder.Entity<PointOfInterest>()
               .Property(p => p.Latitude).HasPrecision(38, 18);

            modelBuilder.Entity<PointOfInterest>()
                .Property(p => p.Longitude).HasPrecision(38, 18);

            modelBuilder.Entity<Trip>().Property(x => x.Link).HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<City>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Itinary>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ItinaryPointOfCrossing>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<PointOfCrossing>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ItinaryPointOfInterest>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<PointOfInterest>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Trip>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUser>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUserFavorite>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUserRating>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<City>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Itinary>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ItinaryPointOfCrossing>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<PointOfInterest>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ItinaryPointOfInterest>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<PointOfCrossing>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Trip>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUser>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUserFavorite>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<TripUserRating>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(p => p.ModifiedDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
