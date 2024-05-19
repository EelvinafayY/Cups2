using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminHomePage.xaml
    /// </summary>
    public partial class AdminHomePage : Page
    {
        public static List<Admin> admins { get; set; }
        Admin contextAdmin;
        public AdminHomePage(Admin admin)
        {
            InitializeComponent();
            admins = DBConnection.admins.ToList();
            contextAdmin = admin;
            Refresh();
            this.DataContext = this;
        }

        public void Refresh()
        {
            SurnameTB.Text = contextAdmin.Surname;
            NameTB.Text = contextAdmin.Name;
            PatronymicTB.Text = contextAdmin.Patronymic;
            PhoneTB.Text = contextAdmin.Phone;
            GenderTB.Text = contextAdmin.Gender.Name;
        }

        private void StajersTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new ViewStajersPage(contextAdmin));
        }

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void ProfileTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AdminHomePage(contextAdmin));
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
