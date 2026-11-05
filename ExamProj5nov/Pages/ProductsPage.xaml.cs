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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts(string searchName = null, string category = null, bool? available = null)
        {
            var db = new Exam5novemEntities();

            var query = db.Products.AsQueryable();

            //по названию
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(e => e.Name.ToLower().Contains(searchName.ToLower()));
            }

            //по категории
            if (FilterBox.SelectedIndex > 0)
            {
                query = query.Where(e => e.Category == FilterBox.SelectedIndex);
            }

            //по доступности
            if (AvailabilityFilter.SelectedIndex == 1) //Доступно
            {
                query = query.Where(e => e.StockQuantity > 0);
            }
            else if (AvailabilityFilter.SelectedIndex == 2) //Недоступно
            {
                query = query.Where(e => e.StockQuantity == 0);
            }

            switch (SortBox.SelectedIndex)
            {
                case 0:
                    query = query.OrderBy(x => x.Name);
                    break;
                case 1:
                    query = query.OrderByDescending(x => x.Name);
                    break;
                case 2:
                    query = query.OrderBy(x => x.Category);
                    break;
                case 3:
                    query = query.OrderByDescending(x => x.Category);
                    break;
            }

            var ProductsList = query.ToList();

            if (ProductsList.Count == 0)
            {
                MessageBox.Show("По вашему запросу ничего не найдено.", "Результат",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ProductsGrid.ItemsSource = ProductsList;
        }
        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            FilterBox.SelectedIndex = 0;
            AvailabilityFilter.SelectedIndex = 0;
            SortBox.SelectedIndex = 0;

            LoadProducts();
        }

        private void AddNewCar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddNewProductPage());
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Products;

            if (product == null)
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
                var productDeleted = db.Products.Find(product.Id);
                db.Products.Remove(productDeleted);
                db.SaveChanges();

                LoadProducts();
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button?.DataContext as Products;
            NavigationService.Navigate(new EditProductPage(product));
        }
    }
}
