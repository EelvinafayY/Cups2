using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Menu
    {
        public int Id_Menu { get; set; }
        public string NamePosition { get; set; }
        public string Compound {  get; set; }
        public string Description { get; set; }
        public int Kod_Type { get; set; }
        public byte[] Photo {  get; set; }
        public string Volume { get; set; }

        [JsonIgnore]
        public Type Type
        {
            get
            {
                return DBConnection.types.FirstOrDefault(c => c.Kod_Type == Kod_Type);
            }
            set
            {
                Kod_Type = value.Kod_Type;
            }
        }

        public virtual ICollection<Lesson_Drink> Lesson_Drink
        {
            get; set;

        }
    }
}
