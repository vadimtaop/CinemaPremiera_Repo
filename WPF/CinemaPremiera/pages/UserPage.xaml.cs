using CinemaPremiera.ADO;
using CinemaPremiera.windows;
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

namespace CinemaPremiera.pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            LoadRole();
            DG_Users.ItemsSource = AppData.db.Auth.ToList();
        }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }

        // Одиночное удаление
        private void BtnClick_TrashDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущего пользователя из строки, где находится кнопка
                var button = sender as Button;
                var user = button.DataContext as ADO.Auth;

                if (user == null)
                {
                    MessageBox.Show("Не удалось получить данные пользователя для удаления.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить пользователя №{user.Auth_ID}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Удаляем заказ
                AppData.db.Auth.Remove(user);
                AppData.db.SaveChanges();

                MessageBox.Show("Пользователь успешно удален.", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Удаление для нескольких строк
        private void BtnClick_Delete(object sender, RoutedEventArgs e)
        {
            var usersToDelete = new List<ADO.Auth>();

            // Собираем отмеченные записи
            foreach (var item in DG_Users.Items)
            {
                var row = DG_Users.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox?.IsChecked == true)
                    {
                        usersToDelete.Add(item as ADO.Auth);
                    }
                }
            }

            if (usersToDelete.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одного значения для удаления.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {usersToDelete.Count} пользователя(ей)?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            // Если пользователь не подтвердил удаление
            if (result != MessageBoxResult.Yes)
            {
                MessageBox.Show("Удаление отменено", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Удаляем выбранные заказы
            try
            {
                foreach (var user in usersToDelete)
                {
                    AppData.db.Auth.Remove(user);
                    AppData.db.SaveChanges();

                    DG_Users.ItemsSource = AppData.db.Auth.ToList(); // Обновляем таблицу
                    MessageBox.Show("Удаление завершено.", "Информация",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Вспомогательный метод для поиска CheckBox в строке
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T) return (T)child;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }
        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string auth_ID_Text = Tbox_Auth_ID.Text;
            string pincode_Text = Tbox_Pincode.Text;
            string role_Text = Cbox_Role.Text;

            // Получаем все строки из таблицы Auth (БД)
            var DataUsers = AppData.db.Auth.ToList();

            // Ищем пользователей по всем фильтрам
            var filteredUsers = DataUsers.Where(o =>
                                    // Фильтры
                                    (string.IsNullOrEmpty(auth_ID_Text) || o.Auth_ID.ToString().Contains(auth_ID_Text)) &&
                                    (string.IsNullOrEmpty(pincode_Text) || o.Pincode.ToString().Contains(pincode_Text)) &&
                                    (string.IsNullOrEmpty(role_Text) || (o.Role != null && o.Role.Title.ToString().Contains(role_Text)))
                                    ).ToList();

            DG_Users.ItemsSource = filteredUsers;
        }
        private void BtnClick_ResetFilters(object sender, RoutedEventArgs e)
        {
            Tbox_Auth_ID.Text = "";
            Tbox_Pincode.Text = "";
            Cbox_Role.Text = "";
            DG_Users.ItemsSource = AppData.db.Auth.ToList();
        }
        private void BtnClick_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем объект данных из строки, где находится кнопка
                var button = sender as Button;
                var user = button.DataContext as ADO.Auth;

                if (user == null)
                {
                    MessageBox.Show("Не удалось получить данные пользователя.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем и настраиваем окно редактирования

                var editUserWindow = new AddUserWindow();

                // Передаем ID пользователя, который редактируется
                editUserWindow.EditingUserId = user.Auth_ID;

                // Заполняем поля
                editUserWindow.Tbox_Auth_ID.Text = user.Auth_ID.ToString();
                editUserWindow.Tbox_Pincode.Text = user.Pincode.ToString();
                editUserWindow.Cbox_Role.SelectedValue = user.Role_ID;

                // Скрываем кнопку "добавить" и показываем кнопку "сохранить"
                editUserWindow.Btn_Add.Visibility = Visibility.Collapsed;
                editUserWindow.Btn_Save.Visibility = Visibility.Visible;

                editUserWindow.Tb_Auth_ID.Visibility = Visibility.Visible;
                editUserWindow.Tbox_Auth_ID.Visibility = Visibility.Visible;

                // Открываем окно
                editUserWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Загружаем все данные в ComboBox из БД
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
