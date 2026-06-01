using AppAnimalerie.ClasseService;

namespace AppAnimalerie.ClasseMetier
{

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
