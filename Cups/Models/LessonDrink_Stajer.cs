using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class LessonDrink_Stajer : INotifyPropertyChanged
    {
        public int Number {  get; set; }
        public int Number_Lesson { get; set; }
        public int Id_Stajer { get; set; }
        public bool Done { get; set; }
        private int _rating;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Rating
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

       
       
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
