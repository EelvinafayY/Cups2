using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cups.Models;

namespace Cups.Services
{
    public static class DBConnection
    {
        public static List<Admin> admins { get; set; }
        public static List<Stajer> stajers { get; set; }
        public static List<Gender> genders { get; set; }
        public static List<Lesson_Drink> lesson_Drinks { get; set; }
        public static List<LessonDrink_Stajer> lessons_Stajer { get; set; }
        public static List<Menu> menu { get; set; }
        public static List<Role> roles { get; set; }
        public static List<Step> steps { get; set; }
        public static List<Models.Type> types { get; set; }
        public static List<View> views { get; set; }
        public static List<Tests> tests { get; set; }

        public static Admin loginedAdmin;
        public static Stajer loginedStajer;
        public static async Task InitializeDBConnection()
        {
            await RefreshEnums();
            await RefreshData();
        }
        public static async Task RefreshData()
        {
            steps = await NetManager.Get<List<Step>>("api/Step/GetAllSteps");
            stajers = await NetManager.Get<List<Stajer>>("api/Stajer/GetAllStajers");
            lessons_Stajer = await NetManager.Get<List<LessonDrink_Stajer>>("api/LessonDrink_Stajer/GetAllLessonDrink_Stajer");
            tests = await NetManager.Get<List<Tests>>("api/Tests/GetAllTests");
        }
        public static async Task RefreshEnums()
        {
            genders = await NetManager.Get<List<Gender>>("api/Gender/GetAllGenders");
            roles = await NetManager.Get<List<Role>>("api/Role/GetAllRoles");
            admins = await NetManager.Get<List<Admin>>("api/Admin/GetAllAdmins");
            menu = await NetManager.Get<List<Menu>>("api/Menu/GetAllMenu");
            lesson_Drinks = await NetManager.Get<List<Lesson_Drink>>("api/Lesson_Drink/GetAllLesson_Drinks");
            views = await NetManager.Get<List<View>>("api/View/GetAllView");
            types = await NetManager.Get<List<Models.Type>>("api/Type/GetAllTypes");
            
           
            


        }

    }
}
