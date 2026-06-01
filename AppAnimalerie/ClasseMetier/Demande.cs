
using AppAnimalerie.ClasseService;


namespace AppAnimalerie.ClasseMetier
{

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
