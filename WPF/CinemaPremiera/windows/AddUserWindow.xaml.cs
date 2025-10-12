using CinemaPremiera.ADO;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Windows.Shapes;

namespace CinemaPremiera.windows
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            LoadRole();
        }

        private void BtnClick_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Tbox_Pincode.Text.Length == 4)
                {
                    Auth users = new Auth();

                    users.Pincode = int.Parse(Tbox_Pincode.Text);
                    if (Cbox_Role.SelectedItem is Role itemRole)
                    {
                        users.Role_ID = itemRole.Role_ID;
                    }
                    else
                    {
                        throw new Exception("Не выбрана роль.");
                    }

                    AppData.db.Auth.Add(users);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные успешно добавлены.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пин-код должен состоять из 4 цифр.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public int EditingUserId { get; set; }
        private void BtnClick_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите внести изменения?", "Предупреждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Получаем ID редактируемого пользователя
                    int userID = this.EditingUserId;

                    // Находим пользователя в БД
                    var user = AppData.db.Auth.FirstOrDefault(o => o.Auth_ID == userID);

                    // Сохраняем изменения
                    if (user != null)
                    {
                        if (Tbox_Pincode.Text.Length == 4)
                        {
                            user.Pincode = int.Parse(Tbox_Pincode.Text);
                            if (Cbox_Role.SelectedItem is Role itemRole)
                            {
                                user.Role_ID = itemRole.Role_ID;
                            }
                            else
                            {
                                throw new Exception("Не выбрана роль.");
                            }

                            AppData.db.SaveChanges();

                            // Закрываем окно
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Пин-код должен состоять из 4 цифр.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadRole()
        {
            try
            {
                // Получаем все роли из БД и сортируем по названию
                var roles = AppData.db.Role.OrderBy(f => f.Title).ToList();

                // Назначем источник данных для ComboBox
                Cbox_Role.ItemsSource = roles;
                // Указываем какое поле отображать (Title)
                Cbox_Role.DisplayMemberPath = "Title";
                // Указываем какое поле будет значением (Role_ID)
                Cbox_Role.SelectedValuePath = "Role_ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
