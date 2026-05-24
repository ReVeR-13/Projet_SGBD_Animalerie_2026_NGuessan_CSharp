namespace AppAnimalerie.ClasseMetier
{
    public static class AllRaceAnimal
    {
        private static readonly Dictionary<string, RaceAnimal> _lesRaces;
        private static int _num;

        static AllRaceAnimal()
        {
            _lesRaces = [];
            _num = 0;
        }

        public static Dictionary<string, RaceAnimal> LesRaces
        {
            get { return _lesRaces; }
        }
        public static int Num
        {
            get
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesRaces);
                }
                return _num;
            }
        }
        public static int Count
        {
            get { return LesRaces.Count; }
        }
        
        public static string LesRacesToString()
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Nom");
            foreach (RaceAnimal a in LesRaces.Values)
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type.Nom}",
                $"{a.Nom}");
            }
            return Forma.Center($"Liste des Races Animal [{i}/{Count}]\n\n") + retVal;
        }
        public static string LesRacesToString(TypeAnimal type)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Nom");
            foreach (RaceAnimal a in Find(type))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type.Nom}",
                $"{a.Nom}");
            }
            return Forma.Center($"Liste des Races Animal [{i}/{Count}]\n\n") + retVal;
        }

        public static void Add(RaceAnimal raceAnimal)
        {
            if (Find(raceAnimal.Id) != null)
            {
                ExceptionLauncher.New("AllRaceAnimal Add", "Cet Id existe deja");
            }
            if (Find(raceAnimal.Type, raceAnimal.Nom) != null)
            {
                ExceptionLauncher.New("AllRaceAnimal Add", $"Il Existe deja ce nom [{raceAnimal.Nom}] pour ce type[{raceAnimal.Type.Nom}]");
            }
            _num++;
            _lesRaces.Add(raceAnimal.Id, raceAnimal);
        }
        public static void Delete(RaceAnimal raceAnimal)
        {
            if (Find(raceAnimal.Id) == null)
            {
                ExceptionLauncher.New("AllRaceAnimal Delete", "Cet Id n'existe pas");
            }
            _lesRaces.Remove(raceAnimal.Id);
        }

        public static RaceAnimal? Find(string id)
        {
            Forma.ParametreNullTesteur(id);

            RaceAnimal? race = null;
            if (_lesRaces.ContainsKey(Forma.TrimUpper(id)))
            {
                race = _lesRaces[Forma.TrimUpper(id)];
            }
            return race;
        }
        public static RaceAnimal? Find(TypeAnimal type, string nom)
        {
            Forma.ParametreNullTesteur(nom);

            RaceAnimal? race = (RaceAnimal?)_lesRaces.Values.Select(a => a.Nom == Forma.TrimUpper(nom) && a.Type == type);

            return race;
        }
        public static IEnumerable<RaceAnimal> FindByNom(string nom)
        {
            Forma.ParametreNullTesteur(nom);

            RaceAnimal? race = null;
            foreach (RaceAnimal v in _lesRaces.Values.Where(a => a.Nom == Forma.TrimUpper(nom)))
            {
                yield return v;

            }
        }
        public static IEnumerable<RaceAnimal> Find(TypeAnimal type)
        {
            Forma.ParametreNullTesteur(type);

            RaceAnimal? race = null;
            foreach (RaceAnimal v in _lesRaces.Values.Where(a => a.Type == type))
            {
                yield return v;

            }
        }


    }
    public class RaceAnimal : ITable, IComparable<RaceAnimal>
    {
        private string _id;
        private DateTime _dateCreation;
        private TypeAnimal _type;
        private string _nom;
        private string? _infos;

        private RaceAnimal(TypeAnimal type, string nom, string? infos)
        {
            this._id = Forma.SimpleId("RAC", AllRaceAnimal.Num + 1);
            _dateCreation = DateTime.Now;
            this.Type = type;
            this.Nom = nom;
            this.Infos = infos;
        }

        public string Id
        {
            get { return this._id; }
            set { this._id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return this._dateCreation; }
            set { this._dateCreation = Forma.Checked_DateCreation(value); }
        }
        public string Nom
        {
            get { return this._nom; }
            set { this._nom = Forma.Checked_Id(value); }
        }
        public string? Infos
        {
            get { return this._infos; }
            set { this._infos = value; }
        }
        public TypeAnimal Type
        {
            get { return this._type; }
            set { this._type = value; }
        }

        public override string ToString()
        {
            string retval =
                Forma.Center($"Fiche de la race n°[ {this.Id} ]\n", 102) +
                Forma.Center(new string('-', 90) + $"\n") +

                Forma.Texta2("Date Creation", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Type", $"{Type.Nom}") +
                Forma.Texta2("Race", $"{Nom}") +
                Forma.Texta2("Infos utile", $"{Infos}");
            return retval;
        }
        public int CompareTo(RaceAnimal? other)
        {
            return this.Id.CompareTo(other.Id);
        }

        public RaceAnimal Update(TypeAnimal? type, string? nom, string? infos)
        {
            this.Type = type ?? this.Type;
            this.Nom = nom ?? this.Nom;
            this.Infos = infos ?? this.Infos;
            return this;
        }

        public static RaceAnimal Creer(TypeAnimal type, string nom, string? infos)
        {
            Forma.ParametreNullTesteur(type);
            Forma.ParametreNullTesteur(nom);
            if (nom.Length < 3)
            {
                ExceptionLauncher.New("RaceAnimal", "Nom Invalide");
            }
            return new RaceAnimal(type, nom, infos);
        }
        public static int Save(RaceAnimal race)
        {
            Forma.ParametreNullTesteur(race);

            AllRaceAnimal.Add(race);

            return 1;
        }
        public static int Delete(RaceAnimal race)
        {
            AllRaceAnimal.Delete(race);
            return 1;
        }

        
    }
}
