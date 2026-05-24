using AppAnimalerie.AccessDB;

namespace AppAnimalerie.ClasseMetier
{

    public static class AllAnimal
    {
        private static readonly Dictionary<string, Animal> _lesAnimaux;
        private static int _numAnimaux;

        static AllAnimal()
        {
            _lesAnimaux = new Dictionary<string, Animal>();
            _numAnimaux = 0;
        }

        public static int NumAnimaux
        {
            get
            {
                if (Count > 0)
                {
                    _numAnimaux = Forma.LastNumero(_lesAnimaux);
                }
                return _numAnimaux;
            }
        }
        public static int Count
        {
            get
            {
                return _lesAnimaux.Count;
            }
        }
        public static string LesAnimaux
        {
            get
            {
                int i = 0;
                string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
                foreach (Animal a in _lesAnimaux.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Type.Nom}",
                    $"{a.Statut}");
                }
                return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
            }
        }
        public static string LesAnimauxDerniers()
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }

            return $"Liste des Top 10 derniers animaux encore au refuge [{i}/{Count}]\n\n" + retVal;
        }

        public static string ListeByStatut()
        {
            return LesAnimaux;
        }
        public static string ListeByStatut(EStatutAnimal eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut == eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }
        public static string ListeByStatut(EStatutAnimal eStatut, EStatutAnimal yStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut == eStatut || a.Statut == yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }
        public static string ListeByStatutNot(EStatutAnimal eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut != eStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count} ]\n\n\n" + retVal;
        }
        public static string ListeByStatutNot(EStatutAnimal eStatut, EStatutAnimal yStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Nom", "Type", "Statut");
            foreach (Animal a in _lesAnimaux.Values.Where(a => a.Statut != eStatut && a.Statut != yStatut))
            {
                i++;
                retVal += Forma.Text(
                $"{i}",
                $"{a.Id}",
                $"{a.DateCreation:dd-MM-yyyy}",
                $"{a.Nom}",
                $"{a.Type.Nom}",
                $"{a.Statut}");
            }
            return $"Liste des Animaux [{i}/{Count}]\n\n" + retVal;
        }

        public static IEnumerable<Animal> Get()
        {
            foreach (Animal animal in _lesAnimaux.Values)
            {
                yield return animal;
            }
        }
        public static IEnumerable<Animal> Get(EStatutAnimal eStatut)
        {
            foreach (Animal animal in _lesAnimaux.Values)
            {
                if (animal.Statut.Equals(eStatut))
                {
                    yield return animal;
                }
            }
        }
        public static IEnumerable<Animal> Get(TypeAnimal type)
        {
            foreach (Animal animal in _lesAnimaux.Values.Where(a => a.Type == type))
            {
                yield return animal;
            }
        }
        public static Animal Rechercher(string idAmi)
        {
            Animal? retVal = null;
            string fid = Forma.TrimUpper(idAmi);
            if (_lesAnimaux.TryGetValue(fid, out Animal? value))
            {
                retVal = value;
            }
            return retVal;
        }

        public static void Add(Animal ami)
        {
            if (Rechercher(ami.Id) != null)
            {
                throw new Exception($"[Groupe Animaux] Cet identifiant existe deja :{ami.Id}");
            }
            _numAnimaux++;
            _lesAnimaux.Add(ami.Id, ami);
        }
        public static void Supprimer(string idami)
        {
            if (Rechercher(idami) == null)
            {
                throw new Exception($"[ Groupe Animaux] Cet Animal est deja supprimé :{idami}");
            }
            _lesAnimaux.Remove(idami);
        }

        public static int DB_Add(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) == null)
            {
                retVal = DB_Animal.Add(ami);
            }
            return retVal;
        }
        public static int DB_Update(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.Update(ami);
            }
            return retVal;
        }
        public static int DB_UpdateStatut(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.UpdateStatut(ami);
            }
            return retVal;
        }
        public static int DB_UpdateAbri(Animal ami)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(ami.Id) != null)
            {
                retVal = DB_Animal.UpdateAbri(ami);
            }
            return retVal;
        }
        public static int DB_Delete(Animal animal)
        {
            int retVal = 0;
            if (DB_Animal.UnAnimalById(animal.Id) != null)
            {
                retVal = DB_Animal.Delete(animal.Id);
            }
            return retVal;
        }

    }
    public static class IdMaker_Animal
    {
        public static string NouveauID
        {
            get
            {
                DateTime dte = DateTime.Today;
                int n = AllAnimal.NumAnimaux + 1;
                string retVal = $"{dte.ToString("yyyyMMdd")}{n.ToString("D5")}";
                return retVal;
            }
        }
    }
    public class Animal : ITable, IComparable<Animal>
    {
        private string _idAnimal;
        private DateTime _date;
        private string _nom;
        private TypeAnimal _type;
        private DateTime _dateNaissance;
        private DateTime? _dateDeces;
        private ESexe _sexe;
        private Couleur _couleur;
        private bool _sterile;
        private DateTime? _dateSterile;
        private string _description;
        private string _particularite;
        private EStatutAnimal _statue;
        private Abri? _abri;

        private readonly Dictionary<string, Couleur> _couleurs;

        private Animal(string nom, TypeAnimal type, DateTime dnaiss, ESexe sexe, Couleur couleur, bool steril, DateTime? dsteril, string descr, string particularite)
        {
            Id = Forma.IdBuilder_Animal();
            DateCreation = DateTime.Now;
            Nom = nom;
            Type = type;
            DateNaissance = dnaiss;
            Sexe = sexe;
            Couleur = couleur;
            _dateDeces = null;
            Sterile = steril;
            DateSterilisation = dsteril;
            Description = descr;
            Particularite = particularite;
            this.Abri = null;

            Statut = EStatutAnimal.EXAMINATION;

            this._couleurs = [];

        }

        public string Id
        {
            get { return _idAnimal; }
            set { _idAnimal = value; }
        }
        public EStatutAnimal Statut
        {
            get
            {
                return _statue;
            }
            set
            {
                _statue = value;
            }
        }
        public Abri? Abri
        {
            get
            {
                return this._abri;
            }
            set
            {
                this._abri = value;
            }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public string Nom
        {
            get { return _nom; }
            set
            {
                if (value.Length < 2)
                {
                    throw new Exception($"[Animal] Le nom n'est pas valide : {value}");
                }
                _nom = value.Trim().ToUpper();
            }
        }
        public TypeAnimal Type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }
        public DateTime DateNaissance
        {
            get { return _dateNaissance; }
            set
            {
                if (value > DateTime.Now)
                {
                    ExceptionLauncher.New("Animal", "Date de naissance... invalide");
                }
                _dateNaissance = value;
            }
        }
        public DateTime? DateDeces
        {
            get
            {
                return this._dateDeces;
            }
            set
            {
                if (value != null && value < DateNaissance)
                {
                    ExceptionLauncher.New("Animal", $"La date de deces n'est pas valide : {value}");
                }
                _dateDeces = value;
            }
        }
        public ESexe Sexe
        {
            get { return _sexe; }
            set
            {
                if (!Enum.IsDefined(typeof(ESexe), value))
                {
                    throw new Exception($"[Animal] Sexe invalide : {value}");
                }
                _sexe = value;
            }
        }
        public Couleur Couleur
        {
            get { return _couleur; }
            set
            {
                _couleur = value;
            }
        }
        public bool Sterile
        {
            get { return _sterile; }
            set { _sterile = value; }
        }
        public DateTime? DateSterilisation
        {
            get { return _dateSterile; }
            set
            {
                if (value != null)
                {
                    if (value < DateNaissance)
                    {
                        ExceptionLauncher.New("Animal", $"La date de sterilisation n'est pas valide : {value}");
                    }

                }

                _dateSterile = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public string Particularite
        {
            get
            {
                return _particularite;
            }
            set
            {
                _particularite = value;
            }
        }

        public string Couleurs
        {
            get
            {
                string? retVal = null;
                foreach (AnimalCouleur vc in GetCouleur())
                {
                    retVal += $"{vc.Couleur.Nom} | ";
                }
                retVal ??= Forma.Center("Pas de couleur secondaire\n");
                return retVal;
            }
        }
        public string Vaccinations
        {
            get
            {
                int i = 0;
                string retVal = Forma.Padding(new string('*', 90)) + "\n" +
                   Forma.Padding(Forma.Text("N°", "Id", "Vaccin", "Vacciné le", "Remarque"));
                if (!GetVaccination().Any())
                {
                    retVal += "\n" + Forma.Center($"Cet animal ne possede pas encore de vaccin.\n");
                }
                foreach (Vaccination vc in GetVaccination())
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.Vaccin.Nom}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             vc.Remaque ?? "--"));
                }

                return Forma.Center($"Liste des Vaccinantions [{i}]\n") + retVal;
            }
        }
        public string Compatibilites
        {
            get
            {
                string retVal = Forma.Center($"Liste des Compatibilités [{AnimalCompatibilitéService.FindAllByAnimal(this).Count}/{AnimalCompatibilitéService.Count}]\n") +
                                Forma.Padding(new string('*', 90)) + "\n" +
                    Forma.Padding(Forma.Text("id", "Compatibilité", "Valeur", "Id"));

                foreach (Compatibilite vc in AllCompatibilite.Get())
                {
                    AnimalCompatibilité? com = this.Compatible(vc);
                    retVal += Forma.Padding(Forma.Text(
                             com == null ? "--" : (com.Compatible == true ? "O" : "X"),
                             $"{vc.Nom}",
                             com == null ? "A Verifier" : (com.Compatible == true ? "Oui" : "Non"),
                             com == null ? "--" : com.Id));
                }

                return retVal;
            }
        }
        public string Adoptions
        {
            get
            {
                int i = 0;
                string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                   Forma.Padding(Forma.Text("N°", "Id", "Date", "Famille", "Statut"));

                if (!GetAdoption().Any())
                {
                    retVal += "\n" + Forma.Center($"---- [ None ] ----\n");
                }
                foreach (Adoption vc in GetAdoption().OrderByDescending(dm => dm.DateCreation))
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             $"{vc.Demande.Statut}"));
                }

                return Forma.Center($"Liste des Adoptions de {this.Nom} [{i}]\n") + retVal;
            }

        }
        public string Accueils
        {
            get
            {
                int i = 0;
                string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                   Forma.Padding(Forma.Text("N°", "Id", "Date", "Famille", "Statut"));

                if (!GetAccueil().Any())
                {
                    retVal += "\n" + Forma.Center($"---- [ None ] ----\n");
                }
                foreach (Accueil vc in GetAccueil().OrderByDescending(dm => dm.DateCreation))
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             $"{vc.Demande.Statut}"));
                }

                return Forma.Center($"Liste des accueils de {this.Nom} [{i}]\n") + retVal;
            }

        }
        public string Entrees
        {
            get
            {
                int i = 0;
                string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                   Forma.Padding(Forma.Text("N°", "Id", "Date", "Famille", "statut Demande"));

                if (!GetEntree().Any())
                {
                    retVal += "\n" + Forma.Center($"---- [ None ] ----\n");
                }
                foreach (Entree vc in GetEntree().OrderByDescending(dm => dm.DateCreation))
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             $"{vc.Demande.Statut}"));
                }

                return Forma.Center($"Liste des entrees de {this.Nom} [{i}]\n") + retVal;
            }

        }
        public string Sorties
        {
            get
            {
                int i = 0;
                string retVal = Forma.Padding(new string('-', 90)) + "\n" +
                   Forma.Padding(Forma.Text("N°", "Id", "Date", "Famille", "statut Demande"));

                if (!GetSortie().Any())
                {
                    retVal += "\n" + Forma.Center($"---- [ None ] ----\n");
                }
                foreach (Sortie vc in GetSortie().OrderByDescending(dm => dm.DateCreation))
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text(
                             $"{i}",
                             $"{vc.Id}",
                             $"{vc.DateCreation:dd-MM-yyyy}",
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             $"{vc.Demande.Statut}"));
                }

                return Forma.Center($"Liste des sorties de {this.Nom} [{i}]\n") + retVal;
            }

        }

        public Demande? LastDemande
        {
            get
            {
                Demande? ret = null;
                foreach (Demande d in GetDemandes().OrderByDescending(dm => dm.DateCreation).Take(1))
                {
                    ret = d;
                }
                return ret;
            }

        }
        public Accueil? LastAccueil
        {
            get
            {
                Accueil? ret = null;
                foreach (Accueil d in AllAccueil.Get(this).OrderByDescending(dm => dm.DateCreation).Take(1))
                {
                    ret = d;
                }
                return ret;
            }

        }
        public Adoption? LastAdoption
        {
            get
            {
                Adoption? ret = null;
                foreach (Adoption d in AllAdoption.Get(this).OrderByDescending(dm => dm.DateCreation).Take(1))
                {
                    ret = d;
                }
                return ret;
            }

        }
        public Sortie? LastSortie
        {
            get
            {
                Sortie? ret = null;
                foreach (Sortie d in GetSortie().OrderByDescending(dm => dm.DateCreation).Take(1))
                {
                    ret = d;
                }
                return ret;
            }

        }
        public Entree? LastEntree
        {
            get
            {
                Entree? ret = null;
                foreach (Entree e in GetEntree().OrderByDescending(dm => dm.DateCreation).Take(1))
                {
                    ret = e;
                }
                return ret;
            }

        }

        public override string ToString()
        {
            string ster = "Non";
            string dster = "--";
            if (Sterile)
            {
                ster = "Oui";
                dster = DateSterilisation.ToString();
            }

            string? info1 = null;
            if (this.Statut != EStatutAnimal.REFUGE)
            {
                info1 = Forma.Center("- [ HORS REFUGE ] -\n\n", 100);
            }

            if (this.Statut == EStatutAnimal.EXAMINATION)
            {
                info1 = Forma.Center("- [ ENTREE À FAIRE ] -\n\n", 100);
            }

            string retVal =
                Forma.Center($"Fiche du pensionnaire n°[ {this.Id} ]\n", 102) +
                Forma.Center(new string('-', 90) + $"\n") +

                info1 +

                Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Type", $"{Type.Nom}") +
                Forma.Texta2("Nom", $"{Nom}") +
                Forma.Texta2("Sexe", $"{Sexe}") +

                Forma.Texta2("Couleur Prim.", $"{Couleur.Nom}") +
                Forma.Texta2("Couleur(s) Sec.", $"{Couleurs.Trim()}") +

                Forma.Texta2("Steril", $"{ster}") +
                Forma.Texta2("Steril", $"{dster}") +

                Forma.Texta2("Description", $"{Description}") +
                Forma.Texta2("Etat", $"{Statut}") +

                Forma.Texta2("Abri", Abri != null ? Abri.Libelle : "--");

            retVal += "\n\n" + this.Vaccinations;
            retVal += "\n\n" + this.Compatibilites;

            return retVal;
        }

        public IEnumerable<AnimalCompatibilité> GetCompatibilite()
        {
            foreach (AnimalCompatibilité comp in AnimalCompatibilitéService.FindAllByAnimal(this).Values)
            {
                yield return comp;
            }
        }
        public IEnumerable<Vaccination> GetVaccination()
        {
            foreach (Vaccination vaccination in Vaccination.ByAnimal(this).Values)
            {
                yield return vaccination;
            }
        }
        public IEnumerable<AnimalCouleur> GetCouleur()
        {
            foreach (AnimalCouleur coloration in AllAnimalCouleur.FindAllByAnimal(this).Values)
            {
                yield return coloration;
            }
        }
        public IEnumerable<Demande> GetDemandes()
        {
            foreach (Demande dm in AllDemande.GetAllDemandeByAnimal(this).Values)
            {
                yield return dm;
            }
        }
        public IEnumerable<Adoption> GetAdoption()
        {
            foreach (Adoption dm in AllAdoption.Get(this))
            {
                yield return dm;
            }
        }
        public IEnumerable<Accueil> GetAccueil()
        {
            foreach (Accueil dm in AllAccueil.Get(this))
            {
                yield return dm;
            }
        }
        public IEnumerable<Sortie> GetSortie()
        {
            foreach (Sortie s in AllSortie.Get(this))
            {
                yield return s;
            }

        }
        public IEnumerable<Entree> GetEntree()
        {
            foreach (Entree e in AllEntree.Get(this))
            {
                yield return e;
            }

        }

        public List<Demande> DemandesEnCours()
        {
            List<Demande> ret = [];

            foreach (Demande d in GetDemandes().Where(a => a.Statut >= EStatutDemande.EN_COURS))
            {
                ret.Add(d);
            }
            return ret;
        }
        public string RefuserAllDemandeEncours()
        {
            int i = 0;
            int y = 0;
            int z = 0;
            string ret = "";

            var collection1 = AllAccueil.GetAllAccueil(this, EStatutValidation.EN_COURS);
            if (collection1.Count > 0)
            {
                foreach (Accueil a in collection1.Values)
                {
                    i++;
                    ret += $"{i} --[ REFUSER ]-- Accueil n° {a.Id}\n";
                    a.Refuser($"Refus: une autre voie à été privilégié [ Animal n° {this.Id} ]");
                }
            }

            var collection2 = AllAdoption.GetAllAdoption(this, EStatutValidation.EN_COURS);
            if (collection2.Count > 0)
            {
                foreach (Adoption a in collection2.Values)
                {
                    y++;
                    ret += $"{y} --[ REFUSER ]-- Adoption n° {a.Id}\n";
                    a.Refuser($"Refus: une autre voie à été privilégié [ Animal n° {this.Id} ]");
                }
            }

            var collection3 = AllDemande.GetAllDemandeByAnimal(this);
            if (collection3.Count > 0)
            {
                foreach (Demande a in collection3.Values.Where(a => a.Statut < EStatutDemande.TERMINEE))
                {
                    z++;
                    ret += $"{z} --[ STOPPER ]-- Demande n° {a.Id}\n";
                    a.Update(EStatutDemande.TERMINEE);
                }
            }
            return ret;
        }
        public int CompareTo(Animal animal)
        {
            return Id.CompareTo(animal.Id);
        }

        public static Animal Creer(string nom, string type, DateTime? dnaiss, string sexe, Couleur couleur, bool? steril, DateTime? dsteril, string descr, string particularite)
        {
            TypeAnimal tp = AllTypeAnimal.FindTypeByNom(type.ToUpper().Trim());
            if (tp == null)
            {
                throw new Exception($"[Animal] Ce type n'existe pas : {type}");
            }

            ESexe s = ESexe.M;
            if (Enum.TryParse(typeof(ESexe), sexe, true, out object val))
            {
                s = (ESexe)val;
            }

            Animal retVal = new(nom, tp, (DateTime)dnaiss, s, couleur, (bool)steril, dsteril, descr, particularite);

            return retVal;
        }
        public int Save()
        {
            int ret = 0;
            if (AllAnimal.Rechercher(this.Id) == null)
            {
                AllAnimal.Add(this);
                ret = AllAnimal.DB_Add(this);
            }

            return ret;
        }
        public string Update(TypeAnimal type, string? nom, ESexe? sexe, DateTime? datnais, bool? steril,string? desc, string? partic)
        {
            string retVal = $"Modification(s) effectuée(s) sur --{Nom}--\n";

            if (type != null)
            {
                Type = type;
                retVal += $"GetUnType modifié: {Type.Nom}\n";
            }

            if (!string.IsNullOrEmpty(nom))
            {
                Nom = nom;
                retVal += $"Nom modifié: {Nom}\n";
            }

            if (sexe != null)
            {
                Sexe = (ESexe)sexe;
                retVal += $"Sexe modifié: {Sexe}\n";
            }

            if (datnais != null)
            {
                DateNaissance = (DateTime)datnais;
                retVal += $"Date de naissance modifié: {DateNaissance}\n";
            }

            if (steril != null && Sterile != steril)
            {
                Sterile = (bool)steril;
                retVal += $"Sterilité modifié: {Sterile}\n";
            }

            if (desc != "--")
            {
                Description = desc;
                retVal += $"Description modifié: {desc}\n";
            }

            if (partic != "--")
            {
                Particularite = partic;
                retVal += $"Particularite modifié: {desc}\n";
            }

            AllAnimal.DB_Update(this);

            return retVal;
        }
        public int Update(EStatutAnimal statut)
        {
            int retVal = 0;
            if (this.Statut != statut)
            {
                if (statut != EStatutAnimal.REFUGE && this.Abri != null)
                {
                    this.Abri.Update(EStatutAbri.DISPONIBLE);
                    this.Abri = null;
                }

                this.Statut = statut;
                retVal = AllAnimal.DB_UpdateStatut(this);

            }
            return retVal;
        }
        public static int Delete(Animal ami)
        {
            int retVal = 0;

            if (AllAnimal.Rechercher(ami.Id) != null)
            {
                OnDelete(ami);

                AllAnimal.Supprimer(ami.Id);
                retVal = AllAnimal.DB_Delete(ami);
            }
            return retVal;
        }

        private static int OnDelete(Animal animal)
        {
            if (AllAnimalCouleur.FindAllByAnimal(animal).Count > 0)
            {
                foreach (AnimalCouleur ac in animal.GetCouleur())
                {
                    ac.Delete();
                }
            }

            if (AllVaccination.FindAllBy(animal).Count > 0)
            {
                foreach (Vaccination ac in animal.GetVaccination())
                {
                    Vaccination.Delete(ac);
                }
            }

            if (AnimalCompatibilitéService.FindAllByAnimal(animal).Count > 0)
            {
                foreach (AnimalCompatibilité ac in animal.GetCompatibilite())
                {
                    AnimalCompatibilité.Delete(ac);
                }
            }

            if (AllDemande.GetAllDemandeByAnimal(animal).Count > 0)
            {
                foreach (Demande d in animal.GetDemandes())
                {
                    Demande.Delete(d);
                }
            }
            return 1;
        }

        public void AddCouleur(Couleur couleur)
        {
            Forma.ParametreNullTesteur(couleur);
            if (couleur == this.Couleur)
            {
                ExceptionLauncher.New("Animal Add Couleur", "Cette couleur est deja la couleur principale de l'animal");
            }

            AnimalCouleur coloration = AnimalCouleur.Creer(this, couleur);
            if (AnimalCouleur.Save(coloration) != 0)
            {
                this._couleurs.Add(coloration.Id, couleur);
            }
        }
        private bool HaveCouleur(Couleur couleur)
        {
            Forma.ParametreNullTesteur(couleur);
            bool retval = false;

            AnimalCouleur? coloration = AllAnimalCouleur.Find(this.Id + couleur.Id);
            if (coloration != null)
            {
                retval = true;
            }
            return retval;
        }
        public void RemoveCouleur(Couleur couleur)
        {
            Forma.ParametreNullTesteur(couleur);

            string id = this.Id + couleur.Id;

            if (!HaveCouleur(couleur))
            {
                ExceptionLauncher.New("Animal Delete Couleur", this.Nom + $" n'a pas cette couleur {couleur.Nom}");
            }
            AllAnimalCouleur.Delete(id);
        }

        public int AddAbri(Abri abri)
        {
            Forma.ParametreNullTesteur(abri);

            int retVal = 0;
            if (this.Statut == EStatutAnimal.REFUGE && abri.Statut == EStatutAbri.DISPONIBLE)
            {
                this.Abri = abri;
                retVal = AllAnimal.DB_UpdateAbri(this);
            }
            return retVal;
        }
        public int RemoveAbri()
        {
            this.Abri = null;
            return AllAnimal.DB_UpdateAbri(this);
        }

        public void AddVaccin(Vaccin vaccin, string? remarque)
        {
            Forma.ParametreNullTesteur(vaccin);

            if (this.Statut != EStatutAnimal.REFUGE)
            {
                ExceptionLauncher.New("Animal AddVaccin", "L'animal n'est pas au refuge");
            }
            Vaccination.Creer(this, vaccin, remarque);
        }
        public void RemoveVaccin(Vaccin vaccin)
        {
            Forma.ParametreNullTesteur(vaccin);

            Vaccination? vaccination = AllVaccination.Find(this.Id + vaccin.Id);
            if (vaccination == null)
            {
                ExceptionLauncher.New("Animal", this.Nom + "n'a pas cette vaccination");
            }
            AllVaccination.Remove(vaccination.Id);
        }

        public void AddCompatibilite(Compatibilite comp, bool valeur, string? remarque)
        {
            if (this.Statut != EStatutAnimal.REFUGE)
            {
                ExceptionLauncher.New("Animal AddCompatibilite", "L'animal n'est pas au refuge");
            }
            AnimalCompatibilité.Creer(this, comp, valeur, remarque);
        }
        public void RemoveCompatibilite(Compatibilite comp)
        {
            AnimalCompatibilité? compatibilité = AnimalCompatibilitéService.Find(this.Id + comp.Id);
            if (compatibilité == null)
            {
                ExceptionLauncher.New("Animal", this.Nom + "n'a pas cette compatibilite");
            }
            AllVaccination.Remove(compatibilité.Id);
        }
        public AnimalCompatibilité Compatible(Compatibilite comp)
        {
            AnimalCompatibilité? retval = null;
            foreach (AnimalCompatibilité c in GetCompatibilite())
            {
                if (comp.Id == c.Compatibilite.Id)
                {
                    retval = c;
                }
            }

            return retval;
        }

    }

}
