using System;
using System.Collections;
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
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {
        private readonly Products current_product;
        public EditProductPage(Products product)
        {
            InitializeComponent();
            current_product = product;

            Name.Text = current_product.Name;
            Category.Text = current_product.Category.ToString();
            Price.Text = current_product.Price.ToString();
            StockQuantity.Text = current_product.StockQuantity.ToString();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Category.Text) ||
                string.IsNullOrWhiteSpace(Price.Text) ||
                string.IsNullOrWhiteSpace(StockQuantity.Text)) 
            {
                MessageBox.Show("Нужно заполнить все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //если не получилось преобразовать ввод юзера в число - просим ввести число
            if (!int.TryParse(Category.Text, out int category) ||
                !int.TryParse(Price.Text, out int price) ||
                !int.TryParse(StockQuantity.Text, out int stock))
            {
                MessageBox.Show("Категория, цена и количество должны быть числами!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var db = new Exam5novemEntities())
                {
                    var editProduct = db.Products.Find(current_product.Id);

                    if (editProduct != null)
                    {
                        editProduct.Name = Name.Text;
                        editProduct.Category = category;
                        editProduct.Price = price;
                        editProduct.StockQuantity = stock;

                        db.SaveChanges();
                        NavigationService.Navigate(new ProductsPage());
                    }
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
