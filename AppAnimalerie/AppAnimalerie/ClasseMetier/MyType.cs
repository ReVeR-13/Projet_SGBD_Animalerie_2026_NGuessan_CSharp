using AppAnimalerie.AccessDB;
using AppAnimalerie.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllTypeContact
    {
        private static readonly Dictionary<string, TypeContact> _lesTypesContacts;
        private static int _numType;
        static AllTypeContact()
        {
            _lesTypesContacts = new Dictionary<string, TypeContact>();
            _numType = 0;
        }

        public static int Count
        {
            get
            {
                return _lesTypesContacts.Count;
            }
        }
        public static int NumType
        {
            get
            {
                if (Count > 0)
                {
                    _numType = Forma.LastNumero(_lesTypesContacts);
                }
                return _numType;
            }
        }
        public static string LesTypesContacts
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "Id", "Date Crea.", "Type", "Desciption");
                foreach (TypeContact a in _lesTypesContacts.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }
                return Forma.Center($"Liste des types Contacts [{i}/{Count}]\n\n") + retVal;
            }
        }


        public static void Add(TypeContact type)
        {
            if (FindByNom(type.Nom) != null)
            {
                ExceptionLauncher.New("MyType Contact", "Cet type existe deja...");
            }
            _lesTypesContacts.Add(type.Id, type);
            _numType++;
        }
        public static void Delete(TypeContact type)
        {
            if (!_lesTypesContacts.ContainsKey(type.Id))
            {
                throw new Exception($"[Groupe Type Contact] Ce type de contact n' existe plus : {type.Nom}");
            }
            _lesTypesContacts.Remove(type.Id);
        }
        public static TypeContact FindByNom(string nom)
        {
            TypeContact type = null;
            foreach (TypeContact t in _lesTypesContacts.Values)
            {
                if (t.Nom == Forma.TrimUpper(nom))
                {
                    type = t;
                    break;
                }
            }

            return type;
        }
        public static TypeContact FindById(string id)
        {
            TypeContact type = null;
            if (_lesTypesContacts.ContainsKey(id.Trim().ToUpper()))
            {
                type = _lesTypesContacts[id.Trim().ToUpper()];
            }
            return type;
        }
        public static string LesTypesContactsManquant(Contact contact)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Desciption");
            foreach (TypeContact a in _lesTypesContacts.Values)
            {
                bool verif = false;
                foreach (TypeContact_Contact b in contact.EnumType())
                {
                    if (a == b.Type)
                    {
                        verif = true;
                    }
                }

                if (!verif)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }

            }
            return $"Liste des Types Contacts [{i}]\n\n" + retVal;
        }

        public static int DB_Add(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactByNom(type.Nom) == null)
            {
                retval = DB_TypeContact.Add(type);

            }
            return retval;
        }
        public static int DB_Update(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactByNom(type.Nom) != null)
            {
                retval = DB_TypeContact.Update(type);

            }
            return retval;
        }
        public static int DB_Delete(TypeContact type)
        {
            int retval = 0;
            if (DB_TypeContact.UnTypesContactByNom(type.Nom) != null)
            {
                retval = DB_TypeContact.Delete(type.Nom);

            }
            return retval;
        }

    }
    public static class AllTypeAnimal
    {
        private static readonly Dictionary<string, TypeAnimal> lesTypes;
        private static int _num;

        static AllTypeAnimal()
        {
            lesTypes = new Dictionary<string, TypeAnimal>();
            _num = 0;
        }
        public static string LesTypes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Types Animals [{Count}]\n\n" +
                    string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Type", "Desciption");
                foreach (TypeAnimal a in lesTypes.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-20}\n"}",
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{a.Nom}",
                    $"{a.Description}");
                }
                return retVal;
            }
        }
        public static int Count
        {
            get
            {
                return lesTypes.Count;
            }
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(lesTypes);
                }
                return _num;
            }
        }

        public static void Add(TypeAnimal type)
        {
            if (FindTypeByNom(type.Nom) != null)
            {
                ExceptionLauncher.New("Groupe Type Animal", $"Cet type d'animal existe deja : {type.Nom}");
            }
            _num++;
            lesTypes.Add(type.Id, type);
        }
        public static void Delete(string id)
        {
            if (!lesTypes.ContainsKey(id))
            {
                throw new Exception($"[Groupe Type Animal] Ce Animal n' existe plus : {id}");
            }
            lesTypes.Remove(id);
        }
        public static TypeAnimal? FindTypeByNom(string nom)
        {
            TypeAnimal? type = null;
            foreach (TypeAnimal item in lesTypes.Values)
            {
                if (item.Nom == Forma.TrimUpper(nom))
                {
                    type = item;
                    break;
                }
            }

            return type;
        }
        public static TypeAnimal? FindTypebyId(string id)
        {
            TypeAnimal? type = null;
            if (lesTypes.ContainsKey(Forma.TrimUpper(id)))
            {
                type = lesTypes[Forma.TrimUpper(id)];
            }
            return type;
        }

        public static int DB_Add(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalByNom(type.Nom) == null)
            {
                retval = DB_TypeAnimal.Add(type);
            }

            return retval;
        }
        public static int DB_Update(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalById(type.Id) != null)
            {
                retval = DB_TypeAnimal.Update(type);
            }

            return retval;
        }
        public static int DB_Delete(TypeAnimal type)
        {
            int retval = 0;
            if (DB_TypeAnimal.UnTypesAnimalById(type.Id) != null)
            {
                retval = DB_TypeAnimal.Delete(type);
            }

            return retval;
        }

    }

    public abstract class MyType : ITable, IComparable<string>
    {
        private DateTime date;
        private string nom;
        private string? description;
        protected MyType(string nom, string description)
        {
            date = DateTime.Now;
            Nom = nom;
            Description = description;
        }
        public abstract string Id
        {
            get;
            set;
        }
        public virtual string Nom
        {
            get { return nom; }
            set
            {
                if (value.Trim().Length < 3 || string.IsNullOrEmpty(value))
                {
                    throw new Exception($"[Type Animal] Le nom n'est pas valide:{value} ");
                }
                nom = Forma.TrimUpper(value);
            }
        }
        public virtual string? Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
        public virtual DateTime DateCreation
        {
            get { return date; }
            set { date = value; }
        }
        public int CompareTo(string? nom)
        {
            return this.Nom.CompareTo(nom);
        }

        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("Nom", $"{Nom}") +
                Forma.Texta2("Description", $"{Description}");

            return retVal;
        }

    }
    public class TypeAnimal : MyType
    {
        private string id;
        private TypeAnimal(string nom, string description) : base(nom, description)
        {
            id = Forma.SimpleId("TAN", AllTypeAnimal.Num + 1);
        }
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ToString()
        {
            string retVal =
                Forma.Texta2("ID", $"{Id}\n") +
                base.ToString();

            return retVal;
        }

        public static TypeAnimal Creer(string nom, string description)
        {
            string f_nom = Forma.TrimUpper(nom);
            if (string.IsNullOrEmpty(f_nom))
            {
                throw new Exception($"[Type Animal] Ce nom est null");
            }
            TypeAnimal retVal = new(f_nom, description);
            
            return retVal;
        }
        public static int Save(TypeAnimal type)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypebyId(type.id) == null)
            {
                AllTypeAnimal.Add(type);
                ret = AllTypeAnimal.DB_Add(type);
            }
            return ret;
        }
        public int Update(string nom, string description)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypebyId(this.Id) != null)
            {
                this.Nom = nom;
                this.Description = description; 
                AllTypeAnimal.DB_Update(this);
            }
            return ret;
        }
        public static int Delete(TypeAnimal type)
        {
            int ret = 0;
            if (AllTypeAnimal.FindTypebyId(type.Id) != null)
            {
                OnDelete(type);
                AllTypeAnimal.Delete(type.Id);
                ret = AllTypeAnimal.DB_Delete(type);
            }
            return ret;
        }

        private static int OnDelete(TypeAnimal type)
        {
            if (AllAnimal.Get(type).Any())
            {
                foreach (Animal ac in AllAnimal.Get(type))
                {
                    Animal.Delete(ac);
                }
            }
            return 1;
        }

    }
    public class TypeContact : MyType
    {
        private string id;
        private TypeContact(string nom, string description) : base(nom, description)
        {
            id = Forma.SimpleId("TCT", AllTypeContact.NumType + 1);
        }
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string ToString()
        {
            string retVal =
                Forma.Center($"LES ROLES CONTACT - [ {this.Id} ] - \n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                Forma.Texta2("ID", $"{Id}") +
                base.ToString();

            return retVal;
        }

        public IEnumerable<TypeContact_Contact> EnumType()
        {
            foreach (TypeContact_Contact type in AllTypeContact_Contact.AllOfType(this).Values)
            {
                yield return type;
            }
        }

        public static TypeContact Creer(string nom, string description)
        {
            Forma.ParametreNullTesteur(nom);

            TypeContact retVal = new TypeContact(nom, description);
            return retVal;
        }
        public static int Save(TypeContact type)
        {
            if (AllTypeContact.FindByNom(type.Nom) != null)
            {
                throw new Exception($"[Type Animal] Ce Animal existe deja:\n{AllTypeContact.FindByNom(type.Nom)}");
            }
            AllTypeContact.Add(type);
            return AllTypeContact.DB_Add(type);
        }
        public int Update(string nom, string description)
        {
            int retVal = 0;
            if (AllTypeContact.FindByNom(Forma.TrimUpper(nom)) == null)
            {
                this.Nom = nom;
                this.Description = description;
                retVal = AllTypeContact.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(TypeContact type)
        {
            int retVal = 0;
            if (AllTypeContact.FindById(type.Id) != null)
            {
                OnDelete(type);
                AllTypeContact.Delete(type);
                retVal = AllTypeContact.DB_Delete(type);
            }

            return retVal;
            AllTypeContact.Delete(type);
        }

        private static int OnDelete(TypeContact type)
        {
            if (AllTypeContact_Contact.AllOfType(type).Count > 0)
            {
                foreach (TypeContact_Contact ac in type.EnumType())
                {
                    TypeContact_Contact.Delete(ac);
                }
            }
            return 1;
        }

    }
    
}



