using Scaffolding.Data.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Data.Entities
{
    public class PaginationEntity<T> where T : IEntityBase
    {

        public int Page { get; set; }
        public int PageSize { get; set; }

        public int Count
        {
            get
            {
                return (null != this.Items) ? this.Items.Count() : 0;
            }
        }


        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalCount / PageSize);
            }
        }

        public IEnumerable<T>? Items { get; set; }
    }
}
