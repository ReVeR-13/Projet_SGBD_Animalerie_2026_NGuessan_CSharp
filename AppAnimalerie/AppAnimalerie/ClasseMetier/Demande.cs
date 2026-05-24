using AppAnimalerie.AccessDB;


namespace AppAnimalerie.ClasseMetier
{
    public static class AllDemande
    {
        private static readonly Dictionary<string, Demande> _lesDemandes;
        private static int _num;

        static AllDemande()
        {
            _num = 0;
            _lesDemandes = new Dictionary<string, Demande>();
        }

        public static int Num
        {
            get 
            {
                if(Count > 0)
                {
                    _num = Forma.LastNumero(_lesDemandes);
                }
                return _num; 
            }
        }
        public static int Count
        {
            get { return _lesDemandes.Count; }
        }
        public static string Listes
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

                foreach (Demande a in _lesDemandes.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Type}",
                    $"{a.Animal.Type.Nom}", 
                    $"{a.Statut}");
                }
                return Forma.Center($"Liste de toutes les Demandes [{i}/{Count}]\n\n") + retVal;
            }
        }
        public static string ListesDernieres
        {
            get
            {
                int i = 0;
                string retVal =Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

                foreach (Demande a in _lesDemandes.Values.OrderByDescending(od => od.DateCreation).Take(7))
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Type}",
                    $"{a.Animal.Type.Nom}", 
                    $"{a.Statut}");
                }
                return Forma.Center($"Liste des derniéres Demandes [{i}/{Count}]\n\n") + retVal;
            }
        }

        public static string ListesByAnimal(Animal animal)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Animal == animal))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste de toutes les Demandes de Animal: {animal.Id} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListesByContact(Contact contact)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Contact == contact))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes de Contact: {contact.Id} [{i}/{Count}]\n\n") + retVal;

        }

        public static string ListeByStatut()
        {
            return Listes;
        }
        public static string ListeByStatut(EStatutDemande eStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes de Statut: {eStatut} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListeByStatut(EStatutDemande eStatut, EStatutDemande yStatut)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut || a.Statut == yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eStatut} | {yStatut} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListeByStatut(ETypeDemande eType)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Type == eType))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eType} [{i}/{Count}]\n\n") + retVal;

        }
        public static string ListeByStatut(EStatutDemande eStatut, ETypeDemande eType)
        {

            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Type", "Type Animal", "Statut");

            foreach (Demande a in _lesDemandes.Values.Where(a => a.Statut == eStatut && a.Type == eType))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Type}",
                $"{a.Animal.Type.Nom}",
                $"{a.Statut}");
            }
            return Forma.Center($"Liste des Demandes: {eStatut} & {eType} [{i}/{Count}]\n\n") + retVal;

        }

        public static IEnumerable<Demande> Get()
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                yield return dem;
            }
        }
        public static IEnumerable<Demande> Get(Contact contact)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Contact == contact)
                {
                    yield return dem;
                }

            }
        }
        public static IEnumerable<Demande> Get(Animal animal)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Animal == animal)
                {
                    yield return dem;
                }

            }
        }
        public static IEnumerable<Demande> Get(EStatutDemande eStatut)
        {
            foreach (Demande dem in _lesDemandes.Values)
            {
                if (dem.Statut == eStatut)
                {
                    yield return dem;
                }

            }
        }

        public static Dictionary<string, Demande> GetAllDemandeByContact(Contact contact)
        {
            Dictionary<string, Demande> retval = [];

            foreach (Demande av in Get(contact))
            {
                retval.Add(av.Id, av);
            }

            return retval;
        }
        public static Dictionary<string, Demande> GetAllDemandeByAnimal(Animal animal)
        {
            Dictionary<string, Demande> retval = [];

            foreach (Demande av in Get(animal))
            {
                retval.Add(av.Id, av);
            }

            return retval;
        }

        public static Demande? Find(string id)
        {
            Demande? ado = null;
            string fid = Forma.TrimUpper(id);
            if (_lesDemandes.TryGetValue(fid, out Demande? value))
            {
                ado = value;
            }
            return ado;
        }
        public static Demande? Find(Contact contact, Animal animal, EStatutDemande eStatut,ETypeDemande eType)
        {
            Demande? ado = null;
            if(Get().Where(a => a.Statut < eStatut && a.Contact == contact && a.Animal == animal && a.Type == eType).Any())
            {
                foreach (Demande d in Get().Where(a => a.Statut < eStatut && 
                                                  a.Contact == contact && 
                                                  a.Animal == animal &&
                                                  a.Type == eType))
                {
                    ado = d;
                }
            }
            return ado;
        }

        public static void Add(Demande demande)
        {
            if (Find(demande.Id) != null)
            {
                ExceptionLauncher.New("Liste Demandes", "Cette demande existe deja");
            }
            _num++;
            _lesDemandes.Add(demande.Id, demande);
        }
        public static void Remove(Demande demande)
        {

            if (Find(demande.Id) == null)
            {
                ExceptionLauncher.New("Liste Demandes", "Cette demande est deja supprimer");
            }
            _lesDemandes.Remove(demande.Id);
        }

        public static int DB_Add(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) == null)
            {
                retval = DB_Demande.Add(demande);
            }
            return retval;
        }
        public static int DB_Update(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) != null)
            {
                retval = DB_Demande.Update(demande);
            }
            return retval;
        }
        public static int DB_Delete(Demande demande)
        {
            int retval = 0;
            if (DB_Demande.UnDemandeById(demande) != null)
            {
                retval = DB_Demande.Delete(demande);
            }
            return retval;
        }
        
    }
    public class Demande : ITable, IComparable<Demande>
    {
        private DateTime _dateOuverture;
        private DateTime? _dateFermeture;
        private Contact _contact;
        private Animal _animal;
        private ETypeDemande _type;
        private EStatutDemande _statut;
        private string _remarque;

        private string _id;

        private Demande(Contact contacts, Animal animal, ETypeDemande? type, string remarque)
        {
            int i = AllDemande.Num + 1;
            _id = Forma.SimpleId("DEM",  i);
            _dateOuverture = DateTime.Now;
            _dateFermeture = null;
            this.Contact = contacts;
            this.Animal = animal;
            this.Type = (ETypeDemande)type;

            if (animal.Statut == EStatutAnimal.EXAMINATION)
            {
                this.Statut = EStatutDemande.EN_COURS;
                this.Type = ETypeDemande.ENTREE;
            }

            if (type == ETypeDemande.SORTIE || type == ETypeDemande.ENTREE)
            {
                this.Statut = EStatutDemande.EN_COURS;
            }

            if (type == ETypeDemande.ADOPTION || type == ETypeDemande.ACCUEIL)
            {
                this.Statut = EStatutDemande.EXAMINATION;
            }

            this.Remarque = remarque;
        }

        public string Id
        {
            get { return _id; }
            set
            {
                _id = Forma.Checked_Id(value);
            }
        }
        public DateTime DateCreation
        {
            get { return _dateOuverture; }
            set { this._dateOuverture = Forma.Checked_DateCreation(value); }
        }
        public DateTime? DateFermeture
        {
            get { return this._dateFermeture; }
            set
            {
                if (value > this.DateCreation)
                {
                    ExceptionLauncher.New("Demande Date Fermeture", "Date invatide");
                }
                this._dateFermeture = value;
            }
        }
        public Contact Contact
        {
            get { return this._contact; }
            set { this._contact = value; }
        }
        public Animal Animal
        {
            get { return this._animal; }
            set
            {
                if (value.Statut == EStatutAnimal.DECEDE)
                {
                    ExceptionLauncher.New("Demande Animal", "ce animal ne peut etre mis en demande");
                }
                this._animal = value;
            }
        }
        public ETypeDemande Type
        {
            get { return this._type; }
            set { this._type = value; }
        }
        public EStatutDemande Statut
        {
            get { return this._statut; }
            set { this._statut = value; }
        }
        public string Remarque
        {
            get { return this._remarque; }
            set { this._remarque = value; }
        }
        public int CompareTo(Demande? other)
        {
            return this.Id.CompareTo(other.Id);
        }
        public override string ToString()
        {
            string? info = null;
            if (this.Statut > EStatutDemande.EN_COURS)
            {
                info = Forma.Center($"- [ {this.Statut} ] -\n\n", 100);
            }
            
            if (this.Statut == EStatutDemande.VALIDATION)
            {
                info = Forma.Center($"- [ EN ATTENT DE VALIDATION ] -\n\n", 100);
            }

            string retVal = 
                Forma.Center($"FICHE DE DEMANDE N° [ {this.Id} ]\n") +
                Forma.Center(new string('-',90)+$"\n") +

                info +

                Forma.Texta2("Date Crea.", $"{DateCreation:dd-MM-yyyy}") +

                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Type Dem.", $"{Type}") +

                Forma.Section("Animal") +

                Forma.Texta2("Animal id", $"{Animal.Id}") +
                Forma.Texta2("Nom", $"{Animal.Nom}") +
                Forma.Texta2("Type An.", $"{Animal.Type.Nom}") +

                Forma.Section("Contacte") +

                Forma.Texta2("Id Contacte", $"{Contact.Gsm}") +
                Forma.Texta2("Contact", $"{Contact.Nom} {Contact.Prenom}") +
                Forma.Texta2("Gsm", $"{Contact.Gsm}") +

                Forma.Section("Infos") +

                Forma.Texta2("Statut", $"{Statut}") +
                Forma.Texta2("Remarque", $"{Remarque}") +
                Forma.Texta2("Date Ferm.", $"{DateFermeture?.ToString("dd-MM-yyyy")}");

            if (Type == ETypeDemande.ENTREE)
            {
                Entree entree = AllEntree.Find(this);

                retVal += Forma.Section("Entree") +

                    Forma.Texta2("Id Entree", entree == null ? "--" : entree.Id) +
                    Forma.Texta2("Date crea.", entree == null ? "--" : entree.DateCreation.ToString("dd-MM-yyyy")) +
                    Forma.Texta2("Motif", entree == null ? "--" : entree.Motifs.Libele);
            }

            if (Type == ETypeDemande.SORTIE)
            {
                Sortie sortie = AllSortie.Find(this);

                retVal += Forma.Section("Sortie") +

                    Forma.Texta2("Id Sortie", sortie == null ? "--" : sortie.Id) +
                    Forma.Texta2("Date crea.", sortie == null ? "--" : sortie.DateCreation.ToString("dd-MM-yyyy")) +
                    Forma.Texta2("Motif", sortie == null ? "--" : sortie.Motifs.Libele);
            }

            if (Type == ETypeDemande.ADOPTION)
            {
                Adoption adoption = AllAdoption.Find(this);

                retVal += Forma.Section("Adoption") +

                    Forma.Texta2("Id Adoption", adoption == null ? "--" : adoption.Id) +
                    Forma.Texta2("Statut", adoption == null ? "--": adoption.Statut.ToString());
            }

            if (Type == ETypeDemande.ACCUEIL)
            {
                Accueil accueil = AllAccueil.Find(this);

                retVal += Forma.Section("Accueil") +

                    Forma.Texta2("Id Adoption", accueil == null ? "--" : accueil.Id) +
                    Forma.Texta2("Statut", accueil == null ? "--" : accueil.Statut.ToString());
            }

            return retVal;
        }

        public int Update(Contact? contacts,Animal? animal ,ETypeDemande? type, EStatutDemande? statut, string remarque)
        {
            if (this.Statut > EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("Demande Update", "Cette demande est terminéé");
            }
            int retVal = 0;
            if (contacts != null && animal != null && type != null && statut != null)
            {
                this.Type = (ETypeDemande)type;
                this.Statut = (EStatutDemande)statut;
                this.Remarque = remarque;
                this.Contact = contacts;
                this.Animal = animal;
                retVal = AllDemande.DB_Update(this);
            }
            return retVal;
        }
        public int Update(ETypeDemande? type)
        {
            int retVal = 0;
            if (type != null)
            {
                this.Type = (ETypeDemande)type;
                retVal = AllDemande.DB_Update(this);
            }
            return retVal;
        }
        public int Update(EStatutDemande? statut)
        {
            int retVal = 0;
            if (statut != null)
            {
                this.Statut = (EStatutDemande)statut;
                retVal = AllDemande.DB_Update(this);
            }
            return retVal;
        }
        public int UpdateDateFin(DateTime? dte)
        {
            this.DateFermeture = dte;
            return AllDemande.DB_Update(this);
        }

        public static Demande Creer(Contact contact, Animal animal, ETypeDemande? type, string remarque)
        {

            if (contact == null || animal == null || type == null)
            {
                ExceptionLauncher.New("Demande Creer", $"Parametre invalide {type}");
            }

            Demande retVal = new(contact, animal, type, remarque);

            return retVal;
        }
        public static int Save(Demande demande)
        {
            int ret = 0;
            if (AllDemande.Find(demande.Id) == null && AllDemande.Find(demande.Contact,demande.Animal,EStatutDemande.TERMINEE,demande.Type) == null)
            {
                if (demande.Animal.Statut == EStatutAnimal.DECEDE && demande.Type != ETypeDemande.DECES)
                {
                    ExceptionLauncher.New("Demande Save", " L animal n'est pas vivant");
                }

                AllDemande.Add(demande);
                ret = AllDemande.DB_Add(demande);
                Sync(demande);
            }
            return ret;
        }
        public static int Delete(Demande demande)
        {
            int ret = 0;
            if (AllDemande.Find(demande.Id) != null)
            {
                OnDelete(demande);

                ret = AllDemande.DB_Delete(demande);
                AllDemande.Remove(demande);
            }
            return ret;
        }
        

        private static void Sync(Demande demande)
        {
            if (demande.Type == ETypeDemande.DECES)
            {
                demande.Animal.Update(EStatutAnimal.DECEDE);
                demande.Statut = EStatutDemande.TERMINEE;
            }

            if (demande.Type == ETypeDemande.NAISSANCE)
            {
                demande.Animal.Update(EStatutAnimal.DECEDE);
                demande.Statut = EStatutDemande.TERMINEE;
            }
        }
        private static int OnDelete(Demande demande)
        {
            Entree? entree = AllEntree.Find(demande);
            if (entree != null)
            {
                Entree.Delete(entree);
            }

            Adoption adoption = AllAdoption.Find(demande);
            if (adoption != null)
            {
                Adoption.Delete(adoption);
            }

            Accueil accueil = AllAccueil.Find(demande);
            if (accueil != null)
            {
                Accueil.Delete(accueil);
            }

            Sortie sortie = AllSortie.Find(demande);
            if (sortie != null)
            {
                Sortie.Delete(sortie);
            }

            return 1;
        }

    }
}
