using Cups.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Stajer
    {
        public int Id_Stajer { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime DateOfRegistr {  get; set; }
        public byte[] Photo { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Id_Role { get; set; }
        public int Id_Gender { get; set; }
        public int Id_Admin { get; set; }
        [JsonIgnore]
        public Role Role
        {
            get
            {
                return DBConnection.roles.FirstOrDefault(c => c.Id_Role == Id_Role);
            }
            set
            {
                Id_Role = value.Id_Role;
            }
        }
        [JsonIgnore]
        public Gender Gender
        {
            get
            {
                return DBConnection.genders.FirstOrDefault(c => c.Id_Gender == Id_Gender);
            }
            set
            {
                Id_Gender = value.Id_Gender;
            }
        }
        [JsonIgnore]
        public Admin Admin
        {
            get
            {
                return DBConnection.admins.FirstOrDefault(c => c.Id_admin == Id_Admin);
            }
            set
            {
                Id_Admin = value.Id_admin;
            }
        }
        

        public virtual ICollection<LessonDrink_Stajer> LessonDrink_Stajer
        {
            get; set;

        }
        public virtual ICollection<Tests> Tests
        {
            get; set;

        }
    }
}
