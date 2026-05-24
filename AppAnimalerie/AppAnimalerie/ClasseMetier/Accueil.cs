using AppAnimalerie.AccessDB;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllAccueil
    {
        private static readonly Dictionary<string, Accueil> _lesAccueils;

        static AllAccueil()
        {
            _lesAccueils = new Dictionary<string, Accueil>();
        }

        public static int Count
        {
            get { return _lesAccueils.Count; }
        }
        public static string Liste
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Statut", "Contact", "Date Annul.", "Raison");

                foreach (Accueil dm in _lesAccueils.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Statut}",
                    $"{dm.Demande.Contact.Nom} {dm.Demande.Contact.Prenom}",
                    dm.DateFin == null ? "--" : dm.DateFin?.ToString("dd-MM-yyyy"),
                    dm.DateFin == null ? "--" : dm.RaisonAnullation);
                }

                return $"Liste des Accueils[{Count}]\n\n" + retVal;
            }
        }

        public static string ListeAccueil(EStatutValidation eStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Statut", "Contact", "Date Annul.", "Raison");

            foreach (Accueil dm in _lesAccueils.Values)
            {
                if (dm.Statut == eStatut)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation:dd-MM-yyyy}",
                    $"{dm.Statut}",
                    $"{dm.Demande.Contact.Nom} {dm.Demande.Contact.Prenom}",
                    dm.DateFin == null ? "--" : dm.DateFin?.ToString("dd-MM-yyyy"),
                    dm.DateFin == null ? "--" : dm.RaisonAnullation);
                }

            }

            return $"Liste des Accueils[{i}]\n\n" + retVal;

        }
        public static string ListeAccueil()
        {
            return Liste;
        }

        public static IEnumerable<Accueil> Get()
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                yield return dema;
            }
        }
        public static IEnumerable<Accueil> Get(Contact contact)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Demande.Contact == contact)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Animal animal)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Demande.Animal == animal)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Contact contact, Animal animal)
        {
            foreach (Accueil dema in Get(contact))
            {
                if (dema.Demande.Animal == animal)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values)
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Contact contact,EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values.Where(a => a.Demande.Contact == contact && a.Statut == eStatut)
                .OrderByDescending(a=> a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Accueil> Get(Animal animal, EStatutValidation eStatut)
        {
            foreach (Accueil dema in _lesAccueils.Values.Where(a => a.Demande.Animal == animal && a.Statut == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }

        public static Dictionary<string, Accueil> GetAllAccueil(Contact contacts)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(contacts))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(animal))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal,Contact contact)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(contact,animal))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(EStatutValidation eStatut)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Accueil> GetAllAccueil(Animal animal,EStatutValidation eStatut)
        {
            Dictionary<string, Accueil> retVal = [];

            foreach (Accueil dm in Get(animal,eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Accueil Find(string id)
        {
            Accueil dem = null;
            string fid = Forma.TrimUpper(id);
            if (_lesAccueils.TryGetValue(fid, out Accueil? value))
            {
                dem = value;
            }
            return dem;
        }
        public static Accueil Find(Demande demande)
        {
            Accueil dem = null;
            foreach (Accueil a in _lesAccueils.Values)
            {
                if (a.Demande == demande)
                {
                    dem = a;
                    break;
                }
            }

            return dem;
        }

        public static void Add(Accueil dem)
        {
            if (Find(dem.Id) != null)
            {
                ExceptionLauncher.New("Liste Accueil", "Cette accueil existe deja");
            }
            _lesAccueils.Add(dem.Id, dem);
        }
        public static void Remove(string id)
        {
            string fid = Forma.TrimUpper(id);
            if (Find(fid) == null)
            {
                ExceptionLauncher.New("Liste accueil", "Cette accueil est deja supprimer");
            }
            _lesAccueils.Remove(fid);
        }

        public static int DB_Add(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) == null)
            {
                ret = DB_Accueil.Add(accueil);
            }
            return ret;
        }
        public static int DB_Update(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) != null)
            {
                ret = DB_Accueil.Update(accueil);
            }
            return ret;
        }
        public static int DB_Delete(Accueil accueil)
        {
            int ret = 0;
            if (DB_Accueil.UnAccueilById(accueil) != null)
            {
                ret = DB_Accueil.Delete(accueil);
            }
            return ret;
        }

    }
    public class Accueil : ITable, IComparable<Accueil>
    {
        private string _id;
        private DateTime _date;
        private DateTime? _dateFin;
        private DateTime? _dateDebut;
        private EStatutValidation _statut;
        private string? _infos;
        private string? _raisonAnnulation;
        private Demande _demande;
        private Accueil(Demande demande, string? infos)
        {
            this.Id = "ACC-" + demande.Id;
            this.DateCreation = DateTime.Now;
            this.DateDebut = null;
            this.DateFin = null;
            Statut = EStatutValidation.EN_COURS;
            Info = infos;
            Demande = demande;

            RaisonAnullation = null;
        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public DateTime? DateFin
        {
            get { return _dateFin; }
            set
            {
                if (value < DateCreation)
                {
                    ExceptionLauncher.New("Accueil Date Fin", "La date de fin n'est pas valide.");
                }
                _dateFin = value;
            }
        }
        public DateTime? DateDebut
        {
            get { return _dateDebut; }
            set
            {
                if (value < DateCreation)
                {
                    ExceptionLauncher.New("Accueil Date Debut", "La date debut n'est pas valide.");
                }
                _dateDebut = value;
            }
        }
        public EStatutValidation Statut
        {
            get { return _statut; }
            set { _statut = value; }
        }
        public Demande Demande
        {
            get { return _demande; }
            set
            {
                if (value.Type != ETypeDemande.ACCUEIL)
                {
                    ExceptionLauncher.New("Accueil Demande", $"La demande n'est pas valide. - Statut : {value.Statut} - MyType : {value.Type}");
                }
                _demande = value;
            }
        }
        public string? Info
        {
            get { return _infos; }
            set { _infos = value; }
        }
        public string? RaisonAnullation
        {
            get { return _raisonAnnulation; }
            set { _raisonAnnulation = value; }
        }
        public Sortie? Sortie
        {
            get
            {
                Sortie? ret = null;
                if (AllSortie.Find(this.Demande) != null)
                {
                    ret = AllSortie.Find(this.Demande);
                }
                return ret;
            }
        }
        public override string ToString()
        {
            string? info = null;
            if (this.Statut > EStatutValidation.EN_COURS)
            {
                info = $"- [ {this.Statut} ] -";

                if (Sortie == null)
                {
                    info += $" SORTIE À CRÈER";
                }
                else
                {
                    info += $" SORTIE CRÈER !!!";
                }
                info = Forma.Center(info + "\n\n", 100);
            }

            string retVal =
                Forma.Center($"FICHE D'ACCUEIL N° [ {this.Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n") +

                info +

                Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Id", Id + "\n") +
                Forma.Texta2("Animal Id", Demande.Animal.Id) +
                Forma.Texta2("Nom animal", Demande.Animal.Nom) +
                Forma.Texta2("GetUnType animal", Demande.Animal.Type.Nom + "\n") +
                Forma.Texta2("Demande id", Demande.Id) +
                Forma.Texta2("Contact", Demande.Contact.Nom) +
                Forma.Texta2("Statut", Statut.ToString() + "\n") +
                Forma.Texta2("Date Debut", DateDebut == null ? "--" : DateDebut.ToString()) +
                Forma.Texta2("Date Fin.", DateFin == null ? "--" : DateFin.ToString()) +
                Forma.Texta2("Raison", DateFin == null ? "--" : RaisonAnullation);
            return retVal;
        }

        public static Accueil? Creer(Demande demande, string? infos)
        {
            if (demande == null || demande.Type != ETypeDemande.ACCUEIL)
            {
                ExceptionLauncher.New("Accueil Creer", $"La demande n'est pas de type Accueil - type {demande.Type}");
            }

            Accueil dem = new(demande, infos);

            return dem;
        }
        public static int Save(Accueil accueil)
        {
            int ret = 0;
            if (AllAccueil.Find(accueil.Id) == null && accueil.Demande.Statut == EStatutDemande.EXAMINATION)
            {
                AllAccueil.Add(accueil);
                ret = AllAccueil.DB_Add(accueil);
                Sync(accueil);
            }
            return ret;
        }

        public Accueil? Accepter()
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Accueil Accepter", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Accepter", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Accueil Accepter", "Cette accueil n'est pas enregistré");
            }


            this.Statut = EStatutValidation.ACCEPTEE;
            this.DateDebut = DateTime.Now;
            this.Demande.Update(EStatutDemande.EN_COURS);
            AllAccueil.DB_Update(this);

            return this;
        }
        public Accueil? Refuser(string? motif)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Accueil Refuser", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Refuser", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Accueil Refuser", "Cette accueil n'est pas enregistré");
            }

            this.Statut = EStatutValidation.REFUSEE;
            this.DateFin = DateTime.Now;
            this.RaisonAnullation = motif;
            this.Demande.Update(EStatutDemande.TERMINEE);
            AllAccueil.DB_Update(this);

            return this;
        }
        public Accueil? Indecis()
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Accueil Indecis", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Accueil Indecis", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Accueil Indecis", "Cette accueil n'est pas enregistré");
            }

            this.Statut = EStatutValidation.EN_COURS;
            this.DateDebut = null;
            this.DateFin = null;
            this.RaisonAnullation = null;
            this.Demande.Update(EStatutDemande.VALIDATION);
            AllAccueil.DB_Update(this);

            return this;
        }

        public int Update(Demande demande, string? infos)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Accueil Update", "La Sortie est deja creer");
            }

            int ret = 0;
            if (AllAccueil.Find(this.Id) != null)
            {
                this.Demande = demande;
                this.Info = infos;

                ret = AllAccueil.DB_Update(this);
            }
            return ret;
        }
        public int Update(DateTime? dteD, DateTime? dteF)
        {
            int ret = 0;
            if (AllAccueil.Find(this.Id) != null)
            {
                this.DateDebut = dteD;
                this.DateFin = dteF;

                ret = AllAccueil.DB_Update(this);
            }
            return ret;
        }

        public static int Delete(Accueil accueil)
        {
            int ret = 0;
            if (AllAccueil.Find(accueil.Id) != null)
            {
                OnDelete(accueil);
                AllAccueil.Remove(accueil.Id);
                ret = AllAccueil.DB_Delete(accueil);
            }
            return ret;
        }
        public int CompareTo(Accueil accueil)
        {
            return this.CompareTo(accueil);
        }

        private static int Sync(Accueil accueil)
        {
            int ret;
            if (accueil.Statut == EStatutValidation.REFUSEE)
            {
                ret = accueil.Demande.Update(EStatutDemande.TERMINEE);

            }
            else if (accueil.Statut == EStatutValidation.EN_COURS)
            {
                ret = accueil.Demande.Update(EStatutDemande.VALIDATION);
            }
            else
            {
                ret = accueil.Demande.Update(EStatutDemande.EN_COURS);
            }
            return ret;
        }
        private static int OnDelete(Accueil accueil)
        {
            if (accueil.Sortie != null)
            {
                Sortie.Delete(accueil.Sortie);
            }

            return 1;
        }


    }
}
