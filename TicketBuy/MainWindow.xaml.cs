using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace TicketBuy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FlightTicketItem> flightTicketItems;
        private string origin;
        private string destination;
        private DateTime? depatureDate;
        private DateTime? arrivalDate;

        private string[] badStatuses = new string[]
        {
            "Отменен",
            "Перенесен",
            "Сел",
        };

        public string Origin { get => origin; set => origin = value; }
        public string Destination { get => destination; set => destination = value; }
        public DateTime? DepatureDate { get => depatureDate; set => depatureDate = value; }
        public DateTime? ArrivalDate { get => arrivalDate; set => arrivalDate = value; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            tbError.Visibility = Visibility.Hidden;

            if (origin == "" || destination == "" || !depatureDate.HasValue || depatureDate.Value.Date < DateTime.Now.Date)
            {
                Console.WriteLine("Search error: origin or destination or depatureDate is empty, or depatureDate is invalid");
                tbError.Text = "origin or destination or depatureDate is empty, or depatureDate is invalid";
                tbError.Visibility = Visibility.Visible;
                return;
            }

            flightTicketItems = GetFlightTicketItemList();

            lvFlightTickets.ItemsSource = null;
            lvFlightTickets.ItemsSource = flightTicketItems;
        }

        private List<FlightTicketItem> GetFlightTicketItemList()
        {
            List<FlightTicketItem> flightTicketItems_return = new List<FlightTicketItem>();

            using (FlightEntities context = new FlightEntities())
            {
                DateTime depatureDateForSearchLeft = depatureDate.Value.Date;
                DateTime depatureDateForSearchRight = depatureDate.Value.Date.AddDays(1).AddSeconds(-1);
                List<ScheduledRoute> scheduledRoutes = context.ScheduledRoute.Where(x => context.ScheduledRouteStatus
                                                                                                .Where(s => !badStatuses.Contains(s.name))
                                                                                                .ToList()
                                                                                                .Contains(x.ScheduledRouteStatus)
                                                                                    && x.Route.Airport.name == Origin
                                                                                    && x.Route.Airport1.name == Destination
                && depatureDateForSearchLeft <= (x.actual_departure_datetime == null ? 
                                                    x.scheduled_departure_datetime : x.actual_departure_datetime)
                && depatureDateForSearchRight >= (x.actual_departure_datetime == null ? 
                                                    x.scheduled_departure_datetime : x.actual_departure_datetime)).ToList();

                if (arrivalDate.HasValue && arrivalDate.Value < depatureDate.Value)
                    flightTicketItems_return = onManyWay(scheduledRoutes, context);
                else
                    flightTicketItems_return = onOneWay(scheduledRoutes, context);
            }

            return flightTicketItems_return;
        }

        private List<FlightTicketItem> onOneWay(List<ScheduledRoute>  scheduledRoutes, FlightEntities context)
        {
            List<FlightTicketItem> flightTicketItems_return = new List<FlightTicketItem>();

            foreach (ScheduledRoute scheduledRoute in scheduledRoutes)
            {
                DateTime depatureDateTime = scheduledRoute.actual_departure_datetime == null ? (DateTime)scheduledRoute.scheduled_departure_datetime
                                                                                        : (DateTime)scheduledRoute.actual_departure_datetime;
                DateTime arrivalDateTime = scheduledRoute.actual_arrival_datetime == null ? (DateTime)scheduledRoute.scheduled_arrival_datetime
                                                                                    : (DateTime)scheduledRoute.actual_arrival_datetime;
                TimeSpan deltaDateTime = arrivalDateTime.Subtract(depatureDateTime);

                FlightTicketItem item = new FlightTicketItem
                {
                    Id = scheduledRoute.id,
                    Cost = Decimal.ToInt32(scheduledRoute.base_cost).ToString() + " ₽",
                    BaggageCost = "Багаж - " + Decimal.ToInt32(scheduledRoute.Route.Airline.baggage_cost).ToString() + " ₽",
                    DepartureTime = depatureDateTime.ToString("HH:mm"),
                    DepartureCity = scheduledRoute.Route.Airport.name,
                    DepartureDate = depatureDateTime.ToString("ddd, dd MMMM"),
                    ArrivalTime = arrivalDateTime.ToString("HH:mm"),
                    ArrivalCity = scheduledRoute.Route.Airport1.name,
                    ArrivalDate = arrivalDateTime.ToString("ddd, dd MMMM"),
                    DepartureCode = FlightInfo.GetAirportCode(scheduledRoute.Route.Airport, context),
                    ArrivalCode = FlightInfo.GetAirportCode(scheduledRoute.Route.Airport1, context),
                    InFlight = "В пути: " + (deltaDateTime.Hours + deltaDateTime.Days * 24).ToString() + "ч " + deltaDateTime.Minutes.ToString() + "м",
                };

                flightTicketItems_return.Add(item);
            }
            return flightTicketItems_return;
        }

        private List<FlightTicketItem> onManyWay(List<ScheduledRoute> scheduledRoutes, FlightEntities context)
        {
            List<FlightTicketItem> flightTicketItems_return = onManyWay(scheduledRoutes, context);
            //List<FlightTicketItem> flightTicketItems_return = new List<FlightTicketItem>();
            //List<FullRouteRoutes> fullRoutes = context.FullRouteRoutes.ToList();


            //foreach(FullRouteRoutes fullRouteRoute in fullRoutes)
            //{
            //    if (!scheduledRoutes.Contains(fullRouteRoute.ScheduledRoute) && fullRouteRoute.index == 0) continue;



            //}


            //foreach (ScheduledRoute scheduledRoute in scheduledRoutes)
            //{
            //    DateTime depatureDateTime = scheduledRoute.actual_departure_datetime == null ? (DateTime)scheduledRoute.scheduled_departure_datetime
            //                                                                            : (DateTime)scheduledRoute.actual_departure_datetime;
            //    DateTime arrivalDateTime = scheduledRoute.actual_arrival_datetime == null ? (DateTime)scheduledRoute.scheduled_arrival_datetime
            //                                                                        : (DateTime)scheduledRoute.actual_arrival_datetime;
            //    TimeSpan deltaDateTime = arrivalDateTime.Subtract(depatureDateTime);

            //    FlightTicketItem item = new FlightTicketItem
            //    {
            //        Id = scheduledRoute.id,
            //        Cost = Decimal.ToInt32(scheduledRoute.base_cost).ToString() + " ₽",
            //        BaggageCost = "Багаж - " + Decimal.ToInt32(scheduledRoute.Route.Airline.baggage_cost).ToString() + " ₽",
            //        DepartureTime = depatureDateTime.ToString("HH:mm"),
            //        DepartureCity = scheduledRoute.Route.Airport.name,
            //        DepartureDate = depatureDateTime.ToString("ddd, dd MMMM"),
            //        ArrivalTime = arrivalDateTime.ToString("HH:mm"),
            //        ArrivalCity = scheduledRoute.Route.Airport1.name,
            //        ArrivalDate = arrivalDateTime.ToString("ddd, dd MMMM"),
            //        DepartureCode = FlightInfo.GetAirportCode(scheduledRoute.Route.Airport, context),
            //        ArrivalCode = FlightInfo.GetAirportCode(scheduledRoute.Route.Airport1, context),
            //        InFlight = "В пути: " + (deltaDateTime.Hours + deltaDateTime.Days * 24).ToString() + "ч " + deltaDateTime.Minutes.ToString() + "м",
            //    };

            //    flightTicketItems_return.Add(item);
            //}
            return flightTicketItems_return;
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            string tempText = tbOrigin.Text;
            tbOrigin.Text = tbDestination.Text;
            tbDestination.Text = tempText;
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)(sender as Button).Tag;
            TicketDetailsWindow ticketDetailsWindow = new TicketDetailsWindow(id);
            ticketDetailsWindow.Show();
            Close();
        }
    }
}
