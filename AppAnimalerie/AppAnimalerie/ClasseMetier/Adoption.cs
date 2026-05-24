using AppAnimalerie.AccessDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllAdoption
    {
        private static readonly Dictionary<string, Adoption> _lesAdoptions;

        static AllAdoption()
        {
            _lesAdoptions = new Dictionary<string, Adoption>();
        }

        public static int Count
        {
            get { return _lesAdoptions.Count; }
        }
        public static string Listes
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Adoptions [{Count}]\n" +
                    string.Format($"{"{0,-4} {1,-19} {2,-11} {3,-11} {4,-16} {5,-20} {6,-10} {7,-10}\n"}",
                                     "N°", "Id", "Date Crea.","Statut", "Id Demande", "Contact","Date Fin","Raison Refus");
                foreach (Adoption vc in _lesAdoptions.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-11} {3,-11} {4,-16} {5,-20} {6,-11} {7,-11}\n"}",
                             $"{i}", 
                             $"{vc.Id}", 
                             $"{vc.DateCreation.ToString("dd-MM-yyyy")}",
                             $"{vc.Statut}",
                             $"{vc.Demande.Id}", 
                             $"{vc.Demande.Contact.Nom} {vc.Demande.Contact.Prenom}",
                             vc.DateF == null? "--" :vc.DateF?.ToString("dd-MM-yyyy"),
                             vc.DateF == null ? "--" : vc.RaisonRefus);
                }

                return retVal;
            }
        }

        public static IEnumerable<Adoption> Get()
        {
            foreach (Adoption dem in _lesAdoptions.Values)
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(Contact contact)
        {
            foreach (Adoption dem in _lesAdoptions.Values)
            {
                if (dem.Demande.Contact == contact)
                {
                    yield return dem;
                }
                
            }
        }
        public static IEnumerable<Adoption> Get(Animal animal)
        {
            foreach (Adoption dem in Get().Where(a => a.Demande.Animal == animal))
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(EStatutValidation validation)
        {
            foreach (Adoption dem in Get().Where(a => a.Statut == validation))
            {
                yield return dem;
            }
        }
        public static IEnumerable<Adoption> Get(Contact contact, EStatutValidation eStatut)
        {
            foreach (Adoption dema in Get().Where(a => a.Demande.Contact == contact && a.Statut == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }
        public static IEnumerable<Adoption> Get(Animal animal, EStatutValidation eStatut)
        {
            foreach (Adoption dema in Get().Where(a => a.Demande.Animal == animal && a.Statut == eStatut)
                .OrderByDescending(a => a.DateCreation))
            {
                if (dema.Statut == eStatut)
                {
                    yield return dema;
                }

            }
        }

        public static Dictionary<string, Adoption> GetAllAdoption(EStatutValidation eStatut)
        {
            Dictionary<string, Adoption> retVal = [];

            foreach (Adoption dm in Get(eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }
        public static Dictionary<string, Adoption> GetAllAdoption(Animal animal, EStatutValidation eStatut)
        {
            Dictionary<string, Adoption> retVal = [];

            foreach (Adoption dm in Get(animal, eStatut))
            {
                retVal.Add(dm.Id, dm);
            }

            return retVal;
        }

        public static Adoption Find(string id)
        {
            Adoption ado = null;
            string fid = Forma.TrimUpper(id);
            if (_lesAdoptions.ContainsKey(fid))
            {
                ado = _lesAdoptions[fid];
            }
            return ado;
        }
        public static Adoption Find(Demande demande)
        {
            Adoption ado = null;
            foreach (Adoption a in _lesAdoptions.Values)
            {
                if (a.Demande == demande)
                {
                    ado = a;
                    break;
                }
            }
            
            return ado;
        }
        public static void Add(Adoption ado)
        {
            if (Find(ado.Id) != null)
            {
                ExceptionLauncher.New("Liste Adoptions", "Cette adoption existe deja");
            }
            _lesAdoptions.Add(ado.Id, ado);
        }
        public static void Remove(string id)
        {
            string fid = Forma.TrimUpper(id);
            if (Find(fid) == null)
            {
                ExceptionLauncher.New("Liste Adoptions", "Cette adoption est deja supprimer");
            }
            _lesAdoptions.Remove(fid);
        }

        public static int DB_Add(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) == null)
            {
                ret = DB_Adoption.Add(ado);
            }
            return ret;
        }
        public static int DB_Update(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) != null)
            {
                ret = DB_Adoption.Update(ado);
            }
            return ret;
        }
        public static int DB_Delete(Adoption ado)
        {
            int ret = 0;
            if (DB_Adoption.UnAdoptionById(ado) != null)
            {
                ret = DB_Adoption.Delete(ado);
            }
            return ret;
        }

    }
    public class Adoption :ITable
    {
        private string _id;
        private Demande _demande;
        private EStatutValidation _statut;
        private DateTime _date;
        private DateTime? _dateD;
        private DateTime? _dateF;
        private string? _raisonRefus;
        private string _infos;

        private Adoption(Demande demande,string info) 
        {
            this._id = "ADO-" + demande.Id;
            this._date = DateTime.Now;
            this.Demande = demande;
            this.Statut = EStatutValidation.EN_COURS;
            this.DateD = null;
            this.DateF = null;
            this._raisonRefus = null;
            this.Info = info;

        }
        public string Id 
        { 
            get { return this._id; }
            set { this._id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation 
        { 
            get { return this._date; }
            set { this._date = Forma.Checked_DateCreation(value); }
        }
        public Demande Demande 
        { 
            get { return _demande; } 
            set 
            {
                if (value.Type != ETypeDemande.ADOPTION )
                {
                    ExceptionLauncher.New("Adoption", $"Demande invalide : Statut {value.Statut} - GetUnType {value.Type}");
                }
                _demande = value; 
            }
        }
        public EStatutValidation Statut
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
        public DateTime? DateD 
        {
            get { return _dateD; }
            set { _dateD = value; }
        }
        public DateTime? DateF 
        {
            get { return _dateF; }
            set { _dateF = value; }
        }
        public string Info 
        {
            get { return _infos; }
            set { _infos = value; }
        }
        public string? RaisonRefus
        {
            get { return _raisonRefus; }
            set { _raisonRefus = value; }
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
                Forma.Center($"FICHE D'ADOPTION N° [ {this.Id} ]\n")+
                Forma.Center(new string('-',90) + $"\n") +

                info +

                Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", Id) +
                Forma.Texta2("ID Demande", this.Demande.Id) + "\n" +

                Forma.Texta2("Contact", $"{this.Demande.Contact.Nom} {this.Demande.Contact.Prenom}") +
                Forma.Texta2("Gsm", this.Demande.Contact.Gsm) + "\n" +

                Forma.Texta2("Id Animal", $"{this.Demande.Animal.Id}") +
                Forma.Texta2("Nom", this.Demande.Animal.Nom) + "\n" +

                Forma.Texta2("Date Debut", DateD == null ? "--" : DateD?.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Date Fin", DateF == null ? "--" : DateF?.ToString("dd-MM-yyyy")) + "\n" +

                Forma.Texta2("Statut", $"{this.Statut}") +
                Forma.Texta2("Raison Refus", RaisonRefus ?? "--") +
                Forma.Texta2("Infos",this.Info);

            return retVal;
        }

        public int Update(Demande demande, string info)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }

            int ret = 0;
            if (AllAdoption.Find(this.Id) != null)
            {
                this.Demande = demande;
                this.Info=info;
                ret = AllAdoption.DB_Update(this);
            }
            return ret;
        }
        public int Update (EStatutValidation statut)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }
            int ret = 0;
            if (AllAdoption.Find(this.Id) != null)
            {
                this.Statut = statut;
                this.DateD = DateTime.Now;
                ret = AllAdoption.DB_Update(this);
                Sync(this);
            }
            return ret;
        }
        public int Update(DateTime? D, DateTime? F)
        {
            int ret = 0;
            if (AllAdoption.Find(this.Id) != null)
            {
                this.DateD = D;
                this.DateF = F;
                ret = AllAdoption.DB_Update(this);
            }
            return ret;
        }
        public int Update(EStatutValidation statut,string refus)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Update", "La Sortie est deja creer");
            }
            int ret = 0;
            if (AllAdoption.Find(this.Id) != null)
            {
                this.Statut = statut; 
                this.RaisonRefus = refus;
                this.DateF = DateTime.Now;
                ret = AllAdoption.DB_Update(this);
                Sync(this);
            }
            return ret;
        }

        public Adoption Accepter()
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Accepter", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }
            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            this.Statut = EStatutValidation.ACCEPTEE;
            this.DateD = DateTime.Now;
            this.Demande.Update(EStatutDemande.EN_COURS);
            AllAdoption.DB_Update(this);

            return this;
        }
        public Adoption Refuser(string? refus)
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Refuser", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            this.Statut = EStatutValidation.REFUSEE;
            this.DateF = DateTime.Now;
            this.RaisonRefus = refus;
            this.Demande.Update(EStatutDemande.TERMINEE);
            AllAdoption.DB_Update(this);

            return this;
        }
        public Adoption Indecis()
        {
            if (this.Sortie != null)
            {
                ExceptionLauncher.New("Adoption Indecis", "La Sortie est deja creer");
            }

            if (this.Demande.Statut == EStatutDemande.TERMINEE || this.Demande.Statut == EStatutDemande.CLOTUREE)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette demande est Terminee");
            }

            if (AllAdoption.Find(this.Id) == null)
            {
                ExceptionLauncher.New("Adoption Accepter", "Cette adoption n'est pas enregistré");
            }

            this.Statut = EStatutValidation.EN_COURS;
            this.DateD = null;
            this.DateF = null;
            this.RaisonRefus = null;
            this.Demande.Update(EStatutDemande.VALIDATION);
            AllAdoption.DB_Update(this);

            return this;
        }

        public static Adoption Creer(Demande demande, string info) 
        {
            if (demande == null )
            {
                ExceptionLauncher.New("Adoption Creer", $"Cette demande est null ");
            }
            Adoption adoption = new Adoption(demande, info);
            return adoption;
        }  
        public static int Save(Adoption adoption)
        {
            int retVal = 0;
            if (AllAdoption.Find(adoption.Id) == null && adoption.Demande.Statut == EStatutDemande.EXAMINATION)
            {
                    AllAdoption.Add(adoption);
                    retVal = AllAdoption.DB_Add(adoption);
                    if (retVal == 1)
                    {
                        Sync(adoption);
                    }
            }
            return retVal;
        } 
        public static int Delete(Adoption adoption)
        {
            int ret = 0;
            if (AllAdoption.Find(adoption.Id) != null)
            { 
                OnDelete(adoption);
                AllAdoption.Remove(adoption.Id);
                ret = AllAdoption.DB_Delete(adoption);
            }
            return ret;
        }

        private static int Sync(Adoption adoption)
        {
            int ret;
            if (adoption.Statut == EStatutValidation.REFUSEE)
            {
                ret = adoption.Demande.Update(EStatutDemande.TERMINEE);

            }else if (adoption.Statut == EStatutValidation.EN_COURS) 
            {
                ret = adoption.Demande.Update(EStatutDemande.VALIDATION);
            }
            else
            {
                ret = adoption.Demande.Update(EStatutDemande.EN_COURS);
            }
            return ret;

        }
        private static int OnDelete(Adoption adoption)
        {
            if (adoption.Sortie != null)
            {
                Sortie.Delete(adoption.Sortie);
            }

            return 1;
        }

    }
}
