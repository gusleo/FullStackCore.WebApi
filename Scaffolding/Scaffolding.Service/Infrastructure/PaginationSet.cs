using Scaffolding.Service.Model.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Infrastructure
{
    public class PaginationSet<T> where T : IModelBase, new()
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int Count
        {
            get
            {
                return Items.Count();
            }
        }
    }
}
