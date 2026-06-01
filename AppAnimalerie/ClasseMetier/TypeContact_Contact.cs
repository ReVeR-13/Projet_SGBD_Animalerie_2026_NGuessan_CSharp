
using AppAnimalerie.ClasseService;

namespace AppAnimalerie.ClasseMetier
{
    public class TypeContact_Contact : ITable,IComparable<TypeContact_Contact>
    {
        private string _id;
        private DateTime _date;
        private Contact _contact;
        private TypeContact _type;

        private TypeContact_Contact(Contact contact, TypeContact type)
        {
            try
            {
                this.Id = $"{contact.Id}{type.Id}";
                this.DateCreation = DateTime.Now;
                this.Contact = contact;
                this.Type = type;
            }
            catch (Exception e)
            {
                ExceptionLauncher.New("TypeContact_Contact Instance", e.Message);
            }
            
        }

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
        public DateTime DateCreation
        {
            get
            {
                return this._date;
            }
            set 
            {
                this._date = Forma.Checked_DateCreation(value); 
            }
        }
        public Contact Contact
        {
            get
            {
                return this._contact;
            }
            set
            {
                this._contact = value;
            }
        }
        public TypeContact Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public int CompareTo(TypeContact_Contact? other)
        {
            return this.Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string retVal = string.Format($"{"{0,-8} : {1,-12}\n"}", "Date", $"{DateCreation.ToString("dd-MM-yyyy")}") +
                string.Format("{0,-8} : {1,-30}\n", "ID", $"{Id}") +
                string.Format("{0,-8} : {1,-12}\n", "Contact", $"{this.Contact.Nom} {this.Contact.Prenom}") +
                string.Format("{0,-8} : {1,-12}\n", "Roles", $"{this.Type.Nom}");

            return retVal;
        }


        public static TypeContact_Contact Creer(Contact contact, TypeContact type)
        {
            string id = $"{contact.Id}{type.Id}";
            if (contact == null || type == null)
            {
                ExceptionLauncher.New("GetUnType Contact - Contact", $"Parametre invalide");
            }

            return new TypeContact_Contact(contact, type);
        }
        public static int Save(TypeContact_Contact tpe)
        {
            AllTypeContact_Contact.Add(tpe);
            return AllTypeContact_Contact.DB_Add(tpe);
        }
        public static int Delete(TypeContact_Contact tpe)
        {
            int ret = 0;
            if (AllTypeContact_Contact.Find(tpe.Id) != null)
            {
                AllTypeContact_Contact.Delete(tpe.Id);
                ret = AllTypeContact_Contact.DB_Delete(tpe);
            }
            return ret;
            
        }


    }
}
