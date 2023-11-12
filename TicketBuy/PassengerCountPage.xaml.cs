using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для PassengerCountPage.xaml
    /// </summary>
    public partial class PassengerCountPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TicketDetailsWindow ticketDetailsWindow;

        private string adultCount = "0";
        private string childCount = "0"; 
        private string infantCount = "0";

        public PassengerCountPage(TicketDetailsWindow ticketDetailsWindow)
        {
            InitializeComponent();
            DataContext = this;
            this.ticketDetailsWindow = ticketDetailsWindow;
        }
        public string AdultCount 
        { 
            get => adultCount;
            set
            {
                if (Int32.Parse(value) < 0)
                    value = "0";
                adultCount = value;
                OnPropertyChanged("AdultCount");
            }
        }
        public string ChildCount 
        { 
            get => childCount;
            set
            {
                if (Int32.Parse(value) < 0)
                    value = "0";
                childCount = value;
                OnPropertyChanged("ChildCount");
            }
        }
        public string InfantCount
        {
            get => infantCount;
            set
            {
                if (Int32.Parse(value) < 0)
                    value = "0";
                infantCount = value;
                OnPropertyChanged("InfantCount");
            }
        }

        private void CountButton_Click(object sender, RoutedEventArgs e)
        {
            string type = (sender as Button).Tag as string;
            int id = Int32.Parse(type.Substring(1));
            int k = 1;
            if (type.Contains("-"))
                k *= -1;

            if (id == 0)
                AdultCount = (Int32.Parse(AdultCount) + k).ToString();
            else if (id == 1)
                ChildCount = (Int32.Parse(ChildCount) + k).ToString();
            else if (id == 2)
                InfantCount = (Int32.Parse(InfantCount) + k).ToString();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if(Int32.Parse(adultCount) == 0 || Int32.Parse(adultCount) + Int32.Parse(childCount) + Int32.Parse(infantCount) == 0)
            {
                return;
            }
            int[] passengerCount = { Int32.Parse(adultCount), Int32.Parse(childCount), Int32.Parse(infantCount) };
            ticketDetailsWindow.ContentFrame.Navigate(new PassengerDetailsPage(passengerCount, ticketDetailsWindow));
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
