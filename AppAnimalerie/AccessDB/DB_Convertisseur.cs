using AppAnimalerie.ClasseMetier;
using AppAnimalerie.ClasseService;
using Npgsql;

namespace AppAnimalerie.AccessDB
{
    public static class DB_Convertisseur
    {
        public static string? String(NpgsqlDataReader reader,string colonne)
        {
            return reader.IsDBNull(reader.GetOrdinal(colonne)) ? null :  reader.GetString(reader.GetOrdinal(colonne));
        }
        public static DateTime? Date(NpgsqlDataReader reader,string colonne)
        {
            return reader.IsDBNull(reader.GetOrdinal(colonne)) ? null : reader.GetDateTime(reader.GetOrdinal(colonne));
        }
        public static bool Bool(NpgsqlDataReader reader,string colonne) { return (bool)reader[colonne]; }

        public static EStatutAbri? StatutAbri(NpgsqlDataReader reader,string colonne)
        {
            EStatutAbri? retval = null;
            string? valeur = DB_Convertisseur.String(reader,colonne);
            if (Enum.TryParse<EStatutAbri>(valeur,out EStatutAbri result))
            {
                retval = result; 
            }
            return retval;
        }
        public static EStatutAnimal? StatutAnimal(NpgsqlDataReader reader, string colonne)
        {
            EStatutAnimal? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse<EStatutAnimal>(valeur, out EStatutAnimal result))
            {
                retval = result;
            }
            return retval;
        }
        public static ETypeDemande? TypeDemande(NpgsqlDataReader reader, string colonne)
        {
            ETypeDemande? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse<ETypeDemande>(valeur, out ETypeDemande result))
            {
                retval = result;
            }
            return retval;
        }
        public static EStatutDemande? StatutDemande(NpgsqlDataReader reader, string colonne)
        {
            EStatutDemande? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse<EStatutDemande>(valeur, out EStatutDemande result))
            {
                retval = result;
            }
            return retval;
        }
        public static EStatutValidation? StatutValidation(NpgsqlDataReader reader, string colonne)
        {
            EStatutValidation? retval = null;
            string? valeur = String(reader, colonne);
            if (Enum.TryParse<EStatutValidation>(valeur, out EStatutValidation result))
            {
                retval = result;
            }
            return retval;
        }

        public static Animal? Animal(NpgsqlDataReader reader,string colonne)
        {
            string info = String(reader, colonne);
            return AllAnimal.Rechercher(info);

        }
        public static Contact? Contact(NpgsqlDataReader reader, string colonne)
        {
            return AllContacts.Find(String(reader, colonne));

        }
        public static Vaccin? Vaccin(NpgsqlDataReader reader, string colonne)
        {
            return AllVaccin.Find(reader.GetString(reader.GetOrdinal(colonne)));

        }
        public static Compatibilite Compatibilite(NpgsqlDataReader reader, string colonne)
        {
            return AllCompatibilite.Find(reader.GetString(reader.GetOrdinal(colonne)));
        }
        public static Couleur Couleur(NpgsqlDataReader reader, string colonne)
        {
            return AllCouleur.Find(reader.GetString(reader.GetOrdinal(colonne)));
        }
        public static Demande? Demande(NpgsqlDataReader reader, string colonne)
        {
            return AllDemande.Find(reader.GetString(reader.GetOrdinal(colonne)));

        }
        public static MotifEntree? MotifEntree(NpgsqlDataReader reader, string colonne)
        {
            return AllMotifsEntrees.FindById(reader.GetString(reader.GetOrdinal(colonne)));

        }
        public static MotifSortie? MotifSortie(NpgsqlDataReader reader, string colonne)
        {
            return AllMotifsSortie.FindById(reader.GetString(reader.GetOrdinal(colonne)));

        }
    }

    public static class Requets
    {
        public static int ExecuteNonQuery(string cmdText, Dictionary<string,(NpgsqlTypes.NpgsqlDbType Type, object Value)> parametres)
        {
            using (NpgsqlCommand npgsql = new NpgsqlCommand(cmdText, AccessDB.SqlConn))
            {
                if (parametres != null)
                {
                    foreach (var param in parametres)
                    {
                        npgsql.Parameters.Add(new NpgsqlParameter(param.Key, param.Value.Type));
                    }
                }

                npgsql.Prepare();

                if(parametres != null)
                {
                    int i = 0;
                    foreach (var param in parametres)
                    {
                        npgsql.Parameters[i].Value = param.Value.Value ?? DBNull.Value;
                        i++;
                    }
                }

                try
                {
                    return npgsql.ExecuteNonQuery();

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                    throw new ExceptionDB(cmdText,ex.Message);
                }
            } 
        }
        
    }
}
