using CinemaPremiera.ADO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Win32;
using System;
using System.Collections;
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
using System.Data.Entity;
using CinemaPremiera.windows;
using ControlzEx.Standard;

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

            LoadFilm();
            LoadPriceList();
            LoadPaymentType();

            DG_Orders.ItemsSource = AppData.db.Orders.ToList();
        }

        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string dateBuyS_Text = Dpicker_DateBuyS.Text;
            string dateBuyPo_Text = Dpicker_DateBuyPo.Text;
            string film_Text = Cbox_Film.Text;
            string dateSession_Text = Dpicker_DateSession.Text;
            string priceList_Text = Cbox_PriceList.Text;
            string count_Text = Tbox_Count.Text;
            string checkSum_Text = Tbox_CheckSum.Text;
            string paymentType_Text = Cbox_PaymentType.Text;
            string note_Text = Tbox_Note.Text;
            string searchText = Tbox_Search.Text.ToLower();

            // Получаем все строки из таблицы Orders (БД)
            var DataOrders = AppData.db.Orders.ToList();

            // Пытаемся распарсить даты (если введены)
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(dateBuyS_Text) && DateTime.TryParse(dateBuyS_Text, out var parsedStartDate))
            {
                startDate = parsedStartDate;
            }
            if(!string.IsNullOrEmpty(dateBuyPo_Text) && DateTime.TryParse(dateBuyPo_Text, out var parsedEndDate))
            {
                endDate = parsedEndDate;
            }

            // Фильтруем заказы по всем фильтрам
            var filteredOrders = DataOrders.Where(o =>
                                    // Фильтры
                                    (startDate == null || o.DateBuy >= startDate) &&
                                    (endDate == null || o.DateBuy <= endDate) &&
                                    (string.IsNullOrEmpty(dateSession_Text) || o.DateSession.ToString("d").Contains(dateSession_Text)) &&
                                    (string.IsNullOrEmpty(film_Text) || (o.Film != null && o.Film.Title.Contains(film_Text))) &&
                                    (string.IsNullOrEmpty(priceList_Text) || (o.PriceList != null && o.PriceList.Price.ToString().Contains(priceList_Text))) &&
                                    (string.IsNullOrEmpty(count_Text) || o.Count.ToString().Contains(count_Text)) &&
                                    (string.IsNullOrEmpty(checkSum_Text) || o.CheckSum.ToString().Contains(checkSum_Text)) &&
                                    (string.IsNullOrEmpty(paymentType_Text) || (o.PaymentType != null && o.PaymentType.Title.ToString().Contains(paymentType_Text))) &&
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
        }

        private void BtnClick_ResetFilters(object sender, RoutedEventArgs e)
        {
            Tbox_Search.Text = "";
            Dpicker_DateBuyS.Text = "";
            Dpicker_DateBuyPo.Text = "";
            Cbox_Film.Text = "";
            Dpicker_DateSession.Text = "";
            Cbox_PriceList.Text = "";
            Tbox_Count.Text = "";
            Tbox_CheckSum.Text = "";
            Cbox_PaymentType.Text = "";
            Tbox_Note.Text = "";
        }

        private void DateBuyS_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, есть ли выбранная дата в Dpicker_DateBuyS
            Dpicker_DateBuyPo.IsEnabled = Dpicker_DateBuyS.SelectedDate.HasValue;

            // Если дата в первом Dpicker сброшена, сбрасываем и второй Dpicker
            if (!Dpicker_DateBuyS.SelectedDate.HasValue)
            {
                Dpicker_DateBuyPo.SelectedDate = null;
            }
        }

        private void DateBuyPo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Dpicker_DateBuyPo.SelectedDate.HasValue &&
                Dpicker_DateBuyPo.SelectedDate < Dpicker_DateBuyS.SelectedDate)
            {
                MessageBox.Show("\"Дата покупки (по)\", не может быть" +
                    "\nраньше чем \"Дата покупки (с)\"", "Ошибка.",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                Dpicker_DateBuyPo.Text = "";
            }
        }

        private void BtnClick_ExportExcel_Full(object sender, RoutedEventArgs e)
        {
            try
            {
                var orders = AppData.db.Orders
                                .Include(o => o.Film)
                                .Include(o => o.PriceList)
                                .Include(o => o.PaymentType)
                                .ToList();

                if (orders == null || !orders.Any())
                {
                    MessageBox.Show("Нет данных для экспорта", "Информация",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"Заказы_(Полная)_{DateTime.Now:dd_MM_yyyy}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Заказы");

                        // Заголовки столбцов
                        var headers = new[]
                        {
                            "ID",
                            "Дата покупки",
                            "Название Фильма",
                            "Дата сеанса",
                            "Цена",
                            "Кол-во",
                            "Сумма в чеке",
                            "Вид оплаты",
                            "Примечание"
                        };

                        // Записываем заголовки
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = headers[i];
                        }

                        // Заполняем данные
                        int row = 2;
                        foreach (var order in orders)
                        {
                            // ID
                            worksheet.Cell(row, 1).Value = order.ID;

                            // Дата покупки (только дата)
                            if (order.DateBuy != null)
                            {
                                worksheet.Cell(row, 2).Value = order.DateBuy;
                                worksheet.Cell(row, 2).Style.DateFormat.Format = "dd.MM.yyyy";
                            }

                            // Название Фильма
                            worksheet.Cell(row, 3).Value = order.Film?.Title ?? "Не указано";

                            // Дата сеанса (только дата)
                            if (order.DateSession != null)
                            {
                                worksheet.Cell(row, 4).Value = order.DateSession;
                                worksheet.Cell(row, 4).Style.DateFormat.Format = "dd.MM.yyyy";
                            }

                            // Цена(из связанной таблицы PriceList, столбец Title)
                            worksheet.Cell(row, 5).Value = order.PriceList?.Price;
                            worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";

                            // Кол-во
                            worksheet.Cell(row, 6).Value = order.Count;

                            // Сумма в чеке
                            worksheet.Cell(row, 7).Value = order.CheckSum;
                            worksheet.Cell(row, 7).Style.NumberFormat.Format = "0.00";

                            // Вид оплаты (из связанной таблицы PaymentType, столбец Title)
                            worksheet.Cell(row, 8).Value = order.PaymentType?.Title ?? "Не указан";

                            // Примечание
                            worksheet.Cell(row, 9).Value = order.Note ?? string.Empty;

                            row++;
                        }

                        // Автоподбор ширины столбцов
                        worksheet.Columns().AdjustToContents();

                        // Сохранение файла
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Экспорт завершен успешно!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClick_ExportExcel_Filter(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем данные из используемых фильтров (TextBox) - те же самые, что и в BtnClick_Apply
                string dateBuyS_Text = Dpicker_DateBuyS.Text;
                string dateBuyPo_Text = Dpicker_DateBuyPo.Text;
                string film_Text = Cbox_Film.Text;
                string dateSession_Text = Dpicker_DateSession.Text;
                string priceList_Text = Cbox_PriceList.Text;
                string count_Text = Tbox_Count.Text;
                string checkSum_Text = Tbox_CheckSum.Text;
                string paymentType_Text = Cbox_PaymentType.Text;
                string note_Text = Tbox_Note.Text;
                string searchText = Tbox_Search.Text.ToLower();

                // Получаем все заказы с включенными связанными данными
                var orders = AppData.db.Orders
                                .Include(o => o.Film)
                                .Include(o => o.PriceList)
                                .Include(o => o.PaymentType)
                                .ToList();

                // Пытаемся распарсить даты (если введены)
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrEmpty(dateBuyS_Text) && DateTime.TryParse(dateBuyS_Text, out var parsedStartDate))
                {
                    startDate = parsedStartDate;
                }
                if (!string.IsNullOrEmpty(dateBuyPo_Text) && DateTime.TryParse(dateBuyPo_Text, out var parsedEndDate))
                {
                    endDate = parsedEndDate;
                }

                // Применяем те же фильтры, что и в BtnClick_Apply
                var filteredOrders = orders.Where(o =>
                                        // Фильтры
                                        (startDate == null || o.DateBuy >= startDate) &&
                                        (endDate == null || o.DateBuy <= endDate) &&
                                        (string.IsNullOrEmpty(dateSession_Text) || o.DateSession.ToString("d").Contains(dateSession_Text)) &&
                                        (string.IsNullOrEmpty(film_Text) || (o.Film != null && o.Film.Title.Contains(film_Text))) &&
                                        (string.IsNullOrEmpty(priceList_Text) || (o.PriceList != null && o.PriceList.Price.ToString().Contains(priceList_Text))) &&
                                        (string.IsNullOrEmpty(count_Text) || o.Count.ToString().Contains(count_Text)) &&
                                        (string.IsNullOrEmpty(checkSum_Text) || o.CheckSum.ToString().Contains(checkSum_Text)) &&
                                        (string.IsNullOrEmpty(paymentType_Text) || (o.PaymentType != null && o.PaymentType.Title.ToString().Contains(paymentType_Text))) &&
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

                if (filteredOrders == null || !filteredOrders.Any())
                {
                    MessageBox.Show("Нет данных для экспорта", "Информация",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"Заказы_(Фильтр)_{DateTime.Now:dd_MM_yyyy}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Заказы");

                        // Заголовки столбцов
                        var headers = new[]
                        {
                    "ID",
                    "Дата покупки",
                    "Название Фильма",
                    "Дата сеанса",
                    "Цена",
                    "Кол-во",
                    "Сумма в чеке",
                    "Вид оплаты",
                    "Примечание"
                };

                        // Записываем заголовки
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = headers[i];
                        }

                        // Заполняем данные
                        int row = 2;
                        foreach (var order in filteredOrders) // Используем filteredOrders вместо orders
                        {
                            // ID
                            worksheet.Cell(row, 1).Value = order.ID;

                            // Дата покупки (только дата)
                            if (order.DateBuy != null)
                            {
                                worksheet.Cell(row, 2).Value = order.DateBuy;
                                worksheet.Cell(row, 2).Style.DateFormat.Format = "dd.MM.yyyy";
                            }

                            // Название Фильма
                            worksheet.Cell(row, 3).Value = order.Film?.Title ?? "Не указано";

                            // Дата сеанса (только дата)
                            if (order.DateSession != null)
                            {
                                worksheet.Cell(row, 4).Value = order.DateSession;
                                worksheet.Cell(row, 4).Style.DateFormat.Format = "dd.MM.yyyy";
                            }

                            // Цена(из связанной таблицы PriceList, столбец Title)
                            worksheet.Cell(row, 5).Value = order.PriceList?.Price;
                            worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";

                            // Кол-во
                            worksheet.Cell(row, 6).Value = order.Count;

                            // Сумма в чеке
                            worksheet.Cell(row, 7).Value = order.CheckSum;
                            worksheet.Cell(row, 7).Style.NumberFormat.Format = "0.00";

                            // Вид оплаты (из связанной таблицы PaymentType, столбец Title)
                            worksheet.Cell(row, 8).Value = order.PaymentType?.Title ?? "Не указан";

                            // Примечание
                            worksheet.Cell(row, 9).Value = order.Note ?? string.Empty;

                            row++;
                        }

                        // Автоподбор ширины столбцов
                        worksheet.Columns().AdjustToContents();

                        // Сохранение файла
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show("Экспорт завершен успешно!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            FormWindow formWindow = new FormWindow();
            formWindow.ShowDialog();
        }
        private void BtnClick_TrashDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий заказ из строки, где находится кнопка
                var button = sender as Button;
                var order = button.DataContext as Orders;

                if (order == null)
                {
                    MessageBox.Show("Не удалось получить данные заказа для удаления.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить заказ №{order.ID}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Удаляем заказ
                AppData.db.Orders.Remove(order);
                AppData.db.SaveChanges();

                MessageBox.Show("Заказ успешно удален.", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnClick_Delete(object sender, RoutedEventArgs e)
        {
            var ordersToDelete = new List<Orders>();

            // Собираем отмеченные записи
            foreach (var item in DG_Orders.Items)
            {
                var row = DG_Orders.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox?.IsChecked == true)
                    {
                        ordersToDelete.Add(item as Orders);
                    }
                }
            }

            if (ordersToDelete.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одного значения для удаления.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {ordersToDelete.Count} заказов?",
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
                foreach (var order in ordersToDelete)
                {
                    AppData.db.Orders.Remove(order);
                    AppData.db.SaveChanges();

                    DG_Orders.ItemsSource = AppData.db.Orders.ToList(); // Обновляем таблицу
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

        private void BtnClick_Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем объект данных из строки, где находится кнопка
                var button = sender as Button;
                var order = button.DataContext as Orders;

                if (order == null)
                {
                    MessageBox.Show("Не удалось получить данные заказа.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем и настраиваем окно редактирования

                var editWindow = new FormWindow();

                // Передаем ID заказа, который редактируется
                editWindow.EditingOrderId = order.ID;

                // Заполняем поля
                editWindow.Dpicker_DateBuy.SelectedDate = order.DateBuy;
                editWindow.Cbox_Film.SelectedValue = order.ID_Film;
                editWindow.Dpicker_DateSession.SelectedDate = order.DateSession;
                editWindow.Cbox_PriceList.SelectedValue = order.ID_PriceList;
                editWindow.Tbox_Count.Text = order.Count.ToString();
                editWindow.Cbox_PaymentType.SelectedValue = order.ID_PaymentType;
                editWindow.Tbox_Note.Text = order.Note.ToString();

                // Скрываем кнопку "добавить" и показываем кнопку "сохранить"
                editWindow.Btn_Add.Visibility = Visibility.Collapsed;
                editWindow.Btn_Save.Visibility = Visibility.Visible;

                // Открываем окно
                editWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message , "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
