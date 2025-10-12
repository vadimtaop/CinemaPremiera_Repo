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
using System.Windows.Shapes;

namespace CinemaPremiera.windows
{
    /// <summary>
    /// Логика взаимодействия для AddPriceListWindow.xaml
    /// </summary>
    public partial class AddPriceListWindow : Window
    {
        public AddPriceListWindow()
        {
            InitializeComponent();
        }

        private void BtnClick_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public int EditingPriceListId { get; set; }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                PriceList priceLists = new PriceList();

                priceLists.Price = int.Parse(Tbox_Price.Text);

                AppData.db.PriceList.Add(priceLists);
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
        private void BtnClick_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите внести изменения?", "Предупреждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Получаем ID редактируемой цены
                    int priceListID = this.EditingPriceListId;

                    // Находим цену в БД
                    var priceList = AppData.db.PriceList.FirstOrDefault(o => o.PriceList_ID == priceListID);

                    // Сохраняем изменения
                    if (priceList != null)
                    {
                        priceList.Price = int.Parse(Tbox_Price.Text);

                        AppData.db.SaveChanges();

                        // Закрываем окно
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
