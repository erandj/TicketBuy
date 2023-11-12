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
using System.Windows.Shapes;

namespace TicketBuy
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : Window
    {
        private List<int> ids = new List<int>();
        private int scheduleRouteId;
        private List<ProductTable> productTables = new List<ProductTable>();
        private PersonPlace[] personPlaces;
        private Dictionary<string, decimal> classFactors = new Dictionary<string, decimal>()
        {
            {"first", 2.3m},
            {"business", 1.8m},
            {"premium", 1.4m},
        };

        public PaymentWindow(PersonPlace[] personPlaces, int scheduleRouteId)
        {
            InitializeComponent();

            DataContext = this;
            this.scheduleRouteId = scheduleRouteId;
            this.personPlaces = personPlaces;

            SetProductTables();
        }

        private void SetProductTables()
        {
            using (FlightEntities context = new FlightEntities())
            {
                Seat[] seats = context.Seat.ToArray();
                ScheduledRoute scheduledRoute = context.ScheduledRoute.First(x => x.id == scheduleRouteId);
                List<string> places = personPlaces.ToList().Select(p => p.Place).ToList();


                decimal baseCost = scheduledRoute.base_cost;

                Console.WriteLine(places[0]);
                foreach (Seat seat in seats)
                {
                    if (!places.Contains(seat.number.ToString() + seat.SeatRow.code))
                    {
                        continue;
                    }

                    ids.Add(seat.id);

                    ProductTable productTable = new ProductTable
                    {
                        TypeCount = seat.SeatClass.name.ToString(),
                        Price = seat.SeatClass.code == "Y" ? baseCost.ToString()
                                : seat.SeatClass.code == "W" ? (baseCost * classFactors["premium"]).ToString()
                                : seat.SeatClass.code == "J" ? (baseCost * classFactors["business"]).ToString()
                                : (baseCost * classFactors["first"]).ToString(),
                    };

                    productTables.Add(productTable);
                }
            }

            ProductTable.ItemsSource = null;
            ProductTable.ItemsSource = productTables;
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            using (FlightEntities context = new FlightEntities())
            {
                int i = 0;
                foreach (var personPlace in personPlaces)
                {
                    Client client = context.Client.FirstOrDefault(x => x.fullname == personPlace.Person);
                    int id = ids[i];

                    if (client == null)
                    {
                        client = new Client { fullname = personPlace.Person, };
                        context.Client.Add(client);
                        try
                        {
                            Console.WriteLine(context.SaveChanges());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    
                    Ticket ticket = new Ticket
                    {
                        client_id = client.id,
                        Seat = context.Seat.FirstOrDefault(x => x.id == id),
                        scheduled_route_id = scheduleRouteId,
                    };

                    context.Ticket.Add(ticket);
                }

                try
                {
                    Console.WriteLine(context.SaveChanges());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
