
using AppAnimalerie.AccessDB;


namespace AppAnimalerie.ClasseMetier
{
    public static class AllContacts
    {
        private static readonly Dictionary<string, Contact> _lesContacts;
        private static int _numContact;
        static AllContacts()
        {
            _lesContacts = new Dictionary<string, Contact>();
            _numContact = 0;
        }

        public static int NumContact
        {
            get
            {
                if (Count > 0)
                {
                    _numContact = Forma.LastNumero(_lesContacts);
                }
                return _numContact;
            }
        }
        public static int Count
        {
            get
            {
                return _lesContacts.Count;
            }
        }
        public static string LesContacts
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "Id", "Date Crea.", "Nom", "Prenom", "Dem.");
                foreach (Contact a in _lesContacts.Values)
                {
                    i++;
                    retVal += Forma.Text(
                    $"{i}",
                    $"{a.Id}",
                    $"{a.DateCreation:dd-MM-yyyy}",
                    $"{a.Nom}",
                    $"{a.Prenom}",
                    $"{AllDemande.GetAllDemandeByContact(a).Count}");
                }
                return Forma.Center($"Liste des Contacts [{i}/{Count}]\n\n") + retVal;
            }
        }

        public static IEnumerable<Contact> Get()
        {
            foreach (Contact cont in _lesContacts.Values)
            {
                yield return cont;
            }
        }
        public static Contact? Find(string idcontact)
        {
            Contact? retVal = null;
            string f_id = Forma.TrimUpper(idcontact);
            if (_lesContacts.TryGetValue(f_id, out Contact? value))
            {
                retVal = value;
            }
            return retVal;
        }
        public static Contact? FindByGsm(string gsm)
        {

            Contact? retVal = null;
            if (Forma.IsNum(gsm))
            {
                string f_gsm = Forma.TrimUpper(gsm);
                foreach (Contact c in _lesContacts.Values)
                {
                    if (c.Gsm == f_gsm)
                    {
                        retVal = c;
                    }
                }
            }
            return retVal;
        }
        public static void Add(Contact contact)
        {
            if (Find(contact.Id) != null)
            {
                throw new Exception($"[ Groupe Animaux] Cet identifiant existe deja :{contact.Id}");
            }
            _numContact++;
            _lesContacts.Add(contact.Id, contact);
        }
        public static void Remove(Contact contact)
        {
            if (Find(contact.Id) == null)
            {
                throw new Exception($"[ Groupe Animaux] Cet Contact est deja supprimé :{contact.Id}");
            }
            _lesContacts.Remove(contact.Id);
        }

        public static int DB_Add(Contact contact)
        {
            int retVal = 0;
            if (DB_Contact.UnContactById(contact.Id) == null)
            {
                retVal = DB_Contact.Add(contact);
            }
            return retVal;
        }
        public static int DB_Update(Contact contact)
        {
            int retVal = 0;
            //les donnees locaux doivent etre à jour d'abord
            if (DB_Contact.UnContactById(contact.Id) != null)
            {
                retVal = DB_Contact.Update(contact);
            }
            return retVal;
        }
        public static int DB_Delete(Contact contact)
        {
            int retVal = 0;
            //les donnees doivent etre supprimer en locale
            if (DB_Contact.UnContactById(contact.Id) != null)
            {
                retVal = DB_Contact.Delete(contact.Id);
            }
            return retVal;
        }

    }

    public class Contact : ITable, IComparable<Contact>
    {
        private string _id;
        private DateTime _date;
        private string _niss;
        private string _nom;
        private string? _prenom;
        private DateTime _datenais;
        private string _gsm;
        private string? _phone;
        private string _mail;
        private string _codePostal;
        private string _localite;
        private string _adresse;

        private Contact(string niss, DateTime datenaiss, string nom, string? prenom, string gsm, string? tel, string mail, string cp, string localite, string adresse)
        {
            _id = Forma.SimpleId("CNT", AllContacts.NumContact + 1);
            _date = DateTime.Now;
            Niss = niss;
            Nom = nom;
            Prenom = prenom?? "--";
            Gsm = gsm;
            Telephone = tel ?? gsm;
            Mail = mail;
            Adresse = adresse;
            this.CodePostal = cp;
            this.Localite = localite;

            DateNaissance = datenaiss;

        }

        public string Id
        {
            get { return _id; }
            set { _id = Forma.Checked_Id(value); }
        }
        public DateTime DateCreation
        {
            get { return _date; }
            set
            {

                _date = Forma.Checked_DateCreation(value);

            }
        }
        public DateTime DateNaissance
        {
            get { return _datenais; }
            set
            {

                _datenais = Forma.Checked_DateCreation(value);
            }
        }
        public string Niss
        {
            get { return _niss; }
            set { _niss = value; }
        }
        public string Nom
        {
            get { return _nom; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Trim().Length < 3)
                {
                    throw new Exception($"[Contacts] Le nom n'est pas valable: {value}");
                }
                _nom = Forma.TrimUpper(value);
            }
        }
        public string? Prenom
        {
            get { return _prenom; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (value.Trim().Length < 2)
                    {
                        throw new Exception($"[Contacts] Le Prenom n'est pas valable: {value}");
                    }
                }
                _prenom = value.Trim();
            }
        }
        public string Gsm
        {
            get { return _gsm; }
            set
            {
                if (!Forma.IsNum(value))
                {
                    throw new Exception($"[Contacts] Le numero de GSM n'est pas valable: {value}");
                }
                _gsm = value.Trim().ToUpper();
            }
        }
        public string? Telephone
        {
            get { return _phone; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (!Forma.IsNum(value))
                    {
                        throw new Exception($"[Contacts] Le num de Telephone n'est pas valable: {value}");
                    }
                }
                _phone = value.Trim().ToUpper();
            }
        }
        public string Mail
        {
            get { return _mail; }
            set
            {
                if (!Forma.IsMail(value))
                {
                    throw new Exception($"[Contacts] Le mail n'est pas valable: {value}");
                }
                _mail = value;
            }
        }
        public string CodePostal
        {
            get
            {
                return _codePostal;
            }
            set
            {
                _codePostal = Forma.TrimUpper(value);
            }
        }
        public string Localite
        {
            get
            {
                return _localite;
            }
            set
            {
                _localite = value;
            }
        }
        public string Adresse
        {
            get
            {
                return _adresse;
            }
            set
            {
                _adresse = value;
            }
        }

        public int TypeCount
        {
            get
            {
                return AllTypeContact_Contact.AllOfContact(this).Count;
            }
        }
        public string LesTypes
        {
            get
            {
                int i = 0;
                string retVal = Forma.Center($"Liste des Types [{TypeCount}]\n") +
                    Forma.Center(new string('-', 90) + $"\n\n") +
                    Forma.Padding(Forma.Text("N°", "Id", "Date Crea.", "Nom", "Descrip."));
                foreach (TypeContact_Contact t in AllTypeContact_Contact.AllOfContact(this).Values)
                {
                    i++;
                    retVal += Forma.Padding(Forma.Text($"{i}", $"{t.Id}", $"{t.DateCreation:dd-MM-yyyy}", $"{t.Type.Nom}", $"{t.Type.Description}"));
                }

                return retVal;
            }
        }
        public string LesTypesManquants
        {
            get
            {
                return AllTypeContact.LesTypesContactsManquant(this);
            }
        }

        public string ListeDemandes
        {
            get
            {
                int i = 0;
                string retVal =
                    Forma.Text("N°", "Id", "Date Crea.", "Statut", "Remarque");

                if (this.GetAllDemandes().Count > 0)
                {
                    foreach (Demande dm in this.GetAllDemandes().Values)
                    {
                        i++;
                        retVal += Forma.Text($"{i}°", $"{dm.Id}", $"{dm.DateCreation:dd-MM-yyyy}", $"{dm.Statut}", $"{dm.Remarque}");
                    }
                }
                else
                {
                    retVal = "Pas de demande";
                }
                return $"Liste des Demande de {Nom} [{i}]\n\n" + retVal;
            }
        }
        public Dictionary<string, Demande> GetAllDemandes()
        {
            return AllDemande.GetAllDemandeByContact(this);
        }
        public string ListeDemandesByStatut(EStatutDemande eStatut)
        {
            int i = 0;
            string retVal = Forma.Text("N°", "Id", "Date Crea.", "Statut", "Remarque");

            if (this.GetAllDemandes().Count > 0)
            {
                foreach (Demande dm in this.GetAllDemandes().Values)
                {
                    if (dm.Statut == eStatut)
                    {
                        i++;
                        retVal += Forma.Text($"{i}°", $"{dm.Id}", $"{dm.DateCreation:dd-MM-yyyy}", $"{dm.Statut}", $"{dm.Remarque}");
                    }

                }
            }
            else
            {
                retVal = "Pas de demande";
            }
            return $"Liste des Demande En Cours... de {Nom} [{i}]\n\n" + retVal;
        }

        public TypeContact_Contact AddType(TypeContact typeContact)
        {
            if (AllTypeContact_Contact.AllOfContact(this).ContainsKey(typeContact.Id))
            {
                ExceptionLauncher.New("Conctact", "Cet type existe deja dans la liste");
            }
            return TypeContact_Contact.Creer(this, typeContact);
        }
        public int RemoveType(TypeContact type)
        {
            int ret = 0;
            TypeContact_Contact cible = AllTypeContact_Contact.Find(this, type);
            if (cible == null)
            {
                ExceptionLauncher.New("Conctact", "Cet type n' existe pas dans la liste");
            }

            return TypeContact_Contact.Delete(cible);
        }
        public IEnumerable<TypeContact_Contact> EnumType()
        {
            foreach (TypeContact_Contact type in AllTypeContact_Contact.AllOfContact(this).Values)
            {
                yield return type;
            }
        }
        public TypeContact_Contact? GetUnType(TypeContact type)
        {
            TypeContact_Contact? retval = null;
            if (type != null && AllTypeContact_Contact.AllOfContact(this).ContainsKey(type.Id))
            {
                retval = AllTypeContact_Contact.AllOfContact(this)[type.Id];
            }
            return retval;
        }

        public int CompareTo(Contact contact)
        {
            return Id.CompareTo(contact.Id);
        }
        public override string ToString()
        {
            string retVal =

                Forma.Center($"CONTACT - [ {this.Id} ]\n") +
                Forma.Center(new string('-', 90) + $"\n\n") +
                Forma.Texta2("Date", $"{DateCreation:dd-MM-yyyy}") +
                Forma.Texta2("ID", $"{Id}") +
                Forma.Texta2("Niss", $"{Niss}") +
                Forma.Texta2("Date de Naiss.", $"{DateNaissance:dd-MM-yyyy}") +
                Forma.Texta2("Nom", $"{Nom}") +
                Forma.Texta2("Prenom", $"{Prenom}") +
                Forma.Texta2("Gsm", $"{Gsm}") +
                Forma.Texta2("Telephone", Telephone?? "--") +
                Forma.Texta2("Email", Mail) +
                Forma.Texta2("Adresse", $"{Adresse} | {Localite} | {CodePostal}") +
                Forma.Texta2("Nbre Type", $"{this.TypeCount}");

            retVal += "\n\n" + this.LesTypes;

            return retVal;
        }

        public static Contact Creer(string niss, DateTime datenaissance, string nom, string prenom, string gsm, string? tel, string mail, string cp, string localite, string adresse)
        {
            Contact retVal = new Contact(niss, datenaissance, nom, prenom, gsm, tel, mail, cp, localite, adresse);

            return retVal;
        }
        public string Modification(string? niss, DateTime? datenaissance, string? nom, string? prenom, string? gsm, string? tel, string? mail, string? cp, string? localite, string? adresse)
        {
            string retVal = $"Updated effectuée(s) sur --{Nom}--\n";
            int i = 0;
            if (!string.IsNullOrEmpty(nom))
            {
                Nom = nom;
                i++;
                retVal += $"Nom modifié: {Nom}\n";
            }

            if (!string.IsNullOrEmpty(prenom))
            {
                Prenom = prenom;
                i++;
                retVal += $"Prenom modifié: {Prenom}\n";
            }

            if (!string.IsNullOrEmpty(niss))
            {
                Niss = niss;
                i++;
                retVal += $"Niss modifié: {Niss}\n";
            }

            if (datenaissance != null)
            {
                DateNaissance = (DateTime)datenaissance;
                i++;
                retVal += $"Date de naissance modifié: {DateNaissance}\n";
            }

            if (!string.IsNullOrEmpty(gsm))
            {
                Gsm = gsm;
                i++;
                retVal += $"Gsm modifié: {Gsm}\n";
            }

            if (!string.IsNullOrEmpty(tel))
            {
                Telephone = tel;
                i++;
                retVal += $"Telephone modifié: {Telephone}\n";
            }

            if (!string.IsNullOrEmpty(mail))
            {
                Mail = mail;
                i++;
                retVal += $"Mail modifié: {Mail}\n";
            }

            if (!string.IsNullOrEmpty(cp))
            {
                CodePostal = cp;
                i++;
                retVal += $"Code postal modifié: {cp}\n";
            }

            if (!string.IsNullOrEmpty(localite))
            {
                Localite = localite;
                i++;
                retVal += $"Localite modifié: {localite}\n";
            }

            if (!string.IsNullOrEmpty(adresse))
            {
                Adresse = adresse;
                i++;
                retVal += $"Adresse modifié: {Adresse}\n";
            }
            if (i > 0)
            {
                AllContacts.DB_Update(this);
            }

            return retVal;


        }
        public static int Save(Contact contact)
        {
            int retVal = 0;
            if (AllContacts.Find(contact.Id) == null)
            {
                AllContacts.Add(contact);
                retVal = AllContacts.DB_Add(contact);
            }
            return retVal;
        }
        public static int Delete(Contact contact)
        {
            int retVal = 0;
            if (AllContacts.Find(contact.Id) != null)
            {
                OnDelete(contact);
                AllContacts.Remove(contact);
                retVal = AllContacts.DB_Delete(contact);
            }

            return retVal;
        }


        private static int OnDelete(Contact contact)
        {
            if (AllTypeContact_Contact.AllOfContact(contact).Count > 0)
            {
                foreach (TypeContact_Contact ac in contact.EnumType())
                {
                    TypeContact_Contact.Delete(ac);
                }
            }

            if (AllDemande.GetAllDemandeByContact(contact).Count > 0)
            {
                foreach (Demande d in contact.GetAllDemandes().Values)
                {
                    Demande.Delete(d);
                }
            }
            return 1;
        }

    }

}
