using AppAnimalerie.AccessDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public static class UserService
    {
        private static readonly Dictionary<string, User> _lesUsers;
        private static int _num;
        static UserService()
        {
            _lesUsers = [];
            _num = 0;
        }

        public static int Num
        {
            get 
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesUsers);
                }
                return _num; 
            }
        }
        public static int Count
        {
            get
            {
                return _lesUsers.Count;
            }
        }
        public static string LesUsers
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Utilisateurs [{Count}]\n\n" +
                    Forma.Text("N°", "Id", "Date Crea.", "Username", "Password", "Email", "Admin", "Active", "Updated", "Dernier Conn.");
                foreach (User a in _lesUsers.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Password}",
                    $"{a.Email}",
                    a.Admin? "Oui":"Non",
                    a.Active? "Oui":"Non",
                    $"{a.Updated:dd-MM-yyyy}",
                    $"{a.LastConnection:dd-MM-yyyy}");
                }
                return retVal;
            }
        }

        public static IEnumerable<User> Get()
        {
            foreach (User cont in _lesUsers.Values)
            {
                yield return cont;
            }
        }
        public static User? Find(string id)
        {
            User? retVal = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesUsers.TryGetValue(f_id, out User? value))
            {
                retVal = value;
            }
            return retVal;
        }
        public static User? FindByMail(string mail)
        {

            User? retVal = null;
            if (Forma.IsMail(mail))
            {
                foreach (User c in _lesUsers.Values)
                {
                    if (c.Email == mail)
                    {
                        retVal = c;
                        break;
                    }
                }
            }
            return retVal;
        }
        public static void Add(User user)
        {
            if (Find(user.Id) != null)
            {
                throw new Exception($"[ Groupe Users] Cet identifiant existe deja :{user.Id}");
            }
            _num++;
            _lesUsers.Add(user.Id, user);
        }
        public static void Remove(string idc)
        {
            if (Find(idc) == null)
            {
                ExceptionLauncher.New($"[ Groupe Users]",$" Cet Animal est deja supprimé :{idc}");
            }
            _lesUsers.Remove(idc);
        }
        
        public static int DB_Add(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) == null)
            {
                retVal = DB_User.Add(user);
            }
            return retVal;
        }
        public static User? DB_FindById(string id)
        {
            User? retVal = null;
            if (!string.IsNullOrEmpty(id))
            {
                retVal = DB_User.UnUserById(id);
            }
            return retVal;
        }
        public static User? DB_FindByEmail(string email)
        {
            User? retVal = null;
            if (Forma.IsMail(email))
            {
                retVal = DB_User.UnUserByEmail(email);
            }
            return retVal;
        }
        public static int DB_Update(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.Update(user);
            }
            return retVal;
        }
        public static int DB_UpdateAdmin(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.UpdateAdmin(user);
            }
            return retVal;
        }
        public static int DB_UpdateActive(User user)
        {
            int retVal = 0;
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.UpdateActive(user);
            }
            return retVal;
        }
        public static int DB_Delete(User user)
        {
            int retVal = 0;
            //les donnees doivent etre supprimer en locale
            if (DB_User.UnUserById(user.Id) != null)
            {
                retVal = DB_User.Delete(user.Id);
            }
            return retVal;
        }

    }
    public class User : ITable, IComparable<User>
    {
        private string _id;
        private DateTime _dateCreation;

        private string _nom;
        private string _password;
        private string _email;
        private string _description;

        private bool _admin;
        private bool _active;

        private DateTime _updated;
        private DateTime _lastConnection;

        private User(string nom, string password, string mail, string description)
        {
            this.Id = Forma.SimpleId("UTI", UserService.Num);
            this.DateCreation = DateTime.Now;

            this.Nom = nom;
            this.Password = password;
            this.Email = mail;
            this.Description = description;

            this.Admin = false;
            this.Active = false;

            this.Updated = DateTime.Now;
            this.LastConnection = DateTime.Now;

        }

        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public DateTime DateCreation
        {
            get { return this._dateCreation; }
            set { this._dateCreation = Forma.Checked_DateCreation(value); }
        }
        public string Nom
        {
            get { return this._nom; }
            set {
                if (value.Length < 4)
                {
                    ExceptionLauncher.New("User Nom", "Le nombre de caractere doit etre plus de 4");
                }
                this._nom = value; 
            }
        }
        public string Password
        {
            get { return this._password; }
            set
            {
                if (value.Length < 8)
                {
                    ExceptionLauncher.New("User Password", "Le nombre de caractere doit etre au mininum de 8");
                }
                this._password = value;
            }
        }
        public string Email
        {
            get { return this._email; }
            set
            {
                if (!Forma.IsMail(value))
                {
                    ExceptionLauncher.New("User Email", "Cet email n'est pas valide");
                }
                this._email = value;
            }
        }
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        public bool Admin
        {
            get { return this._admin; }
            set { this._admin = value; }
        }
        public bool Active
        {
            get { return this._active; }
            set { this._active = value; }
        }
        public DateTime Updated
        {
            get { return this._updated; }
            set { this._updated = value; }
        }
        public DateTime LastConnection
        {
            get { return this._lastConnection; }
            set { this._lastConnection = value; }
        }

        public int CompareTo(User? other)
        {
            return this.Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string retval = Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Username", $"{Nom}") +
                Forma.Texta2("Password", $"{Password}") +
                Forma.Texta2("Email", $"{Email}") +
                Forma.Texta2("isAdmin", Admin? "Oui":"Non") +
                Forma.Texta2("isActive", Active ? "Oui" : "Non") +
                Forma.Texta2("Updated", $"{Updated:dd-MM-yyyy}") +
                Forma.Texta2("Last Connection", $"{LastConnection:dd-MM-yyyy}") +
                Forma.Texta2("Description", $"{Description}"); ;
            return retval;
        }

        public static User Create(string username, string password,string mail,string description)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail))
            {
                ExceptionLauncher.New("User Creer", "Parametres null");
            }

            return new User(username, password, mail ,description);
        }
        public User Update(string username, string password, string mail, string description)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mail))
            {
                ExceptionLauncher.New("User Updated", "Parametres null");
            }

            this.Nom = username;
            this.Password = password;
            this.Email = mail;
            this.Description = description;

            this.Updated = DateTime.Now;

            return this;
        }
        public User UpdateAdmin(bool admin)
        {
            this.Admin = admin;
            this.Updated = DateTime.Now;
            return this;
        }
        public User UpdateActive(bool active)
        {
            this.Active = active;
            this.Updated = DateTime.Now;
            return this;
        }
        public static User Save(User user)
        {
            UserService.Add(user);
            return user;
        }

    }
}
