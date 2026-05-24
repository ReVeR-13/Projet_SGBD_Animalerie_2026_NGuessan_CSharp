using AppAnimalerie.ClasseMetier;


namespace AppAnimalerie.Presentation
{
    static public class AccesConsole
    {
        private static string _titre;
        private static string _ligne;
        private static int _ligneLenght;
        static AccesConsole()
        {
            Titre = "APP ANIMALERIE";
            LigneLenght = 100;
            Ligne = Titre;
            
        }
        public static string Titre
        {
            get { return _titre; }
            set { _titre = Forma.TrimUpper(value); }
        }
        public static int LigneLenght
        {
            get { return _ligneLenght; }
            set { _ligneLenght = value; }
        }
        private static string Ligne
        {
            get { return _ligne; }
            set { _ligne = new string('=',LigneLenght); }
        }

        public static string Center(string text)
        {
            string ret = new(' ',(LigneLenght/2) - (text.Length/2));

            return ret + text;
        }

        static public void Clear()
        {
            Console.Clear();
        }
        static public void CreerEcran(string corps)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(Center(_titre));
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
            Console.WriteLine(Center(corps));
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
            
        }
        static public void CreerEcran(string niveau,string corps)
        {
            Ligne = Titre + " " +niveau;
            Console.Clear();
            Console.WriteLine();
            EnCouleur(Center(_titre + " - " + niveau),niveau,ConsoleColor.Yellow);
            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            Console.WriteLine(corps);
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
        }
        static public void CreerEcran(string niveau, string corps, Dictionary<string, string> listeMenu)
        {
            Ligne = Titre + " " + niveau;
            Console.Clear();
            Console.WriteLine();
            EnCouleur(Center( _titre + " - " + niveau),niveau,ConsoleColor.Cyan);
            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            Console.WriteLine(corps);
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
            Menu(listeMenu);
        }
        public static void CreerEcranSave(string titre, string obj)
        {
            Dictionary<string, string> menu = new()
                    {
                        { "1", "[ SAVE ]" }
                    };
            Ligne = Titre + " " + titre;
            string info1 = "[ ELEMENT CREE ]";
            string info2 = "[ ATTENTION ] Cet element n'est pas sauvegarder [ ATTENTION ]";

            Console.Clear();
            Console.WriteLine();
            EnCouleur(Center(_titre + " - " + titre), titre, ConsoleColor.Cyan);

            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            //-------------------------------------------------------------------
            EnCouleur($"{Center(info1)}\n\n", ConsoleColor.Green);
            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            //-------------------------------------------------------------------

            Console.WriteLine(obj);

            EnCouleur($"\n\n{Center(info2)}\n\n", "ATTENTION", ConsoleColor.Green);
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
            //-------------------------------------------------------------------
            Menu(menu);

        }

        static public void CreerEcran<T>(string niveau, T corps, Dictionary<string, string> listeMenu)where T : ITable
        {
            Ligne = Titre + " " + niveau;
            Console.Clear();
            Console.WriteLine();
            EnCouleur(Center(_titre + " - " + niveau), niveau, ConsoleColor.Cyan);
            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            EnCouleur($"{corps}\n\n",$"{corps.Id}",ConsoleColor.Cyan);
            EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
            Menu(listeMenu);
        }
        static public void CreerEcran<T>(string niveau, T corps)where T : ITable
        {
            Ligne = Titre + " " + niveau;
            Console.Clear();
            Console.WriteLine();
            EnCouleur(Center(_titre + " - " + niveau), niveau, ConsoleColor.Cyan);
            EnCouleur($"{_ligne}\n\n", ConsoleColor.Yellow);
            EnCouleur($"{corps}\n\n", $"{corps.Id}", ConsoleColor.Cyan);
        }


        static public void Menu(string listElement)
        {  
            Console.WriteLine(String.Format("{0,-6} - {1,-25}", $"[1]", $"{listElement}"));
            Console.WriteLine(String.Format("{0,-6} - {1,-25}", "[99]", "Fermer"));
        }
        static public void Menu(Dictionary<string,string> listElement)
        {
            foreach (string s in listElement.Keys)
            {
                EnCouleur(String.Format("{0,-6} - {1,-25}", $"[{s}]", $"{listElement[s]}"),s, ConsoleColor.Cyan);
            }
            EnCouleur(String.Format("{0,-6} - {1,-25}", "[99]", "Fermer"), "99", ConsoleColor.Red);
        }

