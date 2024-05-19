using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ViewStajersPage.xaml
    /// </summary>
    public partial class ViewStajersPage : Page
    {
        public static List<Admin> admins { get; set; }
        public static List<Stajer> stajer { get; set; }
        Admin contextAdmin;
        public ViewStajersPage(Admin admin)
        {
            InitializeComponent();
            contextAdmin = admin;
            SetUp();            
        }
        private async void SetUp()
        {
            await Services.DBConnection.InitializeDBConnection();
            admins = DBConnection.admins.ToList();
            stajer = DBConnection.stajers.ToList();
            stajer = stajer.Where(x => x.Id_Admin == contextAdmin.Id_admin).ToList();
            StajersLV.ItemsSource = stajer;
            this.DataContext = this;
        }

        private void ProfileBT_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Stajer stajer = (Stajer)button.DataContext;
            NavigationService.Navigate(new StajerProfilePage(stajer));
        }

        private void StajersTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new ViewStajersPage(contextAdmin));
        }

        private void MenuTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddStajerProfilePage());
        }

       

        private async void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var stajer = (sender as Button).DataContext as Stajer;
            await NetManager.Delete<bool>($"api/Stajer/Delete/{stajer.Id_Stajer}");
            await DBConnection.RefreshEnums();
            SetUp();
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
