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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamProj5nov.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddNewProductPage.xaml
    /// </summary>
    public partial class AddNewProductPage : Page
    {
        public AddNewProductPage()
        {
            InitializeComponent();
        }
        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            //проверка на пустоту
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Category.Text) ||
                string.IsNullOrWhiteSpace(Price.Text) ||
                string.IsNullOrWhiteSpace(StockQuantity.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //защита от ввода букв в категории, цене и количестве
            if (!int.TryParse(Category.Text, out int category) ||
                !int.TryParse(Price.Text, out int price) ||
                !int.TryParse(StockQuantity.Text, out int stock))
            {
                MessageBox.Show("Категория, цена и количество на складе должны быть числами!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //безопасное сохранение в базу
            try
            {
                using (var db = new Exam5novemEntities())
                {
                    Products product = new Products();

                    product.Name = Name.Text; 
                    product.Category = category;
                    product.Price = price;
                    product.StockQuantity = stock;

                    db.Products.Add(product);
                    db.SaveChanges();

                    NavigationService.Navigate(new ProductsPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.Message}");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) 
        {
            NavigationService.GoBack();
        }
    }
}
