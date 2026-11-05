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

namespace ExamProj5nov.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            LoadClients();
        }
        private void LoadClients(string searchName = null, string category = null, bool? available = null)
        {
            var db = new Exam5novemEntities();

            var query = db.Clients.AsQueryable();

            //по названию
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(e => e.FullName.ToLower().Contains(searchName.ToLower()));
            }

            if (RoleFilterBox.SelectedIndex == 1)
            {
                query = query.Where(c => c.Role == 1);
            }
            else if (RoleFilterBox.SelectedIndex == 2)
            {
                query = query.Where(c => c.Role == 2);
            }

            switch (SortBox.SelectedIndex)
            {
                case 0:
                    query = query.OrderBy(x => x.FullName);
                    break;
                case 1:
                    query = query.OrderByDescending(x => x.FullName);
                    break;
                case 2:
                    query = query.OrderBy(x => x.Role);
                    break;
                case 3:
                    query = query.OrderByDescending(x => x.Role);
                    break;
            }

            var ClientsList = query.ToList();

            if (ClientsList.Count == 0)
            {
                MessageBox.Show("По вашему запросу ничего не найдено.", "Результат",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ProductsGrid.ItemsSource = ClientsList;
        }
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            RoleFilterBox.SelectedIndex = 0;
            
            SortBox.SelectedIndex = 0;

            LoadClients();
        }

        private void AddNewClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewClientPage());
        }

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var client = button?.DataContext as Clients;
            NavigationService.Navigate(new EditClientPage(client));
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var client = button?.DataContext as Clients;

            if (client == null)
            {
                return;
            }

            var result = MessageBox.Show("Точно удалить?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            using (var db = new Exam5novemEntities())
            {
                var clientDeleted = db.Clients.Find(client.Id);
                db.Clients.Remove(clientDeleted);
                db.SaveChanges();

                LoadClients();
            }
        }
    }
}
