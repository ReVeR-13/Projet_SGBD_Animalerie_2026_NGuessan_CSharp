
using AppAnimalerie.ClasseService;


namespace AppAnimalerie.ClasseMetier
{
    public class Abri:ITable
    {
        private string _id;
        private DateTime _date;
        private string _libelle;
        private EStatutAbri _statut;
        private string? _description;
        private Abri(string libelle, string description)
        {
            this.Id = Forma.SimpleId("ABR", AllAbri.Num +1);
            this.DateCreation = DateTime.Now;
            this.Libelle = libelle;
            this.Statut = EStatutAbri.DISPONIBLE;
            this.Description = description;
        }

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = Forma.TrimUpper(value);
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
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Abri", "La date est invalide");
                }
                this._date = value;
            }
        }
        public string Libelle
        {
            get
            {
                return this._libelle;
            }
            set
            { 
                if (value.Length < 2)
                {
                    ExceptionLauncher.New("Abri", "Le libellé est invalide");
                }
                this._libelle = Forma.TrimUpper( value); 
            }
        }
        public EStatutAbri Statut
        {
            get
            {
                return this._statut;
            }
            set
            {
                this._statut = value;
            }
        }
        public string? Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public static Abri Creer(string libelle, string description)
        {
            Abri abri = null;
            if (!string.IsNullOrEmpty(libelle))
            {
                abri = new Abri(libelle, description);
            }
            return abri;
        }
        public static int Save(Abri abri)
        {
            int result = 0;
            if (AllAbri.Find(abri.Id) == null)
            {
                AllAbri.Add(abri);

                result = AllAbri.DB_Add(abri);
            }
            return result;
        }
        public int Update(string libelle, EStatutAbri? statutAbri, string description)
        {
            int modifier = 0;
            if (!string.IsNullOrEmpty(libelle) || statutAbri != null)
            {
                this.Libelle = libelle;
                this.Statut = (EStatutAbri)statutAbri;
                this.Description = description;

                AllAbri.DB_Update(this);

                modifier = 1;
            }
            return modifier;
        }
        public int Update(EStatutAbri? statutAbri)
        {
            int modifier = 0;
            if (statutAbri != null)
            {
                this.Statut = (EStatutAbri)statutAbri;
                AllAbri.DB_Update(this);

                modifier = 1;
            }
            return modifier;
        }
        public static int Delete(Abri abri)
        {
            int result = 0;
            if (true)
            {
                AllAbri.Remove(abri.Id);
                AllAbri.DB_Delete(abri);
                result = 1;
            }
            return result;
        }
        public override string ToString()
        {
            string retVal =
                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id) +
                Forma.Texta2("Nom", Libelle) +
                Forma.Texta2("Statut", Statut.ToString());
                Forma.Texta2("Description", Description);

            return retVal;
        }

    }
}
