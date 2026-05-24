using AppAnimalerie.AccessDB;


namespace AppAnimalerie.ClasseMetier
{
    public static class AllVaccin
    {
        private static Dictionary<string, Vaccin> _lesVaccins;
        private static int _numVacc;

        static AllVaccin()
        {
            _lesVaccins = new Dictionary<string, Vaccin>();
            _numVacc = 0;
        }
        public static int Num
        {
            get 
            {
                if (Count > 0)
                {
                    _numVacc = Forma.LastNumero(_lesVaccins);
                }
                return _numVacc; 
            }
        }
        public static int Count
        {
            get { return _lesVaccins.Count; }
        }
        public static string LesVaccins
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Vaccins [{Count}]\n" +
                    string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Nom", "Descrip."); 
                foreach (Vaccin vc in _lesVaccins.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-16} {2,-11} {3,-10} {4,-10}\n"}",
                             $"{i}", $"{vc.Id}", $"{vc.DateCreation.ToString("dd-MM-yyyy")}", $"{vc.Nom}", $"{vc.Description}"); 
                }

                return retVal;
            }
        }
        public static string Manquants(Animal ami)
        {
            int i =0;
            Dictionary<string, Vaccination> vaccination = Vaccination.ByAnimal(ami);
            string retVal = 
            Forma.Text("N°", "Id", "Date Crea.", "Nom", "Descrip.");
            foreach (Vaccin vc in _lesVaccins.Values)
            {
                bool veri = true;
                
                foreach (Vaccination va in vaccination.Values)
                {
                    if (va.Vaccin == vc)
                    {
                        veri = false;
                    }
                }

                if (veri)
                {
                    i++;
                    retVal += Forma.Text($"{i}", $"{vc.Id}", $"{vc.DateCreation:dd-MM-yyyy}", $"{vc.Nom}", $"{vc.Description}");
                }

            }
            
            return Forma.Center($"Liste des Vaccins non effectués sur - {ami.Nom} - [{i}]\n") + retVal;
        }
        public static IEnumerable<Vaccin> Get()
        {
            foreach (Vaccin vacc in _lesVaccins.Values)
            {
                yield return vacc;
            }
        }

        public static Vaccin Find(string id)
        {
            string f_id  = Forma.TrimUpper(id);
            Vaccin vc = null;
            if (_lesVaccins.ContainsKey(f_id))
            {
                vc = _lesVaccins[f_id];
            }
            return vc;
        }
        public static Vaccin FindByNom(string nom)
        {
            Vaccin vc = null;
            string f_nom = Forma.TrimUpper(nom);
            foreach (Vaccin v in _lesVaccins.Values)
            {
                if (v.Nom == f_nom)
                {
                    vc = v; 
                    break;
                }
            }
            return vc ;
        }

        public static void Add(Vaccin vaccin)
        {
            if (FindByNom(vaccin.Nom) != null)
            {
                ExceptionLauncher.New("Add AllVaccin", "Cet Vaccin existe deja dans la table des vaccin");
            }
            _numVacc++;
            _lesVaccins.Add(vaccin.Id, vaccin);
        }
        public static void Remove(string id)
        {
            string f_id = Forma.TrimUpper(id);
            if (!_lesVaccins.ContainsKey(f_id))
            {
                ExceptionLauncher.New("RemoveVaccin AllVaccin", "Cet Vaccin n'existe pas dans la table des vaccin");
            }
            _lesVaccins.Remove(f_id);
        }

        public static int DB_Add(Vaccin vaccin)
        {
            int ret = 0;
            if (DB_Vaccin.UnVaccinById(vaccin.Id) == null)
            {
                ret = DB_Vaccin.Add(vaccin);
            }
            return ret;
        }
        public static int DB_Update(Vaccin vaccin)
        {
            int ret = 0;
            if (DB_Vaccin.UnVaccinById(vaccin.Id) != null)
            {
                ret = DB_Vaccin.Update(vaccin);
            }
            return ret;
        }
        public static int DB_Delete(Vaccin vaccin)
        {
            int ret = 0;
            if (DB_Vaccin.UnVaccinById(vaccin.Id) != null )
            {
                ret = DB_Vaccin.Delete(vaccin.Id);
            }
            return ret;
        }

    }
    public class Vaccin : ITable
    {
        private string _id;
        private DateTime _date;
        private string _nom;
        private string _description;
        private Vaccin(string nom,string description)
        {
            _id = Forma.SimpleId("VCC", AllVaccin.Num + 1);
            _date = DateTime.Now;
            Nom = nom;
            Description = description;
        }

        public string Id
        {
            get { return _id; }
            set 
            {
                this._id = Forma.Checked_Id(value); 
            }
            
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {
                this._date = Forma.Checked_DateCreation(value);
            }
        }
        public string Nom
        {
            get { return _nom; }
            set 
            { 
                string F_nom = Forma.TrimUpper(value);
                if (F_nom.Length < 3)
                {
                    ExceptionLauncher.New("Non Vaccin", "Le Nom du vaccin n est pas valable");
                }
                _nom = F_nom; 
            }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public override string ToString()
        {
            string retVal =
                Forma.Texta2("Date",DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom", Nom) +
                Forma.Texta2("Description", Description) ;

            return retVal;
        }
        public IEnumerable<Vaccination> GetVaccination()
        {
            foreach (Vaccination vaccination in Vaccination.ByVaccin(this).Values)
            {
                yield return vaccination;
            }
        }

        public static Vaccin Creer(string nom, string description)
        {
            Vaccin retval = null;
            string f_nom = Forma.TrimUpper(nom);
            if (string.IsNullOrEmpty(nom))
            {
                ExceptionLauncher.New("Creer Vaccin", $"Parametre invalide");
            }
            retval = new Vaccin(nom, description);
            
            return retval;
        }
        public int Modifier(string nom, string description)
        {
            int retval = 0;
            if (AllVaccin.FindByNom(nom)== null)
            {
                this.Nom = nom;
                this.Description = description;
                retval = AllVaccin.DB_Update(this);
            }
            return retval;
        }
        public static int Delete(Vaccin vaccin)
        {
            int retval = 0;
            if (AllVaccin.Find(vaccin.Id) != null)
            {
                OnDelete(vaccin);

                AllVaccin.Remove(vaccin.Id);
                retval = AllVaccin.DB_Delete(vaccin);
            }
            return retval;
        }
        public static int Save(Vaccin vaccin)
        {
            int retval = 0;
            if (AllVaccin.Find(vaccin.Id) == null)
            {
                AllVaccin.Add(vaccin);
                retval = AllVaccin.DB_Add(vaccin);
            }
            return retval;
        }
        private static int OnDelete(Vaccin vaccin)
        {
            if (AllVaccination.FindAllBy(vaccin).Count > 0)
            {
                foreach (Vaccination ac in vaccin.GetVaccination())
                {
                    Vaccination.Delete(ac);
                }
            }
            return 1;
        }

    }
}
