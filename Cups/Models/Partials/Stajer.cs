using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Stajer : INotifyPropertyChanged
    {
        private int _numberOfLesson;
        public int NumberOfLessons
        {
            get 
            { 
                int numberOfLessons = Services.DBConnection.lesson_Drinks.Count;
                _numberOfLesson = numberOfLessons;
                return _numberOfLesson; 
            }
            set
            {
                if (_numberOfLesson != value)
                {
                    _numberOfLesson = value;
                    OnPropertyChanged(nameof(NumberOfLessons));
                }
            }

        }
        private int _numberOfDoneLesson;
        public int NumberOfDoneLesson
        {
            get
            {
                int numberOfDoneLesson = Services.DBConnection.lessons_Stajer.Where(l => l.Id_Stajer == Id_Stajer && l.Done == true).Count();
                _numberOfDoneLesson = numberOfDoneLesson;
                return _numberOfDoneLesson;
            }
            set
            {
                if (_numberOfDoneLesson != value)
                {
                    _numberOfDoneLesson = value;
                    OnPropertyChanged(nameof(NumberOfDoneLesson));
                }
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
