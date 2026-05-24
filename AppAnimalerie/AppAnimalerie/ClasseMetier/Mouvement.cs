using AppAnimalerie.AccessDB;
using System.Globalization;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllEntree
    {
        private static Dictionary<string, Entree> _lesEntres;

        static AllEntree()
        {
            _lesEntres = new Dictionary<string, Entree>();
        }
        public static int Count
        {
            get { return _lesEntres.Count; }
        }
        public static string LesEntrees
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Entrees [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Entree vc in _lesEntres.Values.OrderByDescending(a => a.DateCreation))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }
        }
        public static string LesEntreesDerniers
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Entrees [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Entree vc in _lesEntres.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }
        }

        public static IEnumerable<Entree> Get()
        {
            foreach (Entree ent in _lesEntres.Values)
            {
                yield return ent;
            }
        }
        public static IEnumerable<Entree> Get(Animal animal)
        {
            foreach (Entree en in Get().Where(a => a.Demande.Animal == animal).OrderByDescending(v => v.DateCreation))
            {
                yield return en;
            }
        }
        public static IEnumerable<Entree> Get(Contact contact)
        {
            foreach (Entree en in Get().Where(a => a.Demande.Contact == contact).OrderByDescending(v => v.DateCreation))
            {
                yield return en;
            }
        }

        public static Entree? Find(string key)
        {
            Forma.ParametreNullTesteur(key);

            Entree? entree = null;
            string F_key = Forma.TrimUpper(key);
            if (_lesEntres.ContainsKey(F_key))
            {
                entree = _lesEntres[F_key];
            }
            return entree;
        }
        public static Entree? Find(Demande demande)
        {

            Entree? entree = null;
            foreach (Entree e in _lesEntres.Values)
            {
                if (e.Demande == demande)
                {
                    entree = e;
                    break;
                }
            }

            return entree;
        }
        public static void Add(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }
            if (Find(entree.Id) != null)
            {
                ExceptionLauncher.New("AllEntree", $"Cet Id est deja dans la liste : {entree.Id} ");
            }
            _lesEntres.Add(entree.Id, entree);
        }
        public static void Remove(string id)
        {
            string formatId = id.Trim().ToUpper();
            if (Find(formatId) == null)
            {
                ExceptionLauncher.New("AllEntree", $"Cet Id n'est plus dans la liste : {formatId} ");
            }
            _lesEntres.Remove(formatId);
        }

        public static int DB_Add(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }

            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) == null)
            {
                retVal = DB_Entree.Add(entree);
            }
            return retVal;
        }
        public static int DB_Update(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }
            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) != null)
            {
                retVal = DB_Entree.Update(entree);
            }
            return retVal;
        }
        public static int DB_Delete(Entree entree)
        {
            int retVal = 0;
            if (DB_Entree.UnEntreeById(entree) != null)
            {
                retVal = DB_Entree.Delete(entree);
            }
            return retVal;
        }

    }
    public static class AllSortie
    {
        private static Dictionary<string, Sortie> _lesSorties;

        static AllSortie()
        {
            _lesSorties = new Dictionary<string, Sortie>();
        }

        public static int Count
        {
            get { return _lesSorties.Count; }
        }
        public static string LesSorties
        {

            get
            {
                int i = 0;
                string retVal = $"Liste des Sorties [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Sortie vc in _lesSorties.Values)
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation:dd-MM-yyyy}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }

        }
        public static string LesSortiesDernieres
        {

            get
            {
                int i = 0;
                string retVal = $"Liste des 10 dernières Sorties [{Count}]\n" +
                Forma.Text("N°", "Id", "Date Crea.", "Motifs", "Id Demande", "Contact", "Id Animal", "Nom Animal");
                foreach (Sortie vc in _lesSorties.Values.OrderByDescending(ob => ob.DateCreation).Take(10))
                {
                    i++;
                    retVal += Forma.Text(
                        $"{i}",
                        $"{vc.Id}",
                        $"{vc.DateCreation:dd-MM-yyyy}",
                        $"{vc.Motifs.Libele}",
                        $"{vc.Demande.Id}",
                        $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                        $"{vc.Demande.Animal.Id}",
                        $"{vc.Demande.Animal.Nom}");
                }

                return retVal;
            }

        }

        public static IEnumerable<Sortie> Get()
        {
            foreach (Sortie sort in _lesSorties.Values)
            {
                yield return sort;
            }
        }
        public static IEnumerable<Sortie> Get(Animal animal)
        {
            foreach (Sortie sort in Get().Where(a => a.Demande.Animal == animal).OrderByDescending(v => v.DateCreation))
            {
                yield return sort;
            }
        }
        public static IEnumerable<Sortie> Get(Contact contact)
        {
            foreach (Sortie sort in Get().Where(a => a.Demande.Contact == contact).OrderByDescending(v => v.DateCreation))
            {
                yield return sort;
            }
        }

        public static Sortie Find(string key)
        {
            Sortie sortie = null;
            string F_key = Forma.TrimUpper(key);
            if (_lesSorties.ContainsKey(F_key))
            {
                sortie = _lesSorties[F_key];
            }
            return sortie;
        }
        public static Sortie Find(Demande demande)
        {
            Sortie? sortie = null;
            foreach (Sortie s in _lesSorties.Values)
            {
                if (s.Demande == demande)
                {
                    sortie = s;
                    break;
                }
            }

            return sortie;
        }
        public static void Add(Sortie sortie)
        {
            if (Find(sortie.Id) != null)
            {
                ExceptionLauncher.New("AllSortie", $"Cet Id est deja dans la liste : {sortie.Id} ");
            }
            _lesSorties.Add(sortie.Id, sortie);
        }
        public static void Remove(string id)
        {
            string formatId = id.Trim().ToUpper();
            if (Find(formatId) == null)
            {
                ExceptionLauncher.New("AllSortie", $"Cet Id n'est plus dans la liste : {formatId} ");
            }
            _lesSorties.Remove(formatId);
        }

        public static int DB_Add(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) == null)
            {
                retVal = DB_Sortie.Add(sortie);
            }
            return retVal;
        }
        public static int DB_Update(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) != null)
            {
                retVal = DB_Sortie.Update(sortie);
            }
            return retVal;
        }
        public static int DB_Delete(Sortie sortie)
        {
            int retVal = 0;
            if (DB_Sortie.UnSortieById(sortie) != null)
            {
                retVal = DB_Sortie.Delete(sortie);
            }
            return retVal;
        }

    }
    public abstract class Mouvement : ITable
    {
        private DateTime _date;
        private string? _details;

        protected Mouvement(string? details)
        {
            Details = details;
            _date = DateTime.Now;
        }

        public abstract string Id
        {
            get;
            set;
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set { _date = Forma.Checked_DateCreation(value); }
        }
        public string? Details
        {
            get { return _details; }
            set { _details = value; }
        }
        public abstract Demande Demande
        {
            get;
            set;
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", $"{DateCreation}") +
                Forma.Texta2("Id Demande", $"{Demande.Id}") +
                Forma.Texta2("Statut Dem.", $"{Demande.Statut}") +
                Forma.Texta2("Id Animal", $"{Demande.Animal.Id}") +
                Forma.Texta2("Animal", $"{Demande.Animal.Nom}") +
                Forma.Texta2("Contact", $"{Demande.Contact.Nom}") +
                Forma.Texta2("Contact Gsm", $"{Demande.Contact.Gsm}") +
                Forma.Texta2("Adresse", $"{Demande.Contact.Adresse}") +
                Forma.Texta2("Details", $"{Details}");

            return retVal;
        }
    }

    public class Sortie : Mouvement, IComparable<Sortie>
    {
        private Demande _demande;
        private MotifSortie _motifs;
        private string _id;
        private Sortie(Demande demande, MotifSortie motifs, string details) : base(details)
        {
            Id = "STE-" + demande.Id;
            Motifs = motifs;
            Demande = demande;
        }

        public override string Id
        {
            get { return _id; }
            set
            {
                _id = Forma.Checked_Id(value);
            }
        }
        public override Demande Demande
        {
            get { return this._demande; }
            set
            {
                if (value.Type == ETypeDemande.ENTREE)
                {
                    ExceptionLauncher.New("Sortie Demande", "Cette demande ne peut etre affectée ici...");
                }
                this._demande = value;
            }
        }
        public MotifSortie Motifs
        {
            get { return _motifs; }

            set => _motifs = value;
        }

        public int CompareTo(Sortie sortie)
        {
            return _id.CompareTo(sortie._id);
        }
        public override string ToString()
        {
            string retVal =
                Forma.Center($"SORTIE N° [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                $"{base.ToString()}" +
                Forma.Texta2("Motif", $"{Motifs.Libele}") +
                Forma.Texta2("Motif Details", $"{Motifs.Details}");
            return retVal;
        }

        public static Sortie Creer(Demande demande, MotifSortie motifs, string details)
        {
            Forma.ParametreNullTesteur(demande);
            Forma.ParametreNullTesteur(motifs);

            if (demande.Type == ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("Creer Sortie", "Cet demande est de type entree");
            }
            Sortie retVal = new(demande, motifs, details);

            return retVal;
        }
        public static int Save(Sortie sortie)
        {
            if (sortie.Demande.Statut != EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("Creer Sortie", $"Cette demande n'est pas en cours {sortie.Demande.Id}");
            }

            int retVal = 0;
            if (AllSortie.Find(sortie.Id) == null)
            {
                AllSortie.Add(sortie);
                retVal = AllSortie.DB_Add(sortie);
                Sync(sortie);

            }
            return retVal;
        }
        public int Update(MotifSortie motifs, string details)
        {
            Forma.ParametreNullTesteur(motifs);

            int retVal = 0;

            if (AllSortie.Find(this.Id) != null)
            {
                this.Motifs = motifs;
                this.Details = details;
                retVal = AllSortie.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(Sortie sortie)
        {
            int retVal = 0;
            if (AllSortie.Find(sortie.Id) != null)
            {
                OnDelete(sortie);

                AllSortie.Remove(sortie.Id);
                retVal = AllSortie.DB_Delete(sortie);
            }
            return retVal;
        }

        public static void Sync(Sortie sortie)
        {
            if (sortie.Demande.Type == ETypeDemande.ACCUEIL)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.ACCUEIL);
            }

            if (sortie.Demande.Type == ETypeDemande.ADOPTION)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.ADOPTION);
            }

            if (sortie.Demande.Type == ETypeDemande.SORTIE)
            {
                sortie.Demande.Animal.Update(EStatutAnimal.PROPRIETAIRE);
            }

            if (sortie.Demande.Animal.Abri != null)
            {
                sortie.Demande.Animal.RemoveAbri();
            }
            ;
            sortie.Demande.Update(EStatutDemande.TERMINEE);
            sortie.Demande.Animal.RefuserAllDemandeEncours();
        }
        private static int OnDelete(Sortie sortie)
        {
            if (sortie.Demande.Animal.LastDemande == sortie.Demande)
            {
                sortie.Demande.Update(EStatutDemande.EN_COURS);
                sortie.Demande.Animal.Update(EStatutAnimal.REFUGE);
            }

            return 1;
        }

    }
    public class Entree : Mouvement, IComparable<Entree>
    {
        private MotifEntree _motifs;
        private string _id;
        private Demande _demande;

        private Entree(Demande demande, MotifEntree motifs, string details) : base(details)
        {
            Id = "ENT-" + demande.Id;
            Motifs = motifs;
            Demande = demande;
        }
        public override string Id
        {
            get { return _id; }
            set
            {
                _id = Forma.Checked_Id(value);
            }
        }
        public override Demande Demande
        {
            get { return this._demande; }
            set
            {
                if (value.Type != ETypeDemande.ENTREE)
                {
                    ExceptionLauncher.New("Entree Demande", "Cette demande ne peut etre affectée ici...");
                }
                this._demande = value;
            }
        }
        public MotifEntree Motifs
        {
            get { return _motifs; }

            set => _motifs = value;
        }

        public static Entree Creer(Demande demande, MotifEntree motifs, string details)
        {
            Forma.ParametreNullTesteur(demande);
            Forma.ParametreNullTesteur(motifs);

            Entree? retVal = null;
            if (demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("AllEntree", $"Ceci n est pas un entree : {demande.Type} ");
            }

            return new Entree(demande, motifs, details);

        }
        public int Update(MotifEntree motifs, string details)
        {
            Forma.ParametreNullTesteur(motifs);

            if (this.Demande.Statut > EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("ENTREE", "La demande est n'est pas en cours");
            }

            int retVal = 0;
            if (AllEntree.Find(this.Id) != null)
            {
                this.Motifs = motifs;
                this.Details = details;
                retVal = AllEntree.DB_Update(this);
            }
            return retVal;
        }
        public static int Save(Entree entree)
        {
            if (entree.Demande.Type != ETypeDemande.ENTREE)
            {
                ExceptionLauncher.New("Entree Save", $"Ceci n est pas un entree : {entree.Demande.Type} ");
            }

            if (entree.Demande.Statut != EStatutDemande.EN_COURS)
            {
                ExceptionLauncher.New("Entree Save", "Cette demande n 'est pas valide");
            }

            int retVal = 0;
            if (AllEntree.Find(entree.Id) == null && entree.Demande.Statut == EStatutDemande.EN_COURS)
            {
                OnSave(entree);
                AllEntree.Add(entree);
                retVal = AllEntree.DB_Add(entree);
            }
            return retVal;
        }
        public static int Delete(Entree entree)
        {
            int retVal = 0;
            if (AllEntree.Find(entree.Id) != null)
            {
                OnDelete(entree);

                AllEntree.Remove(entree.Id);
                retVal = AllEntree.DB_Delete(entree);
            }
            return retVal;
        }

        public int CompareTo(Entree entree)
        {
            return _id.CompareTo(entree._id);
        }
        public override string ToString()
        {
            string retVal =
                Forma.Center($"ENTREE N° [ {Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                $"{base.ToString()}" +
                Forma.Texta2("Motif", $"{Motifs.Libele}") +
                Forma.Texta2("Motif Details", $"{Motifs.Details}");
            return retVal;
        }


        private static void OnSave(Entree entree)
        {
            if (entree.Demande.Animal.Statut == EStatutAnimal.ACCUEIL)
            {
                Accueil? accueil = entree.Demande.Animal.LastAccueil;
                accueil.Update(accueil.DateDebut, DateTime.Now);
            }

            if (entree.Demande.Animal.Statut == EStatutAnimal.ADOPTION)
            {
                Adoption? adoption = entree.Demande.Animal.LastAdoption;
                adoption.Update(adoption.DateD, DateTime.Now);
            }

            entree.Demande.Animal.Update(EStatutAnimal.REFUGE);
            entree.Demande.Update(EStatutDemande.TERMINEE);
        }
        private static int OnDelete(Entree entree)
        {
            if (entree.Demande.Animal.LastDemande == entree.Demande)
            {
                entree.Demande.Update(EStatutDemande.EN_COURS);
                entree.Demande.Animal.Update(EStatutAnimal.REFUGE);
            }

            return 1;
        }
    }
}
