using Cups.Models;
using Cups.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
using Type = Cups.Models.Type;

namespace Cups.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListDrinksPage.xaml
    /// </summary>
    public partial class ListDrinksPage : Page
    {
        public static List<Type> types { get; set; }
        public Cups.Models.Type contextType;
        public static List<Models.Menu> menus { get; set; }
        Admin contextAdmin;
        public ListDrinksPage(string typeName)
        {
            InitializeComponent();
            contextAdmin = DBConnection.loginedAdmin;
            var a = DBConnection.types.FirstOrDefault(t => t.Name == typeName);
            if (a != null)
            {
                contextType = a; // Присваиваем сам объект типа Type
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

        private void BackBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DrinksPage());
        }       

        private void ViewDrinkBT_Click(object sender, RoutedEventArgs e)
        {
            var drink = (sender as Button).DataContext as Models.Menu;
            if (drink != null)
            {
                NavigationService.Navigate(new ViewDrinkAdmin(drink));
            }
        }

        private void AddBT_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTB.Text;
            NavigationService.Navigate(new AddDrinkPage(name));
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
