using AppAnimalerie.AccessDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllAbri
    {
        private static readonly Dictionary<string, Abri> _lesAbris;
        private static int _num;
        static AllAbri()
        {
            _lesAbris = new Dictionary<string, Abri>();
            _num = 0;
        }
        public static void Add(Abri abri)
        {
            if (Find(abri.Libelle) != null)
            {
                ExceptionLauncher.New("Add AllAbri", "Cet nom d'abri existe deja");
            }
            _num++;
            _lesAbris.Add(abri.Id, abri);
        }
        //---------------------------------------
        public static int DB_Add(Abri abri)
        {
            int ret = 0;
            if (DB_Abri.UnAbriByNom_db(abri.Libelle) == null)
            {
                ret = DB_Abri.Add(abri);
            }
            return ret;
        }
        public static int DB_Update(Abri abri)
        {
            return DB_Abri.Update(abri);
        }
        public static int DB_Delete(Abri abri)
        {
            int ret = 0;
            if (DB_Abri.UnAbriId_db(abri.Id) != null )
            {
                ret = DB_Abri.Delete(abri.Id);
            }
            return ret;
        }
        //----------------------------------------
        public static void Remove(string id)
        {
            string f_id = Forma.TrimUpper(id);
            if (Find(f_id) == null)
            {
                ExceptionLauncher.New("Delete All_From_db", "Cet statut n'est pas en liste");
            }
            DB_Abri.Delete(f_id);
            _lesAbris.Remove(f_id);
        }
        public static Abri? Find(string id)
        {
            Abri abri = null;
            string f_id = Forma.TrimUpper(id);
            if (_lesAbris.ContainsKey(f_id))
            {
                abri = _lesAbris[f_id];
            }
            return abri;
        }
        public static Abri? FindbyNom(string libelle)
        {
            Abri? abri = null;
            string f_libelle = Forma.TrimUpper(libelle);
            foreach (Abri ab in _lesAbris.Values)
            {
                if (ab.Libelle == f_libelle)
                {
                    abri = ab;
                }
            }

            return abri;
        }
        public static string LesAbris
        {
            get
            {
                int i = 0;
                string retVal = $"Liste des Abris [{Count}]\n\n" +
                    Forma.Text("N°", "Id", "Date Crea.", "Libelle","Statut" ,"Desciption");
                foreach (Abri a in _lesAbris.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Libelle}",
                    $"{a.Statut}",
                    $"{a.Description}");
                }
                return retVal;
            }
        }
        public static int Count
        {
            get
            {
                return _lesAbris.Count;
            }
        }
        public static int Num
        {
            get 
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesAbris);
                }
                return _num; 
            }
        }
        public static IEnumerable<Abri> Get()
        {
            foreach (Abri sta in _lesAbris.Values)
            {
                yield return sta;
            }
        }

    }
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
