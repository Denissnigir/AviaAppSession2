using AviaAppSession2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace AviaAppSession2.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditFlightWindow.xaml
    /// </summary>
    public partial class EditFlightWindow : Window
    {
        Schedules schedules1;
        public EditFlightWindow(Schedules schedules)
        {
            InitializeComponent();
            schedules1 = schedules;
            ScheduleGrid.DataContext = schedules1;
            TimeMB.Text = schedules1.Time.ToString();
        }

        private void UpdateInfo(object sender, RoutedEventArgs e)
        {
            string[] data = TimeMB.Text.Split(':');
            if ((Convert.ToInt32(data[0]) >= 0 && Convert.ToInt32(data[0]) <= 23) && (Convert.ToInt32(data[1]) >= 0 && Convert.ToInt32(data[0]) <= 59))
            {
                TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(data[0]), Convert.ToInt32(data[1]), 0);
                schedules1.Time = timeSpan;
                Context._con.SaveChanges();
                ScheduleWindow scheduleWindow = new ScheduleWindow();
                scheduleWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите корректное время");
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            ScheduleWindow scheduleWindow = new ScheduleWindow();
            scheduleWindow.Show();
            this.Close();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
