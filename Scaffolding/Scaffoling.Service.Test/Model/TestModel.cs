using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffoling.Service.Test.Model
{
    public class TestModel : WriteHistoryBaseModel, IModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
