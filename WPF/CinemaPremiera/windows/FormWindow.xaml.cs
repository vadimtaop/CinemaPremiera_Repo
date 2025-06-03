using CinemaPremiera.ADO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Логика взаимодействия для FormWindow.xaml
    /// </summary>
    public partial class FormWindow : Window
    {
        public FormWindow()
        {
            InitializeComponent();

            LoadFilm();
            LoadPriceList();
            LoadPaymentType();
        }

        private void BtnClick_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                Orders orders = new Orders();

                orders.DateBuy = Dpicker_DateBuy.SelectedDate.Value;
                if (Cbox_Film.SelectedItem is Film selectedFilm)
                {
                    orders.ID_Film = selectedFilm.ID;
                }
                else
                {
                    throw new Exception("Фильм не найден.");
                }
                orders.DateSession = Dpicker_DateSession.SelectedDate.Value;
                if (Cbox_PriceList.SelectedItem is PriceList itemPriceList)
                {
                    orders.ID_PriceList = itemPriceList.ID;
                }
                else
                {
                    throw new Exception("Не выбрана цена.");
                }
                orders.Count = int.Parse(Tbox_Count.Text);
                orders.CheckSum = decimal.Parse(Tbox_CheckSum.Text);
                if (Cbox_PaymentType.SelectedItem is PaymentType itemPaymentType)
                {
                    orders.ID_PaymentType = itemPaymentType.ID;
                }
                else
                {
                    throw new Exception("Не выбран способ оплаты.");
                }
                orders.Note = Tbox_Note.Text;

                AppData.db.Orders.Add(orders);
                AppData.db.SaveChanges();
                MessageBox.Show("Данные успешно добавлены.", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Tc_CalcCheckSum(object sender, EventArgs e)
        {
            // 1. Получаем цену (правильный способ)
            decimal price = GetSelectedPrice();

            // 2. Получаем количество
            int count = GetCount();

            // 3. Вычисляем сумму
            decimal total = price * count;

            // 4. Выводим результат
            Tbox_CheckSum.Text = total.ToString("0.00");

        }
        private decimal GetSelectedPrice()
        {
            if (Cbox_PriceList.SelectedItem == null)
                return 0;

            var priceProperty = Cbox_PriceList.SelectedItem.GetType().GetProperty("Price");
            if (priceProperty != null)
                return (decimal)priceProperty.GetValue(Cbox_PriceList.SelectedItem);

            return 0;
        }
        private int GetCount()
        {
            return int.TryParse(Tbox_Count.Text, out int count) ? count : 0;
        }



        // Загружаем все данные в ComboBox из БД
        private void LoadFilm()
        {
            try
            {
                // Получаем все фильмы из БД и сортируем по названию
                var films = AppData.db.Film.OrderBy(f => f.Title).ToList();

                // Назначем источник данных для ComboBox
                Cbox_Film.ItemsSource = films;
                // Указываем какое поле отображать (Title)
                Cbox_Film.DisplayMemberPath = "Title";
                // Указываем какое поле будет значением (ID)
                Cbox_Film.SelectedValuePath = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadPriceList()
        {
            try
            {
                // Получаем все цены из БД и сортируем по названию
                var price = AppData.db.PriceList.OrderBy(p => p.Price).ToList();

                // Назначем источник данных для ComboBox
                Cbox_PriceList.ItemsSource = price;
                // Указываем какое поле отображать (Price)
                Cbox_PriceList.DisplayMemberPath = "Price";
                // Указываем какое поле будет значением (ID)
                Cbox_PriceList.SelectedValuePath = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadPaymentType()
        {
            try
            {
                // Получаем все спобосы оплаты из БД и сортируем по названию
                var paymentType = AppData.db.PaymentType.OrderBy(f => f.Title).ToList();

                // Назначем источник данных для ComboBox
                Cbox_PaymentType.ItemsSource = paymentType;
                // Указываем какое поле отображать (Title)
                Cbox_PaymentType.DisplayMemberPath = "Title";
                // Указываем какое поле будет значением (ID)
                Cbox_PaymentType.SelectedValuePath = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
