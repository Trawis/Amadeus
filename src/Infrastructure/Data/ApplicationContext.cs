using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Core.Common.Constants;
using Core.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ApplicationContext : DbContext, IApplicationContext
  {
    private readonly IDateTime _dateTime;

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public ApplicationContext(
      DbContextOptions<ApplicationContext> options,
      IDateTime dateTime) : base(options)
    {
      _dateTime = dateTime;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Added:
            entry.Entity.CreatedAt = _dateTime.UtcNow;
            break;
          case EntityState.Modified:
            entry.Entity.ModifiedAt = _dateTime.UtcNow;
            break;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.HasDefaultSchema(Constants.Database.DEFAULT_SCHEMA);
      builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
  }
}
