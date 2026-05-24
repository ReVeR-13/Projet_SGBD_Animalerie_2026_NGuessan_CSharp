using AppAnimalerie.AccessDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public static class AllMotifsEntrees
    {
        private static readonly Dictionary<string, MotifEntree> _lesMotifsEntrees;
        private static int _num;

        static AllMotifsEntrees()
        {
            _lesMotifsEntrees = new Dictionary<string, MotifEntree>();
            _num = 0;
        }

        public static int Num
        {
            get 
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesMotifsEntrees);
                }
                return _num; 
            }
        }
        public static int Count
        {
            get { return _lesMotifsEntrees.Count; }
        }
        public static string LesEntrees
        {
            get
            {
                int i = 0;
                string retVal = string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Libelle", "Details");

                foreach (MotifEntree dm in _lesMotifsEntrees.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{dm.Libele}",
                    $"{dm.Details}");
                }

                return $"Liste des Motifs Entree [{Count}]\n\n" + retVal;
            }
        }
        public static void Add(MotifEntree motifs)
        {

            if (FindByLibelle(motifs.Id) != null)
            {
                throw new Exception($"[AllMotifsEntrees] cet type est deja enregistré : {motifs.Libele}");
            }
            _num++;
            _lesMotifsEntrees.Add(motifs.Id, motifs);
        }
        public static MotifEntree FindByLibelle(string libele)
        {
            MotifEntree retval = null;
            string flibelle = Forma.TrimUpper(libele);
            foreach (MotifEntree m in _lesMotifsEntrees.Values)
            {
                if (m.Libele == flibelle)
                {
                    retval = m; 
                    break;  
                }
            }
            return retval;
        }
        public static MotifEntree FindById(string id)
        {
            MotifEntree retval = null;
            string fid = Forma.TrimUpper(id);
            if (_lesMotifsEntrees.ContainsKey(fid))
            {
                retval = _lesMotifsEntrees[fid];
            }
            return retval;
        }
        public static void Remove(string libele)
        {
            string formatLibele = libele.Trim().ToUpper();
            if (FindByLibelle(formatLibele) == null)
            {
                throw new Exception($"[AllMotifsEntrees] cet type n'est encore enregistré : {formatLibele}");
            }
            _lesMotifsEntrees.Remove(formatLibele);
        }

        public static int DB_Add(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) == null)
            {
                ret = DB_MotifEntree.Add(motif);
            }
            return ret;
        }
        public static int DB_Update(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) != null)
            {
                ret = DB_MotifEntree.Update(motif);
            }
            return ret;
        }
        public static int DB_Delete(MotifEntree motif)
        {
            int ret = 0;
            if (DB_MotifEntree.UnMotifById(motif) != null)
            {
                ret = DB_MotifEntree.Delete(motif);
            }
            return ret;
        }
    }
    public static class AllMotifsSortie
    {
        private static readonly Dictionary<string, MotifSortie> _lesMotifsSorties;
        private static int _num;

        static AllMotifsSortie()
        {
            _lesMotifsSorties = new Dictionary<string, MotifSortie>();
            _num = 0;
        }

        public static int Num
        {
            get 
            {
                if (Count > 0)
                {
                    _num = Forma.LastNumero(_lesMotifsSorties);
                }
                return _num; 
            }
        }
        public static int Count
        {
            get { return _lesMotifsSorties.Count; }
        }
        public static string LesSorties
        {
            get
            {
                int i = 0;
                string retVal = string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    "N°", "Id", "Date Crea.", "Libelle", "Details");

                foreach (MotifSortie dm in _lesMotifsSorties.Values)
                {
                    i++;
                    retVal += string.Format($"{"{0,-4} {1,-19} {2,-12} {3,-15} {4,-20}\n"}",
                    $"{i}°",
                    $"{dm.Id}",
                    $"{dm.DateCreation.ToString("dd-MM-yyyy")}",
                    $"{dm.Libele}",
                    $"{dm.Details}");
                }

                return $"Liste des Motifs Sortie [{Count}]\n\n" + retVal;
            }
        }
        public static void Add(MotifSortie motifs)
        {

            if (FindByLibelle(motifs.Id) != null)
            {
                throw new Exception($"[AllMotifsEntrees] cet motif est deja enregistré : {motifs.Libele}");
            }
            _num++;
            _lesMotifsSorties.Add(motifs.Id, motifs);
        }
        public static MotifSortie? FindByLibelle(string libele)
        {
            MotifSortie? retval = null;
            string formatlibele = Forma.TrimUpper(libele);
            foreach (MotifSortie l in _lesMotifsSorties.Values)
            {
                if (l.Libele == formatlibele)
                {
                    retval = l; break;
                }
            }
            
            return retval;
        }
        public static MotifSortie? FindById(string id)
        {
            MotifSortie? retval = null;
            string formatlibele = Forma.TrimUpper(id);
            if (_lesMotifsSorties.ContainsKey(formatlibele))
            {
                retval = _lesMotifsSorties[formatlibele];
            }
            return retval;
        }
        public static void Remove(string id)
        {
            string formatLibele = Forma.TrimUpper(id);
            if (FindByLibelle(formatLibele) == null)
            {
                throw new Exception($"[AllMotifsEntrees] cet motif n'est encore enregistré : {formatLibele}");
            }
            _lesMotifsSorties.Remove(formatLibele);
        }

        public static int DB_Add(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) == null)
            {
                ret = DB_MotifSortie.Add(motif);
            }
            return ret;
        }
        public static int DB_Update(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) != null)
            {
                ret = DB_MotifSortie.Update(motif);
            }
            return ret;
        }
        public static int DB_Delete(MotifSortie motif)
        {
            int ret = 0;
            if (DB_MotifSortie.UnMotifById(motif) != null)
            {
                ret = DB_MotifSortie.Delete(motif);
            }
            return ret;
        }
    }

    public abstract class Motifs :ITable 
    {
        private DateTime _date;  
        private string _libele;
        private string _Details;
        protected Motifs(string libele,string details)
        {
            this.DateCreation = DateTime.Now;
            Libele = libele;
            Details = details;
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
        public string Libele
        {
            get { return _libele; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()) || value.Trim().Length < 3)
                {
                    throw new Exception($"[Motifs] Cet libele n'est pas valide : {value}");
                }
                _libele = value.Trim().ToUpper();
            }
        }
        public string Details
        {
            get { return _Details; }
            set
            {
                string val = value.Trim();
                if (string.IsNullOrEmpty(value.Trim()))
                {
                    val="Pas de Detail";
                }
                _Details = val;
            }
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Date", DateCreation.ToString("dd-MM-yyyy")) +
                Forma.Texta2("Nom", Libele) +
                Forma.Texta2("Description", Details);
            
            return retVal;
        }

    }

    public class MotifEntree : Motifs
    {
        private string _id;
        private MotifEntree(string libele, string details):base(libele, details)
        {
            _id = Forma.SimpleId("MOE", AllMotifsEntrees.Num + 1);
        }
        public override string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public static MotifEntree Creer(string libele, string description)
        {
            MotifEntree retVal = null;
            string FormatLibele = libele.Trim().ToUpper();
            if (string.IsNullOrEmpty(libele))
            {
                throw new Exception($"[Motif Entree] Les parametres sont invalides");
            }
            retVal = new MotifEntree(FormatLibele, description);
            return retVal;
        }
        public static int Save(MotifEntree motifEntree)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindById(motifEntree.Id) == null)
            {
                AllMotifsEntrees.Add(motifEntree);
                retVal = AllMotifsEntrees.DB_Add(motifEntree);
            }
            return retVal;
        }
        public int Update (string libele, string description)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindByLibelle(libele) == null)
            {
                this.Libele = libele;
                this.Details = description;
                retVal = AllMotifsEntrees.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(MotifEntree motif)
        {
            int retVal = 0;
            if (AllMotifsEntrees.FindById(motif.Id) != null)
            {
                AllMotifsEntrees.Remove(motif.Id);
                retVal = AllMotifsEntrees.DB_Delete(motif);
            }
            return retVal;
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Id", Id)+ base.ToString();
            return retVal;
        }
    }
    public class MotifSortie : Motifs
    {
        private string _id;
        private MotifSortie(string libele, string details) : base(libele, details)
        {
            _id = Forma.SimpleId("MOS", AllMotifsSortie.Num + 1);
        }
        public override string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public static MotifSortie Creer(string libele, string description)
        {
            MotifSortie retVal = null;
            string FormatLibele = libele.Trim().ToUpper();
            if (string.IsNullOrEmpty(libele))
            {
                throw new Exception($"[Motif Sortie] Les parametres sont invalides");
            }
            retVal = new MotifSortie(FormatLibele, description);
            return retVal;
        }
        public static int Save(MotifSortie motif)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindById(motif.Id) == null)
            {
                AllMotifsSortie.Add(motif);
                retVal = AllMotifsSortie.DB_Add(motif);
            }
            return retVal;
        }
        public int Update(string libele, string description)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindByLibelle(libele) == null)
            {
                this.Libele = Forma.TrimUpper(libele);
                this.Details = description;
                retVal = AllMotifsSortie.DB_Update(this);
            }
            return retVal;
        }
        public static int Delete(MotifSortie motif)
        {
            int retVal = 0;
            if (AllMotifsSortie.FindById(motif.Id) != null)
            {
                AllMotifsSortie.Remove(motif.Id);
                retVal = AllMotifsSortie.DB_Delete(motif);
            }
            return retVal;
        }
        public override string ToString()
        {
            string retVal = Forma.Texta2("Id", Id) + base.ToString();
            return retVal;
        }
    }
}
