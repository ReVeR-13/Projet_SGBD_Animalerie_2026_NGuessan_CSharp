using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAnimalerie.ClasseMetier
{
    public enum ESexe
    {
        M,
        F
    }
    public enum EStatutAnimal
    {
        REFUGE,
        EXAMINATION,
        ACCUEIL,
        ADOPTION,
        PROPRIETAIRE,
        DECEDE
    }
    public enum EStatutAbri
    {
        DISPONIBLE,
        OCCUPE,
        HORS_SERVICE,
    }
    public enum EStatutDemande
    {
        EXAMINATION,
        EN_COURS,
        VALIDATION,
        TERMINEE,
        CLOTUREE,
        ANNULEE
    }
    public enum ETypeDemande
    {
        ENTREE,
        SORTIE,
        ADOPTION,
        ACCUEIL,
        DECES,
        NAISSANCE,
        INFO
    }
    public enum EStatutValidation
    {
        EN_COURS,
        ACCEPTEE,
        REFUSEE
    }
    public enum ETypeCompatibilite
    {
        Chat,
        Chien,
        Jeune_Enfant,
        Enfant,
        Jardin,
        Poney
    }
    public enum EValeurCompatibilite
    {
        Non,
        Oui
    }
    public interface ITable
    {
        string Id { get; }
        DateTime DateCreation { get; }
    } 
  
}
