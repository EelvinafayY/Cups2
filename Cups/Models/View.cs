using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class View
    {
        public int kod_View { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Type> Type
        {
            get; set;

        }
    }
}
