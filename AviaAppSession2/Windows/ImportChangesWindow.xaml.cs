using AviaAppSession2.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AviaAppSession2.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImportChangesWindow.xaml
    /// </summary>
    public partial class ImportChangesWindow : Window
    {
        public ImportChangesWindow()
        {
            InitializeComponent();
        }

        private void ImportFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if ((bool)fileDialog.ShowDialog())
            {
                FileNameTB.Text = fileDialog.FileName.Split('\\').Last();
                var data = File.ReadAllLines(fileDialog.FileName);
                int counterSuccessful = 0;
                int counterMissing = 0;
                foreach(var x in data)
                {
                    var y = x.Split(',');
                    if(y[0] == "ADD")
                    {
                        try
                        {
                            Schedules schedules = new Schedules();
                            schedules.Date = Convert.ToDateTime(y[1]);
                            string[] time = y[2].Split(':');
                            schedules.Time = new TimeSpan(Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                            schedules.FlightNumber = y[3];
                            var departureNumber = Context._con.Airports.ToList()
                                                                       .Where(p => p.IATACode == y[4])
                                                                       .FirstOrDefault();
                            var arrivalNumber = Context._con.Airports.ToList()
                                                                      .Where(p => p.IATACode == y[5])
                                                                      .FirstOrDefault();
                            var route = Context._con.Routes.Where(p => p.DepartureAirportID == departureNumber.ID && p.ArrivalAirportID == arrivalNumber.ID)
                                                           .FirstOrDefault();
                            schedules.RouteID = route.ID;
                            schedules.AircraftID = Convert.ToInt32(y[6]);
                            schedules.EconomyPrice = Convert.ToDecimal(y[7].Replace('.', ','));
                            bool state = true;
                            if (y[8] == "OK")
                            {
                                schedules.Confirmed = state;
                            }
                            else
                            {
                                schedules.Confirmed = !state;
                            }
                            Context._con.Schedules.Add(schedules);
                            Context._con.SaveChanges();
                            counterSuccessful++;
                        }
                        catch
                        {
                            counterMissing++;
                            
                        }
                    } 
                    else if(y[0] == "EDIT")
                    {
                        Schedules schedules = new Schedules();
                        schedules.Date = Convert.ToDateTime(y[1]);
                        string[] time = y[2].Split(':');
                        schedules.Time = new TimeSpan(Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);
                        schedules.FlightNumber = y[3];
                        var departureNumber = Context._con.Airports.ToList()
                                                                   .Where(p => p.IATACode == y[4])
                                                                   .FirstOrDefault();
                        var arrivalNumber = Context._con.Airports.ToList()
                                                                  .Where(p => p.IATACode == y[5])
                                                                  .FirstOrDefault();
                        var route = Context._con.Routes.Where(p => p.DepartureAirportID == departureNumber.ID && p.ArrivalAirportID == arrivalNumber.ID)
                                                       .FirstOrDefault();
                        schedules.RouteID = route.ID;
                        schedules.AircraftID = Convert.ToInt32(y[6]);
                        schedules.EconomyPrice = Convert.ToDecimal(y[7].Replace('.', ','));
                        bool state = true;
                        if (y[8] == "OK")
                        {
                            schedules.Confirmed = state;
                        }
                        else
                        {
                            schedules.Confirmed = !state;
                        }

                        Schedules curSched = Context._con.Schedules.Where(p => p.Date == schedules.Date && p.Time == schedules.Time && p.RouteID == schedules.RouteID).FirstOrDefault();
                        if(curSched != null)
                        {
                            curSched.Confirmed = schedules.Confirmed;
                            Context._con.SaveChanges();
                            counterSuccessful++;

                        }
                        else
                        {
                            counterMissing++;

                        }
                    }
                }
                MissingFieldsTB.Text = counterMissing.ToString();
                ChangesAppliedTB.Text = counterSuccessful.ToString();
                DuplicateRecordsTB.Text = "0";
            }
        }
    }
}
