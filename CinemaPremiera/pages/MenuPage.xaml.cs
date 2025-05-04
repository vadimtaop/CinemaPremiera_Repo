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
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();

            DG_Orders.ItemsSource = AppData.db.Orders.ToList();
        }

        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string dateBuyS_Text = Dpicker_DateBuyS.Text;
            string dateBuyPo_Text = Dpicker_DateBuyPo.Text;
            string film_Text = Tbox_Film.Text;
            string priceList_Tag = (Сbox_PriceList.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            string count_Text = Tbox_Count.Text;
            string checkSum_Text = Tbox_CheckSum.Text;
            string paymentType_Tag = (Сbox_PaymentType.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            string note_Text = Tbox_Note.Text;
            string searchText = Tbox_Search.Text.ToLower();

            // Получаем все строки из таблицы Orders (БД)
            var DataOrders = AppData.db.Orders.ToList();

            // Фильтруем заказы по всем фильтрам
            var filteredOrders = DataOrders.Where(o =>
                                    // Фильтры
                                    (string.IsNullOrEmpty(dateBuyS_Text) || o.DateBuy.ToString("d").Contains(dateBuyS_Text)) && // Поиск по дате С и ПО работает не правильно!
                                    (string.IsNullOrEmpty(dateBuyPo_Text) || o.DateBuy.ToString("d").Contains(dateBuyPo_Text)) && // Поиск по дате С и ПО работает не правильно!
                                    (string.IsNullOrEmpty(film_Text) || (o.Film != null && o.Film.Title.Contains(film_Text))) &&
                                    (string.IsNullOrEmpty(priceList_Tag) || (o.PriceList != null && o.PriceList.ID.ToString().Contains(priceList_Tag))) &&
                                    (string.IsNullOrEmpty(count_Text) || o.Count.ToString().Contains(count_Text)) &&
                                    (string.IsNullOrEmpty(checkSum_Text) || o.CheckSum.ToString().Contains(checkSum_Text)) &&
                                    (string.IsNullOrEmpty(paymentType_Tag) || (o.PaymentType != null && o.PaymentType.ID.ToString().Contains(paymentType_Tag))) &&
                                    (string.IsNullOrEmpty(note_Text) || (o.Note != null && o.Note.Contains(note_Text))) &&
                                    // Поиск
                                    (string.IsNullOrEmpty(searchText) ||
                                        o.DateBuy.ToString("d").ToLower().Contains(searchText) ||
                                        (o.Film != null && o.Film.Title.ToLower().Contains(searchText)) ||
                                        (o.DateSession.ToString("d").Contains(searchText)) ||
                                        (o.PriceList != null && o.PriceList.Price.ToString().Contains(searchText)) ||
                                        (o.Count.ToString().Contains(searchText)) ||
                                        (o.CheckSum.ToString().Contains(searchText)) ||
                                        (o.PaymentType != null && o.PaymentType.Title.ToLower().Contains(searchText)) ||
                                        (o.Note?.ToLower().Contains(searchText) ?? false))

                                    ).ToList();

            DG_Orders.ItemsSource = filteredOrders;




            //// Если строка поиска пустая, то показываем все записи
            //if (string.IsNullOrWhiteSpace(searchText))
            //{
            //    DG_Orders.ItemsSource = DataOrders;
            //    return;
            //}
        }
    }
}
