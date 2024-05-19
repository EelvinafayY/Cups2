using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Step
    {
        public int Number_Step { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number_Lesson {  get; set; }
        [JsonIgnore]
        public Lesson_Drink Lesson_Drink
        {
            get
            {
                return DBConnection.lesson_Drinks.FirstOrDefault(c => c.Number_Lesson == Number_Lesson);
            }
            set
            {
                Number_Lesson = value.Number_Lesson;
            }
        }
    }
}
