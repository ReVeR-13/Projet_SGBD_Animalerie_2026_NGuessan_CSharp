using AppAnimalerie.AccessDB;
using AppAnimalerie.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllTypeContact_Contact
    {
        private static readonly Dictionary<string, TypeContact_Contact> _lesTypeContact_Contact;
        private static int _numType;
        static AllTypeContact_Contact()
        {
            _lesTypeContact_Contact = new Dictionary<string, TypeContact_Contact>();
            _numType = 0;
        }
        public static void Add(TypeContact_Contact type)
        {
            if (Find(type.Id) != null)
            {
                ExceptionLauncher.New("GetUnType Contact", "Cet Contact a deja cet role...");
            }
            _lesTypeContact_Contact.Add(type.Id, type);
            _numType++;
        }
        public static void Delete(string id)
        {
            if (!_lesTypeContact_Contact.ContainsKey(id.Trim().ToUpper()))
            {
                throw new Exception($"[MyType Contact - Contact] Ce contact n'a pas ce type de contact : {id}");
            }
            _lesTypeContact_Contact.Remove(id.Trim().ToUpper());
        }

        public static TypeContact_Contact Find(string id)
        {
            TypeContact_Contact type = null;
            if (_lesTypeContact_Contact.ContainsKey(Forma.TrimUpper(id)))
            {
                type = _lesTypeContact_Contact[Forma.TrimUpper(id)];
            }
            return type;
        }
        public static TypeContact_Contact Find(Contact contacts, TypeContact type)
        {
            TypeContact_Contact tpe = null;
            foreach (TypeContact_Contact t in AllOfContact(contacts).Values)
            {
                if (t.Type == type)
                {
                    tpe = t;
                }
            }
            
            return tpe;
        }
        public static Dictionary<string, TypeContact_Contact> AllOfContact(Contact contact)
        {
            Dictionary<string, TypeContact_Contact> retval = new Dictionary<string, TypeContact_Contact>();
            foreach (TypeContact_Contact t in _lesTypeContact_Contact.Values)
            {
                if (t.Contact == contact)
                {
                    retval.Add(t.Type.Id, t);
                }
            }
            return retval;
        }
        public static Dictionary<string, TypeContact_Contact> AllOfType(TypeContact type)
        {
            Dictionary<string, TypeContact_Contact> retval = new Dictionary<string, TypeContact_Contact>();
            foreach (TypeContact_Contact t in _lesTypeContact_Contact.Values)
            {
                if (t.Type == type)
                {
                    retval.Add(t.Id, t);
                }
            }
            return retval;
        }
        public static TypeContact_Contact? DB_Find(TypeContact_Contact type)
        {
            return DB_TypeCnt_Contact.UnRoles(type);
        }

        public static string LesTypes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Types Contacts [{Count}]\n\n" +
                    string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-13} {4,-15}\n"}",
                    "N°", "Id", "Date Crea.", "Contact", "Roles");
                foreach (TypeContact_Contact a in _lesTypeContact_Contact.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-30} {2,-11} {3,-13} {4,-15}\n"}",
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{a.Contact.Nom}",
                    $"{a.Type.Nom}");
                }
                return retVal;
            }
        }
        public static int Count
        {
            get
            {
                return _lesTypeContact_Contact.Count;
            }
        }
        public static int NumType
        {
            get
            {
                return _numType;
            }
        }

        public static int DB_Add(TypeContact_Contact type)
        {
            int ret = 0;

            if (DB_TypeCnt_Contact.UnRoles(type) == null)
            {
                ret = DB_TypeCnt_Contact.Add(type); ;
            }
            return ret;
        }
        public static int DB_Delete(TypeContact_Contact type)
        {
            int ret = 0;

            if (DB_TypeCnt_Contact.UnRoles(type) != null)
            {
                ret = DB_TypeCnt_Contact.Delete(type); ;
            }
            return ret;
        }
    }
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
