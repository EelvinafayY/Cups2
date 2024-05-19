using Newtonsoft.Json;
using Cups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cups.Models
{
    public partial class Admin
    {
        public int Id_admin { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }
        public int Id_Gender { get; set; }
        public int Id_Role { get; set; }
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

        public virtual ICollection<Stajer> Stajer
        {
            get; set;

        }
    }
}
