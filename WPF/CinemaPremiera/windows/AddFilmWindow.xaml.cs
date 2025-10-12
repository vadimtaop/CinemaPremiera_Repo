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
    /// Логика взаимодействия для AddFilmWindow.xaml
    /// </summary>
    public partial class AddFilmWindow : Window
    {
        public AddFilmWindow()
        {
            InitializeComponent();
        }

        private void BtnClick_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClick_Add(object sender, RoutedEventArgs e)
        {
            try
            {
                Film films = new Film();

                films.Title = Tbox_Title.Text;
                films.AgeLimit = int.Parse(Tbox_AgeLimit.Text);
                films.DurationInMinutes = int.Parse(Tbox_DurationInMinutes.Text);
                films.Genre = Tbox_Genre.Text;

                AppData.db.Film.Add(films);
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
        public int EditingFilmId { get; set; }
        private void BtnClick_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите внести изменения?", "Предупреждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Получаем ID редактируемого фильма
                    int filmID = this.EditingFilmId;

                    // Находим фильм в БД
                    var film = AppData.db.Film.FirstOrDefault(o => o.Film_ID == filmID);

                    // Сохраняем изменения
                    if (film != null)
                    {
                        film.Title = Tbox_Title.Text;
                        film.AgeLimit = int.Parse(Tbox_AgeLimit.Text);
                        film.DurationInMinutes = int.Parse(Tbox_DurationInMinutes.Text);
                        film.Genre = Tbox_Genre.Text;

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
