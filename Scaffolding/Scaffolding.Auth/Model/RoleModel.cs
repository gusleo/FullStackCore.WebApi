using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffolding.Auth.Model
{
    /// <summary>
    /// Model class to define user role
    /// </summary>
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
