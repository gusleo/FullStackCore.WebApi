using Scaffolding.Data.Entities.Abstract;

namespace Scaffolding.Data.Test.Entities
{
    public class TestEntity : WriteHistoryBase, IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
