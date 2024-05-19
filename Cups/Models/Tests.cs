using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Tests
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int Id_Stajer { get; set; }
        public int Points { get; set; }
        [JsonIgnore]
        public Stajer Stajer
        {
            get
            {
                return DBConnection.stajers.FirstOrDefault(c => c.Id_Stajer == Id_Stajer);
            }
            set
            {
                Id_Stajer = value.Id_Stajer;
            }
        }
    }
}
