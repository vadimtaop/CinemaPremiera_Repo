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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();

            LoadFilm();
            LoadPriceList();
            LoadPaymentType();

            DG_Orders.ItemsSource = AppData.db.Order.ToList();
        }

        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string order_ID_Text = Tbox_Order_ID.Text;
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

            // Получаем все строки из таблицы Order (БД)
            var DataOrders = AppData.db.Order.ToList();

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

            // Ищем заказы по всем фильтрам
            var filteredOrders = DataOrders.Where(o =>
                                    // Фильтры
                                    (startDate == null || o.DateBuy >= startDate) &&
                                    (endDate == null || o.DateBuy <= endDate) &&
                                    (string.IsNullOrEmpty(order_ID_Text) || o.Order_ID.ToString().Contains(order_ID_Text)) &&
                                    (string.IsNullOrEmpty(dateSession_Text) || o.DateSession.ToString("d").Contains(dateSession_Text)) &&
                                    (string.IsNullOrEmpty(film_Text) || (o.Film != null && o.Film.Title.Contains(film_Text))) &&
                                    (string.IsNullOrEmpty(priceList_Text) || (o.PriceList != null && o.PriceList.Price.ToString().Contains(priceList_Text))) &&
                                    (string.IsNullOrEmpty(count_Text) || o.Count.ToString().Contains(count_Text)) &&
                                    (string.IsNullOrEmpty(checkSum_Text) || o.CheckSum.ToString().Contains(checkSum_Text)) &&
                                    (string.IsNullOrEmpty(paymentType_Text) || (o.PaymentType != null && o.PaymentType.Title.ToString().Contains(paymentType_Text))) &&
                                    (string.IsNullOrEmpty(note_Text) || (o.Note != null && o.Note.Contains(note_Text))) &&
                                    // Поиск
                                    (string.IsNullOrEmpty(searchText) ||
                                        (o.Order_ID.ToString().Contains(searchText)) ||
                                        (o.DateBuy.ToString("d").ToLower().Contains(searchText)) ||
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
            Tbox_Order_ID.Text = "";
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
            DG_Orders.ItemsSource = AppData.db.Order.ToList();
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
                var orders = AppData.db.Order
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
                            worksheet.Cell(row, 1).Value = order.Order_ID;

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
                var orders = AppData.db.Order
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
                            worksheet.Cell(row, 1).Value = order.Order_ID;

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
        // Импорт данных из таблицы Excel
        private void BtnClick_ImportExcel(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Выберите файл Excel для импорта"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook(openFileDialog.FileName))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var rows = worksheet.RowsUsed().Skip(1);

                        var importedOrders = new List<ADO.Order>();

                        foreach (var row in rows)
                        {
                            try
                            {
                                // Безопасное чтение данных
                                var order = new ADO.Order
                                {
                                    DateBuy = DateTime.Parse(row.Cell(2).Value.ToString()),
                                    Film = GetOrCreateFilm(row.Cell(3).Value.ToString()),
                                    DateSession = DateTime.Parse(row.Cell(4).Value.ToString()),
                                    PriceList = GetPriceList(decimal.Parse(row.Cell(5).Value.ToString())),
                                    Count = int.Parse(row.Cell(6).Value.ToString()),
                                    CheckSum = decimal.Parse(row.Cell(7).Value.ToString()),
                                    PaymentType = GetOrCreatePaymentType(row.Cell(8).Value.ToString()),
                                    Note = row.Cell(9).Value.ToString() ?? string.Empty
                                };

                                importedOrders.Add(order);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка в строке {row.RowNumber()}: {ex.Message}\n" +
                                             $"Проверьте правильность данных в этой строке.",
                                             "Ошибка импорта",
                                             MessageBoxButton.OK,
                                             MessageBoxImage.Error);
                                return;
                            }
                        }

                        if (importedOrders.Any())
                        {
                            AppData.db.Order.AddRange(importedOrders);
                            AppData.db.SaveChanges();

                            MessageBox.Show($"Успешно импортировано {importedOrders.Count} записей",
                                          "Импорт завершен",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog();
        }
        
        // Одиночное удаление
        private void BtnClick_TrashDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий заказ из строки, где находится кнопка
                var button = sender as Button;
                var order = button.DataContext as ADO.Order;

                if (order == null)
                {
                    MessageBox.Show("Не удалось получить данные заказа для удаления.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить заказ №{order.Order_ID}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Удаляем заказ
                AppData.db.Order.Remove(order);
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
        
        // Удаление для нескольких строк
        private void BtnClick_Delete(object sender, RoutedEventArgs e)
        {
            var ordersToDelete = new List<ADO.Order>();

            // Собираем отмеченные записи
            foreach (var item in DG_Orders.Items)
            {
                var row = DG_Orders.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox?.IsChecked == true)
                    {
                        ordersToDelete.Add(item as ADO.Order);
                    }
                }
            }

            if (ordersToDelete.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одного значения для удаления.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {ordersToDelete.Count} заказ(ов)?",
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
                    AppData.db.Order.Remove(order);
                    AppData.db.SaveChanges();

                    DG_Orders.ItemsSource = AppData.db.Order.ToList(); // Обновляем таблицу
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
                var order = button.DataContext as ADO.Order;

                if (order == null)
                {
                    MessageBox.Show("Не удалось получить данные заказа.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем и настраиваем окно редактирования

                var editOrderWindow = new AddOrderWindow();

                // Передаем ID заказа, который редактируется
                editOrderWindow.EditingOrderId = order.Order_ID;

                // Заполняем поля
                editOrderWindow.Tbox_Order_ID.Text = order.Order_ID.ToString();
                editOrderWindow.Dpicker_DateBuy.SelectedDate = order.DateBuy;
                editOrderWindow.Cbox_Film.SelectedValue = order.Film_ID;
                editOrderWindow.Dpicker_DateSession.SelectedDate = order.DateSession;
                editOrderWindow.Cbox_PriceList.SelectedValue = order.PriceList_ID;
                editOrderWindow.Tbox_Count.Text = order.Count.ToString();
                editOrderWindow.Cbox_PaymentType.SelectedValue = order.PaymentType_ID;
                editOrderWindow.Tbox_Note.Text = order.Note.ToString();

                // Скрываем кнопку "добавить" и показываем кнопку "сохранить"
                editOrderWindow.Btn_Add.Visibility = Visibility.Collapsed;
                editOrderWindow.Btn_Save.Visibility = Visibility.Visible;

                editOrderWindow.Tb_Order_ID.Visibility = Visibility.Visible;
                editOrderWindow.Tbox_Order_ID.Visibility = Visibility.Visible;

                // Открываем окно
                editOrderWindow.ShowDialog();
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
                // Указываем какое поле будет значением (Film_ID)
                Cbox_Film.SelectedValuePath = "Film_ID";
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
                // Указываем какое поле будет значением (PriceList_ID)
                Cbox_PriceList.SelectedValuePath = "PriceList_ID";
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
                // Указываем какое поле будет значением (PaymentType_ID)
                Cbox_PaymentType.SelectedValuePath = "PaymentType_ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Вспомогательные методы для работы с связанными таблицами
        private Film GetOrCreateFilm(string title)
        {
            var film = AppData.db.Film.FirstOrDefault(f => f.Title == title);
            if (film == null)
            {
                film = new Film { Title = title };
                AppData.db.Film.Add(film);
                AppData.db.SaveChanges();
            }
            return film;
        }

        private PriceList GetPriceList(decimal price)
        {
            var priceItem = AppData.db.PriceList.FirstOrDefault(p => p.Price == price);

            if (priceItem == null)
            {
                throw new Exception($"Тариф с ценой {price} не найден в базе данных. " +
                                  "Добавьте тариф вручную перед импортом.");
            }

            return priceItem;
        }

        private PaymentType GetOrCreatePaymentType(string title)
        {
            var paymentType = AppData.db.PaymentType.FirstOrDefault(p => p.Title == title);
            if (paymentType == null)
            {
                paymentType = new PaymentType { Title = title };
                AppData.db.PaymentType.Add(paymentType);
                AppData.db.SaveChanges();
            }
            return paymentType;
        }
    }
}
