using Scaffolding.Data.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scaffolding.Data.Test.Entities
{
    public class TestEntity : WriteHistoryBase, IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual TestEntityRelation Related { get; set; }
    }

    public class TestEntityRelation : WriteHistoryBase, IEntityBase {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid TestEntityId { get; set; }

        [ForeignKey(nameof(TestEntityId))]
        public virtual TestEntity TestEntity { get; }
    }
}
