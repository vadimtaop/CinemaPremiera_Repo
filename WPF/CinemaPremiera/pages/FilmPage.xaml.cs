using CinemaPremiera.ADO;
using CinemaPremiera.windows;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Win32;
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
    /// Логика взаимодействия для FilmPage.xaml
    /// </summary>
    public partial class FilmPage : Page
    {
        public FilmPage()
        {
            InitializeComponent();

            DG_Films.ItemsSource = AppData.db.Film.ToList();
        }
        // Одиночное удаление
        private void BtnClick_TrashDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем текущий фильм из строки, где находится кнопка
                var button = sender as Button;
                var film = button.DataContext as ADO.Film;

                if (film == null)
                {
                    MessageBox.Show("Не удалось получить данные фильма для удаления.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить фильм №{film.Film_ID}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                // Удаляем заказ
                AppData.db.Film.Remove(film);
                AppData.db.SaveChanges();

                MessageBox.Show("Фильм успешно удален.", "Информация",
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
            var filmsToDelete = new List<ADO.Film>();

            // Собираем отмеченные записи
            foreach (var item in DG_Films.Items)
            {
                var row = DG_Films.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    var checkBox = FindVisualChild<CheckBox>(row);
                    if (checkBox?.IsChecked == true)
                    {
                        filmsToDelete.Add(item as ADO.Film);
                    }
                }
            }

            if (filmsToDelete.Count == 0)
            {
                MessageBox.Show("Не выбранно ни одного значения для удаления.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {filmsToDelete.Count} фильм(ов)?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            // Если пользователь не подтвердил удаление
            if (result != MessageBoxResult.Yes)
            {
                MessageBox.Show("Удаление отменено", "Информация",
                              MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Удаляем выбранные фильмы
            try
            {
                foreach (var film in filmsToDelete)
                {
                    AppData.db.Film.Remove(film);
                    AppData.db.SaveChanges();

                    DG_Films.ItemsSource = AppData.db.Film.ToList(); // Обновляем таблицу
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
                var film = button.DataContext as ADO.Film;

                if (film == null)
                {
                    MessageBox.Show("Не удалось получить данные фильма.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Создаем и настраиваем окно редактирования

                var editFilmWindow = new AddFilmWindow();

                // Передаем ID фильма, который редактируется
                editFilmWindow.EditingFilmId = film.Film_ID;

                // Заполняем поля
                editFilmWindow.Tbox_Film_ID.Text = film.Film_ID.ToString();
                editFilmWindow.Tbox_Title.Text = film.Title.ToString();
                editFilmWindow.Tbox_AgeLimit.Text = film.AgeLimit.ToString();
                editFilmWindow.Tbox_DurationInMinutes.Text = film.DurationInMinutes.ToString();
                editFilmWindow.Tbox_Genre.Text = film.Genre.ToString();

                // Скрываем кнопку "добавить" и показываем кнопку "сохранить"
                editFilmWindow.Btn_Add.Visibility = Visibility.Collapsed;
                editFilmWindow.Btn_Save.Visibility = Visibility.Visible;

                editFilmWindow.Tb_Film_ID.Visibility = Visibility.Visible;
                editFilmWindow.Tbox_Film_ID.Visibility = Visibility.Visible;

                // Открываем окно
                editFilmWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            AddFilmWindow addFilmWindow = new AddFilmWindow();
            addFilmWindow.ShowDialog();
        }

        private void BtnClick_ExportExcel_Full(object sender, RoutedEventArgs e)
        {
            try
            {
                var films = AppData.db.Film.ToList();

                if (films == null || !films.Any())
                {
                    MessageBox.Show("Нет данных для экспорта", "Информация",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"Фильмы_(Полная)_{DateTime.Now:dd_MM_yyyy}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Фильмы");

                        // Заголовки столбцов
                        var headers = new[]
                        {
                            "ID",
                            "Название Фильма",
                            "Возрастное ограничение",
                            "Длительность в минутах",
                            "Жанр"
                        };

                        // Записываем заголовки
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = headers[i];
                        }

                        // Заполняем данные
                        int row = 2;
                        foreach (var film in films)
                        {
                            // ID
                            worksheet.Cell(row, 1).Value = film.Film_ID;

                            // Название Фильма
                            worksheet.Cell(row, 2).Value = film.Title ?? "Не указано";

                            // Возрастное ограничение
                            worksheet.Cell(row, 3).Value = film.AgeLimit;

                            // Длительность в минутах
                            worksheet.Cell(row, 4).Value = film.DurationInMinutes;

                            // Жанр
                            worksheet.Cell(row, 5).Value = film.Genre ?? "Не указано";

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
                string filmID_Text = Tbox_FilmID.Text;
                string title_Text = Tbox_Title.Text;
                string ageLimit_Text = Tbox_AgeLimit.Text;
                string durationInMinutes_Text = Tbox_DurationInMinutes.Text;
                string genre_Text = Tbox_Genre.Text;
                string searchText = Tbox_Search.Text.ToLower();

                // Получаем все заказы с включенными связанными данными
                var DataFilms = AppData.db.Film.ToList();

                // Применяем те же фильтры, что и в BtnClick_Apply
                var filteredFilms = DataFilms.Where(o =>
                                        // Фильтры
                                        (string.IsNullOrEmpty(filmID_Text) || o.Film_ID.ToString().Contains(filmID_Text)) &&
                                        (string.IsNullOrEmpty(title_Text) || (o.Title != null && o.Title.Contains(title_Text))) &&
                                        (string.IsNullOrEmpty(ageLimit_Text) || o.AgeLimit.ToString().Contains(ageLimit_Text)) &&
                                        (string.IsNullOrEmpty(durationInMinutes_Text) || o.DurationInMinutes.ToString().Contains(durationInMinutes_Text)) &&
                                        (string.IsNullOrEmpty(genre_Text) || (o.Genre != null && o.Genre.Contains(genre_Text))) &&
                                        // Поиск
                                        (string.IsNullOrEmpty(searchText) ||
                                            (o.Film_ID.ToString().Contains(searchText)) ||
                                            (o.Title != null && o.Title.ToLower().Contains(searchText)) ||
                                            (o.AgeLimit.ToString().Contains(searchText)) ||
                                            (o.DurationInMinutes.ToString().Contains(searchText)) ||
                                            (o.Genre?.ToLower().Contains(searchText) ?? false))
                                        ).ToList();

                if (filteredFilms == null || !filteredFilms.Any())
                {
                    MessageBox.Show("Нет данных для экспорта", "Информация",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"Фильмы_(Фильтр)_{DateTime.Now:dd_MM_yyyy}.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Фильмы");

                        // Заголовки столбцов
                        var headers = new[]
                        {
                            "ID",
                            "Название Фильма",
                            "Возрастное ограничение",
                            "Длительность в минутах",
                            "Жанр"
                        };

                        // Записываем заголовки
                        for (int i = 0; i < headers.Length; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = headers[i];
                        }

                        // Заполняем данные
                        int row = 2;
                        foreach (var film in filteredFilms) // Используем filteredFilms вместо films
                        {
                            // ID
                            worksheet.Cell(row, 1).Value = film.Film_ID;

                            // Название Фильма
                            worksheet.Cell(row, 2).Value = film.Title ?? "Не указано";

                            // Возрастное ограничение
                            worksheet.Cell(row, 3).Value = film.AgeLimit;

                            // Длительность в минутах
                            worksheet.Cell(row, 4).Value = film.DurationInMinutes;

                            // Жанр
                            worksheet.Cell(row, 5).Value = film.Genre ?? "Не указано";

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

                        var importedFilms = new List<ADO.Film>();

                        foreach (var row in rows)
                        {
                            try
                            {
                                // Безопасное чтение данных
                                var film = new ADO.Film
                                {
                                    Title = row.Cell(2).Value.ToString(),
                                    AgeLimit = int.Parse(row.Cell(3).Value.ToString()),
                                    DurationInMinutes = int.Parse(row.Cell(4).Value.ToString()),
                                    Genre = row.Cell(5).Value.ToString() ?? string.Empty
                                };

                                importedFilms.Add(film);
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

                        if (importedFilms.Any())
                        {
                            AppData.db.Film.AddRange(importedFilms);
                            AppData.db.SaveChanges();

                            MessageBox.Show($"Успешно импортировано {importedFilms.Count} записей",
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

        private void BtnClick_Apply(object sender, RoutedEventArgs e)
        {
            // Получаем данные из используемых фильтров (TextBox)
            string filmID_Text = Tbox_FilmID.Text;
            string title_Text = Tbox_Title.Text;
            string ageLimit_Text = Tbox_AgeLimit.Text;
            string durationInMinutes_Text = Tbox_DurationInMinutes.Text;
            string genre_Text = Tbox_Genre.Text;
            string searchText = Tbox_Search.Text.ToLower();

            // Получаем все строки из таблицы Film (БД)
            var DataFilms = AppData.db.Film.ToList();

            // Ищем фильмы по всем фильтрам
            var filteredFilms = DataFilms.Where(o =>
                                    // Фильтры
                                    (string.IsNullOrEmpty(filmID_Text) || o.Film_ID.ToString().Contains(filmID_Text)) &&
                                    (string.IsNullOrEmpty(title_Text) || (o.Title != null && o.Title.Contains(title_Text))) &&
                                    (string.IsNullOrEmpty(ageLimit_Text) || o.AgeLimit.ToString().Contains(ageLimit_Text)) &&
                                    (string.IsNullOrEmpty(durationInMinutes_Text) || o.DurationInMinutes.ToString().Contains(durationInMinutes_Text)) &&
                                    (string.IsNullOrEmpty(genre_Text) || (o.Genre != null && o.Genre.Contains(genre_Text))) &&
                                    // Поиск
                                    (string.IsNullOrEmpty(searchText) ||
                                        (o.Film_ID.ToString().Contains(searchText)) ||
                                        (o.Title != null && o.Title.ToLower().Contains(searchText)) ||
                                        (o.AgeLimit.ToString().Contains(searchText)) ||
                                        (o.DurationInMinutes.ToString().Contains(searchText)) ||
                                        (o.Genre?.ToLower().Contains(searchText) ?? false))
                                    ).ToList();

            DG_Films.ItemsSource = filteredFilms;
        }

        private void BtnClick_ResetFilters(object sender, RoutedEventArgs e)
        {
            Tbox_Search.Text = "";
            Tbox_FilmID.Text = "";
            Tbox_Title.Text = "";
            Tbox_AgeLimit.Text = "";
            Tbox_DurationInMinutes.Text = "";
            Tbox_Genre.Text = "";
            DG_Films.ItemsSource = AppData.db.Film.ToList();
        }
    }
}
