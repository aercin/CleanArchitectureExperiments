using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService) : base(options)
        {
            this._domainEventService = domainEventService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        private async Task DispatchEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<IHasDomainEvent>()
                                                   .Select(x => x.Entity.DomainEvents)
                                                   .SelectMany(x => x)
                                                   .Where(domainEvent => !domainEvent.IsPublished).ToList();

            foreach (DomainEvent item in domainEventEntities)
            {
                item.IsPublished = true;
                await this._domainEventService.Publish(item);
            }
        }
    }
}
