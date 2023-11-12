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
using System.Xml.Linq;

namespace TicketBuy
{
    /// <summary>
    /// Логика взаимодействия для PassengerDetails.xaml
    /// </summary>
    public partial class PassengerDetailsPage : Page
    {
        private int[] passengerCount = new int[3];
        private List<PassengerViewItem> passengerViewItems= new List<PassengerViewItem>();
        private TicketDetailsWindow ticketDetailsWindow;

        public PassengerDetailsPage(int[] passengerCount, TicketDetailsWindow ticketDetailsWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.passengerCount = passengerCount;
            this.ticketDetailsWindow = ticketDetailsWindow;

            SetItemsSource();
        }

        private void SetItemsSource()
        {
            for (int i = 0; i < passengerCount[0]; i++)
                passengerViewItems.Add(GetPassengerViewItem(0));

            for (int i = 0; i < passengerCount[1]; i++)
                passengerViewItems.Add(GetPassengerViewItem(1));

            for (int i = 0; i < passengerCount[2]; i++)
                passengerViewItems.Add(GetPassengerViewItem(2));

            PassengerView.ItemsSource = passengerViewItems;
        }

        private PassengerViewItem GetPassengerViewItem(int type)
        {
            PassengerViewItem passengerViewItem = new PassengerViewItem();

            switch (type)
            {
                case 0:
                    passengerViewItem.Type = "Взрослый";
                    break;
                case 1:
                    passengerViewItem.Type = "Ребенок";
                    break;
                case 2:
                    passengerViewItem.Type = "Младенец";
                    break;
            }

            return passengerViewItem;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ChechFullness())
                return;

            PlaceSelectionWindow placeSelectionWindow = new PlaceSelectionWindow(passengerViewItems, ticketDetailsWindow.Id);

            placeSelectionWindow.Show();
            ticketDetailsWindow.Close();
        }

        private bool ChechFullness()
        {
            tbError.Visibility = Visibility.Hidden;

            foreach (PassengerViewItem passengerViewItem in passengerViewItems)
            {
                if (passengerViewItem.Type == ""
                    || passengerViewItem.Sex == ""
                    || passengerViewItem.Сitizenship == ""
                    || passengerViewItem.Document == ""
                    || passengerViewItem.DocumentDetails == ""
                    || passengerViewItem.Surname == ""
                    || passengerViewItem.Name == ""
                    || passengerViewItem.Patronymic == ""
                    || !passengerViewItem.Birthday.HasValue
                    || (passengerViewItem.Document == "Заграничный паспорт" && !passengerViewItem.DocumentExpirationDate.HasValue)
                    || (passengerViewItem.Type == "Взрослый" && passengerViewItem.Document == "Свидетельство о рождении")
                    || (passengerViewItem.Type != "Взрослый" && passengerViewItem.Document != "Свидетельство о рождении"))
                {
                    tbError.Visibility = Visibility.Visible;
                    return false;
                }
            }
            return true;
        }
    }
}
