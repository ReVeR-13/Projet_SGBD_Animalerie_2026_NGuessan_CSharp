
using AppAnimalerie.AccessDB;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllCompatibilite
    {
        private static readonly Dictionary<string, Compatibilite> _lesCompatibilites;
        private static int _num;

        static AllCompatibilite()
        {
            _num = 0;
            _lesCompatibilites = [];
        }

        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesCompatibilites);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return _lesCompatibilites.Count; }
        }
        public static string LesCompatibilites
        {
            get
            {
                int i = 0;
                string retVal = 
                    Forma.Text("N°", "ID", "Date Crea.", "Nom", "Details");
                foreach (Compatibilite vc in _lesCompatibilites.Values)
                {
                    i++;
                    retVal += Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Nom}",
                             $"{vc.Details}");
                }
                return $"Liste des Compatibilités [{i}/{Count}]\n" + retVal;
            }
        }
        public static string Manquants(Animal ami)
        {
            int i = 0;
            string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                Forma.Padding(Forma.Text("N°", "Date Crea.", "Nom", "Details"));

            foreach (Compatibilite vc in _lesCompatibilites.Values)
            {
                bool veri = true;

                foreach (AnimalCompatibilité va in AnimalCompatibilitéService.FindAllByAnimal(ami).Values)
                {
                    if (va.Compatibilite == vc)
                    {
                        veri = false;
                    }
                }

                if (veri)
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Nom}",
                             $"{vc.Details}"));
                }

            }

            return Forma.Center($"Liste des Compatibilités manquants [{i}/{Count}]\n") + retVal;
        }
        public static IEnumerable<Compatibilite> Get()
        {
            foreach (Compatibilite cont in _lesCompatibilites.Values)
            {
                yield return cont;
            }
        }
        public static Compatibilite Find(string id)
        {
            string f_id = Forma.TrimUpper(id);
            Compatibilite vc = null;
            if (_lesCompatibilites.ContainsKey(f_id))
            {
                vc = _lesCompatibilites[f_id];
            }
            return vc;
        }
        public static Compatibilite FindByNom(string nom)
        {
            string f_nom = Forma.TrimUpper(nom);
            Compatibilite vc = null;
            foreach (Compatibilite c in _lesCompatibilites.Values)
            {
                if (c.Nom == f_nom)
                {
                    vc = c;
                }
            }

            return vc;
        }
        public static void Add(Compatibilite comp)
        {
            if (Find(comp.Id) != null)
            {
                ExceptionLauncher.New("Add Compatibilite", "Cette compatibilé existe deja dans la table");
            }
            _num++;
            _lesCompatibilites.Add(Forma.TrimUpper(comp.Id), comp);
        }
        public static void Remove(Compatibilite compatibilite)
        {
            if (Find(compatibilite.Id) == null)
            {
                ExceptionLauncher.New("RemoveVaccin AllCompatibilite", "Cette compatibilité n'existe pas dans la table");
            }
            _lesCompatibilites.Remove(compatibilite.Id);
        }

        public static int DB_Add(Compatibilite comp)
        {
            int ret = 0;
            if (DB_Compatibilite.UnCompatibiliteById(comp.Id) == null)
            {
                ret = DB_Compatibilite.Add(comp);
            }
            return ret;
        }
        public static int DB_Update(Compatibilite comp)
        {
            int ret = 0;
            if (DB_Compatibilite.UnCompatibiliteById(comp.Id) != null)
            {
                ret = DB_Compatibilite.Update(comp);
            }
            return ret;
        }
        public static int DB_Delete(Compatibilite comp)
        {
            int ret = 0;
            if (DB_Compatibilite.UnCompatibiliteById(comp.Id) != null)
            {
                ret = DB_Compatibilite.Delete(comp.Id);
            }
            return ret;
        }

    }

    public class Compatibilite : ITable, IComparable<Compatibilite>
    {
        private string _id;
        private DateTime _date;
        private string _nom;
        private string _details;
        private Compatibilite(string nom, string details)
        {
            this._id = Forma.SimpleId("CMP", AllCompatibilite.Num + 1);
            this._date = DateTime.Now;
            this.Nom = nom;
            this._details = details;
        }

        public string Id
        {
            get { return this._id; }
            set
            {
                if (value == null || value.Length != 15)
                {
                    ExceptionLauncher.New("Compatibilite ID", "L'id est invalide");
                }
                this._id = Forma.TrimUpper(value);
            }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Compatibilite Date Creation", "La date est invalide");
                }
                this._date = value;
            }
        }
        public string Details
        {
            get { return this._details; }
            set { this._details = value; }
        }
        public string Nom
        {
            get { return this._nom; }
            set
            {
                if (value.Length < 2 && !Forma.IsCaractere(value))
                {
                    ExceptionLauncher.New("Compatibilité", "Le nom est invalide :" + value);
                }
                this._nom = Forma.TrimUpper(value);
            }
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Id", this.Id) +
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Nom", this.Nom) +
                Forma.Texta2("Details", Details);

            return retVal;
        }
        public int CompareTo(Compatibilite comp)
        {
            return this.Nom.CompareTo(comp.Nom);
        }
        public IEnumerable<AnimalCompatibilité> GetAnimalCompatibilité()
        {
            foreach (AnimalCompatibilité comp in AnimalCompatibilitéService.FindAllByCompatibilite(this).Values)
            {
                yield return comp;
            }
        }

        public static Compatibilite Creer(string nom, string details)
        {
            Compatibilite retval = null;

            retval = new Compatibilite(nom, details);
            return retval;
        }
        public static int Save(Compatibilite compatibilite)
        {
            int retval = 0;
            if (AllCompatibilite.Find(compatibilite.Id) == null)
            {
                AllCompatibilite.Add(compatibilite);
                retval = AllCompatibilite.DB_Add(compatibilite);
            }
            return retval;
        }
        public int Update(string details, string nom)
        {
            int retval = 0;
            if (AllCompatibilite.FindByNom(Forma.TrimUpper(nom)) == null)
            {
                this.Nom = nom;
                this.Details = details;

                retval = AllCompatibilite.DB_Update(this);
            }
            return retval;
        }
        public static int Delete(Compatibilite compatibilite)
        {
            int retval = 0;
            if (AllCompatibilite.Find(compatibilite.Id) != null)
            {
                OnDelete(compatibilite);

                AllCompatibilite.Remove(compatibilite);
                retval = AllCompatibilite.DB_Delete(compatibilite);
            }
            return retval;
        }
        private static int OnDelete(Compatibilite compatibilite)
        {
            if (AnimalCompatibilitéService.FindAllByCompatibilite(compatibilite).Count > 0)
            {
                foreach (AnimalCompatibilité ac in compatibilite.GetAnimalCompatibilité())
                {
                    AnimalCompatibilité.Delete(ac);
                }
            }
            return 1;
        }

    }
}
