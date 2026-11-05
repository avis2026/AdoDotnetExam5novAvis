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
    /// Логика взаимодействия для EditClientPage.xaml
    /// </summary>
    public partial class EditClientPage : Page
    {
        private readonly Clients current_client;
        public EditClientPage(Clients client)
        {
            InitializeComponent();
            current_client = client;

            FullName.Text = current_client.FullName;
            Email.Text = current_client.Email;
            Password.Text = current_client.Password;
            Role.Text = current_client.Role.ToString();
        }
        private void SaveClient_Click(object sender, RoutedEventArgs e)
        {
            //проверка на пустоту
            if (string.IsNullOrWhiteSpace(FullName.Text) ||
                string.IsNullOrWhiteSpace(Email.Text) ||
                string.IsNullOrWhiteSpace(Password.Text) ||
                string.IsNullOrWhiteSpace(PhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(Role.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Email.Text.Contains("@") || !Email.Text.Contains("."))
            {
                MessageBox.Show("Введите корректный Email!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(!long.TryParse(PhoneNumber.Text, out _))
            {
                MessageBox.Show("Номер телефона должен состоять только из цифр!", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(Role.Text, out int role) || (role != 1 && role != 2))
            {
                MessageBox.Show("Роль должна быть указана цифрой: 1 - админ или 2 - пользователь", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using(var db = new Exam5novemEntities())
                {
                    Clients client = new Clients();

                    client.FullName = FullName.Text;
                    client.Email = Email.Text;
                    client.Password = Password.Text;
                    client.PhoneNumber = PhoneNumber.Text;
                    client.Role = role;

                    db.Clients.Add(client);
                    db.SaveChanges();

                    NavigationService.Navigate(new ClientsPage());

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.Message}");
            }
            
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
