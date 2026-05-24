using AppAnimalerie.ClasseMetier;
using AppAnimalerie.Presentation;
using Npgsql;

namespace AppAnimalerie.AccessDB
{
    public static class DB_Vaccin
    {
        public static Dictionary<string, Vaccin> All_From_Db()
        {
            Dictionary<string, Vaccin> retval = new Dictionary<string, Vaccin>();

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_vaccin, date_creation, nom_vaccin, description " +
                                                           $"from t_vaccin " +
                                                           $"order by id_vaccin ",
                                                           AccessDB.SqlConn);

            try
            {
                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                while (reader.Read())
                {

                    string id = DB_Convertisseur.String(reader, "id_vaccin");
                    DateTime? dateCreation = DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_vaccin");
                    string descr = DB_Convertisseur.String(reader, "description");

                    Vaccin vaccin = Vaccin.Creer(nom,descr);
                    vaccin.Id = id;
                    vaccin.DateCreation = (DateTime)dateCreation;
                    retval.Add(vaccin.Id, vaccin);

                    if (AllVaccin.Find(vaccin.Id) == null)
                    {
                        AllVaccin.Add(vaccin);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AccesConsole.Attendre();
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Vaccin? UnVaccinById(string id)
        {
            Vaccin? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_vaccin, date_creation, nom_vaccin, description " +
                                                           $"from t_vaccin " +
                                                           $"where id_vaccin = @id ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@id", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@id"].Value = id;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_vaccin");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string nom = DB_Convertisseur.String(reader, "nom_vaccin");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = Vaccin.Creer(nom, descr);
                    retval.Id = idty;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                AccesConsole.Afficher(ex.Message);
                AccesConsole.Attendre();
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }
        public static Vaccin? UnVaccinNom(string nom)
        {
            Vaccin? retval = null;

            using NpgsqlCommand sqlcmd = new NpgsqlCommand($"select id_vaccin, date_creation, nom_vaccin, description " +
                                                           $"from t_vaccin " +
                                                           $"where nom_vaccin = @nom ",
                                                           AccessDB.SqlConn);

            try
            {
                sqlcmd.Parameters.Add(new NpgsqlParameter("@nom", NpgsqlTypes.NpgsqlDbType.Varchar));

                sqlcmd.Prepare();

                sqlcmd.Parameters["@nom"].Value = nom;

                using NpgsqlDataReader reader = sqlcmd.ExecuteReader();
                if (reader.Read())
                {
                    string idty = DB_Convertisseur.String(reader, "id_vaccin");
                    DateTime dateCreation = (DateTime)DB_Convertisseur.Date(reader, "date_creation");
                    string fnom = DB_Convertisseur.String(reader, "nom_vaccin");
                    string descr = DB_Convertisseur.String(reader, "description");

                    retval = Vaccin.Creer(fnom, descr);
                    retval.Id = idty;
                    retval.DateCreation = (DateTime)dateCreation;

                }
            }
            catch (Exception ex)
            {
                AccesConsole.Afficher(ex.Message);
                AccesConsole.Attendre();
                throw new ExceptionDB(sqlcmd.CommandText, ex.Message);
            }

            return retval;
        }

        public static int Add(Vaccin vaccin)
        {
            string cmdtext = "insert into t_vaccin(id_vaccin, date_creation, nom_vaccin, description ) values (@id, @date, @nom, @descr) ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@date",(NpgsqlTypes.NpgsqlDbType.Date, vaccin.DateCreation) },
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccin.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Description) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Update(Vaccin vaccin)
        {

            string cmdtext = "update t_vaccin set nom_vaccin = @nom, description = @descr where id_vaccin = @id ";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@nom",(NpgsqlTypes.NpgsqlDbType.Varchar, vaccin.Nom) },
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Id) },
                { "@descr",(NpgsqlTypes.NpgsqlDbType.Varchar , vaccin.Description) }
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);

        }
        public static int Delete(string id)
        {
            string cmdtext = "delete from t_vaccin where id_vaccin = @id";

            var parametres = new Dictionary<string, (NpgsqlTypes.NpgsqlDbType Type, object Value)>
            {
                { "@id",(NpgsqlTypes.NpgsqlDbType.Varchar, id) },
            };

            return Requets.ExecuteNonQuery(cmdtext, parametres);
        }
    }
}
