using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicketBuy
{
    /// <summary>
    /// Логика взаимодействия для PlaceSelectionWindow.xaml
    /// </summary>
    public partial class PlaceSelectionWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<PassengerViewItem> passengerViewItems = new List<PassengerViewItem>();
        private PersonPlace[] personPlaces;
        private List<List<SeatSelectionItem>> topSeatList;
        private int scheduleRouteId;

        public List<List<SeatSelectionItem>> TopSeatList
        {
            get => topSeatList;
            set
            {
                topSeatList = value;
                OnPropertyChanged("TopSeatList");
            }
        }

        public PlaceSelectionWindow(List<PassengerViewItem> passengerViewItems, int scheduleRouteId)
        {
            InitializeComponent();
            DataContext = this;

            this.passengerViewItems = passengerViewItems;
            this.scheduleRouteId = scheduleRouteId;

            topSeatList = GetAircraftSeat();
            SetPersonPlaces();
        }

        private List<List<SeatSelectionItem>> GetAircraftSeat()
        {
            List<List<SeatSelectionItem>> seatSelectionItemsReturn = new List<List<SeatSelectionItem>>();

            using (FlightEntities context = new FlightEntities())
            {
                ScheduledRoute scheduledRoute = context.ScheduledRoute.FirstOrDefault(x => x.id == scheduleRouteId);
                Aircraft aircraft = scheduledRoute.Aircraft;
                int[] selectedSeatIds = context.Ticket.Select(x => x.seat_id).ToArray();


                int linesCount = aircraft.lines_count;
                int maxNumber = aircraft.seat_capacity/(linesCount*3);

                for (int num=1; num < maxNumber+1; num++)
                {
                    List<SeatSelectionItem> seatSelectionItemsRow = new List<SeatSelectionItem>();
                    for (int row = 1; row < 7; row++)
                    {
                        Seat seat = context.Seat.FirstOrDefault(x => x.aircraft_id == aircraft.id && x.number == num && x.seat_row_id == row);
                        if (seat == null)
                        {
                            seat = new Seat()
                            {
                                number = num,
                                SeatRow = context.SeatRow.First(x => x.id == row),
                                SeatClass = num <= aircraft.business_line ? context.SeatClass.FirstOrDefault(x => x.code == "J") :
                                                                                num <= aircraft.premium_line+aircraft.business_line? 
                                                                                    context.SeatClass.FirstOrDefault(x => x.code == "W") :
                                                                                    context.SeatClass.FirstOrDefault(x => x.code == "Y"),
                                Aircraft = aircraft,
                            };
                            context.Seat.Add(seat);
                        }

                        SeatSelectionItem seatSelectionItem = new SeatSelectionItem
                        {
                            SeatNumber = seat.number,
                            SeatRow = seat.SeatRow.code,
                            IsSelected = selectedSeatIds.Contains(seat.id),
                            SeatClass = seat.SeatClass.code,
                        };

                        seatSelectionItemsRow.Add(seatSelectionItem);
                    }
                    seatSelectionItemsReturn.Add(seatSelectionItemsRow);
                }
                context.SaveChanges();
            }

            return seatSelectionItemsReturn;
        }

        private void SetPersonPlaces()
        {
            personPlaces = new PersonPlace[passengerViewItems.Count()];
            int i = 0;
            foreach (var item in passengerViewItems)
            {
                if (item.Type == "Младенец")
                    continue;
                PersonPlace personPlace = new PersonPlace();
                personPlace.Person = item.Surname + " " + item.Name + " " + item.Patronymic;
                personPlace.Place = "Не выбрано";
                personPlaces[i] = personPlace;
                i++;
            }

            PersonPlasesTable.ItemsSource= personPlaces;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PaymentWindow paymentWindow = new PaymentWindow(personPlaces, scheduleRouteId);
            paymentWindow.Show();
            Close();
        }

        private void cbSeat_Checked(object sender, RoutedEventArgs e)
        {
            var selectedSeats = TopSeatList.SelectMany(x => x.Where(y => y.IsSelected)).ToArray();
            List<string> selectedSeatsName = selectedSeats.Select(x => x.SeatNumber + x.SeatRow).ToList();

            if (selectedSeats.Count() == 0 || personPlaces.Count() == 0)
                return;


            foreach(var personPlace in personPlaces)
            {
                if (personPlace.Place != "Не выбрано" && personPlace.Place != "")
                    continue;

                var places1 = personPlaces.Select(x => x.Place);

                foreach (var seat in selectedSeats)
                {
                    string place = seat.SeatNumber.ToString() + seat.SeatRow.ToString();
                    if (places1.Contains(place))
                        continue;

                    personPlace.Place = place;
                    break;
                }
            }

            var places = personPlaces.Select(x => x.Place);

            foreach (var seat in selectedSeats)
            {
                string place = seat.SeatNumber.ToString() + seat.SeatRow.ToString();
                if (!places.Contains(place))
                    seat.IsSelected = false;
            }
        }

        private void cbSeat_Unchecked(object sender, RoutedEventArgs e)
        {
            var selectedSeats = TopSeatList.SelectMany(x => x.Where(y => !y.IsSelected)).ToArray();
            var places = personPlaces.Select(x => x.Place);

            foreach (var seat in selectedSeats)
            {
                string place = seat.SeatNumber.ToString() + seat.SeatRow.ToString();
                if (places.Contains(place))
                {
                    var personPlace = personPlaces.FirstOrDefault(x => x.Place == seat.SeatNumber.ToString() + seat.SeatRow.ToString());
                    personPlace.Place = "Не выбрано";
                }
            }
        }

        public void OnPropertyChanged([CallerMemberName] string popertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(popertyName));
        }
    }
}
