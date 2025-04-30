using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CinemaPremiera
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClick_Exit(object sender, RoutedEventArgs e)
        {
            // Получаем путь к текущему исполняемому файлу
            string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

            Process.Start(applicationPath);
            Application.Current.Shutdown();
        }
    }
}
