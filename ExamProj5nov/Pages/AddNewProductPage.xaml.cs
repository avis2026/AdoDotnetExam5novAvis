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
            try
            {
                using (var db = new Exam5novemEntities())
                {
                    //добавить проверки
                    Products product = new Products();
                    db.Products.Add(product);

                    product.Name = Name.Text;
                    product.Category = Convert.ToInt32(Category.Text);
                    product.Price = Convert.ToInt32(Price.Text);
                    product.StockQuantity = Convert.ToInt32(StockQuantity.Text);
                    db.SaveChanges();
                    NavigationService.Navigate(new ProductsPage());
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) 
        {
            NavigationService.GoBack();
        }
    }
}
