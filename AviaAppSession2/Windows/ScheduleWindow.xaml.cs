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
using AviaAppSession2.Model;

namespace AviaAppSession2.Windows
{
    /// <summary>
    /// Логика взаимодействия для ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        List<Schedules> schedules = new List<Schedules>();
        List<Airports> airports = new List<Airports>();
        List<string> sortList = new List<string>()
        {
            "Сортировать по дате",
            "Сортировать по цене",
            "Сортировать по статусу"
        };

        public ScheduleWindow()
        {
            InitializeComponent();
            airports = Context._con.Airports.ToList();
            DepartmentAirportsCB.ItemsSource = airports;
            ArrivalAirportsCB.ItemsSource = airports;

            DateCB.ItemsSource = sortList;
            DateCB.SelectedIndex = 0;

            Fliter();
        }

        private void Fliter()
        {
            schedules = Context._con.Schedules.ToList();
            if(DepartmentAirportsCB.SelectedItem is Airports airports && ArrivalAirportsCB.SelectedItem is Airports airports1)
            {
                schedules = Context._con.Schedules.Where(p => p.Routes.DepartureAirportID == airports.ID && p.Routes.ArrivalAirportID == airports1.ID).ToList();
            }

            if(OutboundDP.SelectedDate is DateTime date)
            {
                schedules = schedules.Where(p => p.Date == date.Date).ToList();
            }

            switch (DateCB.SelectedIndex)
            {
                case 0:
                    schedules = schedules.OrderBy(p => p.Date).ToList();
                    break;
                case 1:
                    schedules = schedules.OrderBy(p => p.EconomyPrice).ToList();
                    break;
                case 2:
                    schedules = schedules.OrderBy(p => p.Confirmed).ToList();
                    break;
            }

            if (!string.IsNullOrWhiteSpace(FlightNumberTB.Text))
            {
                schedules = schedules.Where(p => p.FlightNumber.Contains(FlightNumberTB.Text)).ToList();
            }
            ScheduleDG.ItemsSource = schedules;
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            Fliter();

        }

        private void CancelFlight(object sender, RoutedEventArgs e)
        {
            if(ScheduleDG.SelectedItem is Schedules schedules)
            {
                schedules.Confirmed = !schedules.Confirmed;
            }
            
            Context._con.SaveChanges();
            Fliter();
        }

        private void EditFlight(object sender, RoutedEventArgs e)
        {
            if(ScheduleDG.SelectedItem is Schedules schedules)
            {
                EditFlightWindow editFlightWindow = new EditFlightWindow(schedules);
                editFlightWindow.Show();
                this.Close();
            }
        }

        private void ImportChanges(object sender, RoutedEventArgs e)
        {
            ImportChangesWindow importChangesWindow = new ImportChangesWindow();
            importChangesWindow.Show();
            this.Close();
        }
    }
}
