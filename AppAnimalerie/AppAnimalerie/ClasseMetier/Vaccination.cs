using AppAnimalerie.AccessDB;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllVaccination
    {
        private static readonly Dictionary<string, Vaccination> _lesVaccinations;
        static AllVaccination()
        {
            _lesVaccinations = new Dictionary<string, Vaccination>();
        }
        public static int Count
        {
            get { return _lesVaccinations.Count; }
        }
        public static string LesVaccinantions
        {
            get
            {
                int i = 0;
                string retVal = $"Liste de toutes les Vaccinantions [{Count}]\n" +
                    Forma.Text("N°", "Id", "Date Crea.", "Vaccin", "Animal", "Remarque");
                foreach (Vaccination vc in _lesVaccinations.Values)
                {
                    i++;
                    retVal += Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Vaccin.Nom}",
                             $"{vc.Animal.Nom}",
                             vc.Remaque ?? "--");
                }

                return retVal;
            }
        }
        public static string LesDernierVaccinantions
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Vaccin", "Animal", "Remarque");
                foreach (Vaccination vc in _lesVaccinations.Values.OrderByDescending(a => a.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Vaccin.Nom}",
                             $"{vc.Animal.Nom}",
                             vc.Remaque ?? "--");
                }

                return $"Liste des Vaccinantions [{i}/{Count}]\n" + retVal;
            }
        }
        public static void Add(Vaccination vacc)
        {
            if (_lesVaccinations.ContainsKey(vacc.Id))
            {
                ExceptionLauncher.New("Vaccinations", "Cet Animal possede deja cet vaccin");
            }
            _lesVaccinations.Add(vacc.Id, vacc);
        }
        public static void Remove(string id)
        {
            if (!_lesVaccinations.ContainsKey(id))
            {
                ExceptionLauncher.New("Vaccinations", "Cet Animal ne possede pas cet vaccin");
            }
            _lesVaccinations.Remove(id);
        }
        public static Vaccination Find(string id)
        {
            Vaccination? retval = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesVaccinations.ContainsKey(f_id))
            {
                retval = _lesVaccinations[f_id];
            }
            return retval;
        }
        public static string ListeByAnimal(string idAnimal)
        {
            string f_id = Forma.TrimUpper(idAnimal);
            int i = 0;
            string retval = string.Format($"{"{0,-4} {1,-11} {2,-10} {3,-10} {4,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Nom", "Descrip.");
            foreach (Vaccination av in _lesVaccinations.Values)
            {
                if (av.Animal.Id == f_id)
                {
                    i++;
                    retval += string.Format($"{"{0,-4} {1,-11} {2,-10} {3,-10} {4,-10}\n"}",
                             $"{i}", $"{av.Id}", $"{av.DateCreation.ToString("dd-MM-yyyy")}", $"{av.Vaccin.Nom}", $"{av.Remaque}"); ;
                }
            }
            return $"Liste des Vaccinations de {AllAnimal.Rechercher(f_id).Nom} [{i}]\n" + retval;
        }
        public static string ListeByVaccin(string idvaccin)
        {
            string f_id = Forma.TrimUpper(idvaccin);
            int i = 0;
            string retval = string.Format($"{"{0,-4} {1,-11} {2,-10} {3,-10} {4,-10} {5,-10}\n"}",
                                     "N°", "Id", "Date Crea.", "Animal", "Vaccin.", "Descrip.");
            foreach (Vaccination av in _lesVaccinations.Values)
            {
                if (av.Vaccin.Id == f_id)
                {
                    i++;
                    retval += string.Format($"{"{0,-4} {1,-11} {2,-10} {3,-10} {4,-10} {5,-10}\n"}",
                             $"{i}",
                             $"{av.Id}",
                             $"{av.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{av.Animal.Nom}",
                             $"{av.Vaccin.Nom}",
                             $"{av.Remaque}");
                }
            }
            return $"Liste des animaux ayant le vaccin {AllVaccin.Find(f_id).Nom} [{i}]\n" + retval;
        }
        public static Dictionary<string, Vaccination> FindAllBy(Animal animal)
        {
            Dictionary<string, Vaccination> retval = new();

            foreach (Vaccination av in _lesVaccinations.Values.Where(a => a.Animal == animal))
            {
                retval.Add(av.Id, av);
            }
            return retval;
        }
        public static Dictionary<string, Vaccination> FindAllBy(Vaccin vaccin)
        {
            Dictionary<string, Vaccination> retval = new Dictionary<string, Vaccination>();

            foreach (Vaccination av in _lesVaccinations.Values.Where(a => a.Vaccin == vaccin))
            {
                retval.Add(av.Id, av);
            }
            return retval;
        }

        public static int DB_Add(Vaccination vaccination)
        {
            int retval = 0;
            if (DB_Vaccination.UnVaccinationById(vaccination.Id) == null)
            {
                retval = DB_Vaccination.Add(vaccination);
            }
            return retval;
        }
        public static int DB_Update(Vaccination vaccination)
        {
            int retval = 0;
            if (DB_Vaccination.UnVaccinationById(vaccination.Id) != null)
            {
                retval = DB_Vaccination.Update(vaccination);
            }
            return retval;
        }
        public static int DB_Delete(Vaccination vaccination)
        {
            int retval = 0;
            if (DB_Vaccination.UnVaccinationById(vaccination.Id) != null)
            {
                retval = DB_Vaccination.Delete(vaccination.Id);
            }
            return retval;
        }

    }
    public class Vaccination : ITable, IComparable<Vaccination>
    {
        private string _id;
        private DateTime _date;
        private Animal _animal;
        private Vaccin _vaccin;
        private string? _remaque;
        private Vaccination(Animal animal, Vaccin vaccin, string? remarque)
        {
            this._id = animal.Id + vaccin.Id;
            this._date = DateTime.Now;
            this.Animal = animal;
            this.Vaccin = vaccin;
            this.Remaque = remarque;

        }
        public string Id
        {
            get { return this._id; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ExceptionLauncher.New("Vaccination id", "L Id est incorrecte");
                }
                this._id = value;
            }
        }
        public DateTime DateCreation
        {
            get { return this._date; }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Vaccination Dade Creation", "La Date est incorrecte");
                }
                this._date = value;
            }
        }
        public Animal Animal
        {
            get { return this._animal; }
            set { this._animal = value; }
        }
        public Vaccin Vaccin
        {
            get { return this._vaccin; }
            set { this._vaccin = value; }
        }
        public string? Remaque
        {
            get { return this._remaque; }
            set { this._remaque = value; }
        }

        public int CompareTo(Vaccination? other)
        {
            return this.Id.CompareTo(other?._id);
        }
        public string ToString()
        {
            string retVal =
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom Vaccin", Vaccin.Nom) +
                Forma.Texta2("Nom Animal", Animal.Nom) +
                Forma.Texta2("Remarque", Remaque == null ? "--" : Remaque);

            return retVal;
        }

        public static Dictionary<string, Vaccination> ByAnimal(Animal animal)
        {
            return AllVaccination.FindAllBy(animal);
        }
        public static Dictionary<string, Vaccination> ByVaccin(Vaccin vaccin)
        {
            return AllVaccination.FindAllBy(vaccin);
        }

        public static Vaccination Creer(Animal animal, Vaccin vaccin, string? remarque)
        {
            Vaccination animalVaccin = new(animal, vaccin, remarque);
            return animalVaccin;
        }
        public static int Save(Vaccination vaccination)
        {
            int ret = 0;
            if (AllVaccination.Find(vaccination.Id) == null)
            {
                AllVaccination.Add(vaccination);
                ret = AllVaccination.DB_Add(vaccination);
            }
            return ret;
        }
        public int Update(Animal animal, Vaccin vaccin, string? remarque)
        {
            int ret = 0;
            if (animal != null && vaccin != null)
            {
                this.Animal = animal;
                this.Vaccin = vaccin;
                this.Remaque = remarque;

                AllVaccination.DB_Update(this);
            }
            return ret;
        }
        public static int Delete(Vaccination vaccination)
        {
            int ret = 0;
            if (AllVaccination.Find(vaccination.Id) != null)
            {
                AllVaccination.Remove(vaccination.Id);
                ret = AllVaccination.DB_Delete(vaccination);
            }
            return ret;
        }
    }
}
