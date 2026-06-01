using AppAnimalerie.AccessDB;
using AppAnimalerie.ClasseMetier;
using AppAnimalerie.ClasseService;

namespace AppAnimalerie.Presentation
{
    public class Presentation
    {

        public Presentation()
        {
            Connection();
        }
        public static void Connection()
        {
            int statut = 0;

            while (statut != 99)
            {
                try
                {
                    Console.Clear();

                    AccesConsole.EnCouleur("------------------------------------------\n", ConsoleColor.Yellow);
                    AccesConsole.EnCouleur("             APP ANIMALERIE\n", ConsoleColor.Cyan);
                    AccesConsole.EnCouleur("------------------------------------------\n\n\n", ConsoleColor.Yellow);
                    AccesConsole.EnCouleur("               CONNECTION\n\n", ConsoleColor.Yellow);


                    string? email = "apollo@hotmail.be";//Questions.Login();
                    string? pass = "Apollo1234";//Questions.Password();

                    if (!Authentification.Login(email, pass))
                    {
                        ExceptionLauncher.New("Connection", "Login et Mot de passe erronés");
                    }

                    User user = UserService.DB_FindByEmail(email);

                    Starteur();
                    MenuPrincipal(user);

                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Interface Connection] Erreur : {e.Message}");
                    statut = 0;
                    AccesConsole.Attendre();
                }
            }

        }
        static void Starteur()
        {
            DB_Couleur.All_From_db();
            DB_TypeAnimal.AllTypesAnimal();
            DB_Animal.Db_listeAnimaux();

            DB_TypeContact.All_From_Db();
            DB_Contact.All_From_Db();
            DB_TypeCnt_Contact.AllRoles();
            DB_AnimalCouleur.All_From_Db();

            DB_Abri.All_From_db();

            DB_Vaccin.All_From_Db();
            DB_Vaccination.All_From_Db();

            DB_Compatibilite.All_From_Db();
            DB_AnimalCompatibilité.All_From_Db();

            DB_Demande.All_From_Db();

            DB_MotifEntree.All_From_Db();
            DB_MotifSortie.All_From_Db();

            DB_Entree.All_From_Db();
            DB_Sortie.All_From_Db();

            DB_Adoption.All_From_Db();
            DB_Accueil.All_From_Db();
            DB_User.All_From_Db();

        }
        public static void MenuPrincipal(User user)
        {
            int? retVal = 0;

            while (retVal != 99)
            {
                try
                {

                    Dictionary<string, string> menupp = new()
                    {
                        { "1", "Gerer les Demande" },
                        { "2", "Gerer les Animaux" },
                        { "3", "Gerer les Contacts" },
                        { "4", "les Parametres" }
                    };

                    AccesConsole.CreerEcran("MENU PRINCIPAL");
                    AccesConsole.Menu(menupp);
                    retVal = AccesConsole.SaisirInt("Entrer votre choix ici : ");

                    if (retVal <= menupp.Count && retVal > 0)
                    {
                        switch (retVal)
                        {
                            case 1:
                                {
                                    Interface_Demande();
                                }
                                break;
                            case 2:
                                {
                                    Interface_Animal();
                                }
                                break;
                            case 3:
                                {
                                    Interface_Contacts();
                                }
                                break;
                            case 4:
                                {
                                    Interface_Parametres();
                                }
                                break;

                        }
                        AccesConsole.Attendre();
                    }

                    if (retVal == 99)
                    {
                        bool logout = Questions.LogOut();
                        if (logout)
                        {
                            Authentification.LogOut(user.Email);
                            retVal = 99;
                        }
                        else
                        {
                            retVal = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MenuPrincipal] Erreur : {ex.Message}");
                    retVal = 0;
                    AccesConsole.Attendre();
                }
            }

        }

        //---------------------------------------------------------demandes

        public static void Interface_Demande()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {

                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir toutes Les Demandes" },
                        { "2", "Consuler une demande" },
                        { "3", "Nouvelle demande" },
                        { "4", "> Gerer les entrees   <" },
                        { "5", "> Gerer les adoptions <" },
                        { "6", "> Gerer les accueils  <" },
                        { "7", "> Gerer les sorties   <" }
                    };

                    AccesConsole.CreerEcran("GESTION DES DEMANDES", AllDemande.ListeByStatut(EStatutDemande.EN_COURS, EStatutDemande.VALIDATION), menu);

                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_AllDemande();
                                }
                                break;
                            case 2:
                                {
                                    Demande? demande = Questions.Demande();
                                    if (demande != null)
                                    {
                                        Interface_Demande_Un(demande);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Demande invalide");
                                    }

                                }
                                break;
                            case 3:
                                {

                                    Interface_Demande_Creer();

                                }
                                break;
                            case 4:
                                {

                                    Interface_Entrees();

                                }
                                break;
                            case 5:
                                {
                                    Interface_Adoption();
                                }
                                break;
                            case 6:
                                {
                                    Interface_Accueil();
                                }
                                break;
                            case 7:
                                {
                                    Interface_Sorties();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }


                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static ETypeDemande? Interface_ChoixType()
        {
            AccesConsole.CreerEcran($"LES DEMANDES - NOUVELLE DEMANDE");
            AccesConsole.Afficher(ETypeDemande.INFO);
            return Questions.TypeDemande();
        }
        public static void Interface_Demande_Creer()
        {
            ETypeDemande? type = Interface_ChoixType();

            AccesConsole.CreerEcran($"LES DEMANDES - NOUVELLE DEMANDE", AllContacts.LesContacts);
            Console.WriteLine();

            Contact? fcontact = ChoixContactDemande();

            if (fcontact == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " Le contact n'est pas valide");
            }

            if (type == ETypeDemande.ENTREE)
            {
                AccesConsole.Afficher(AllAnimal.ListeByStatutNot(EStatutAnimal.REFUGE, EStatutAnimal.DECEDE));
            }
            else
            {
                AccesConsole.Afficher(AllAnimal.ListeByStatut(EStatutAnimal.REFUGE));
            }

