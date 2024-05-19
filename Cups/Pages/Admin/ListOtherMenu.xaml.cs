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
    /// Логика взаимодействия для ListOtherMenu.xaml
    /// </summary>
    public partial class ListOtherMenu : Page
    {
        Admin contextAdmin;
        public static List<Models.Type> types { get; set; }
        public Cups.Models.Type contextType;
        public static List<Models.Menu> menus { get; set; }
        public ListOtherMenu(string typeName)
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
            var a = DBConnection.types.FirstOrDefault(t => t.Name == typeName);
            if (a != null)
            {
                contextType = a; 
            }
            SetUp();
        }

        private async void SetUp()
        {
            await Services.DBConnection.InitializeDBConnection();
            types = DBConnection.types.ToList();
            menus = DBConnection.menu.ToList();
            menus = menus.Where(m => m.Kod_Type == contextType.Kod_Type).ToList();
            NameTB.Text = contextType.Name;
            DrinksLV.ItemsSource = menus;
            this.DataContext = this;
        } 
        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            if(contextType.Kod_Type == 7 || contextType.Kod_Type == 8 || contextType.Kod_Type == 9 ||
                contextType.Kod_Type == 10 || contextType.Kod_Type == 11 || contextType.Kod_Type == 12)
            {
                NavigationService.Navigate(new EatPage());
            }
            else
            {
                NavigationService.Navigate(new DesertsPage());
            }
        }



        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTB.Text;
            NavigationService.Navigate(new AddPositionPage(name));
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

        private void ViewPositionBT_Click(object sender, RoutedEventArgs e)
        {
            var dish = (sender as Button).DataContext as Models.Menu;
            if (dish != null)
            {
                NavigationService.Navigate(new ViewPositionPage(dish));

            }
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await Services.DBConnection.InitializeDBConnection();
        }

        private async void DeleteBT_Click(object sender, RoutedEventArgs e)
        {
            var position = (sender as Button).DataContext as Models.Menu;
            await NetManager.Delete<bool>($"api/Menu/Delete/{position.Id_Menu}");
            await DBConnection.RefreshEnums();
            SetUp();
        }
        private void ExitTI_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DBConnection.loginedAdmin = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
