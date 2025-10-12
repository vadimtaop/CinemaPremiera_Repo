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
    /// Логика взаимодействия для PriceListPage.xaml
    /// </summary>
    public partial class PriceListPage : Page
    {
        public PriceListPage()
        {
            InitializeComponent();
            DG_PriceLists.ItemsSource = AppData.db.PriceList.OrderByDescending(o => o.Price).ToList();
        }

        private void BtnClick_ResetFilters(object sender, RoutedEventArgs e)
        {
            Tbox_Price.Text = "";
            DG_PriceLists.ItemsSource = AppData.db.PriceList.OrderByDescending(o => o.Price).ToList();
        }
        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string price_Text = Tbox_Price.Text;

            // Получаем все строки из таблицы PriceList (БД)
            var DataPriceLists = AppData.db.PriceList.ToList();

            // Ищем цены по всем фильтрам
            var filteredPriceLists = DataPriceLists.Where(o =>
                                    // Фильтры
                                    (string.IsNullOrEmpty(price_Text) || o.Price.ToString().Contains(price_Text))
                                    ).OrderByDescending(o => o.Price).ToList();

            DG_PriceLists.ItemsSource = filteredPriceLists;
        }
        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            AddPriceListWindow addPriceListWindow = new AddPriceListWindow();
            addPriceListWindow.ShowDialog();
        }
        // Удаление для нескольких строк
        private void BtnClick_Delete(object sender, RoutedEventArgs e)
        {
            var priceListsToDelete = new List<ADO.PriceList>();

            // Собираем отмеченные записи
            foreach (var item in DG_PriceLists.Items)
            {
                var row = DG_PriceLists.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox?.IsChecked == true)
                    {
                        priceListsToDelete.Add(item as ADO.PriceList);
                    }
                }
            }

            if (priceListsToDelete.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одного значения для удаления.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {priceListsToDelete.Count} цен(у)?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            // Если пользователь не подтвердил удаление
            if (result != MessageBoxResult.Yes)
            {
                MessageBox.Show("Удаление отменено", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Удаляем выбранные цены
            try
            {
                foreach (var priceList in priceListsToDelete)
                {
                    AppData.db.PriceList.Remove(priceList);
                    AppData.db.SaveChanges();

                    DG_PriceLists.ItemsSource = AppData.db.PriceList.OrderByDescending(o => o.Price).ToList(); // Обновляем таблицу
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
        // Одиночное удаление
        private void BtnClick_TrashDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий цену из строки, где находится кнопка
                var button = sender as Button;
                var priceList = button.DataContext as ADO.PriceList;

                if (priceList == null)
                {
                    MessageBox.Show("Не удалось получить данные цены для удаления.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить цену {priceList.Price}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Удаляем заказ
                AppData.db.PriceList.Remove(priceList);
                AppData.db.SaveChanges();

                MessageBox.Show("Цена успешна удалена.", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClick_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем объект данных из строки, где находится кнопка
                var button = sender as Button;
                var priceList = button.DataContext as ADO.PriceList;

                if (priceList == null)
                {
                    MessageBox.Show("Не удалось получить данные цены.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем и настраиваем окно редактирования

                var editPriceListWindow = new AddPriceListWindow();

                // Передаем ID цены, который редактируется
                editPriceListWindow.EditingPriceListId = priceList.PriceList_ID;

                // Заполняем поля
                editPriceListWindow.Tbox_Price.Text = priceList.Price.ToString();

                // Скрываем кнопку "добавить" и показываем кнопку "сохранить"
                editPriceListWindow.Btn_Add.Visibility = Visibility.Collapsed;
                editPriceListWindow.Btn_Save.Visibility = Visibility.Visible;

                // Открываем окно
                editPriceListWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
