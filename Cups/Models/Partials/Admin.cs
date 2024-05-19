using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Cups.Models
{
    public partial class Admin
    {
        public string FullName
        {
            get
            {
                string fullname = this.Surname + " " + this.Name + " " + this.Patronymic;
                return fullname;
                
            }
        }
    }
}
