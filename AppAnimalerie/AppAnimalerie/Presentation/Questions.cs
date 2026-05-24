
using AppAnimalerie.ClasseMetier;



namespace AppAnimalerie.Presentation
{
    public static class Questions
    {
        public static string? Login()
        {
            return AccesConsole.SaisirChaine($"-Email : ");
        }
        public static bool LogOut()
        {
            return AccesConsole.SaisirBoolean($"Voulez-vous vous deconnecter ? [O/N] : ");
        }
        public static string? Password()
        {
            return AccesConsole.SaisirChaine($"-Password : ");
        }

        public static int? Choix()
        {
            return AccesConsole.SaisirInt("Entrer votre choix ici (Ex. 1) : ");
        }
        public static bool Delete(string? str)
        {
            return AccesConsole.SaisirBoolean($"Voulez-vous supprimer ceci - {str} - ? [O/N] : ");
        }
        public static string DB_Access(int operation)
        {
            string? info = (operation == 1) ? "[Succes] Operation effectuée avec succes" : "[Echec] Operation non effectuée";
            return info;
        }
        public static string? Infos()
        {
            string? info = AccesConsole.SaisirChaine("Infos/Remarques/Description? (Facultatif): ");
            return info;
        }
        public static Abri? Abri()
        {
            return AllAbri.Find(AccesConsole.SaisirChaine("Entrer l'ID de l'abri (Ex. ABR...): "));
        }
        public static EStatutAbri? StatutAbri()
        {
            EStatutAbri? retval = null;
            string? valeur = AccesConsole.SaisirChaine("Libelle statut  (Ex. Disponible): ");
            if (Enum.TryParse<EStatutAbri>(valeur, out EStatutAbri result))
            {
                retval = result;
            }
            return retval;
            
        }
        public static ESexe? Sexe()
        {
            ESexe? retval = null;
            string? sexe = AccesConsole.SaisirChaine("Sexe: ");
            if (Enum.TryParse(typeof(ESexe), sexe, true, out var val))
            {
                retval = (ESexe)val;
            }
            return retval;
        }

        public static Couleur? Couleur()
        {
            return AllCouleur.FindByNom(AccesConsole.SaisirChaine("Entrer le nom de la couleurs (Ex. Rouge): "));
        }
        public static Animal? Animal()
        {
            return AllAnimal.Rechercher(AccesConsole.SaisirChaine("Entrer l'Id de l'Animal (Ex. 123456244): "));
        }
        public static TypeAnimal? TypeAnimal()
        {
            return AllTypeAnimal.FindTypeByNom(AccesConsole.SaisirChaine("Entrer le Libelle du type de Animal (Ex. Chien): "));
        }

        public static Contact? Contact()
        {
            return AllContacts.Find(AccesConsole.SaisirChaine("Entrer l'Id du Contact (Ex. CONT07): "));
        }
        public static TypeContact? TypeContact()
        {
            return AllTypeContact.FindByNom(AccesConsole.SaisirChaine("Entrer le Libelle du type de Contact (Ex. Benevole): "));
        }

        public static Vaccin? Vaccin()
        {
            return AllVaccin.FindByNom(AccesConsole.SaisirChaine("Entrer le Nom du vaccin (Ex. rage): "));
        }
        public static Vaccination? Vaccination()
        {
            return AllVaccination.Find(AccesConsole.SaisirChaine("Entrer l'ID de Vaccination (Ex. 234): "));
        }

        public static Compatibilite? Compatibilite()
        {
            return AllCompatibilite.FindByNom(AccesConsole.SaisirChaine("Entrer le Nom de la Compatibilité (Ex. jardin): "));
        }
        public static AnimalCompatibilité? AnimalCompatibilité()
        {
            return AnimalCompatibilitéService.Find(AccesConsole.SaisirChaine("Entrer l Id du test de compatibilité (Ex. 12353): "));
        }
        public static AnimalCouleur? AnimalCouleur()
        {
            return AllAnimalCouleur.Find(AccesConsole.SaisirChaine("Entrer l Id de la coloration (Ex. 12353): "));
        }

        public static Demande? Demande()
        {
            return AllDemande.Find(AccesConsole.SaisirChaine("Entrer l'Id demande (Ex. DEMO1) : "));
        }
        public static ETypeDemande? TypeDemande()
        {
            string? valeur =Forma.TrimUpper( AccesConsole.SaisirChaine("Entrer le Libelle du type demande (Ex. ENTREE): "));
            ETypeDemande? retval = null;
            
            if (Enum.TryParse<ETypeDemande>(valeur, out ETypeDemande result))
            {
                retval = result;
            }
            return retval;
            
        }
        public static EStatutDemande? StatutDemande()
        {
            string? valeur = Forma.TrimUpper( AccesConsole.SaisirChaine("Entrer le Libelle du Statut demande (Ex. Terminee): "));
            EStatutDemande? retval = null;

            if (Enum.TryParse<EStatutDemande>(valeur, out EStatutDemande result))
            {
                retval = result;
            }
            return retval;
            
        }
        public static EStatutAnimal? StatutAnimal()
        {
            string? valeur = Forma.TrimUpper( AccesConsole.SaisirChaine("Entrer le Libelle du Statut animal (Ex. Refuge): "));
            EStatutAnimal? retval = null;

            if (Enum.TryParse<EStatutAnimal>(valeur, out EStatutAnimal result))
            {
                retval = result;
            }
            return retval;

        }

        public static Entree? Entree()
        {
            return AllEntree.Find(AccesConsole.SaisirChaine("Entrer l'Id de l'entree (Ex. ENT-...): "));
        }
        public static MotifEntree? MotifEntree()
        {
            return AllMotifsEntrees.FindById(AccesConsole.SaisirChaine("Entrer l'Id du motif (Ex. Mos-...): "));
        }
        public static Sortie? Sorties()
        {
            return AllSortie.Find(AccesConsole.SaisirChaine("Entrer l'Id de la sortie (Ex. ENT-...): "));
        }
        public static MotifSortie? MotifSortie()
        {
            return AllMotifsSortie.FindById(AccesConsole.SaisirChaine("Entrer l'Id du motif (Ex. Mos-...): "));
        }
        public static Adoption? Adoption()
        {
            return AllAdoption.Find(AccesConsole.SaisirChaine("Entrer l'Id de l'Adoption (Ex. ADO-...): "));
        }
        public static Accueil? Accueil()
        {
            return AllAccueil.Find(AccesConsole.SaisirChaine("Entrer l'Id de l'Accueil (Ex. ACC-...): "));
        }
        public static EStatutValidation? StatutValidation()
        {
            string? valeur = Forma.TrimUpper(AccesConsole.SaisirChaine("Entrer le Libelle Statut (Ex. En_cours): "));
            EStatutValidation? retval = null;

            if (Enum.TryParse<EStatutValidation>(valeur, out EStatutValidation result))
            {
                retval = result;
            }
            return retval;

        }

    }
}
