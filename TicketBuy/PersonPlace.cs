using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    public class PersonPlace : INotifyPropertyChanged
    {
        private string person;
        private string place;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Person
        {
            get { return person; }
            set { person = value; OnPropertyChanged(); }
        }

        public string Place
        {
            get { return place; }
            set { place = value; OnPropertyChanged(); }
        }

        public void OnPropertyChanged([CallerMemberName] string popertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(popertyName));
        }
    }
}
