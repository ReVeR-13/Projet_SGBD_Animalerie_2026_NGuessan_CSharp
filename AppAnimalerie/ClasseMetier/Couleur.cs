
using AppAnimalerie.ClasseService;

namespace AppAnimalerie.ClasseMetier
{
    public class Couleur : ITable, IComparable<Couleur>
    {
        private string _id;
        private DateTime _date;
        private string _nom;

        private Couleur(string nom)
        {
            this.Id = Forma.SimpleId("COL", AllCouleur.Num +1);
            this.DateCreation = DateTime.Now;
            this.Nom = nom;
        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return this._date; }
            set { this._date = Forma.Checked_DateCreation(value); }
        }
        public string Nom 
        { 
            get { return this._nom;} 
            set { this._nom = Forma.TrimUpper(value); }
        }

        public int CompareTo(Couleur? other)
        {
            return this.Nom.CompareTo(other.Nom);
        }
        public override string ToString()
        {
            string retval = Forma.Padding($"Fiche de Couleur N° [{this.Id}]\n")  +
                Forma.Padding($"********************************************\n") +
                Forma.Texta2("DATE", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("ID", Id) +
                Forma.Texta2("NOM", Nom);
            return retval;
        }

        public IEnumerable<AnimalCouleur> GetAnimalCouleur()
        {
            foreach (AnimalCouleur coloration in AllAnimalCouleur.FindAllByCouleur(this).Values)
            {
                yield return coloration;
            }
        }

        public static Couleur Creer(string nom)
        {
            if (string.IsNullOrEmpty(nom))
            {
                ExceptionLauncher.New("Couleur Creer", "Parametre null");
            }
            return new Couleur(nom);
        }
        public Couleur Update(string nom)
        {
            if (string.IsNullOrEmpty(nom))
            {
                ExceptionLauncher.New("Couleur Update", "Parametre null");
            }

            if (AllCouleur.FindByNom(nom) != null)
            {
                ExceptionLauncher.New("Couleur Update", "Cet nom existe deja");
            }
            this.Nom = nom;
            AllCouleur.DB_Update(this);

            return this;
        }
        public static int Save(Couleur couleur)
        {
            if (couleur == null)
            {
                ExceptionLauncher.New("Couleur Save", "Parametre null");
            }
            AllCouleur.Add(couleur);
            AllCouleur.DB_Add(couleur);
            return 1;
        }
        public static int Delete(Couleur couleur)
        {
            int retVal = 0;

            if (AllCouleur.Find(couleur.Id) != null)
            {
                OnDelete(couleur);

                AllCouleur.Delete(couleur.Id);
                retVal = AllCouleur.DB_Delete(couleur);
            }
            return retVal;
        }
        private static int OnDelete(Couleur couleur)
        {
            if (AllAnimalCouleur.FindAllByCouleur(couleur).Count > 0)
            {
                foreach (AnimalCouleur ac in couleur.GetAnimalCouleur())
                {
                    ac.Delete();
                }
            }

            return 1;
        }
    }
}
