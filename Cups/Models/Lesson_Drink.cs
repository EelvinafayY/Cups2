using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Lesson_Drink
    {
        public int Number_Lesson { get; set; }
        public int Id_Menu { get; set; }

        [JsonIgnore]
        public Menu Menu
        {
            get
            {
                return DBConnection.menu.FirstOrDefault(c => c.Id_Menu == Id_Menu);
            }
            set
            {
                Id_Menu = value.Id_Menu;
            }
        }

        public virtual ICollection<LessonDrink_Stajer> LessonDrink_Stajer
        {
            get; set;

        }
        public virtual ICollection<Step> Step 
        {
            get; set;

        }
    }
}
