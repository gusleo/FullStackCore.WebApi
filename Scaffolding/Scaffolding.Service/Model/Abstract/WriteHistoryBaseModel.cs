using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Service.Model.Abstract
{
    public abstract class WriteHistoryBaseModel
    {

        public Guid CreatedById { get; private set; }

        public Guid UpdatedById { get; private set; }



        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public DateTime? DeletedDate { get; private set; }
        public Guid? DeletedById { get; private set; }
        public bool IsDeleted { get; private set; }

        public virtual UserModel? UpdatedByUser { get; private set; } = default!;

        public virtual UserModel? CreatedByUser { get; private set; } = default!;

    }
}
