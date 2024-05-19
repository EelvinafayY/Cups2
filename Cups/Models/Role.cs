using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Role
    {
        public int Id_Role { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Admin> Admin { get; set; }
        public virtual ICollection<Stajer> Stajer { get; set; }
    }
}
