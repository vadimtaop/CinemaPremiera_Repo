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
            // Получаем текст для поиска (без учета регистра)
            string searchText = Tbox_Search.Text.ToLower();

            // Получаем все строки из таблицы Orders
            var DataOrders = AppData.db.Orders.ToList();

            // Если строка поиска пустая, то показываем все записи
            if (string.IsNullOrWhiteSpace(searchText))
            {
                DG_Orders.ItemsSource = DataOrders;
                return;
            }

            // Фильтруем заказы по всем полям
            var filteredOrders = DataOrders.Where(o => o.DateBuy.ToString("d").ToLower().Contains(searchText) ||
                                                       (o.Film != null && o.Film.Title.ToLower().Contains(searchText)) ||
                                                       (o.DateSession.ToString("d").Contains(searchText)) ||
                                                       (o.PriceList != null && o.PriceList.Price.ToString().Contains(searchText)) ||
                                                       (o.Count.ToString().Contains(searchText)) ||
                                                       (o.CheckSum.ToString().Contains(searchText)) ||
                                                       (o.PaymentType != null && o.PaymentType.Title.ToLower().Contains(searchText)) ||
                                                       (o.Note?.ToLower().Contains(searchText) ?? false)).ToList();
            // Вывод отфильтрованных данных
            DG_Orders.ItemsSource = filteredOrders;
        }
    }
}
