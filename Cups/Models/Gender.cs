using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public class Gender
    {
        public int Id_Gender { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Stajer> Stajer
        {
            get; set;

        }
        public virtual ICollection<Admin> Admin
        {
            get; set;

        }
    }
}
