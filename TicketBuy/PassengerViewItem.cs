using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBuy
{
    public class PassengerViewItem
    {
        private string type;
        private string sex;
        private string citizenship;
        private string document;
        private string documentDetails;
        private DateTime? documentExpirationDate;
        private string surname;
        private string name;
        private string patronymic;
        private DateTime? birthday;

        public string Type { get => type; set => type = value; }
        public string Sex { get => sex; set => sex = value; }
        public string Сitizenship { get => citizenship; set => citizenship = value; }
        public string Document { get => document; set => document = value; }
        public string DocumentDetails { get => documentDetails; set => documentDetails = value; }
        public DateTime? DocumentExpirationDate { get => documentExpirationDate; set => documentExpirationDate = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Name { get => name; set => name = value; }
        public string Patronymic { get => patronymic; set => patronymic = value; }
        public DateTime? Birthday { get => birthday; set => birthday = value; }
    }
}
