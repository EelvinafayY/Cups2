using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Type
    {
        public int Kod_Type { get; set; }
        public string Name { get; set; }
        public int Kod_View { get; set; }
        [JsonIgnore]
        public View View
        {
            get
            {
                return DBConnection.views.FirstOrDefault(c => c.kod_View == Kod_View);
            }
            set
            {
                Kod_View = value.kod_View;
            }
        }

        public virtual ICollection<Menu> Menu
        {
            get; set;

        }
    }
}
