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
            string dateSession_Text = Dpicker_DateSession.Text;
            string priceList_Tag = (Сbox_PriceList.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            string count_Text = Tbox_Count.Text;
            string checkSum_Text = Tbox_CheckSum.Text;
            string paymentType_Tag = (Сbox_PaymentType.SelectedItem as ComboBoxItem)?.Tag?.ToString();
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
        }

        private void BtnClick_ResetFilters(object sender, RoutedEventArgs e)
        {
            Tbox_Search.Text = "";
            Dpicker_DateBuyS.Text = "";
            Dpicker_DateBuyPo.Text = "";
            Tbox_Film.Text = "";
            Dpicker_DateSession.Text = "";
            Сbox_PriceList.SelectedIndex = 0;
            Tbox_Count.Text = "";
            Tbox_CheckSum.Text = "";
            Сbox_PaymentType.SelectedIndex = 0;
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
                string film_Text = Tbox_Film.Text;
                string dateSession_Text = Dpicker_DateSession.Text;
                string priceList_Tag = (Сbox_PriceList.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                string count_Text = Tbox_Count.Text;
                string checkSum_Text = Tbox_CheckSum.Text;
                string paymentType_Tag = (Сbox_PaymentType.SelectedItem as ComboBoxItem)?.Tag?.ToString();
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
    }
}