            Animal? animal = ChoixAnimalDemande();
            if (animal == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " L animal n'est pas valide");
            }
            Interface_Demande_Creer(fcontact, animal, type);

        }
        public static void Interface_Demande_Creer(Contact contact)
        {
            ETypeDemande? type = Interface_ChoixType();

            AccesConsole.CreerEcran($"LES DEMANDES - NOUVELLE DEMANDE", AllAnimal.ListeByStatut(EStatutAnimal.EXAMINATION));
            Console.WriteLine();

            if (contact == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " Le contact n'est pas valide");
            }

            Animal? animal = ChoixAnimalDemande();

            if (animal == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " L animal n'est pas valide");
            }

            Interface_Demande_Creer(contact, animal, type);


        }
        public static void Interface_Demande_Creer(Animal animal)
        {
            ETypeDemande? type = Interface_ChoixType();

            AccesConsole.CreerEcran($"LES DEMANDES - NOUVELLE DEMANDE", AllContacts.LesContacts);
            Console.WriteLine();
            if (animal == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " L animal n'est pas valide");
            }

            Contact? contact = ChoixContactDemande();

            if (contact == null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " Le contact n'est pas valide");
            }
            Interface_Demande_Creer(contact, animal, type);
        }
        public static void Interface_Demande_Creer(Contact contact, Animal animal, ETypeDemande? type)
        {
            if (AllDemande.Find(contact, animal, EStatutDemande.TERMINEE, (ETypeDemande)type) != null)
            {
                ExceptionLauncher.New("Interface Demande Creer", " Le contact a deja une demande EN COURS du meme type concernant cette animal");
            }

            AccesConsole.CreerEcran($"LES DEMANDES - NOUVELLE DEMANDE");
            Console.WriteLine();

            string? remarque = Questions.Infos();

            Demande demande = Demande.Creer(contact, animal, (ETypeDemande)type, remarque);
            if (demande != null)
            {
                Interface_Demande_Save(demande);
            }
            else
            {
                Console.WriteLine("Demande non créé");
            }

        }
        public static void Interface_Demande_Save(Demande demande)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES DEMANDES - DEMANDE CREE - {demande.Id}", demande.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        int i = Demande.Save(demande);
                        AccesConsole.Afficher(i);
                        if (i == 1)
                        {
                            bool suivant = AccesConsole.SaisirBoolean($"Voulez-vous continuer vers {demande.Type} ? [O/N] ");
                            if (suivant)
                            {
                                Interface_Gestionnaire_Demande(demande);
                            }
                        }
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Demande_Modifier(Demande demande)
        {
            AccesConsole.CreerEcran($"LES DEMANDES - n°{demande.Id} - MODIFICATION", demande.ToString());

            Contact? new_conta = Questions.Contact();
            new_conta ??= demande.Contact;

            Animal? animal = Questions.Animal();
            animal ??= demande.Animal;

            ETypeDemande? type = Questions.TypeDemande();
            type ??= demande.Type;

            EStatutDemande? statut = Questions.StatutDemande();
            statut ??= demande.Statut;

            string? remarque = Questions.Infos();
            if (new_conta != null && animal != null && type != null && statut != null)
            {
                AccesConsole.Afficher(demande.Update(new_conta, animal, (ETypeDemande)type, (EStatutDemande)statut, remarque));
            }
            else
            {
                AccesConsole.Erreur("Des infos sont null");
            }

        }
        public static void Interface_Demande_Supprimer(Demande demande)
        {
            bool choix = Questions.Delete(demande.Id);
            if (choix)
            {
                AccesConsole.Afficher(Demande.Delete(demande));
            }

        }

        public static void Interface_AllDemande()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "** Gerer une demande" },
                        { "2", "** Voir les nouvelles demandes" },
                        { "3", "** Voir les domandes en cours" },
                        { "4", "** Voir les domandes terminees" },
                        { "5", "** Voir les domandes cloturees" }
                    };

                    AccesConsole.CreerEcran("LES DEMANDES - TOUTES LES DEMANDES", AllDemande.Listes, menu);
                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Demande? demande = Questions.Demande();
                                    if (demande != null)
                                    {
                                        Interface_Demande_Un(demande);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("demande invalide");
                                    }
                                }
                                break;
                            case 2:
                                {
                                    AccesConsole.CreerEcran("LES DEMANDES - NOUVELLES DEMANDES", AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, EStatutDemande.VALIDATION));
                                }
                                break;
                            case 3:
                                {
                                    AccesConsole.CreerEcran("LES DEMANDES - DEMANDES EN COURS", AllDemande.ListeByStatut(EStatutDemande.EN_COURS));
                                }
                                break;
                            case 4:
                                {
                                    AccesConsole.CreerEcran("LES DEMANDES - DEMANDES TERMINEES", AllDemande.ListeByStatut(EStatutDemande.TERMINEE));
                                }
                                break;
                            case 5:
                                {
                                    AccesConsole.CreerEcran("LES DEMANDES - DEMANDES TERMINEES", AllDemande.ListeByStatut(EStatutDemande.CLOTUREE));
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Demande_Un(Demande demande)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? bloque = null;
                    if (demande.Statut > EStatutDemande.EN_COURS)
                    {
                        bloque = $"[Bloqué] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", FirtLigne(demande) },
                        { "2", $"** Details de l'animal **" },
                        { "3", $"** Details du contact  **" },
                        { "4", $"{bloque}Modifier la demande" },
                        { "5", $"{bloque}Modifier le motif de cette demande" },
                        { "6", "Supprimer cette demande" }
                    };

                    AccesConsole.CreerEcran($"LES DEMANDES - {demande.Id} -", demande, menu);

                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Gestionnaire_Demande(demande);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Animal_un(demande.Animal);
                                }
                                break;

                            case 3:
                                {
                                    Interface_Contact_Un(demande.Contact);
                                }
                                break;
                            case 4:
                                {
                                    if (demande.Statut > EStatutDemande.EN_COURS)
                                    {
                                        ExceptionLauncher.New("Demande Interface", "Cette demande est terminéé");
                                    }
                                    Interface_Demande_Modifier(demande);
                                }
                                break;
                            case 5:
                                {
                                    if (demande.Statut > EStatutDemande.EN_COURS)
                                    {
                                        ExceptionLauncher.New("Demande Interface", "Cette demande est terminéé");
                                    }
                                    Console.WriteLine();

                                    AccesConsole.Afficher(ETypeDemande.ENTREE);

                                    ETypeDemande? eType = Questions.TypeDemande();
                                    if (eType != null)
                                    {
                                        AccesConsole.Afficher(demande.Update(eType));
                                    }

                                }
                                break;
                            case 6:
                                {
                                    Interface_Demande_Supprimer(demande);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Gestionnaire_Demande(Demande demande)
        {
            if (demande == null)
            {
                ExceptionLauncher.New("Gestionnaire Demande", "Parametre vide");
            }


            switch (demande.Type)
            {
                case ETypeDemande.ADOPTION:
                    {
                        if (AffichageData(demande))
                        {
                            Interface_Adoption_Un(AllAdoption.Find(demande));
                        }
                        else
                        {
                            Interface_Adoption_Creer(demande);
                        }
                    }
                    break;
                case ETypeDemande.ACCUEIL:
                    {
                        if (AffichageData(demande))
                        {
                            Interface_Accueil_Un(AllAccueil.Find(demande));
                        }
                        else
                        {
                            Interface_Accueil_Creer(demande);
                        }
                    }
                    break;
                case ETypeDemande.ENTREE:
                    {
                        if (AffichageData(demande))
                        {
                            Interface_Entrees_Un(AllEntree.Find(demande));
                        }
                        else
                        {
                            Interface_Entrees_Creer(demande);
                        }
                    }
                    break;
                case ETypeDemande.SORTIE:
                    {
                        if (AffichageData(demande))
                        {
                            Interface_Sorties_Un(AllSortie.Find(demande));
                        }
                        else
                        {
                            Interface_Sorties_Creer(demande);
                        }
                    }
                    break;
            }

        }
        private static bool AffichageData(Demande demande)
        {
            return demande.Type switch
            {
                ETypeDemande.ENTREE => AllEntree.Find(demande) != null || demande.Statut != EStatutDemande.EN_COURS,
                ETypeDemande.ADOPTION => AllAdoption.Find(demande) != null || demande.Statut != EStatutDemande.EXAMINATION,
                ETypeDemande.ACCUEIL => AllAccueil.Find(demande) != null || demande.Statut != EStatutDemande.EXAMINATION,
                ETypeDemande.SORTIE => AllSortie.Find(demande) != null || demande.Statut != EStatutDemande.EN_COURS,
                _ => true,
            };
        }

        //---------------------------------------------------------animaux

        public static void Interface_Animal()
        {
            int? choix = 0;
            while (choix != 99)
            {

                try
                {

                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les animaux" },
                        { "2", "Voir un animal" },
                        { "3", "Creer un animal" },
                        { "4", "Voir les types d'animal" },
                        { "5", "Voir tous les vaccins" },
                        { "6", "Voir tous les compatibilités" },
                        { "7", "Voir tous les Couleur" },
                    };

                    AccesConsole.CreerEcran("LES ANIMAUX", AllAnimal.ListeByStatut(EStatutAnimal.REFUGE, EStatutAnimal.EXAMINATION));
                    AccesConsole.Menu(menu);

                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Animal_All();
                                }
                                break;
                            case 2:
                                {
                                    Animal animal = Questions.Animal();
                                    if (animal != null)
                                    {
                                        Interface_Animal_un(animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Afficher("Animal invalide");
                                    }

                                }
                                break;
                            case 3: { Interface_Animal_creer(); } break;
                            case 4:
                                {
                                    Interface_Animal_Type();
                                }
                                break;
                            case 5:
                                {
                                    Interface_Vaccins();
                                }
                                break;
                            case 6:
                                {
                                    Interface_Compatibilite();
                                }
                                break;
                            case 7:
                                {
                                    Interface_Couleur();
                                }
                                break;

                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MenuPrincipal] Erreur : {ex.Message}");
                    choix = 0;
                    AccesConsole.Attendre();
                }


            }
        }
        public static Animal Interface_Animal_creer()
        {

            AccesConsole.CreerEcran("ANIMAL - CREATION");

            string? nom = AccesConsole.SaisirChaine("Nom :");

            AccesConsole.Afficher("\n" + AllTypeAnimal.LesTypes + "\n");
            string? type = AccesConsole.SaisirChaine("Type (ex. chien) :");

            DateTime dnaiss = (DateTime)AccesConsole.SaisirDate("Date de naissance (ex. 10-05-2023) :");
            string? sexe = AccesConsole.SaisirChaine("Sexe :");

            AccesConsole.Afficher("\n" + AllCouleur.LesCouleurs + "\n");
            Couleur? color = Questions.Couleur();

            bool steril = AccesConsole.SaisirBoolean("Steril [O/N]:");
            DateTime? datester = null;
            if (steril)
            {
                datester = AccesConsole.SaisirDate("Date de Sterilisation (ex. 03-09-2022) :");
            }

            string? descr = AccesConsole.SaisirChaine("Description :");
            if (string.IsNullOrEmpty(descr))
            {
                descr = "--";
            }

            string? particularite = AccesConsole.SaisirChaine("Particularité :");
            if (string.IsNullOrEmpty(particularite))
            {
                particularite = "--";
            }

            Animal animal = Animal.Creer(nom, type, dnaiss, sexe, color, steril, datester, descr, particularite);



            if (animal == null)
            {
                ExceptionLauncher.New("Interface Animal Creer", "Creation echouée");
            }
            Interface_Animal_Save(animal);

            return animal;

        }
        public static void Interface_Animal_Save(Animal animal)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ANIMAUX - ANIMAUX CREE - {animal.Id}", animal.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(animal.Save());

                        /*bool Q_couleur = AccesConsole.SaisirBoolean($"Voulez-vous ajouter couleur secondaire à {animal.Nom}? [O/N] :");
                        if (Q_couleur)
                        {
                            Interface_Coloration_Add(animal);
                        }

                        bool Q_vaccin = AccesConsole.SaisirBoolean($"Voulez-vous ajouter un vaccin à {animal.Nom}? [O/N] :");
                        if (Q_vaccin)
                        {
                            Interface_Animal_un_AddVaccination(animal);
                        }

                        bool Q_compa = AccesConsole.SaisirBoolean($"Voulez-vous ajouter une compatibilité à {animal.Nom}? [O/N]:");
                        if (Q_compa)
                        {
                            Interface_Animal_un_AddCompatibilite(animal);
                        }*/

                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Animal_All()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "** Voir les animaux presents au refuge" },
                        { "2", "** Voir les animaux en adoption" },
                        { "3", "** Voir les animaux en accueil" },
                        { "4", "** Voir les animaux chez leurs proprietaires" },
                        { "5", "** Voir les animaux decedés" }
                    };

                    AccesConsole.CreerEcran("LES ANIMAUX - TOUS LES ANIMAUX", AllAnimal.LesAnimaux, menu);
                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES ANIMAUX - AU REFUGE", AllAnimal.ListeByStatut(EStatutAnimal.REFUGE));
                                }
                                break;

                            case 2:
                                {
                                    AccesConsole.CreerEcran("LES ANIMAUX - EN ADOPTION", AllAnimal.ListeByStatut(EStatutAnimal.ADOPTION));
                                }
                                break;
                            case 3:
                                {
                                    AccesConsole.CreerEcran("LES ANIMAUX - EN ACCUEIL", AllAnimal.ListeByStatut(EStatutAnimal.ACCUEIL));
                                }
                                break;
                            case 4:
                                {
                                    AccesConsole.CreerEcran("LES ANIMAUX - PROPRIETAIRE", AllAnimal.ListeByStatut(EStatutAnimal.PROPRIETAIRE));
                                }
                                break;
                            case 5:
                                {
                                    AccesConsole.CreerEcran("LES ANIMAUX - DECEDES", AllAnimal.ListeByStatut(EStatutAnimal.DECEDE));
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
            AccesConsole.Attendre();
        }
        public static void Interface_Animal_un(Animal ami)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? info = null;
                    if (ami.Statut != EStatutAnimal.REFUGE)
                    {
                        info = $"[Bloquer] ";
                    }

                    Dictionary<string, string> menu = new()
                        {
                            { "1", "Modifier" },
                            { "2", "Supprimer" },
                            { "3", $"{info}Assigner un Abri" },
                            { "4", $"{info}Gerer les Vaccinations" },
                            { "5", $"{info}Gerer les Compatibilités" },
                            { "6", $"{info}Gerer les Couleurs" },
                            { "7", "Voir historique des demandes" }
                        };

                    if (ami.Statut != EStatutAnimal.REFUGE && ami.Statut != EStatutAnimal.EXAMINATION)
                    {
                        menu.Add($"{8}", "** Voir la demande encours **");
                    }

                    AccesConsole.CreerEcran($"LES ANIMAUX - {ami.Nom} -", ami, menu);
                    choix = Questions.Choix();

                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Animal_un_Modification(ami);
                                }
                                break;
                            case 2:
                                {
                                    if (Interface_Animal_un_Delete(ami))
                                    {
                                        choix = 99;
                                    }

                                }
                                break;
                            case 3:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE)
                                    {
                                        ExceptionLauncher.New("Add Abri", "Cet animal est hors du refuge.");
                                    }
                                    Interface_Animal_un_UpdateAbri(ami);
                                }
                                break;
                            case 4:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE)
                                    {
                                        ExceptionLauncher.New("Gerer Vaccination", "Cet animal est hors du refuge.");
                                    }
                                    Interface_Animal_un_ShowVaccinantion(ami);
                                }
                                break;
                            case 5:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE)
                                    {
                                        ExceptionLauncher.New("Gerer Compatibilités", "Cet animal est hors du refuge.");
                                    }
                                    Interface_Animal_un_ShowCompatibilite(ami);
                                }
                                break;
                            case 6:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE)
                                    {
                                        ExceptionLauncher.New("Gerer Couleur", "Cet animal est hors du refuge.");
                                    }
                                    Interface_Coloration_Animal(ami);
                                }
                                break;
                            case 7:
                                {
                                    Interface_Animal_un_Historique(ami);
                                }
                                break;
                            case 8:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE && ami.Statut != EStatutAnimal.EXAMINATION)
                                    {
                                        Interface_Demande_Un(ami.LastSortie.Demande);
                                    }

                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Animal Interface] Erreur : {ex.Message}");
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }


        }
        public static void Interface_Animal_un_Modification(Animal ami)
        {

            AccesConsole.CreerEcran($"LES ANIMAUX - {ami.Nom} - MODIFICATION", ami);
            AccesConsole.EnCouleur("Faire **Enter** si vous ne souhaitez pas modifier une ms\n", ConsoleColor.Yellow);

            TypeAnimal? tp = Questions.TypeAnimal();

            string? nom = AccesConsole.SaisirChaine("Nom: ");

            ESexe? sexe = Questions.Sexe();

            DateTime? dnais = AccesConsole.SaisirDate("Date de naiss.(ex. 10-02-2012): ");

            bool? sterile = AccesConsole.SaisirBooleanNullable("Sterile [O/N]: ") ?? ami.Sterile;

            string? desc = AccesConsole.SaisirChaine("Description: ");

            string? part = AccesConsole.SaisirChaine("Particularité: ");

            AccesConsole.Afficher(ami.Update(tp, nom, sexe, dnais, sterile, desc, part));

        }
        public static bool Interface_Animal_un_Delete(Animal animal)
        {
            bool choix = Questions.Delete(animal.Nom);
            if (choix)
            {
                AccesConsole.Afficher(Animal.Delete(animal));
            }
            return choix;
        }
        public static void Interface_Animal_un_Historique(Animal animal)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir Les Accueil" },
                        { "2", "Voir Les Adoption"},
                        { "3", "Voir Les Sortie" },
                        { "4", "Voir Les Entree" }
                    };

                    AccesConsole.CreerEcran($"HISTORIQUE DES DEMANDES - {animal.Nom}", AllDemande.ListesByAnimal(animal), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran($"HISTORIQUE DES DEMANDES - {animal.Nom}", animal.Accueils);
                                }
                                break;
                            case 2:
                                {
                                    AccesConsole.CreerEcran($"HISTORIQUE DES DEMANDES - {animal.Nom}", animal.Adoptions);
                                }
                                break;
                            case 3:
                                {
                                    AccesConsole.CreerEcran($"HISTORIQUE DES DEMANDES - {animal.Nom}", animal.Sorties);
                                }
                                break;
                            case 4:
                                {
                                    AccesConsole.CreerEcran($"HISTORIQUE DES DEMANDES - {animal.Nom}", animal.Entrees);
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }

        public static void Interface_Abri()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir Les Abri" },
                        { "2", "Gerer un Abri" },
                        { "3", "Creer un Abri" },
                        { "4", "Delete un Abri" }
                    };

                    AccesConsole.CreerEcran("ABRIS", AllAbri.LesAbris, menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("ABRIS", AllAbri.LesAbris);
                                }
                                break;
                            case 2:
                                {
                                    Abri? abri = Questions.Abri();
                                    if (abri != null)
                                    {
                                        Interface_Abri_un(abri);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Role invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Abri_Creer();
                                }
                                break;
                            case 4:
                                {
                                    Abri? abri = Questions.Abri();
                                    if (abri != null)
                                    {
                                        Interface_Abri_Supprimer(abri);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Role invalide");
                                    }

                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Abri_Creer()
        {
            AccesConsole.CreerEcran("LES ABRIS - CREER");

            string? nom = AccesConsole.SaisirChaine("Libelle de l 'abri (Ex. A2): ");
            string? descr = AccesConsole.SaisirChaine("Description : ");


            if (!string.IsNullOrEmpty(nom) && nom.Length >= 2)
            {
                Abri abri = Abri.Creer(nom, descr);
                if (abri != null)
                {
                    Interface_Abri_Save(abri);
                }

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }
        }
        public static void Interface_Abri_Save(Abri abri)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ABRIS - ABRI CREE - {abri.Id}", abri.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Abri.Save(abri));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Abri_un(Abri abri)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"Abri - {abri.Libelle}", abri, menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Abri_Modifier(abri); ;
                                }
                                break;
                            case 2:
                                {
                                    Interface_Abri_Supprimer(abri);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Abri_Modifier(Abri abri)
        {
            AccesConsole.CreerEcran($"LES ABRIS - {abri.Libelle} - MODIFIER", abri.ToString());

            string? nom = AccesConsole.SaisirChaine("Nom de l Abri (Ex. A3): ");
            nom ??= abri.Libelle;

            string? description = AccesConsole.SaisirChaine("Description de l Abri (Ex. A3): ");
            description ??= abri.Description;

            EStatutAbri? statut = (EStatutAbri)Questions.StatutAbri();
            statut ??= abri.Statut;

            AccesConsole.Afficher(abri.Update(nom, statut, description));

        }
        public static void Interface_Abri_Supprimer(Abri abri)
        {
            bool choix = Questions.Delete(abri.Libelle);
            if (choix)
            {
                AccesConsole.Afficher(Abri.Delete(abri));
            }
        }
        public static void Interface_Animal_un_UpdateAbri(Animal ami)
        {

            AccesConsole.CreerEcran($"ANIMAL - {ami.Nom} - ASSIGNATION D'ABRI", AllAbri.LesAbris);

            Abri? abri = Questions.Abri();
            if (abri != null && abri.Statut == EStatutAbri.DISPONIBLE)
            {
                AccesConsole.Afficher(ami.AddAbri(abri));
            }
            else
            {
                AccesConsole.Info("Abri est invalide");
            }
        }

        public static void Interface_Animal_Type()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir les types d'animal" },
                        { "2", "Voir un type d'animal" },
                        { "3", "Creer un type d'animal" },
                    };

                    AccesConsole.CreerEcran("LES TYPES D'ANIMAUX", AllTypeAnimal.LesTypes, menu);
                    choix = Questions.Choix();

                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES TYPES D'ANIMAUX", AllTypeAnimal.LesTypes);
                                }
                                break;
                            case 2:
                                {

                                    TypeAnimal? type = Questions.TypeAnimal();
                                    if (type != null)
                                    {
                                        Interface_Animal_TypeUn(type);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("GetUnType animal invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Animal_TypeCreer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }

        }
        public static void Interface_Animal_TypeCreer()
        {
            AccesConsole.CreerEcran("LES TYPES D'ANIMAUX - CREATION DE TYPE", AllTypeAnimal.LesTypes);

            string? nom = AccesConsole.SaisirChaine("Nom : ");
            string? descr = AccesConsole.SaisirChaine("Description : ");

            TypeAnimal t = TypeAnimal.Creer(nom, descr);
            if (t != null)
            {
                Interface_TypeAnimal_Save(t);
            }
            else
            {
                AccesConsole.Info($"[Creation de motif] Echec : {nom}");
            }
        }
        public static void Interface_TypeAnimal_Save(TypeAnimal type)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES TYPES ANIMAL - TYPE CREE - {type.Id}", type.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(TypeAnimal.Save(type));
                        AccesConsole.Info($"[Creation de motif] Success : {type.Nom}");

                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Animal_TypeUn(TypeAnimal type)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES TYPES D'ANIMAUX - {type.Nom} ", type, menu);
                    choix = Questions.Choix();

                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Animal_TypeModifier(type);
                                }
                                break;
                            case 2:
                                {

                                    Interface_Animal_TypeSupp(type);
                                    choix = 99;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Animal_TypeModifier(TypeAnimal type)
        {
            AccesConsole.CreerEcran($"LES TYPES D'ANIMAUX - {type.Nom} - MODIFIER", type.ToString());

            if (type != null)
            {
                string? nom = AccesConsole.SaisirChaine("Nom : ");
                string? descr = AccesConsole.SaisirChaine("Description : ");

                AccesConsole.Afficher(type.Update(nom, descr));

            }

        }
        public static void Interface_Animal_TypeSupp(TypeAnimal type)
        {
            AccesConsole.CreerEcran($"LES TYPES - SUPPESSION - {type.Id} -", type);
            bool choix = Questions.Delete(type.Nom);
            if (choix)
            {
                AccesConsole.Afficher(TypeAnimal.Delete(type));
            }
        }

        public static void Interface_Couleur()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir les couleurs" },
                        { "2", "Gerer une couleur" },
                        { "3", "Creer une couleur" },
                    };

                    AccesConsole.CreerEcran("LES COULEURS", AllCouleur.LesDernieresCouleurs, menu);
                    choix = Questions.Choix();

                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES COULEURS - LISTE", AllCouleur.LesCouleurs);
                                }
                                break;
                            case 2:
                                {

                                    Couleur? couleur = Questions.Couleur();
                                    if (couleur != null)
                                    {
                                        Interface_Couleur_Un(couleur);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Couleur invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Couleur_Creer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Couleur_Creer()
        {
            AccesConsole.CreerEcran("LES COULEURS - CREATION DE COULEUR", AllCouleur.LesDernieresCouleurs);

            string? nom = AccesConsole.SaisirChaine("Nom : ");

            Couleur t = Couleur.Creer(nom);
            if (t != null)
            {
                Interface_Couleur_Save(t);
            }
            else
            {
                AccesConsole.Info($"[Interface Creation Couleur] Echec : {nom}");
            }
        }
        public static void Interface_Couleur_Save(Couleur couleur)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES COULEURS - COULEUR CREE - {couleur.Id}", couleur.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Couleur.Save(couleur));
                        AccesConsole.Info($"[Interface Creation Couleur] Success : {couleur.Nom}");
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Couleur_Un(Couleur couleur)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES COULEURS - {couleur.Nom} ", couleur, menu);
                    choix = Questions.Choix();

                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Couleur_Modifier(couleur);
                                }
                                break;
                            case 2:
                                {

                                    Interface_Couleur_Supp(couleur);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Couleur_Modifier(Couleur couleur)
        {
            AccesConsole.CreerEcran($"LES COULEURS - MODIFICATION DE COULEUR - {couleur.Id} -", couleur.ToString());

            if (couleur != null)
            {
                string? nom = AccesConsole.SaisirChaine("Nom : ");
                AccesConsole.Afficher(couleur.Update(nom).ToString());
            }
        }
        public static void Interface_Couleur_Supp(Couleur couleur)
        {
            AccesConsole.CreerEcran($"LES COULEURS - SUPPESSION - {couleur.Id} -", couleur.ToString());
            bool choix = Questions.Delete(couleur.Nom);
            if (choix)
            {
                AccesConsole.Afficher(AllCouleur.Delete(couleur.Id));
            }
        }

        public static void Interface_Coloration()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les colorations" },
                        { "2", "Gerer une coloration" },
                        { "3", "Ajouter une coloration a un animal" },
                        { "4", "Delete une coloration" }
                    };

                    AccesConsole.CreerEcran($"LES COLORATIONS", AllAnimalCouleur.DerniersListe, menu);

                    choix = Questions.Choix();
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran($"LES COLORATIONS", AllAnimalCouleur.Liste);
                                }
                                break;
                            case 2:
                                {
                                    AnimalCouleur coloration = Questions.AnimalCouleur();
                                    if (coloration != null)
                                    {
                                        Interface_Coloration_Un(coloration);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cette coloration n 'existe pas");
                                    }
                                }
                                break;
                            case 3:
                                {
                                    Interface_Coloration_Add();
                                }
                                break;
                            case 4:
                                {
                                    AnimalCouleur? coloration = Questions.AnimalCouleur();
                                    if (coloration != null)
                                    {
                                        Interface_Coloration_Delete(coloration);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cette coloration n 'existe pas");
                                    }
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        AccesConsole.EnCouleur($"[Vaccination Interface] Erreur : {ex.Message}", "Vaccination Interface", ConsoleColor.Red);
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }

        }
        public static void Interface_Coloration_Add()
        {
            AccesConsole.CreerEcran($"LES COLORATIONS", AllAnimal.LesAnimaux);
            Animal animal = Questions.Animal();

            Interface_Coloration_Add(animal);
        }
        public static void Interface_Coloration_Add(Animal animal)
        {
            AccesConsole.CreerEcran($"LES COLORATIONS - {animal.Nom} -", AllCouleur.Manquants(animal));
            Couleur couleur = Questions.Couleur();

            AnimalCouleur an = AnimalCouleur.Creer(animal, couleur);
            if (an != null)
            {
                Interface_Coloration_Save(an);
            }
        }
        public static void Interface_Coloration_Save(AnimalCouleur animalCouleur)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES COLORATIONS - COLORATION CREE - {animalCouleur.Id}", animalCouleur.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(AnimalCouleur.Save(animalCouleur));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Coloration_Un(AnimalCouleur an)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Delete cette coloration" }
                    };

                    AccesConsole.CreerEcran($"LES COLORATIONS - LES COLORATIONS DE {an.Animal.Nom} -", an, menu);

                    choix = AccesConsole.SaisirInt("Choix : ");
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Coloration_Delete(an);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        Console.WriteLine($"[Coloration Interface] Erreur : {ex.Message}");
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }
        }
        public static void Interface_Coloration_Animal(Animal animal)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Ajouter une coloration" },
                        { "2", "Delete cette coloration" }
                    };

                    AccesConsole.CreerEcran($"LES COLORATIONS - LES COLORATIONS DE {animal.Nom} -", animal.Couleurs, menu);

                    choix = AccesConsole.SaisirInt("Choix : ");
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Coloration_Add(animal);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Coloration_Delete(animal);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        Console.WriteLine($"[Coloration Interface] Erreur : {ex.Message}");
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }
        }
        public static void Interface_Coloration_Delete(Animal animal)
        {
            AccesConsole.CreerEcran($"LES COLORATIONS - {animal.Nom} -", animal.Couleurs);
            Couleur? couleur = Questions.Couleur();
            if (couleur != null)
            {
                AnimalCouleur? coloration = AllAnimalCouleur.Find(animal.Id + couleur.Id);
                Interface_Coloration_Delete(coloration);
            }
            else
            {
                AccesConsole.Erreur("[Interface coloration Delete] Cette couleur n existe pas");
            }
        }
        public static void Interface_Coloration_Delete(AnimalCouleur coloration)
        {
            if (coloration != null)
            {
                bool choix = Questions.Delete(coloration.Id);
                if (choix)
                {
                    AccesConsole.Afficher(coloration.Delete());
                }
            }
            else
            {
                AccesConsole.Erreur("[Interface coloration Delete] Cet animal n a pas cette couleur");
            }
        }

        public static void Interface_Vaccins()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir Les Vaccins" },
                        { "2", "Gerer un Vaccin" },
                        { "3", "Creer un Vaccin" }
                    };

                    AccesConsole.CreerEcran("LES VACCINS", AllVaccin.LesVaccins, menu);

                    Console.WriteLine();


                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES VACCINS - LISTE", AllVaccin.LesVaccins);
                                }
                                break;
                            case 2:
                                {
                                    Vaccin? vaccin = Questions.Vaccin();
                                    if (vaccin != null)
                                    {
                                        Interface_Vaccins_Un(vaccin);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Vaccin invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Vaccins_Creer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception e)
                {
                    AccesConsole.Afficher(e.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Vaccins_Creer()
        {
            AccesConsole.CreerEcran("Les VACCINS - CREATION DE VACCIN", AllVaccin.LesVaccins);

            string? nom = AccesConsole.SaisirChaine("Nom (Ex. rage): ");
            string? description = AccesConsole.SaisirChaine("Description  (Ex. Malaldie...): ");


            if (!string.IsNullOrEmpty(nom) && nom.Length >= 2)
            {
                Vaccin vaccin = Vaccin.Creer(nom, description);
                if (vaccin == null)
                {
                    ExceptionLauncher.New("Interface vaccin Creer", "Creation echouée");
                }
                Interface_Vaccin_Save(vaccin);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }

        }
        public static void Interface_Vaccin_Save(Vaccin vaccin)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES VACCINS - VACCIN CREE - {vaccin.Id}", vaccin.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Vaccin.Save(vaccin));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Vaccins_Un(Vaccin vaccin)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Update les données" },
                        { "2", "supprimer cet vaccin" }
                    };

                    AccesConsole.CreerEcran($"LES VACCINS - {vaccin.Nom}", vaccin, menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Vaccin_Modifier(vaccin); ;
                                }
                                break;
                            case 2:
                                {
                                    Interface_Vaccin_Supprimer(vaccin);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Vaccin_Modifier(Vaccin vaccin)
        {
            AccesConsole.CreerEcran($"LES VACCINS - {vaccin.Nom} - MODIFICATION", vaccin.ToString());

            string? nom = AccesConsole.SaisirChaine("Nom (Ex. rage): ");
            nom ??= vaccin.Nom;

            string? description = AccesConsole.SaisirChaine("Description  (Ex. Malaldie...): ");
            description ??= vaccin.Description;

            AccesConsole.Afficher(vaccin.Modifier(nom, description));
        }
        public static void Interface_Vaccin_Supprimer(Vaccin vaccin)
        {
            bool choix = Questions.Delete(vaccin.Nom);
            if (choix)
            {
                AccesConsole.Afficher(Vaccin.Delete(vaccin));
            }
        }

        public static void Interface_Vaccinantion()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les vaccinations" },
                        { "2", "Gerer une vaccinations" },
                        { "3", "Ajouter une vaccination" },
                        { "4", "Delete une vaccination" }
                    };

                    AccesConsole.CreerEcran($"LES VACCINATIONS", AllVaccination.LesDernierVaccinantions, menu);

                    choix = Questions.Choix();
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran($"LES VACCINATIONS", AllVaccination.LesVaccinantions);
                                }
                                break;
                            case 2:
                                {
                                    Vaccination vaccination = Questions.Vaccination();
                                    if (vaccination != null)
                                    {
                                        Interface_Animal_un_ShowVaccinantion(vaccination.Animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cette vaccination n 'existe pas");
                                    }
                                }
                                break;
                            case 3:
                                {
                                    Animal animal = Questions.Animal();
                                    if (animal != null)
                                    {
                                        Interface_Animal_un_AddVaccination(animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cet animal n 'existe pas");
                                    }
                                }
                                break;
                            case 4:
                                {
                                    Vaccination vaccination = Questions.Vaccination();
                                    if (vaccination != null)
                                    {
                                        Interface_Animal_un_DeleteVaccination(vaccination.Animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cette vaccination n 'existe pas");
                                    }
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        AccesConsole.EnCouleur($"[Vaccination Interface] Erreur : {ex.Message}", "Vaccination Interface", ConsoleColor.Red);
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }

        }
        public static void Interface_Animal_un_AddVaccination(Animal ami)
        {
            AccesConsole.CreerEcran($"LES VACCINATIONS - {ami.Nom} - FAIRE UNE VACCINATION", AllVaccin.Manquants(ami));

            Vaccin? vaccin = Questions.Vaccin();
            string? remarque = AccesConsole.SaisirChaine("Ajouter une remarque: ");

            if (vaccin != null)
            {
                Vaccination vaccination = Vaccination.Creer(ami, vaccin, remarque);
                if (vaccination != null)
                {
                    Interface_Vaccination_Save(vaccination);
                }

            }
            else
            {
                AccesConsole.Erreur($"Vaccin non ajouté à {ami.Nom}");
            }
        }
        public static void Interface_Vaccination_Save(Vaccination vaccination)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES VACCINATIONS - VACCINATION CREE - {vaccination.Id}", vaccination.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Vaccination.Save(vaccination));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Animal_un_ShowVaccinantion(Animal ami)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Ajouter un Vaccin" },
                        { "2", "Delete un Vaccin" },
                    };

                    AccesConsole.CreerEcran($"LES VACCINATIONS - LES VACCINATION DE {ami.Nom} -", ami.Vaccinations, menu);

                    choix = AccesConsole.SaisirInt("Choix : ");
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Animal_un_AddVaccination(ami);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Animal_un_DeleteVaccination(ami);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        Console.WriteLine($"[Animal Vaccin Interface] Erreur : {ex.Message}");
                        choix = 0;
                        AccesConsole.Attendre();
                    }

                }
            }

        }
        public static void Interface_Animal_un_DeleteVaccination(Animal animal)
        {
            AccesConsole.CreerEcran($"LES VACCINATIONS - {animal.Nom} - SUPPRESSION", animal.Vaccinations);
            if (Vaccination.ByAnimal(animal).Count > 0)
            {

                Vaccination vaccination = Questions.Vaccination();
                if (vaccination != null && Questions.Delete(vaccination.Vaccin.Nom))
                {
                    AccesConsole.Afficher(Vaccination.Delete(vaccination));
                }
                else
                {
                    AccesConsole.Afficher("Operation annulée");
                }

            }
            else
            {

                AccesConsole.Afficher($"** {animal.Nom} ** n'a pas de Vaccination ");
            }
        }

        public static void Interface_Compatibilite()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous Les Compatibilités" },
                        { "2", "Gerer une Compatibilité" },
                        { "3", "Creer une Compatibilité" }
                    };

                    AccesConsole.CreerEcran("LES COMPATIBILITES", AllCompatibilite.LesCompatibilites, menu);

                    Console.WriteLine();

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES COMPATIBILITES - LISTES", AllCompatibilite.LesCompatibilites);
                                }
                                break;
                            case 2:
                                {
                                    Compatibilite? compatibilite = Questions.Compatibilite();
                                    if (compatibilite != null)
                                    {
                                        Interface_Compatibilite_Un(compatibilite);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("compatibilité invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Compatibilite_Creer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception e)
                {
                    AccesConsole.Afficher(e.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Compatibilite_Creer()
        {
            AccesConsole.CreerEcran("LES COMPATIBILITES - CREER");

            string? nom = AccesConsole.SaisirChaine("Nom (Ex. jeune): ");
            string? description = AccesConsole.SaisirChaine("Description  (Ex. Malaldie...): ");

            if (!string.IsNullOrEmpty(nom) && nom.Length >= 2)
            {
                Compatibilite compatibilite = Compatibilite.Creer(nom, description);
                if (compatibilite == null)
                {
                    ExceptionLauncher.New("Interface Compatibilité Creer", "Creation echouée");
                }
                Interface_Compatibilite_Save(compatibilite);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }
            AccesConsole.Attendre();
        }
        public static void Interface_Compatibilite_Save(Compatibilite compatibilite)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES COMPATIBILITES - COMPATIBILITE CREE - {compatibilite.Id}", compatibilite.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Compatibilite.Save(compatibilite));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Compatibilite_Un(Compatibilite compatibilite)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Update les données" },
                        { "2", "Delete cette compatibilité" }
                    };

                    AccesConsole.CreerEcran($"LES COMPATIBILITES - {compatibilite.Nom}", compatibilite.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Compatibilites_Modifier(compatibilite); ;
                                }
                                break;
                            case 2:
                                {
                                    Interface_Compatibilites_Supprimer(compatibilite);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Compatibilites_Modifier(Compatibilite compatibilite)
        {
            AccesConsole.CreerEcran($"LES COMPATIBILITES - {compatibilite.Nom} - MODIFICATION", compatibilite.ToString());

            string? nom = AccesConsole.SaisirChaine("Nom (Ex. jeune): ");
            nom ??= compatibilite.Nom;

            string? description = AccesConsole.SaisirChaine("Description  (Ex. ...): ");
            description ??= compatibilite.Details;

            AccesConsole.Afficher(compatibilite.Update(nom, description));

        }
        public static void Interface_Compatibilites_Supprimer(Compatibilite compatibilite)
        {
            bool choix = Questions.Delete(compatibilite.Nom);
            if (choix)
            {
                AccesConsole.Afficher(Compatibilite.Delete(compatibilite));
            }
        }

        public static void Interface_Compatibilite_Testes()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les compatibilités testés" },
                        { "2", "Gerer une compatibilités testés" },
                        { "3", "Ajouter une compatibilités testés" },
                        { "4", "Delete une compatibilités testés" }
                    };

                    AccesConsole.CreerEcran($"LES COMPATIBILITES TESTÉS", AnimalCompatibilitéService.LesDerniersCompatibilites, menu);

                    choix = Questions.Choix();
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran($"LES COMPATIBILITES TESTÉS - LISTES", AnimalCompatibilitéService.LesCompatibilites);
                                    AccesConsole.Attendre();
                                }
                                break;
                            case 2:
                                {
                                    AnimalCompatibilité test = Questions.AnimalCompatibilité();
                                    if (test != null)
                                    {
                                        Interface_Animal_un_ShowCompatibilite(test.Animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cet test de compatibilité n 'existe pas");
                                    }
                                }
                                break;
                            case 3:
                                {
                                    Animal animal = Questions.Animal();
                                    if (animal != null)
                                    {
                                        Interface_Animal_un_AddCompatibilite(animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cet test de compatibilité n 'existe pas");
                                    }
                                }
                                break;
                            case 4:
                                {
                                    Vaccination vaccination = Questions.Vaccination();
                                    if (vaccination != null)
                                    {
                                        Interface_Animal_un_SuppCompatibilite(vaccination.Animal);
                                    }
                                    else
                                    {
                                        AccesConsole.Erreur("Cette vaccination n 'existe pas");
                                    }
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        AccesConsole.EnCouleur($"[Vaccination Interface] Erreur : {ex.Message}", "Vaccination Interface", ConsoleColor.Red);
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }

        }
        public static void Interface_Animal_un_AddCompatibilite(Animal ami)
        {

            AccesConsole.CreerEcran($"ANIMAL COMPATIBILITES - {ami.Nom} - AJOUTER COMPATIBILITES", AllCompatibilite.Manquants(ami));

            Compatibilite? comp = Questions.Compatibilite();
            bool valeur = AccesConsole.SaisirBoolean("Compatible [O/N]: ");
            string? remarque = Questions.Infos();

            if (comp != null)
            {
                AnimalCompatibilité cop = AnimalCompatibilité.Creer(ami, comp, valeur, remarque);
                if (cop != null)
                {
                    Interface_TestCompatibilite_Save(cop);
                }

            }
            else
            {
                Console.WriteLine($"[Erreur] Compatibilité non ajouté à {ami.Nom}");
            }
        }
        public static void Interface_TestCompatibilite_Save(AnimalCompatibilité test)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES TESTS COMPATIBILITES - TEST COMPATIBILITE CREE - {test.Id}", test.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(AnimalCompatibilité.Save(test));
                        Console.WriteLine($"Compatibilité Ajouter à {test.Animal.Nom} !!!");

                        choix = 99;

                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Animal_un_ShowCompatibilite(Animal ami)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? bloque = null;
                    if (ami.Statut != EStatutAnimal.REFUGE)
                    {
                        bloque = $"[Bloqué] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{bloque}Ajouter une Compatibilité" },
                        { "2", "Supprimer une Compatibilité" }
                    };

                    AccesConsole.CreerEcran($"ANIMAL COMPATIBILITES - {ami.Nom} - COMPATIBILITÉ", ami.Compatibilites, menu);

                    choix = Questions.Choix();
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (ami.Statut != EStatutAnimal.REFUGE)
                                    {
                                        ExceptionLauncher.New("Add Compatibilité", "L'animal n'est pas au refuge");
                                    }
                                    Interface_Animal_un_AddCompatibilite(ami);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Animal_un_SuppCompatibilite(ami);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    {
                        Console.WriteLine($"[Animal Compatibilité Interface] Erreur : {ex.Message}");
                        choix = 0;
                        AccesConsole.Attendre();
                    }
                }
            }

        }
        public static void Interface_Animal_un_SuppCompatibilite(Animal ami)
        {

            if (AnimalCompatibilitéService.FindAllByAnimal(ami).Count > 0)
            {
                Compatibilite? comp = Questions.Compatibilite();
                AnimalCompatibilité anicomp = AnimalCompatibilitéService.Find(ami, comp);
                if (anicomp != null)
                {
                    bool choix = Questions.Delete(comp.Nom);
                    if (choix)
                    {
                        AccesConsole.Afficher(AnimalCompatibilité.Delete(anicomp));
                        AccesConsole.Attendre();
                    }
                }
                else
                {
                    AccesConsole.Info("Cette compatibilité n'est pas valide");
                }

            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"** {ami.Nom} ** n' a pas de Compatibilité ");
            }
        }

        //---------------------------------------------------------contacts

        public static void Interface_Contacts()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {

                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous Les Contacts" },
                        { "2", "Gerer un Contact" },
                        { "3", "Creer un Contact" },
                        { "4", "Les Roles" }
                    };

                    AccesConsole.CreerEcran("LES CONTACTS", AllContacts.LesContacts, menu);

                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix >= 1)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Contact_All();
                                }
                                break;

                            case 2:
                                {
                                    Contact? contacts = Questions.Contact();
                                    if (contacts != null)
                                    {
                                        Interface_Contact_Un(contacts);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Contact invalide");
                                    }

                                }
                                break;

                            case 3:
                                {
                                    Interface_Contact_Creer();
                                }
                                break;

                            case 4:
                                {
                                    Interface_Contact_Roles();


                                }
                                break;

                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Contacte Interface] Erreur : {ex.Message}");
                    choix = 0;
                    AccesConsole.Attendre();

                }

            }
        }
        public static void Interface_Contact_All()
        {

            AccesConsole.CreerEcran("Tous les Contacts", AllContacts.LesContacts);

        }
        public static Contact? Interface_Contact_Creer()
        {
            Contact? new_contact = null;
            AccesConsole.CreerEcran("LES CONTACTS - CREATION", AllTypeContact.LesTypesContacts);

            string? nom = AccesConsole.SaisirChaine("Nom : ");
            string? prenom = AccesConsole.SaisirChaine("Prenom : ");
            string? niss = AccesConsole.SaisirChaine("Niss : ");
            DateTime dtenais = (DateTime)AccesConsole.SaisirDate("Date de naissance: ");
            string? gsm = AccesConsole.SaisirChaine("Gsm : ");
            string? tel = AccesConsole.SaisirChaine("Telephone : ");
            string? email = AccesConsole.SaisirChaine("Email : ");
            string? localite = AccesConsole.SaisirChaine("Localité : ");
            string? cp = AccesConsole.SaisirChaine("Code postale : ");
            string? adresse = AccesConsole.SaisirChaine("Adresse : ");

            if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(gsm) && nom.Length > 2)
            {
                new_contact = Contact.Creer(niss, dtenais, nom, prenom, gsm, tel, email, cp, localite, adresse);

                if (new_contact == null)
                {
                    ExceptionLauncher.New("Interface Contact Creer", "Creation echouée");
                }
                Interface_Contact_Save(new_contact);
            }
            else
            {
                AccesConsole.Info("Parametre Invalide");
            }
            return new_contact;
        }
        public static void Interface_Contact_Save(Contact contact)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES CONTACTS - CONTACT CREE - {contact.Id}", contact.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Afficher(Contact.Save(contact));
                                    choix = 99;
                                }
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Contact_Un(Contact contacts)
        {
            if (contacts != null)
            {
                int? choix = 0;
                while (choix != 99)
                {
                    try
                    {
                        Dictionary<string, string> menu = new()
                        {
                            { "1", "Modifier" },
                            { "2", "Supprimer" },
                            { "3", "Gerer les roles" },
                            { "4", "Voir les Demandes" }
                        };

                        AccesConsole.CreerEcran($"Contacts: {contacts.Nom}", contacts.ToString(), menu);

                        choix = AccesConsole.SaisirInt("Entrer votre choix ici : ");

                        if (choix > 0 && choix <= menu.Count)
                        {
                            switch (choix)
                            {
                                case 1:
                                    {
                                        Interface_Contact_Un_Modification(contacts);
                                    }
                                    break;
                                case 2:
                                    {

                                        bool validation = Questions.Delete(contacts.Nom);
                                        if (validation)
                                        {
                                            AccesConsole.Afficher(Contact.Delete(contacts));
                                            choix = 99;
                                        }

                                    }
                                    break;
                                case 3:
                                    {
                                        Interface_Contact_Un_AllRoles(contacts);
                                    }
                                    break;
                                case 4:
                                    {
                                        Interface_Contact_Un_AllDemandes(contacts);
                                    }
                                    break;
                            }
                            AccesConsole.Attendre();
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Contact Interface Un] Erreur : {ex.Message}");
                        choix = 0;
                        AccesConsole.Attendre();
                    }


                }
            }

        }
        public static void Interface_Contact_Un_Modification(Contact contact)
        {

            AccesConsole.CreerEcran($"LES CONTACTS - {contact.Nom} - MODIFICATION", contact.ToString());

            AccesConsole.Info("Faire ** Enter ** si vous ne souhaitez pas modifier une ms\n");

            string? nom = AccesConsole.SaisirChaine("Nom: ");

            string? pnom = AccesConsole.SaisirChaine("Prenom: ");

            string? niss = AccesConsole.SaisirChaine("niss: ");

            DateTime? dnais = AccesConsole.SaisirDate("Date de naiss.(ex. 10-02-2012): ");

            string? gsm = AccesConsole.SaisirChaine("Gsm: ");

            string? tel = AccesConsole.SaisirChaine("tel: ");

            string? mail = AccesConsole.SaisirChaine("Mail: ");

            string? cp = AccesConsole.SaisirChaine("Code postal: ");
            string? localite = AccesConsole.SaisirChaine("Localité: ");
            string? adresse = AccesConsole.SaisirChaine("Adresse: ");

            AccesConsole.Afficher(contact.Modification(niss, dnais, nom, pnom, gsm, tel, mail, cp, localite, adresse));

        }
        public static void Interface_Contact_Un_AllDemandes(Contact contacts)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir les demande En Cours" },
                        { "2", "Creer une nouvelle Demande" },
                        { "3", "Modifier un Demande" },
                        { "4", "Surpprimer une Demande" }
                    };

                    AccesConsole.CreerEcran($"LES CONTACTS - {contacts.Nom} -- DEMANDES", contacts.ListeDemandes, menu);

                    choix = Questions.Choix();
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran($"LES CONTACTS - {contacts.Nom} - Demandes en cours ...", contacts.ListeDemandesByStatut(EStatutDemande.EN_COURS));
                                }
                                break;
                            case 2:
                                {
                                    Interface_Demande_Creer();
                                }
                                break;
                            case 3:
                                {
                                    Demande? demande = Questions.Demande();
                                    if (demande != null && demande.Contact == contacts)
                                    {
                                        Interface_Demande_Modifier(demande);
                                    }

                                }
                                break;
                            case 4:
                                {

                                    Demande? dm = Questions.Demande();
                                    if (dm != null)
                                    {

                                        Interface_Demande_Supprimer(dm);

                                    }
                                    else
                                    {
                                        Console.WriteLine("Suppression Refusée");
                                    }
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }

        }

        public static void Interface_Contact_Roles()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les Role" },
                        { "2", "Gerer un Role" },
                        { "3", "Creer un Roles" }
                    };

                    AccesConsole.CreerEcran("LES ROLES", AllTypeContact.LesTypesContacts, menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {

                            case 1:
                                {
                                    AccesConsole.CreerEcran("TOUS LES ROLES", AllTypeContact.LesTypesContacts, menu);
                                }
                                break;
                            case 2:
                                {
                                    TypeContact? role = Questions.TypeContact();
                                    if (role != null)
                                    {
                                        Interface_Contact_Roles_un(role);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Role invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Contact_Roles_Creer();
                                }
                                break;

                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }

        }
        public static void Interface_Contact_Roles_Creer()
        {
            AccesConsole.CreerEcran("LES ROLES - CREER");

            string? nom = AccesConsole.SaisirChaine("Libelle du type de Contact (Ex. Benevole): ");
            string? descr = AccesConsole.SaisirChaine("Description du role: ");

            if (nom != null && nom.Length > 3)
            {
                TypeContact type = TypeContact.Creer(nom, descr);
                if (type == null)
                {
                    ExceptionLauncher.New("Interface role contact Creer", "Creation echouée");
                }
                Interface_Contact_Roles_Save(type);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }

        }
        public static void Interface_Contact_Roles_Save(TypeContact type)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ROLES - ROLE CREE - {type.Id}", type.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Afficher(TypeContact.Save(type));
                                    choix = 99;
                                }
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Contact_Roles_un(TypeContact role)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES ROLES - {role.Nom}", role.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Contact_Roles_Modifier(role);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Contact_Roles_Supprimer(role);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Contact_Roles_Modifier(TypeContact role)
        {
            AccesConsole.CreerEcran($"LES ROLES - {role.Nom} - MODIFIER", role.ToString());

            string? nom = AccesConsole.SaisirChaine("Libelle du MyType de Contact (Ex. Benevole): ");
            nom = string.IsNullOrEmpty(nom) ? role.Nom : nom;

            string? descr = AccesConsole.SaisirChaine("Description du role: ");
            descr = string.IsNullOrEmpty(descr) ? role.Description : descr;

            AccesConsole.Afficher(role.Update(nom, descr));

            AccesConsole.Info("Role modifié ..");
        }
        public static void Interface_Contact_Roles_Supprimer(TypeContact role)
        {
            bool choix = Questions.Delete(role.Nom);
            if (choix)
            {
                TypeContact.Delete(role);
                AccesConsole.Info("Role contact supprimé...");
            }
        }

        public static void Interface_Contact_Un_AllRoles(Contact contacts)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? bloque = null;

                    if (contacts.TypeCount <= 0)
                    {
                        bloque = "[Bloqué] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{bloque}Voir un Role" },
                        { "2", "Ajouter un Role" },
                        { "3", "Delete un Role" }
                    };

                    AccesConsole.CreerEcran($"LES CONTACTS - {contacts.Nom} - Roles", contacts.LesTypes, menu);

                    choix = AccesConsole.SaisirInt("Choix : ");
                    if (choix > 0 && choix <= menu.Count)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (contacts.TypeCount > 0)
                                    {
                                        TypeContact? role = Questions.TypeContact();
                                        AccesConsole.CreerEcran($"LES CONTACTS - {contacts.Nom} -- Roles - {role.Nom}", role.ToString());
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Cet contact ne possede pas encore de role");
                                    }

                                }
                                break;
                            case 2:
                                {


                                    Interface_Contact_Un_AddRoles(contacts);

                                }
                                break;
                            case 3:
                                {
                                    Interface_Contact_Un_DeleteRoles(contacts);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }
            }
        }
        public static void Interface_Contact_Un_AddRoles(Contact contact)
        {
            AccesConsole.CreerEcran($"LES ROLES AJOUTES - {contact.Nom} - Ajouter Roles ", contact.LesTypesManquants);
            TypeContact? role = Questions.TypeContact();
            if (role != null)
            {
                TypeContact_Contact contact_role = contact.AddType(role);
                if (contact_role != null)
                {
                    Interface_AddContact_Roles_Save(contact_role);
                }

            }
            else
            {
                AccesConsole.Afficher("Role invalide");
            }
        }
        public static void Interface_AddContact_Roles_Save(TypeContact_Contact contact_role)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ROLES AJOUTES - AJOUT ROLE CREE - {contact_role.Id}", contact_role.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Afficher(TypeContact_Contact.Save(contact_role));
                                    choix = 99;
                                }
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Contact_Un_DeleteRoles(Contact contact)
        {
            TypeContact? type = Questions.TypeContact();
            if (type != null && contact.GetUnType(type) != null)
            {
                contact.RemoveType(type);
            }
            else
            {
                AccesConsole.Info("MyType invalide");
            }
        }

        //---------------------------------------------------------entrees

        public static void Interface_Entrees()
        {
            int? choix = 0;
            while (choix != 99)
            {
                Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les entrees" },
                        { "2", "Gerer une entrees" },
                        { "3", "Creer une entrees" },
                        { "4", "Voir tous les motifs" }
                    };

                AccesConsole.CreerEcran("LES ENTREES", AllEntree.LesEntrees, menu);

                Console.WriteLine();

                choix = Questions.Choix();

                if (choix != 99 && choix <= 6 && choix >= 1)
                {
                    switch (choix)
                    {
                        case 1:
                            {
                                AccesConsole.CreerEcran("LES ENTREES - LES LISTES", AllEntree.LesEntrees);
                            }
                            break;
                        case 2:
                            {
                                Entree? entree = Questions.Entree();
                                if (entree != null)
                                {
                                    Interface_Entrees_Un(entree);
                                }
                                else
                                {
                                    AccesConsole.Info($"Entree invalide");
                                }

                            }
                            break;
                        case 3:
                            {
                                Interface_Entrees_Creer();
                            }
                            break;
                        case 4:
                            {
                                Interface_Motifs_Entree();
                            }
                            break;
                    }
                }

            }
        }
        public static void Interface_Entrees_Creer()
        {
            AccesConsole.CreerEcran("LES ENTREES - CREER", AllMotifsEntrees.LesEntrees);

            Demande? dm = Questions.Demande();
            Interface_Entrees_Creer(dm);

        }
        public static void Interface_Entrees_Creer(Demande demande)
        {
            AccesConsole.CreerEcran("LES ENTREES - CREER", AllMotifsEntrees.LesEntrees);

            MotifEntree? moe = Questions.MotifEntree();
            string? detail = Questions.Infos();

            if (moe != null)
            {
                Entree entree = Entree.Creer(demande, moe, detail);
                if (entree == null)
                {
                    ExceptionLauncher.New("Interface Entree Creer", "Creation echouée");
                }
                Interface_Entrees_Save(entree);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }

        }
        public static void Interface_Entrees_Save(Entree entree)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ENTREES - ENTREE CREE - {entree.Id}", entree.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Entree.Save(entree));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Entrees_Un(Entree entree)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? mod = null;
                    if (entree.Demande.Statut != EStatutDemande.EN_COURS)
                    {
                        mod = $"[{entree.Demande.Statut}] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{mod}Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES ENTREES - {entree.Id}", entree.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (entree.Demande.Statut > EStatutDemande.EN_COURS)
                                    {
                                        ExceptionLauncher.New("LES ENTREES", "La demande est terminée");
                                    }
                                    Interface_Entrees_Un_modifier(entree);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Entrees_Un_supprimer(entree);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Entrees_Un_modifier(Entree entree)
        {
            AccesConsole.CreerEcran($"LES ENTREES - {entree.Id} - MODIFIER", entree.ToString());

            MotifEntree? motif = Questions.MotifEntree();
            motif ??= entree.Motifs;

            string? detail = AccesConsole.SaisirChaine($"Details : ");

            AccesConsole.Afficher(entree.Update(motif, detail));

            AccesConsole.Attendre();
        }
        public static void Interface_Entrees_Un_supprimer(Entree entree)
        {
            bool validation = Questions.Delete(entree.Id);
            if (validation)
            {
                AccesConsole.Afficher(Entree.Delete(entree));
            }
            else
            {
                AccesConsole.Info("MyType invalide");
            }
            AccesConsole.Attendre();
        }

        public static void Interface_Motifs_Entree()
        {
            int? choix = 0;
            while (choix != 99)
            {
                Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les motifs" },
                        { "2", "Gerer un motif" },
                        { "3", "Creer un motif" },
                    };

                AccesConsole.CreerEcran("LES MOTIFS D' ENTREES", AllMotifsEntrees.LesEntrees, menu);

                Console.WriteLine();

                choix = Questions.Choix();

                if (choix != 99 && choix <= 6 && choix >= 1)
                {
                    switch (choix)
                    {
                        case 1:
                            {
                                AccesConsole.CreerEcran("LES MOTIFS D' ENTREES - LA LISTES", AllMotifsEntrees.LesEntrees);
                            }
                            break;
                        case 2:
                            {
                                MotifEntree? entree = Questions.MotifEntree();
                                if (entree != null)
                                {
                                    Interface_Motifs_Entree_Un(entree);
                                }
                                else
                                {
                                    AccesConsole.Info($"Entree invalide");
                                }

                            }
                            break;
                        case 3:
                            {
                                Interface_Motifs_Entree_Creer();
                            }
                            break;
                    }
                }

            }
        }
        public static void Interface_Motifs_Entree_Creer()
        {
            AccesConsole.CreerEcran("LES MOTIFS D'ENTREE - CREER");

            string? nom = AccesConsole.SaisirChaine("Libelle du motif (Ex. Errant): ");
            string? descr = AccesConsole.SaisirChaine("Description du motif: ");

            if (nom != null && nom.Length > 3)
            {
                MotifEntree motif = MotifEntree.Creer(nom, descr);
                if (motif == null)
                {
                    ExceptionLauncher.New("Interface Motif sortie Creer", "Creation echouée");
                }
                Interface_MotifsEntrees_Save(motif);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }
        }
        public static void Interface_MotifsEntrees_Save(MotifEntree motif)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES MOTIFS D'ENTREE - MOTIF CREE - {motif.Id}", motif.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(MotifEntree.Save(motif));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Motifs_Entree_Un(MotifEntree motif)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES MOTIFS D' ENTREES - {motif.Id}", motif.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Motifs_Entree_Update(motif);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Motifs_Entree_Delete(motif);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Motifs_Entree_Update(MotifEntree motif)
        {
            AccesConsole.CreerEcran($"LES MOTIFS D' ENTREES - {motif.Id} - MODIFIER", motif.ToString());

            string? libelle = AccesConsole.SaisirChaine("Libellé : ");
            libelle ??= motif.Libele;

            string? detail = Questions.Infos();
            detail ??= motif.Details;

            AccesConsole.Afficher(motif.Update(libelle, detail));

            AccesConsole.Attendre();
        }
        public static void Interface_Motifs_Entree_Delete(MotifEntree motif)
        {
            bool validation = Questions.Delete(motif.Id);
            if (validation)
            {
                AccesConsole.Afficher(MotifEntree.Delete(motif));
            }
            else
            {
                AccesConsole.Info("MyType invalide");
            }
            AccesConsole.Attendre();
        }

        //---------------------------------------------------------adoption
        public static void Interface_Adoption()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir Les Adoptions" },
                        { "2", "Gerer une Adoption" },
                        { "3", "Creer une Adoption" }
                    };

                    AccesConsole.CreerEcran("LES ADOPTIONS", AllAdoption.Listes, menu);

                    Console.WriteLine();

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES ADOPTIONS - Listes", AllAdoption.Listes);
                                }
                                break;
                            case 2:
                                {
                                    Adoption? ado = Questions.Adoption();
                                    if (ado != null)
                                    {
                                        Interface_Adoption_Un(ado);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Adoption invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Adoption_Creer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception e)
                {
                    AccesConsole.EnCouleur("\n" + e.Message, ConsoleColor.Yellow);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static Adoption Interface_Adoption_Creer()
        {

            AccesConsole.CreerEcran("LES ADOPTIONS - Creer", AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ADOPTION));
            Demande? dm = Questions.Demande();

            return Interface_Adoption_Creer(dm); ;
        }
        public static Adoption Interface_Adoption_Creer(Demande demande)
        {
            Adoption? ado = null;

            AccesConsole.CreerEcran("LES ADOPTIONS - Creer", AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ADOPTION));
            string? info = AccesConsole.SaisirChaine("Info: ");

            if (demande != null && demande.Statut == EStatutDemande.EXAMINATION && demande.Type == ETypeDemande.ADOPTION)
            {
                ado = Adoption.Creer(demande, info);
                if (ado == null)
                {
                    ExceptionLauncher.New("Interface Adoption Creer", "Creation echouée");
                }
                Interface_Adoption_Save(ado);

            }
            else
            {
                AccesConsole.EnCouleur("Parametres invalide", ConsoleColor.Yellow);
            }


            return ado;
        }
        public static void Interface_Adoption_Save(Adoption adoption)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ADOPTIONS - ADOPTION CREE - {adoption.Id}", adoption.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Adoption.Save(adoption));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Adoption_Un(Adoption adoption)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? statut_ado = null;
                    string? statut_dm = null;
                    if (adoption.Statut != EStatutValidation.EN_COURS)
                    {
                        statut_ado = $"[{adoption.Statut}] ";
                    }
                    if (adoption.Demande.Statut == EStatutDemande.TERMINEE || adoption.Demande.Statut == EStatutDemande.CLOTUREE)
                    {
                        statut_dm = $"[{adoption.Demande.Statut}] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{statut_ado}** Statuer **" },
                        { "2", $"{statut_dm}Modifier" },
                        { "3", "Supprimer" }
                    };

                    if (adoption.Statut == EStatutValidation.ACCEPTEE)
                    {
                        menu.Add("4", $"[ GERER LA SORTIE ]");
                    }

                    AccesConsole.CreerEcran($"LES ADOPTIONS - {adoption.Id}", adoption.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (adoption.Demande.Statut >= EStatutDemande.TERMINEE)
                                    {
                                        ExceptionLauncher.New($"Interface Adoption Un", $"Cette demande est cloturée - statut : {adoption.Statut}");
                                    }

                                    Interface_Adoption_Update_Statut(adoption);
                                }
                                break;
                            case 2:
                                {
                                    if (adoption.Demande.Statut > EStatutDemande.VALIDATION)
                                    {
                                        ExceptionLauncher.New($"Interface Adoption Un", $"Cette demande est cloturée - statut : {adoption.Demande.Statut}");
                                    }

                                    Interface_Adoption_Update(adoption);
                                }
                                break;
                            case 3:
                                {
                                    Interface_Adoption_Delete(adoption);
                                    choix = 99;
                                }
                                break;
                            case 4:
                                {
                                    if (adoption.Statut == EStatutValidation.ACCEPTEE)
                                    {
                                        if (AllSortie.Find(adoption.Demande) == null)
                                        {
                                            Interface_Sorties_Creer(adoption.Demande);
                                        }
                                        else
                                        {
                                            Interface_Sorties_Un(AllSortie.Find(adoption.Demande));
                                        }

                                    }

                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Adoption_Update_Statut(Adoption adoption)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "** Accepter **" },
                        { "2", "** Refuser  **" },
                        { "3", "** Indecis  **" },
                    };

                    AccesConsole.CreerEcran($"LES ADOPTIONS - {adoption.Id} - la Decision", adoption.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Clear();
                                    AccesConsole.EnCouleur("\n\n" + adoption.Accepter(), adoption.Statut.ToString(), ConsoleColor.Green);

                                }
                                break;
                            case 2:
                                {
                                    string? refus = AccesConsole.SaisirChaine($" raison du refus : ");
                                    AccesConsole.Clear();
                                    AccesConsole.EnCouleur("\n\n" + adoption.Refuser(refus), adoption.Statut.ToString(), ConsoleColor.Red);
                                }
                                break;
                            case 3:
                                {
                                    AccesConsole.Clear();
                                    AccesConsole.EnCouleur("\n\n" + adoption.Indecis(), adoption.Statut.ToString(), ConsoleColor.Yellow);
                                }
                                break;

                        }
                        choix = 99;

                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }

        }
        public static void Interface_Adoption_Update(Adoption adoption)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Date Debut et/ou Fin" },
                        { "2", "Update la demande" },
                    };

                    AccesConsole.CreerEcran($"LES ADOPTIONS - {adoption.Id} - MODIFIER", adoption.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        AccesConsole.MsgUpdate();
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Info($"Ajout de date de debut ou/et fin");
                                    DateTime? dateD = AccesConsole.SaisirDate("Date de debut de l 'accueil : ");
                                    dateD ??= adoption.DateD;

                                    DateTime? dateF = AccesConsole.SaisirDate("Date de Fin de l 'accueil : ");
                                    dateF ??= adoption.DateF;

                                    AccesConsole.Afficher(adoption.Update(dateD, dateF));
                                }
                                break;
                            case 2:
                                {
                                    AccesConsole.Info($"Modification de la demande");

                                    Demande? demande = Questions.Demande();
                                    demande ??= adoption.Demande;

                                    string? detail = Questions.Infos();
                                    detail ??= adoption.Info;

                                    AccesConsole.Afficher(adoption.Update(demande, detail));

                                    AccesConsole.Attendre();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }

        }
        public static void Interface_Adoption_Delete(Adoption adoption)
        {
            bool validation = Questions.Delete(adoption.Id);
            if (validation)
            {
                bool valdem = AccesConsole.SaisirBoolean($"Voulez-vous supprimer la demande liée {adoption.Demande.Id} : ");
                if (valdem)
                {
                    AccesConsole.Afficher(Demande.Delete(adoption.Demande));
                }
                else
                {
                    AccesConsole.Afficher(adoption.Demande.Update(EStatutDemande.EXAMINATION));
                }

                AccesConsole.Afficher(Adoption.Delete(adoption));
            }
            else
            {
                AccesConsole.Info("Adoption invalide");
            }
            AccesConsole.Attendre();
        }

        //---------------------------------------------------------accueil
        public static void Interface_Accueil()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir Les accueil" },
                        { "2", "Gerer une accueil" },
                        { "3", "Creer une accueil" }
                    };

                    AccesConsole.CreerEcran("LES ACCUEILS", AllAccueil.Liste, menu);

                    Console.WriteLine();

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.CreerEcran("LES ACCUEILS - Liste", AllAccueil.Liste);
                                }
                                break;
                            case 2:
                                {
                                    Accueil? acc = Questions.Accueil();
                                    if (acc != null)
                                    {
                                        Interface_Accueil_Un(acc);
                                    }
                                    else
                                    {
                                        AccesConsole.Info("Accueil invalide");
                                    }

                                }
                                break;
                            case 3:
                                {
                                    Interface_Accueil_Creer();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }
                }
                catch (Exception e)
                {
                    AccesConsole.EnCouleur("\n" + e.Message, ConsoleColor.Yellow);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static Accueil? Interface_Accueil_Creer()
        {
            AccesConsole.CreerEcran("LES ACCUEILS - Creer", AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ACCUEIL));
            Demande? dm = Questions.Demande();

            return Interface_Accueil_Creer(dm);
        }
        public static Accueil? Interface_Accueil_Creer(Demande demande)
        {
            Accueil? acc = null;

            AccesConsole.CreerEcran("LES ACCUEILS - Creer", AllDemande.ListeByStatut(EStatutDemande.EXAMINATION, ETypeDemande.ACCUEIL));
            string? info = AccesConsole.SaisirChaine("Info: ");


            if (demande != null && demande.Statut == EStatutDemande.EXAMINATION && demande.Type == ETypeDemande.ACCUEIL)
            {
                acc = Accueil.Creer(demande, info);
                if (acc == null)
                {
                    ExceptionLauncher.New("Interface Accueil Creer", "Creation echouée");
                }
                Interface_Accueil_Save(acc);
            }
            else
            {
                AccesConsole.EnCouleur("Parametres invalide", ConsoleColor.Yellow);
            }


            return acc;
        }
        public static void Interface_Accueil_Save(Accueil accueil)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES ACCUEILS - ACCUEIL CREE - {accueil.Id}", accueil.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Accueil.Save(accueil));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Accueil_Un(Accueil accueil)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? statut_ado = null;
                    string? statut_dm = null;
                    if (accueil.Statut != EStatutValidation.EN_COURS)
                    {
                        statut_ado = $"[{accueil.Statut}] ";
                    }

                    if (accueil.Demande.Statut == EStatutDemande.TERMINEE || accueil.Demande.Statut == EStatutDemande.CLOTUREE)
                    {
                        statut_dm = $"[{accueil.Demande.Statut}] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{statut_ado}** Statuer **" },
                        { "2", $"{statut_dm}Modifier" },
                        { "3", "Supprimer" }
                    };

                    if (accueil.Statut == EStatutValidation.ACCEPTEE)
                    {
                        menu.Add("4", $"[ GERER LA SORTIE ]");
                    }

                    AccesConsole.CreerEcran($"LES ACCUEILS - {accueil.Id}", accueil.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (accueil.Demande.Statut == EStatutDemande.TERMINEE || accueil.Demande.Statut == EStatutDemande.CLOTUREE)
                                    {
                                        ExceptionLauncher.New($"Interface Accueil Un", $"Cette demande est cloturée - statut : {accueil.Demande.Statut}");
                                    }
                                    Interface_Accueil_Update_Statut(accueil);
                                }
                                break;
                            case 2:
                                {
                                    if (accueil.Demande.Statut == EStatutDemande.TERMINEE || accueil.Demande.Statut == EStatutDemande.CLOTUREE)
                                    {
                                        ExceptionLauncher.New($"Interface Accueil Un", $"Cette demande est terminée|cloturée - statut  : {accueil.Demande.Statut}");
                                    }
                                    Interface_Accueil_Update(accueil);
                                }
                                break;
                            case 3:
                                {
                                    Interface_Accueil_Delete(accueil);
                                    choix = 99;
                                }
                                break;
                            case 4:
                                {
                                    if (accueil.Statut == EStatutValidation.ACCEPTEE)
                                    {
                                        if (AllSortie.Find(accueil.Demande) == null)
                                        {
                                            if (accueil.Demande.Statut == EStatutDemande.TERMINEE || accueil.Demande.Statut == EStatutDemande.CLOTUREE)
                                            {
                                                ExceptionLauncher.New($"Interface Accueil Un", $"Cette demande est terminée|cloturée - statut  : {accueil.Demande.Statut}");
                                            }
                                            Interface_Sorties_Creer(accueil.Demande);
                                        }
                                        else
                                        {
                                            Interface_Sorties_Un(AllSortie.Find(accueil.Demande));
                                        }

                                    }

                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Accueil_Update_Statut(Accueil accueil)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "** Accepter **" },
                        { "2", "** Refuser  **" },
                        { "3", "** Indecis  **" },
                    };

                    AccesConsole.CreerEcran($"LES ACCUEILS - {accueil.Id} - la Decision", accueil.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Accueil_Update_StatutAccepter(accueil);

                                }
                                break;
                            case 2:
                                {
                                    Interface_Accueil_Update_StatutRefuser(accueil);
                                }
                                break;
                            case 3:
                                {
                                    Interface_Accueil_Update_StatutNull(accueil);
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }

        }
        public static void Interface_Accueil_Update_StatutAccepter(Accueil accueil)
        {
            AccesConsole.EnCouleur("\n\n" + accueil.Accepter(), accueil.Statut.ToString(), ConsoleColor.Green);

            AccesConsole.Attendre();

            AccesConsole.Clear();
            int i = accueil.Demande.Animal.DemandesEnCours().Count;
            if (i > 0)
            {
                bool suppAll = AccesConsole.SaisirBoolean($"voulez-vous annuller tout les autres demande en cours [{i}] ? [O/N] ");
                if (suppAll)
                {
                    AccesConsole.Clear();
                    AccesConsole.EnCouleur("\n" + accueil.Demande.Animal.RefuserAllDemandeEncours(), "REFUSER", ConsoleColor.Red);
                }
            }



        }
        public static void Interface_Accueil_Update_StatutRefuser(Accueil accueil)
        {
            string? refus = AccesConsole.SaisirChaine($" raison du refus : ");
            AccesConsole.EnCouleur("\n\n" + accueil.Refuser(refus), accueil.Statut.ToString(), ConsoleColor.Red);
        }
        public static void Interface_Accueil_Update_StatutNull(Accueil accueil)
        {
            AccesConsole.EnCouleur("\n\n" + accueil.Indecis(), accueil.Statut.ToString(), ConsoleColor.Yellow);
        }

        public static void Interface_Accueil_Update(Accueil accueil)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Date Debut et/ou Fin" },
                        { "2", "Update la demande" },
                    };

                    AccesConsole.CreerEcran($"LES ACCUEILS - {accueil.Id} - MODIFIER", accueil.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        AccesConsole.MsgUpdate();
                        switch (choix)
                        {
                            case 1:
                                {
                                    AccesConsole.Info($"Ajout de date de debut ou/et fin");
                                    DateTime? dateD = AccesConsole.SaisirDate("Date de debut de l 'accueil : ");
                                    dateD ??= accueil.DateDebut;

                                    DateTime? dateF = AccesConsole.SaisirDate("Date de Fin de l 'accueil : ");
                                    dateF ??= accueil.DateFin;

                                    AccesConsole.Afficher(accueil.Update(dateD, dateF));
                                }
                                break;
                            case 2:
                                {
                                    AccesConsole.Info($"Modification de la demande");

                                    Demande? demande = Questions.Demande();
                                    demande ??= accueil.Demande;

                                    string? detail = Questions.Infos();
                                    detail ??= accueil.Info;

                                    AccesConsole.Afficher(accueil.Update(demande, detail));

                                    AccesConsole.Attendre();
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Erreur(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }

        }
        public static void Interface_Accueil_Delete(Accueil accueil)
        {
            bool validation = Questions.Delete(accueil.Id);
            if (validation)
            {
                bool valdem = AccesConsole.SaisirBoolean($"Voulez-vous supprimer la demande liée {accueil.Demande.Id} : ");
                if (valdem)
                {
                    AccesConsole.Afficher(Demande.Delete(accueil.Demande));
                }
                else
                {
                    AccesConsole.Afficher(accueil.Demande.Update(EStatutDemande.EXAMINATION));
                }

                AccesConsole.Afficher(Accueil.Delete(accueil));
            }
            else
            {
                AccesConsole.Info("Accueil invalide");
            }
            AccesConsole.Attendre();
        }

        //---------------------------------------------------------Sorties

        public static void Interface_Sorties()
        {
            int? choix = 0;
            while (choix != 99)
            {
                Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les sorties" },
                        { "2", "Gerer une sorties" },
                        { "3", "Creer une sorties" },
                        { "4", "Voir tous les motifs" }
                    };

                AccesConsole.CreerEcran("LES SORTIES", AllSortie.LesSorties, menu);

                Console.WriteLine();

                choix = Questions.Choix();

                if (choix != 99 && choix <= 6 && choix >= 1)
                {
                    switch (choix)
                    {
                        case 1:
                            {
                                AccesConsole.CreerEcran("LES SORTIES - LES LISTES", AllSortie.LesSorties);
                            }
                            break;
                        case 2:
                            {
                                Sortie? sortie = Questions.Sorties();
                                if (sortie != null)
                                {
                                    Interface_Sorties_Un(sortie);
                                }
                                else
                                {
                                    AccesConsole.Info($"Entree invalide");
                                }

                            }
                            break;
                        case 3:
                            {
                                Interface_Sorties_Creer();
                            }
                            break;
                        case 4:
                            {
                                Interface_Motifs_Sortie();
                            }
                            break;
                    }
                }

            }
        }
        public static void Interface_Sorties_Creer()
        {
            AccesConsole.CreerEcran("LES SORTIES - CREER", AllDemande.ListeByStatut(EStatutDemande.EN_COURS));

            Demande? dm = Questions.Demande();
            Interface_Sorties_Creer(dm);
        }
        public static void Interface_Sorties_Creer(Demande demande)
        {
            if (demande == null)
            {
                ExceptionLauncher.New("Sortie Creer", "Parametre null");
            }

            AccesConsole.CreerEcran("LES SORTIES - CREER", AllMotifsSortie.LesSorties);

            MotifSortie? mos = Questions.MotifSortie();
            string? detail = Questions.Infos();

            if (mos != null)
            {
                Sortie sortie = Sortie.Creer(demande, mos, detail);
                if (sortie == null)
                {
                    ExceptionLauncher.New("Interface Sortie Creer", "Creation echouée");
                }
                Interface_Sorties_Save(sortie);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }
        }
        public static void Interface_Sorties_Save(Sortie sortie)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES SORTIES - SORTIE CREE - {sortie.Id}", sortie.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(Sortie.Save(sortie));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        public static void Interface_Sorties_Un(Sortie sortie)
        {

            if (sortie == null)
            {
                ExceptionLauncher.New("Interface Sortie", "Valeur de parametre null");
            }

            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    string? statut_dm = null;
                    if (sortie.Demande.Statut == EStatutDemande.TERMINEE || sortie.Demande.Statut == EStatutDemande.CLOTUREE)
                    {
                        statut_dm = $"[Bloqué] ";
                    }

                    Dictionary<string, string> menu = new()
                    {
                        { "1", $"{statut_dm}Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES SORTIES - {sortie.Id}", sortie.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    if (sortie.Demande.Statut > EStatutDemande.EN_COURS)
                                    {
                                        ExceptionLauncher.New($"Interface Accueil Un", $"Cette demande est terminée|cloturée - statut  : {sortie.Demande.Statut}");
                                    }

                                    Interface_Sorties_Un_modifier(sortie);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Sorties_Un_supprimer(sortie);
                                    choix = 99;
                                }
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Sorties_Un_modifier(Sortie sortie)
        {
            AccesConsole.CreerEcran($"LES SORTIES - {sortie.Id} - MODIFIER", sortie.ToString());

            MotifSortie? motif = Questions.MotifSortie();
            motif ??= sortie.Motifs;

            string? detail = AccesConsole.SaisirChaine($"Details : ");

            AccesConsole.Afficher(sortie.Update(motif, detail));

            AccesConsole.Attendre();
        }
        public static void Interface_Sorties_Un_supprimer(Sortie sortie)
        {
            bool validation = Questions.Delete(sortie.Id);
            if (validation)
            {
                AccesConsole.Afficher(Sortie.Delete(sortie));
            }
            else
            {
                AccesConsole.Info("MyType invalide");
            }
        }

        public static void Interface_Motifs_Sortie()
        {
            int? choix = 0;
            while (choix != 99)
            {
                Dictionary<string, string> menu = new()
                    {
                        { "1", "Voir tous les motifs" },
                        { "2", "Gerer un motif" },
                        { "3", "Creer un motif" },
                    };

                AccesConsole.CreerEcran("LES MOTIFS D' SORTIE", AllMotifsSortie.LesSorties, menu);

                Console.WriteLine();

                choix = Questions.Choix();

                if (choix <= menu.Count && choix >= 1)
                {
                    switch (choix)
                    {
                        case 1:
                            {
                                AccesConsole.CreerEcran("LES MOTIFS D' SORTIE - LA LISTES", AllMotifsSortie.LesSorties);
                            }
                            break;
                        case 2:
                            {
                                MotifSortie? ms = Questions.MotifSortie();
                                if (ms != null)
                                {
                                    Interface_Motifs_Sortie_Un(ms);
                                }
                                else
                                {
                                    AccesConsole.Info($"Sortie invalide");
                                }

                            }
                            break;
                        case 3:
                            {
                                Interface_Motifs_Sortie_Creer();
                            }
                            break;
                    }
                }

            }
        }
        public static void Interface_Motifs_Sortie_Creer()
        {
            AccesConsole.CreerEcran("LES MOTIFS D'SORTIE - CREER");

            string? nom = AccesConsole.SaisirChaine("Libelle du motif (Ex. accueil): ");
            string? descr = AccesConsole.SaisirChaine("Description du motif: ");

            if (!string.IsNullOrEmpty(nom))
            {
                MotifSortie motif = MotifSortie.Creer(nom, descr);
                if (motif == null)
                {
                    ExceptionLauncher.New("Interface Motif sortie Creer", "Creation echouée");
                }
                Interface_MotifsSorties_Save(motif);

            }
            else
            {
                AccesConsole.Info("Parametres invalide");
            }
        }
        public static void Interface_MotifsSorties_Save(MotifSortie motif)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    AccesConsole.CreerEcranSave($"LES MOTIFS SORTIES - MOTIF CREE - {motif.Id}", motif.ToString());
                    choix = Questions.Choix();

                    if (choix == 1)
                    {
                        AccesConsole.Afficher(MotifSortie.Save(motif));
                        choix = 99;
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Motifs_Sortie_Un(MotifSortie motif)
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "Modifier" },
                        { "2", "Supprimer" }
                    };

                    AccesConsole.CreerEcran($"LES MOTIFS D' SORTIE - {motif.Id}", motif.ToString(), menu);

                    choix = Questions.Choix();
                    if (choix <= menu.Count && choix > 0)
                    {
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Motifs_Sortie_Update(motif);
                                }
                                break;
                            case 2:
                                {
                                    Interface_Motifs_Sortie_Delete(motif);
                                    choix = 99;
                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }

                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }
        public static void Interface_Motifs_Sortie_Update(MotifSortie motif)
        {
            AccesConsole.CreerEcran($"LES MOTIFS D' SORTIE - {motif.Id} - MODIFIER", motif.ToString());

            string? libelle = AccesConsole.SaisirChaine("Libellé : ");
            libelle ??= motif.Libele;

            string? detail = Questions.Infos();
            detail ??= motif.Details;

            AccesConsole.Afficher(motif.Update(libelle, detail));

            AccesConsole.Attendre();
        }
        public static void Interface_Motifs_Sortie_Delete(MotifSortie motif)
        {
            bool validation = Questions.Delete(motif.Id);
            if (validation)
            {
                AccesConsole.Afficher(MotifSortie.Delete(motif));
            }
            else
            {
                AccesConsole.Info("MyType invalide");
            }
            AccesConsole.Attendre();
        }

        //---------------------------------------------------------Parametres

        public static void Interface_Parametres()
        {
            int? choix = 0;
            while (choix != 99)
            {
                try
                {
                    Dictionary<string, string> menu = new()
                    {
                        { "1", "> Gerer les types Animaux   <" },
                        { "2", "> Gerer les vaccins         <" },
                        { "3", "> Gerer les vaccinations    <" },
                        { "4", "> Gerer les compatibilités  <" },
                        { "5", "> Gerer les animaux_compat  <" },
                        { "6", "> Gerer les roles contacts  <" },
                        { "7", "> Gerer les motifs entrees  <" },
                        { "8", "> Gerer les motifs sorties  <" },
                        { "9", "> Gerer les Couleurs        <" },
                        { "10","> Gerer les Couleurs Appl.  <" },
                        { "11","> Gerer les Abris           <" },
                    };

                    AccesConsole.CreerEcran("PARAMETRES");
                    AccesConsole.Menu(menu);

                    choix = Questions.Choix();

                    if (choix <= menu.Count && choix > 0)
                    {
                        AccesConsole.Afficher(choix.ToString());
                        switch (choix)
                        {
                            case 1:
                                {
                                    Interface_Animal_Type();
                                }
                                break;
                            case 2:
                                {
                                    Interface_Vaccins();
                                }
                                break;
                            case 3:
                                {
                                    Interface_Vaccinantion();
                                }
                                break;
                            case 4:
                                {
                                    Interface_Compatibilite();
                                }
                                break;
                            case 5:
                                {
                                    Interface_Compatibilite_Testes();
                                }
                                break;
                            case 6:
                                {

                                    Interface_Contact_Roles();

                                }
                                break;
                            case 7:
                                {

                                    Interface_Motifs_Entree();

                                }
                                break;
                            case 8:
                                {

                                    Interface_Motifs_Sortie();

                                }
                                break;
                            case 9:
                                {

                                    Interface_Couleur();

                                }
                                break;
                            case 10:
                                {

                                    Interface_Coloration();

                                }
                                break;

                            case 11:
                                {

                                    Interface_Abri();

                                }
                                break;
                        }
                        AccesConsole.Attendre();
                    }


                }
                catch (Exception ex)
                {
                    AccesConsole.Info(ex.Message);
                    choix = 0;
                    AccesConsole.Attendre();
                }

            }
        }

        //---------------------------------------------------------
        private static Contact? ChoixContactDemande()
        {
            Contact? contact = null;
            AccesConsole.NX("contact");

            string? idcontact = Forma.TrimUpper(AccesConsole.SaisirChaine("Entrer Id Contacte : "));
            switch (idcontact)
            {
                case "X":
                    {

                    }
                    break;
                case "N":
                    {
                        contact = Interface_Contact_Creer();
                        AccesConsole.Clear();
                    }
                    break;
                default:
                    {
                        contact = AllContacts.Find(idcontact);
                    }
                    break;
            }
            return contact;
        }
        private static Animal? ChoixAnimalDemande()
        {
            Animal? animal = null;

            AccesConsole.NX("animal");
            string? idanimal = Forma.TrimUpper(AccesConsole.SaisirChaine("Entrer Id Animal : "));
            switch (idanimal)
            {
                case "X":
                    {

                    }
                    break;
                case "N":
                    {
                        animal = Interface_Animal_creer();
                        AccesConsole.Clear();
                    }
                    break;
                default:
                    {
                        animal = AllAnimal.Rechercher(idanimal);
                    }
                    break;
            }
            return animal;
        }

        private static string FirtLigne(Demande demande)
        {
            string firtligne = $"** [{demande.Statut}] Voir l'{demande.Type} **";
            if (demande.Statut == EStatutDemande.EXAMINATION || (demande.Type == ETypeDemande.ENTREE && demande.Statut == EStatutDemande.EN_COURS))
            {
                firtligne = $"** Faire l'{demande.Type} **";
            }

            if ((demande.Type == ETypeDemande.ADOPTION && demande.Statut != EStatutDemande.VALIDATION && AllAdoption.Find(demande) != null))
            {
                string statut = AllAdoption.Find(demande).Statut.ToString();
                firtligne = $"** [{statut}] Voir l'{demande.Type} **";
            }

            if ((demande.Type == ETypeDemande.ACCUEIL && demande.Statut != EStatutDemande.VALIDATION && AllAccueil.Find(demande) != null))
            {
                string statut = AllAccueil.Find(demande).Statut.ToString();
                firtligne = $"** [{statut}] Voir l'{demande.Type} **";
            }

            if ((demande.Type == ETypeDemande.ADOPTION && demande.Statut == EStatutDemande.VALIDATION && AllAdoption.Find(demande) != null) ||
                (demande.Type == ETypeDemande.ACCUEIL && demande.Statut == EStatutDemande.VALIDATION && AllAccueil.Find(demande) != null))
            {
                firtligne = $"** [en attent Decision] Voir l'{demande.Type} **";
            }

            return firtligne;
        }
    }
}