        public static string? SaisirChaine(string? s)
        {
            string? choix = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);
                choix = Console.In.ReadLine();
            }
            
            return choix;
        }
        
        public static bool SaisirBoolean(string? s)
        {
            char? choix = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);
                choix = Convert.ToChar(Console.In.ReadLine());
            }
            
            return SaisirBoolean(choix);
        }
        public static bool SaisirBoolean(char? c)
        {
            bool result = false;
            if (c == 'O')
            {
                result = true;
            }
            return result;
        }
        public static bool? SaisirBooleanNullable(string? s)
        {
            bool? choix = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);

                string? res = Console.In.ReadLine();
                char? valeur = string.IsNullOrWhiteSpace(res) ? null : Convert.ToChar(res);

                choix = SaisirBoolean(valeur);
                
                
            }

            return choix;
        }

        public static DateTime? SaisirDate(string? s)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);
                date = Date(Console.In.ReadLine());
            }

            return date;
        }
        public static DateTime? Date(string? s)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(s))
            {
                date = Convert.ToDateTime(s);
            }
            return date;
        }

        public static int? SaisirInt(string? s)
        {
            int? result = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);
                result = Convert.ToInt32(Console.In.ReadLine());
            }
            
            return result;
        }
        public static Double? SaisirDouble(string? s)
        {
            Double? result = null;
            if (!string.IsNullOrEmpty(s))
            {
                EnCouleur($"{_ligne}\n", ConsoleColor.Yellow);
                Console.Write(s);
                result = Convert.ToDouble(Console.In.ReadLine());
            }
            
            return result;
        }

        public static void Info(string? s)
        {
            EnCouleur("\n\n[Info] "+s,ConsoleColor.Cyan);
        }
        public static void Erreur(string? s)
        {
            EnCouleur("\n\n[Erreur] " + s, ConsoleColor.Red);
        }
        public static void MsgUpdate()
        {
            EnCouleur("\n\n[Aides] Faire - Entrer - pour garder les mëmes infos ...", ConsoleColor.Yellow);
        }
        public static void Afficher(string? s)
        {
            Console.WriteLine("\n" + s);
        }
        public static void Afficher(int i)
        {
            string resultat = i == 0 ? "[Succes] Operation effectuée avec succes" : "[Echec] Operation non effectuée";
            ConsoleColor color = i == 0 ? ConsoleColor.Green : ConsoleColor.Red;
            EnCouleur("\n" + Questions.DB_Access(i) + "\n",resultat,color);
        }
        public static void Afficher(Enum @enum)
        {
            string ret = "Les types de demande existant\n";
            int i = 0;
            Type type = @enum.GetType();
            var item = Enum.GetNames(type);

            foreach (string a in item)
            {
                i++;
                ret += $"[{i}] - {a}     ";
            }
            Console.WriteLine(ret);
        }
        public static void Attendre()
        {
            EnCouleur("\nPressez une touche pour continuer ...",ConsoleColor.Cyan);
            Console.ReadKey();
            Console.WriteLine("\n");
        }
        public static void NX(string s)
        {
            Console.WriteLine($"\n- [N] pour creer un nouveau {s} -\n- [X] Annuler -");
        }

        public static void EnCouleur(string text, ConsoleColor color)
        {
            
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();

        }
        public static void EnCouleur(string text,string cible, ConsoleColor color)
        {
            int start = 0;

            while (start < text.Length)
            {
                int i = text.IndexOf(cible, start, StringComparison.OrdinalIgnoreCase);
                if (i < 0)
                {
                    Console.Write(text[start..]);
                    break;
                }

                Console.Write(text[start..i]);

                EnCouleur(cible, color);

                start = i+ cible.Length;
            }
            Console.WriteLine();
        }
        
    }
}
