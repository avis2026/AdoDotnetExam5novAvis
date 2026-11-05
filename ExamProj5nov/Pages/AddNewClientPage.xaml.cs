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
    /// Логика взаимодействия для AddNewClientPage.xaml
    /// </summary>
    public partial class AddNewClientPage : Page
    {
        public AddNewClientPage()
        {
            InitializeComponent();
        }

        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            //добавить проверки
            try
            {
                using (var db = new Exam5novemEntities())
                {
                    Clients client = new Clients();
                    db.Clients.Add(client);

                    client.FullName = FullName.Text;
                    client.Email = Email.Text;
                    client.Password = Password.Text;
                    client.PhoneNumber = PhoneNumber.Text;
                    db.SaveChanges();
                    NavigationService.Navigate(new ClientsPage());
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
