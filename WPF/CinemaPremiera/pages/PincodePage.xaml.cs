using CinemaPremiera.ADO;
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
    /// Логика взаимодействия для PincodePage.xaml
    /// </summary>
    public partial class PincodePage : Page
    {
        public PincodePage()
        {
            InitializeComponent();
            _ = LoadPincodeAsync();
        }

        private async Task LoadPincodeAsync()
        {
            try
            {
                // Пересоздаем контекст БД
                var db = new CinemaPremieraDBEntities();

                var truePincode = await Task.Run(() => db.Auth.AsNoTracking().FirstOrDefault()?.Pincode);

                this.TruePincode = truePincode;
                db.Dispose(); // Закрываем подключение
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пин-кода: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                this.TruePincode = null;
            }
        }
        private int? TruePincode { get; set; }

        private async void Tbox_Pincode_Tc(object sender, TextChangedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length == 4)
            {
                try
                {
                    // Показываем индикатор загрузки
                    Spanel_Loading.Visibility = Visibility.Visible;
                    Tbox_Pincode.IsEnabled = false;

                    // Ждем завершения загрузки пин-кода (если еще не загружен)
                    if (TruePincode == null)
                    {
                        await LoadPincodeAsync();
                    }

                    // Искусственная задержка
                    await Task.Delay(300);

                    if (int.TryParse(Tbox_Pincode.Text, out int enteredPin) && enteredPin == TruePincode)
                    {
                        NavigationService.Navigate(new MenuPage());

                        if (Application.Current.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.Btn_Menu.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введен неверный PIN-код.\nПовторите попытку.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);

                        Tbox_Pincode.Clear();
                    }
                }
                finally
                {
                    // Скрываем индикатор загрузки
                    Spanel_Loading.Visibility = Visibility.Collapsed;
                    Tbox_Pincode.IsEnabled = true;
                }
            }




        }

        // Кнопки с цифрами

        private void BtnClick_One(object sender, RoutedEventArgs e)
        {
            if(Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "1";
            }
        }
        private void BtnClick_Two(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "2";
            }
        }
        private void BtnClick_Three(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "3";
            }
        }
        private void BtnClick_Four(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "4";
            }
        }
        private void BtnClick_Five(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "5";
            }
        }
        private void BtnClick_Six(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "6";
            }
        }
        private void BtnClick_Seven(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "7";
            }
        }
        private void BtnClick_Eight(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "8";
            }
        }
        private void BtnClick_Nine(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "9";
            }
        }
        private void BtnClick_Zero(object sender, RoutedEventArgs e)
        {
            if (Tbox_Pincode.Text.Length < 4)
            {
                Tbox_Pincode.Text += "0";
            }
        }
        private void BtnClick_Backspace(object sender, RoutedEventArgs e)
        {
            if(Tbox_Pincode.Text.Length > 0)
            {
                Tbox_Pincode.Text = Tbox_Pincode.Text.Substring(0, Tbox_Pincode.Text.Length - 1);
            }
        }
    }
}
