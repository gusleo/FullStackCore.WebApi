using Scaffolding.Data.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffolding.Data.Test.Entities
{
    public class TestEntity : WriteHistoryBase, IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public virtual TestEntityRelation Related { get; set; } = default!;
    }

    public class TestEntityRelation : WriteHistoryBase, IEntityBase {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid TestEntityId { get; set; }

        [ForeignKey(nameof(TestEntityId))]
        public virtual TestEntity TestEntity { get; } = default!;
    }
}
