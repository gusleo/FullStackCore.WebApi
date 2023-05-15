using Scaffolding.Service.Model.Abstract;

namespace Core.Service.Services.Test.Models
{
    public class TestModel : WriteHistoryBaseModel, IModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
