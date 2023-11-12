using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    public class SeatSelectionItem : INotifyPropertyChanged
    {
        private int seatNumber;
        private string seatRow;
        private bool isSelected;
        private string seatClass;
        public event PropertyChangedEventHandler PropertyChanged;

        public int SeatNumber
        {
            get { return seatNumber; }
            set { seatNumber = value; OnPropertyChanged(); }
        }

        public string SeatRow
        {
            get { return seatRow; }
            set { seatRow = value; OnPropertyChanged(); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }

        public string SeatClass
        {
            get { return seatClass; }
            set { seatClass = value; OnPropertyChanged(); }
        }

        public void OnPropertyChanged([CallerMemberName] string popertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(popertyName));
        }

    }
}
