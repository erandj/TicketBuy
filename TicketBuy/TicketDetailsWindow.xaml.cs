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

namespace TicketBuy
{
    /// <summary>
    /// Логика взаимодействия для TicketDetailsWindow2.xaml
    /// </summary>
    public partial class TicketDetailsWindow : Window
    {
        private int id;
        private string flightCode;

        public string FlightCode { get => flightCode; set => flightCode = value; }
        public int Id { get => id; set => id = value; }

        public TicketDetailsWindow(int id)
        {
            InitializeComponent();
            DataContext = this;
            Id = id;

            using (FlightEntities context = new FlightEntities())
            {
                ScheduledRoute scheduledRoute = context.ScheduledRoute.FirstOrDefault(x => x.id == id);
                flightCode = FlightInfo.GetFlightCode(scheduledRoute.Route, context);
            }

            ContentFrame.Navigate(new PassengerCountPage(this));
        }

    }
}
