using ManageUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using smartManage.Tools;

namespace smartManage.Model
{
    public class clsMetier
    {
        const string DirectoryUtilLog = "Log"; //***Les variables globales***
        private static string _ConnectionString, _host, _db, _user, _pwd;
        private static clsMetier Fact;
        public static string bdEnCours = "";
        private SqlConnection conn;
        #region prerecquis
        public static clsMetier GetInstance()
        {
            if (Fact == null)
                Fact = new clsMetier();
            return Fact;
        }
        private object getParameter(IDbCommand cmd, string name, DbType type, int size, object value)
        {
            IDbDataParameter param = cmd.CreateParameter();
            param.Size = size;
            param.DbType = type;
            param.ParameterName = name;
            param.Value = value;
            return param;
        }
        public void Initialize(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            conn = new SqlConnection(ConnectionString);
        }
        public void Initialize(clsConnexion con)
        {
            _host = con.Serveur;// host;
            _db = con.DB; ;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("server={0}; database={1}; user={2}; pwd={3}", _host, _db, _user, _pwd);
            conn = new SqlConnection(sch);
        }
        public void Initialize(clsConnexion con, int type)
        {
            _host = con.Serveur;// host;
            _db = con.DB; ;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("server={0}; database={1}; user={2}; pwd={3}", _host, _db, _user, _pwd);
            switch (type)
            {
                //sql server 2005
                case 1: sch = string.Format("Data Source={0};Persist Security Info=True; Initial Catalog={1};User ID={2}; Password={3}", _host, _db, _user, _pwd); break;
                //sql server 2008 Data Source=WIN7-PC\SQLEXPRESS;Initial Catalog=bihito;Persist Security Info=True;User ID=sa;Password=sa
                case 2: sch = string.Format("Data Source={0};Persist Security Info=True; Initial Catalog={1};User ID={2}; Password={3}", _host, _db, _user, _pwd); break;
                case 3: break;
            }
            conn = new SqlConnection(sch);
        }
        public void Initialize(string host, string db, string user, string pwd)
        {
            _host = host;
            _db = db;
            _user = user;
            _pwd = pwd;
            string sch = string.Format("server={0}; database={1}; user={2}; pwd={3}", _host, _db, _user, _pwd);
            conn = new SqlConnection(sch);
        }
        public void setDB(string db)
        {
            _db = db;
        }
        public bool isConnect()
        {
            bool bl = true;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                conn.Close();
            }
            catch (Exception exc)
            {
                bl = false;
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat de la connexion à la BD sans paramètre : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return bl;
        }
        public bool isConnect(clsConnexion con)
        {
            bool bl = true;
            _host = con.Serveur;// host;
            _db = con.DB;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("server={0}; database=Master; user={1}; pwd={2}", con.Serveur, con.User, con.Pwd);
            conn = new SqlConnection(sch);
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                conn.Close();
            }
            catch (Exception exc)
            {
                sch = string.Format("server={0}; database={1};id user={2}; pwd={3}", _host, _db, _user, _pwd);
                bl = false;
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat connexion à la BD avec paramètre : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return bl;
        }
        public List<string> getAllDB()
        {
            List<string> lst = new List<string>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT name FROM sysdatabases where name!='master' order by name");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            lst.Add(dr["name"].ToString());
                    }
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Récupération de toutes les bases de Données SQLServer : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lst;
        }
        public string getCurrentDataBase()
        {
            string bd = "";
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    //Sélection de la base des données en cours
                    cmd.CommandText = string.Format("SELECT DB_NAME() AS bd_encours");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            bd = (dr["bd_encours"].ToString());
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Récupération de toutes les bases de Données SQLServer : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return bd;
        }

        public void CloseConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                else
                    return;
            }
            catch (Exception exc)
            {
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Récupération de toutes les bases de Données SQLServer : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
        }
        #endregion prerecquis
        #region  CLSCOMPTE
        public clscompte getClscompte(object intid)
        {
            clscompte varclscompte = new clscompte();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM compte WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclscompte.Id = int.Parse(dr["id"].ToString());
                            varclscompte.Numero = dr["numero"].ToString();
                            varclscompte.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscompte.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscompte.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscompte.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'compte' avec la classe 'clscompte' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclscompte;
        }

        public List<clscompte> getAllClscompte(string criteria)
        {
            List<clscompte> lstclscompte = new List<clscompte>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM compte  WHERE";
                    sql += "  numero LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscompte varclscompte = null;
                        while (dr.Read())
                        {

                            varclscompte = new clscompte();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscompte.Id = int.Parse(dr["id"].ToString());
                            varclscompte.Numero = dr["numero"].ToString();
                            varclscompte.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscompte.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscompte.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscompte.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscompte.Add(varclscompte);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'compte' avec la classe 'clscompte' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscompte;
        }

        public List<clscompte> getAllClscompte()
        {
            List<clscompte> lstclscompte = new List<clscompte>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM compte ORDER BY numero ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscompte varclscompte = null;
                        while (dr.Read())
                        {

                            varclscompte = new clscompte();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscompte.Id = int.Parse(dr["id"].ToString());
                            varclscompte.Numero = dr["numero"].ToString();
                            varclscompte.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscompte.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscompte.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscompte.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscompte.Add(varclscompte);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'compte' avec la classe 'clscompte' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscompte;
        }

        public int insertClscompte(clscompte varclscompte)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO compte ( id,numero,user_created,date_created,user_modified,date_modified ) VALUES (@id,@numero,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscompte.Id));
                    if (varclscompte.Numero != null) cmd.Parameters.Add(getParameter(cmd, "@numero", DbType.String, 10, varclscompte.Numero));
                    else cmd.Parameters.Add(getParameter(cmd, "@numero", DbType.String, 10, DBNull.Value));
                    if (varclscompte.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscompte.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscompte.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscompte.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscompte.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscompte.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscompte.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscompte.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'compte' avec la classe 'clscompte' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClscompte(clscompte varclscompte)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE compte  SET numero=@numero,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclscompte.Numero != null) cmd.Parameters.Add(getParameter(cmd, "@numero", DbType.String, 10, varclscompte.Numero));
                    else cmd.Parameters.Add(getParameter(cmd, "@numero", DbType.String, 10, DBNull.Value));
                    if (varclscompte.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscompte.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscompte.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscompte.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscompte.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscompte.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscompte.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscompte.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscompte.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'compte' avec la classe 'clscompte' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClscompte(clscompte varclscompte)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM compte  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscompte.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'compte' avec la classe 'clscompte' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSCOMPTE 
        #region  CLSGROUPE
        public clsgroupe getClsgroupe(object intid)
        {
            clsgroupe varclsgroupe = new clsgroupe();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM groupe WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsgroupe.Id = int.Parse(dr["id"].ToString());
                            varclsgroupe.Designation = dr["designation"].ToString();
                            if (!dr["niveau"].ToString().Trim().Equals("")) varclsgroupe.Niveau = int.Parse(dr["niveau"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'groupe' avec la classe 'clsgroupe' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsgroupe;
        }

        public List<clsgroupe> getAllClsgroupe(string criteria)
        {
            List<clsgroupe> lstclsgroupe = new List<clsgroupe>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM groupe  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsgroupe varclsgroupe = null;
                        while (dr.Read())
                        {

                            varclsgroupe = new clsgroupe();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsgroupe.Id = int.Parse(dr["id"].ToString());
                            varclsgroupe.Designation = dr["designation"].ToString();
                            if (!dr["niveau"].ToString().Trim().Equals("")) varclsgroupe.Niveau = int.Parse(dr["niveau"].ToString());
                            lstclsgroupe.Add(varclsgroupe);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'groupe' avec la classe 'clsgroupe' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsgroupe;
        }

        public List<clsgroupe> getAllClsgroupe()
        {
            List<clsgroupe> lstclsgroupe = new List<clsgroupe>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM groupe ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsgroupe varclsgroupe = null;
                        while (dr.Read())
                        {

                            varclsgroupe = new clsgroupe();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsgroupe.Id = int.Parse(dr["id"].ToString());
                            varclsgroupe.Designation = dr["designation"].ToString();
                            if (!dr["niveau"].ToString().Trim().Equals("")) varclsgroupe.Niveau = int.Parse(dr["niveau"].ToString());
                            lstclsgroupe.Add(varclsgroupe);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'groupe' avec la classe 'clsgroupe' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsgroupe;
        }

        public int insertClsgroupe(clsgroupe varclsgroupe)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO groupe ( id,designation,niveau ) VALUES (@id,@designation,@niveau  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgroupe.Id));
                    if (varclsgroupe.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 30, varclsgroupe.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 30, DBNull.Value));
                    if (varclsgroupe.Niveau.HasValue) cmd.Parameters.Add(getParameter(cmd, "@niveau", DbType.Int32, 4, varclsgroupe.Niveau));
                    else cmd.Parameters.Add(getParameter(cmd, "@niveau", DbType.Int32, 4, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'groupe' avec la classe 'clsgroupe' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsgroupe(clsgroupe varclsgroupe)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE groupe  SET designation=@designation,niveau=@niveau  WHERE 1=1  AND id=@id ");
                    if (varclsgroupe.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 30, varclsgroupe.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 30, DBNull.Value));
                    if (varclsgroupe.Niveau.HasValue) cmd.Parameters.Add(getParameter(cmd, "@niveau", DbType.Int32, 4, varclsgroupe.Niveau));
                    else cmd.Parameters.Add(getParameter(cmd, "@niveau", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgroupe.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'groupe' avec la classe 'clsgroupe' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsgroupe(clsgroupe varclsgroupe)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM groupe  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgroupe.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'groupe' avec la classe 'clsgroupe' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSGROUPE 
        #region  CLSMARQUE
        public clsmarque getClsmarque(object intid)
        {
            clsmarque varclsmarque = new clsmarque();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM marque WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsmarque.Id = int.Parse(dr["id"].ToString());
                            varclsmarque.Designation = dr["designation"].ToString();
                            varclsmarque.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmarque.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmarque.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmarque.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'marque' avec la classe 'clsmarque' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsmarque;
        }

        public List<clsmarque> getAllClsmarque(string criteria)
        {
            List<clsmarque> lstclsmarque = new List<clsmarque>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM marque  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmarque varclsmarque = null;
                        while (dr.Read())
                        {

                            varclsmarque = new clsmarque();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmarque.Id = int.Parse(dr["id"].ToString());
                            varclsmarque.Designation = dr["designation"].ToString();
                            varclsmarque.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmarque.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmarque.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmarque.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsmarque.Add(varclsmarque);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'marque' avec la classe 'clsmarque' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmarque;
        }

        public List<clsmarque> getAllClsmarque()
        {
            List<clsmarque> lstclsmarque = new List<clsmarque>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM marque ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmarque varclsmarque = null;
                        while (dr.Read())
                        {

                            varclsmarque = new clsmarque();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmarque.Id = int.Parse(dr["id"].ToString());
                            varclsmarque.Designation = dr["designation"].ToString();
                            varclsmarque.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmarque.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmarque.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmarque.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsmarque.Add(varclsmarque);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'marque' avec la classe 'clsmarque' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmarque;
        }

        public int insertClsmarque(clsmarque varclsmarque)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO marque ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmarque.Id));
                    if (varclsmarque.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsmarque.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmarque.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmarque.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmarque.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmarque.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmarque.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'marque' avec la classe 'clsmarque' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsmarque(clsmarque varclsmarque)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE marque  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsmarque.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsmarque.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmarque.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmarque.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmarque.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmarque.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmarque.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmarque.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmarque.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'marque' avec la classe 'clsmarque' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsmarque(clsmarque varclsmarque)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM marque  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmarque.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'marque' avec la classe 'clsmarque' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSMARQUE 
        #region  CLSMODELE
        public clsmodele getClsmodele(object intid)
        {
            clsmodele varclsmodele = new clsmodele();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM modele WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsmodele.Id = int.Parse(dr["id"].ToString());
                            varclsmodele.Designation = dr["designation"].ToString();
                            varclsmodele.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmodele.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmodele.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmodele.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'modele' avec la classe 'clsmodele' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsmodele;
        }

        public List<clsmodele> getAllClsmodele(string criteria)
        {
            List<clsmodele> lstclsmodele = new List<clsmodele>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM modele  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmodele varclsmodele = null;
                        while (dr.Read())
                        {

                            varclsmodele = new clsmodele();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmodele.Id = int.Parse(dr["id"].ToString());
                            varclsmodele.Designation = dr["designation"].ToString();
                            varclsmodele.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmodele.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmodele.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmodele.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsmodele.Add(varclsmodele);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'modele' avec la classe 'clsmodele' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmodele;
        }

        public List<clsmodele> getAllClsmodele()
        {
            List<clsmodele> lstclsmodele = new List<clsmodele>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM modele ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmodele varclsmodele = null;
                        while (dr.Read())
                        {

                            varclsmodele = new clsmodele();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmodele.Id = int.Parse(dr["id"].ToString());
                            varclsmodele.Designation = dr["designation"].ToString();
                            varclsmodele.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmodele.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmodele.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmodele.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsmodele.Add(varclsmodele);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'modele' avec la classe 'clsmodele' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmodele;
        }

        public int insertClsmodele(clsmodele varclsmodele)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO modele ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmodele.Id));
                    if (varclsmodele.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsmodele.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmodele.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmodele.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmodele.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmodele.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmodele.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'modele' avec la classe 'clsmodele' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsmodele(clsmodele varclsmodele)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE modele  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsmodele.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsmodele.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmodele.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmodele.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmodele.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmodele.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmodele.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmodele.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmodele.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'modele' avec la classe 'clsmodele' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsmodele(clsmodele varclsmodele)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM modele  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmodele.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'modele' avec la classe 'clsmodele' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSMODELE 
        #region  CLSCOULEUR
        public clscouleur getClscouleur(object intid)
        {
            clscouleur varclscouleur = new clscouleur();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM couleur WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclscouleur.Id = int.Parse(dr["id"].ToString());
                            varclscouleur.Designation = dr["designation"].ToString();
                            varclscouleur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscouleur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscouleur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscouleur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'couleur' avec la classe 'clscouleur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclscouleur;
        }

        public List<clscouleur> getAllClscouleur(string criteria)
        {
            List<clscouleur> lstclscouleur = new List<clscouleur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM couleur  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscouleur varclscouleur = null;
                        while (dr.Read())
                        {

                            varclscouleur = new clscouleur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscouleur.Id = int.Parse(dr["id"].ToString());
                            varclscouleur.Designation = dr["designation"].ToString();
                            varclscouleur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscouleur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscouleur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscouleur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscouleur.Add(varclscouleur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'couleur' avec la classe 'clscouleur' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscouleur;
        }

        public List<clscouleur> getAllClscouleur()
        {
            List<clscouleur> lstclscouleur = new List<clscouleur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM couleur ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscouleur varclscouleur = null;
                        while (dr.Read())
                        {

                            varclscouleur = new clscouleur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscouleur.Id = int.Parse(dr["id"].ToString());
                            varclscouleur.Designation = dr["designation"].ToString();
                            varclscouleur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscouleur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscouleur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscouleur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscouleur.Add(varclscouleur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'couleur' avec la classe 'clscouleur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscouleur;
        }

        public int insertClscouleur(clscouleur varclscouleur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO couleur ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscouleur.Id));
                    if (varclscouleur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclscouleur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscouleur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscouleur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscouleur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscouleur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscouleur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'couleur' avec la classe 'clscouleur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClscouleur(clscouleur varclscouleur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE couleur  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclscouleur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclscouleur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscouleur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscouleur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscouleur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscouleur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscouleur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscouleur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscouleur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'couleur' avec la classe 'clscouleur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClscouleur(clscouleur varclscouleur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM couleur  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscouleur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'couleur' avec la classe 'clscouleur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSCOULEUR 
        #region  CLSPOIDS
        public clspoids getClspoids(object intid)
        {
            clspoids varclspoids = new clspoids();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM poids WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclspoids.Id = int.Parse(dr["id"].ToString());
                            if (!dr["valeur"].ToString().Trim().Equals("")) varclspoids.Valeur = double.Parse(dr["valeur"].ToString());
                            varclspoids.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspoids.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspoids.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspoids.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'poids' avec la classe 'clspoids' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclspoids;
        }

        public List<clspoids> getAllClspoids(string criteria)
        {
            List<clspoids> lstclspoids = new List<clspoids>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM poids  WHERE";
                    sql += "  user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clspoids varclspoids = null;
                        while (dr.Read())
                        {

                            varclspoids = new clspoids();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspoids.Id = int.Parse(dr["id"].ToString());
                            if (!dr["valeur"].ToString().Trim().Equals("")) varclspoids.Valeur = double.Parse(dr["valeur"].ToString());
                            varclspoids.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspoids.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspoids.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspoids.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspoids.Add(varclspoids);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'poids' avec la classe 'clspoids' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspoids;
        }

        public List<clspoids> getAllClspoids()
        {
            List<clspoids> lstclspoids = new List<clspoids>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM poids ORDER BY valeur ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clspoids varclspoids = null;
                        while (dr.Read())
                        {

                            varclspoids = new clspoids();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspoids.Id = int.Parse(dr["id"].ToString());
                            if (!dr["valeur"].ToString().Trim().Equals("")) varclspoids.Valeur = double.Parse(dr["valeur"].ToString());
                            varclspoids.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspoids.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspoids.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspoids.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspoids.Add(varclspoids);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'poids' avec la classe 'clspoids' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspoids;
        }

        public int insertClspoids(clspoids varclspoids)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO poids ( id,valeur,user_created,date_created,user_modified,date_modified ) VALUES (@id,@valeur,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspoids.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@valeur", DbType.Single, 4, varclspoids.Valeur));
                    if (varclspoids.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspoids.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspoids.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspoids.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspoids.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspoids.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspoids.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspoids.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'poids' avec la classe 'clspoids' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClspoids(clspoids varclspoids)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE poids  SET valeur=@valeur,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@valeur", DbType.Single, 4, varclspoids.Valeur));
                    if (varclspoids.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspoids.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspoids.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspoids.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspoids.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspoids.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspoids.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspoids.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspoids.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'poids' avec la classe 'clspoids' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClspoids(clspoids varclspoids)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM poids  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspoids.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'poids' avec la classe 'clspoids' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSPOIDS 
        #region  CLSTYPE_ORDINATEUR
        public clstype_ordinateur getClstype_ordinateur(object intid)
        {
            clstype_ordinateur varclstype_ordinateur = new clstype_ordinateur();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_ordinateur WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_ordinateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_ordinateur.Designation = dr["designation"].ToString();
                            varclstype_ordinateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_ordinateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_ordinateur;
        }

        public List<clstype_ordinateur> getAllClstype_ordinateur(string criteria)
        {
            List<clstype_ordinateur> lstclstype_ordinateur = new List<clstype_ordinateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_ordinateur  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_ordinateur varclstype_ordinateur = null;
                        while (dr.Read())
                        {

                            varclstype_ordinateur = new clstype_ordinateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_ordinateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_ordinateur.Designation = dr["designation"].ToString();
                            varclstype_ordinateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_ordinateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_ordinateur.Add(varclstype_ordinateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_ordinateur;
        }

        public List<clstype_ordinateur> getAllClstype_ordinateur()
        {
            List<clstype_ordinateur> lstclstype_ordinateur = new List<clstype_ordinateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_ordinateur ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_ordinateur varclstype_ordinateur = null;
                        while (dr.Read())
                        {

                            varclstype_ordinateur = new clstype_ordinateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_ordinateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_ordinateur.Designation = dr["designation"].ToString();
                            varclstype_ordinateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_ordinateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_ordinateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_ordinateur.Add(varclstype_ordinateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_ordinateur;
        }

        public int insertClstype_ordinateur(clstype_ordinateur varclstype_ordinateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_ordinateur ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_ordinateur.Id));
                    if (varclstype_ordinateur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_ordinateur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_ordinateur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_ordinateur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_ordinateur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_ordinateur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_ordinateur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_ordinateur(clstype_ordinateur varclstype_ordinateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_ordinateur  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_ordinateur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_ordinateur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_ordinateur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_ordinateur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_ordinateur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_ordinateur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_ordinateur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_ordinateur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_ordinateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_ordinateur(clstype_ordinateur varclstype_ordinateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_ordinateur  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_ordinateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_ordinateur' avec la classe 'clstype_ordinateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_ORDINATEUR 
        #region  CLSTYPE_IMPRIMANTE
        public clstype_imprimante getClstype_imprimante(object intid)
        {
            clstype_imprimante varclstype_imprimante = new clstype_imprimante();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_imprimante WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_imprimante.Id = int.Parse(dr["id"].ToString());
                            varclstype_imprimante.Designation = dr["designation"].ToString();
                            varclstype_imprimante.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_imprimante.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_imprimante.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_imprimante.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_imprimante' avec la classe 'clstype_imprimante' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_imprimante;
        }

        public List<clstype_imprimante> getAllClstype_imprimante(string criteria)
        {
            List<clstype_imprimante> lstclstype_imprimante = new List<clstype_imprimante>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_imprimante  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_imprimante varclstype_imprimante = null;
                        while (dr.Read())
                        {

                            varclstype_imprimante = new clstype_imprimante();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_imprimante.Id = int.Parse(dr["id"].ToString());
                            varclstype_imprimante.Designation = dr["designation"].ToString();
                            varclstype_imprimante.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_imprimante.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_imprimante.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_imprimante.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_imprimante.Add(varclstype_imprimante);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_imprimante' avec la classe 'clstype_imprimante' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_imprimante;
        }

        public List<clstype_imprimante> getAllClstype_imprimante()
        {
            List<clstype_imprimante> lstclstype_imprimante = new List<clstype_imprimante>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_imprimante ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_imprimante varclstype_imprimante = null;
                        while (dr.Read())
                        {

                            varclstype_imprimante = new clstype_imprimante();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_imprimante.Id = int.Parse(dr["id"].ToString());
                            varclstype_imprimante.Designation = dr["designation"].ToString();
                            varclstype_imprimante.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_imprimante.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_imprimante.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_imprimante.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_imprimante.Add(varclstype_imprimante);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_imprimante' avec la classe 'clstype_imprimante' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_imprimante;
        }

        public int insertClstype_imprimante(clstype_imprimante varclstype_imprimante)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_imprimante ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_imprimante.Id));
                    if (varclstype_imprimante.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_imprimante.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_imprimante.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_imprimante.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_imprimante.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_imprimante.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_imprimante.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_imprimante' avec la classe 'clstype_imprimante' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_imprimante(clstype_imprimante varclstype_imprimante)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_imprimante  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_imprimante.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_imprimante.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_imprimante.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_imprimante.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_imprimante.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_imprimante.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_imprimante.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_imprimante.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_imprimante.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_imprimante' avec la classe 'clstype_imprimante' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_imprimante(clstype_imprimante varclstype_imprimante)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_imprimante  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_imprimante.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_imprimante' avec la classe 'clstype_imprimante' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_IMPRIMANTE 
        #region  CLSTYPE_AMPLIFICATEUR
        public clstype_amplificateur getClstype_amplificateur(object intid)
        {
            clstype_amplificateur varclstype_amplificateur = new clstype_amplificateur();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_amplificateur WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_amplificateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_amplificateur.Designation = dr["designation"].ToString();
                            varclstype_amplificateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_amplificateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_amplificateur;
        }

        public List<clstype_amplificateur> getAllClstype_amplificateur(string criteria)
        {
            List<clstype_amplificateur> lstclstype_amplificateur = new List<clstype_amplificateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_amplificateur  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_amplificateur varclstype_amplificateur = null;
                        while (dr.Read())
                        {

                            varclstype_amplificateur = new clstype_amplificateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_amplificateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_amplificateur.Designation = dr["designation"].ToString();
                            varclstype_amplificateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_amplificateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_amplificateur.Add(varclstype_amplificateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_amplificateur;
        }

        public List<clstype_amplificateur> getAllClstype_amplificateur()
        {
            List<clstype_amplificateur> lstclstype_amplificateur = new List<clstype_amplificateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_amplificateur ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_amplificateur varclstype_amplificateur = null;
                        while (dr.Read())
                        {

                            varclstype_amplificateur = new clstype_amplificateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_amplificateur.Id = int.Parse(dr["id"].ToString());
                            varclstype_amplificateur.Designation = dr["designation"].ToString();
                            varclstype_amplificateur.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_amplificateur.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_amplificateur.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_amplificateur.Add(varclstype_amplificateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_amplificateur;
        }

        public int insertClstype_amplificateur(clstype_amplificateur varclstype_amplificateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_amplificateur ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_amplificateur.Id));
                    if (varclstype_amplificateur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_amplificateur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_amplificateur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_amplificateur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_amplificateur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_amplificateur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_amplificateur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_amplificateur(clstype_amplificateur varclstype_amplificateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_amplificateur  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_amplificateur.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_amplificateur.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_amplificateur.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_amplificateur.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_amplificateur.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_amplificateur.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_amplificateur.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_amplificateur.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_amplificateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_amplificateur(clstype_amplificateur varclstype_amplificateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_amplificateur  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_amplificateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_amplificateur' avec la classe 'clstype_amplificateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_AMPLIFICATEUR 
        #region  CLSTYPE_ROUTEUR_AP
        public clstype_routeur_AP getClstype_routeur_AP(object intid)
        {
            clstype_routeur_AP varclstype_routeur_AP = new clstype_routeur_AP();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_routeur_AP WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_routeur_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_routeur_AP.Designation = dr["designation"].ToString();
                            varclstype_routeur_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_routeur_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_routeur_AP;
        }

        public List<clstype_routeur_AP> getAllClstype_routeur_AP(string criteria)
        {
            List<clstype_routeur_AP> lstclstype_routeur_AP = new List<clstype_routeur_AP>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_routeur_AP  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_routeur_AP varclstype_routeur_AP = null;
                        while (dr.Read())
                        {

                            varclstype_routeur_AP = new clstype_routeur_AP();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_routeur_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_routeur_AP.Designation = dr["designation"].ToString();
                            varclstype_routeur_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_routeur_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_routeur_AP.Add(varclstype_routeur_AP);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_routeur_AP;
        }

        public List<clstype_routeur_AP> getAllClstype_routeur_AP()
        {
            List<clstype_routeur_AP> lstclstype_routeur_AP = new List<clstype_routeur_AP>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_routeur_AP ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_routeur_AP varclstype_routeur_AP = null;
                        while (dr.Read())
                        {

                            varclstype_routeur_AP = new clstype_routeur_AP();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_routeur_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_routeur_AP.Designation = dr["designation"].ToString();
                            varclstype_routeur_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_routeur_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_routeur_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_routeur_AP.Add(varclstype_routeur_AP);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_routeur_AP;
        }

        public int insertClstype_routeur_AP(clstype_routeur_AP varclstype_routeur_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_routeur_AP ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_routeur_AP.Id));
                    if (varclstype_routeur_AP.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_routeur_AP.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_routeur_AP.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_routeur_AP.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_routeur_AP.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_routeur_AP.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_routeur_AP.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_routeur_AP(clstype_routeur_AP varclstype_routeur_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_routeur_AP  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_routeur_AP.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_routeur_AP.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_routeur_AP.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_routeur_AP.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_routeur_AP.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_routeur_AP.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_routeur_AP.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_routeur_AP.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_routeur_AP.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_routeur_AP(clstype_routeur_AP varclstype_routeur_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_routeur_AP  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_routeur_AP.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_routeur_AP' avec la classe 'clstype_routeur_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_ROUTEUR_AP 
        #region  CLSTYPE_AP
        public clstype_AP getClstype_AP(object intid)
        {
            clstype_AP varclstype_AP = new clstype_AP();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_AP WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_AP.Designation = dr["designation"].ToString();
                            varclstype_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_AP' avec la classe 'clstype_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_AP;
        }

        public List<clstype_AP> getAllClstype_AP(string criteria)
        {
            List<clstype_AP> lstclstype_AP = new List<clstype_AP>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_AP  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_AP varclstype_AP = null;
                        while (dr.Read())
                        {

                            varclstype_AP = new clstype_AP();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_AP.Designation = dr["designation"].ToString();
                            varclstype_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_AP.Add(varclstype_AP);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_AP' avec la classe 'clstype_AP' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_AP;
        }

        public List<clstype_AP> getAllClstype_AP()
        {
            List<clstype_AP> lstclstype_AP = new List<clstype_AP>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_AP ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_AP varclstype_AP = null;
                        while (dr.Read())
                        {

                            varclstype_AP = new clstype_AP();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_AP.Id = int.Parse(dr["id"].ToString());
                            varclstype_AP.Designation = dr["designation"].ToString();
                            varclstype_AP.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_AP.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_AP.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_AP.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_AP.Add(varclstype_AP);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_AP' avec la classe 'clstype_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_AP;
        }

        public int insertClstype_AP(clstype_AP varclstype_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_AP ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_AP.Id));
                    if (varclstype_AP.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_AP.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_AP.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_AP.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_AP.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_AP.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_AP.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_AP' avec la classe 'clstype_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_AP(clstype_AP varclstype_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_AP  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_AP.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_AP.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_AP.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_AP.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_AP.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_AP.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_AP.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_AP.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_AP.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_AP' avec la classe 'clstype_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_AP(clstype_AP varclstype_AP)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_AP  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_AP.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_AP' avec la classe 'clstype_AP' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_AP 
        #region  CLSTYPE_SWITCH
        public clstype_switch getClstype_switch(object intid)
        {
            clstype_switch varclstype_switch = new clstype_switch();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_switch WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_switch.Id = int.Parse(dr["id"].ToString());
                            varclstype_switch.Designation = dr["designation"].ToString();
                            varclstype_switch.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_switch.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_switch.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_switch.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_switch' avec la classe 'clstype_switch' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_switch;
        }

        public List<clstype_switch> getAllClstype_switch(string criteria)
        {
            List<clstype_switch> lstclstype_switch = new List<clstype_switch>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_switch  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_switch varclstype_switch = null;
                        while (dr.Read())
                        {

                            varclstype_switch = new clstype_switch();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_switch.Id = int.Parse(dr["id"].ToString());
                            varclstype_switch.Designation = dr["designation"].ToString();
                            varclstype_switch.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_switch.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_switch.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_switch.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_switch.Add(varclstype_switch);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_switch' avec la classe 'clstype_switch' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_switch;
        }

        public List<clstype_switch> getAllClstype_switch()
        {
            List<clstype_switch> lstclstype_switch = new List<clstype_switch>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_switch ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_switch varclstype_switch = null;
                        while (dr.Read())
                        {

                            varclstype_switch = new clstype_switch();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_switch.Id = int.Parse(dr["id"].ToString());
                            varclstype_switch.Designation = dr["designation"].ToString();
                            varclstype_switch.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_switch.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_switch.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_switch.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_switch.Add(varclstype_switch);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_switch' avec la classe 'clstype_switch' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_switch;
        }

        public int insertClstype_switch(clstype_switch varclstype_switch)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_switch ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_switch.Id));
                    if (varclstype_switch.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_switch.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_switch.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_switch.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_switch.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_switch.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_switch.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_switch' avec la classe 'clstype_switch' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_switch(clstype_switch varclstype_switch)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_switch  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_switch.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_switch.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_switch.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_switch.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_switch.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_switch.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_switch.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_switch.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_switch.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_switch' avec la classe 'clstype_switch' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_switch(clstype_switch varclstype_switch)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_switch  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_switch.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_switch' avec la classe 'clstype_switch' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_SWITCH 
        #region  CLSTYPE_CLAVIER
        public clstype_clavier getClstype_clavier(object intid)
        {
            clstype_clavier varclstype_clavier = new clstype_clavier();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_clavier WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_clavier.Id = int.Parse(dr["id"].ToString());
                            varclstype_clavier.Designation = dr["designation"].ToString();
                            varclstype_clavier.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_clavier.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_clavier.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_clavier.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_clavier' avec la classe 'clstype_clavier' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_clavier;
        }

        public List<clstype_clavier> getAllClstype_clavier(string criteria)
        {
            List<clstype_clavier> lstclstype_clavier = new List<clstype_clavier>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_clavier  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_clavier varclstype_clavier = null;
                        while (dr.Read())
                        {

                            varclstype_clavier = new clstype_clavier();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_clavier.Id = int.Parse(dr["id"].ToString());
                            varclstype_clavier.Designation = dr["designation"].ToString();
                            varclstype_clavier.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_clavier.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_clavier.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_clavier.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_clavier.Add(varclstype_clavier);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_clavier' avec la classe 'clstype_clavier' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_clavier;
        }

        public List<clstype_clavier> getAllClstype_clavier()
        {
            List<clstype_clavier> lstclstype_clavier = new List<clstype_clavier>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_clavier ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_clavier varclstype_clavier = null;
                        while (dr.Read())
                        {

                            varclstype_clavier = new clstype_clavier();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_clavier.Id = int.Parse(dr["id"].ToString());
                            varclstype_clavier.Designation = dr["designation"].ToString();
                            varclstype_clavier.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_clavier.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_clavier.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_clavier.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_clavier.Add(varclstype_clavier);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_clavier' avec la classe 'clstype_clavier' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_clavier;
        }

        public int insertClstype_clavier(clstype_clavier varclstype_clavier)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_clavier ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_clavier.Id));
                    if (varclstype_clavier.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclstype_clavier.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclstype_clavier.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_clavier.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_clavier.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_clavier.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_clavier.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_clavier.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_clavier.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_clavier.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_clavier' avec la classe 'clstype_clavier' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_clavier(clstype_clavier varclstype_clavier)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_clavier  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_clavier.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclstype_clavier.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclstype_clavier.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_clavier.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_clavier.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_clavier.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_clavier.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_clavier.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_clavier.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_clavier.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_clavier.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_clavier' avec la classe 'clstype_clavier' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_clavier(clstype_clavier varclstype_clavier)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_clavier  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_clavier.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_clavier' avec la classe 'clstype_clavier' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_CLAVIER 
        #region  CLSETAT_MATERIEL
        public clsetat_materiel getClsetat_materiel(object intid)
        {
            clsetat_materiel varclsetat_materiel = new clsetat_materiel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM etat_materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsetat_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsetat_materiel.Designation = dr["designation"].ToString();
                            varclsetat_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsetat_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsetat_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsetat_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'etat_materiel' avec la classe 'clsetat_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsetat_materiel;
        }

        public List<clsetat_materiel> getAllClsetat_materiel(string criteria)
        {
            List<clsetat_materiel> lstclsetat_materiel = new List<clsetat_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM etat_materiel  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsetat_materiel varclsetat_materiel = null;
                        while (dr.Read())
                        {

                            varclsetat_materiel = new clsetat_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsetat_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsetat_materiel.Designation = dr["designation"].ToString();
                            varclsetat_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsetat_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsetat_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsetat_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsetat_materiel.Add(varclsetat_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'etat_materiel' avec la classe 'clsetat_materiel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsetat_materiel;
        }

        public List<clsetat_materiel> getAllClsetat_materiel()
        {
            List<clsetat_materiel> lstclsetat_materiel = new List<clsetat_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM etat_materiel ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsetat_materiel varclsetat_materiel = null;
                        while (dr.Read())
                        {

                            varclsetat_materiel = new clsetat_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsetat_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsetat_materiel.Designation = dr["designation"].ToString();
                            varclsetat_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsetat_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsetat_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsetat_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsetat_materiel.Add(varclsetat_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'etat_materiel' avec la classe 'clsetat_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsetat_materiel;
        }

        public int insertClsetat_materiel(clsetat_materiel varclsetat_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO etat_materiel ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsetat_materiel.Id));
                    if (varclsetat_materiel.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsetat_materiel.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsetat_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsetat_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsetat_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsetat_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsetat_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'etat_materiel' avec la classe 'clsetat_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsetat_materiel(clsetat_materiel varclsetat_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE etat_materiel  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsetat_materiel.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsetat_materiel.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsetat_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsetat_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsetat_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsetat_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsetat_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsetat_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsetat_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'etat_materiel' avec la classe 'clsetat_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsetat_materiel(clsetat_materiel varclsetat_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM etat_materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsetat_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'etat_materiel' avec la classe 'clsetat_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSETAT_MATERIEL 
        #region  CLSTYPE_OS
        public clstype_OS getClstype_OS(object intid)
        {
            clstype_OS varclstype_OS = new clstype_OS();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_OS WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_OS.Id = int.Parse(dr["id"].ToString());
                            varclstype_OS.Designation = dr["designation"].ToString();
                            varclstype_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_OS' avec la classe 'clstype_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_OS;
        }

        public List<clstype_OS> getAllClstype_OS(string criteria)
        {
            List<clstype_OS> lstclstype_OS = new List<clstype_OS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_OS  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_OS varclstype_OS = null;
                        while (dr.Read())
                        {

                            varclstype_OS = new clstype_OS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_OS.Id = int.Parse(dr["id"].ToString());
                            varclstype_OS.Designation = dr["designation"].ToString();
                            varclstype_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_OS.Add(varclstype_OS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_OS' avec la classe 'clstype_OS' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_OS;
        }

        public List<clstype_OS> getAllClstype_OS()
        {
            List<clstype_OS> lstclstype_OS = new List<clstype_OS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_OS ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_OS varclstype_OS = null;
                        while (dr.Read())
                        {

                            varclstype_OS = new clstype_OS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_OS.Id = int.Parse(dr["id"].ToString());
                            varclstype_OS.Designation = dr["designation"].ToString();
                            varclstype_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_OS.Add(varclstype_OS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_OS' avec la classe 'clstype_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_OS;
        }

        public int insertClstype_OS(clstype_OS varclstype_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_OS ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_OS.Id));
                    if (varclstype_OS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, varclstype_OS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, DBNull.Value));
                    if (varclstype_OS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_OS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_OS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_OS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_OS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_OS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_OS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_OS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_OS' avec la classe 'clstype_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_OS(clstype_OS varclstype_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_OS  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_OS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, varclstype_OS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, DBNull.Value));
                    if (varclstype_OS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_OS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_OS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_OS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_OS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_OS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_OS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_OS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_OS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_OS' avec la classe 'clstype_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_OS(clstype_OS varclstype_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_OS  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_OS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_OS' avec la classe 'clstype_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_OS 
        #region  CLSARCHITECTURE_OS
        public clsarchitecture_OS getClsarchitecture_OS(object intid)
        {
            clsarchitecture_OS varclsarchitecture_OS = new clsarchitecture_OS();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM architecture_OS WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsarchitecture_OS.Id = int.Parse(dr["id"].ToString());
                            varclsarchitecture_OS.Designation = dr["designation"].ToString();
                            varclsarchitecture_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsarchitecture_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsarchitecture_OS;
        }

        public List<clsarchitecture_OS> getAllClsarchitecture_OS(string criteria)
        {
            List<clsarchitecture_OS> lstclsarchitecture_OS = new List<clsarchitecture_OS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM architecture_OS  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsarchitecture_OS varclsarchitecture_OS = null;
                        while (dr.Read())
                        {

                            varclsarchitecture_OS = new clsarchitecture_OS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsarchitecture_OS.Id = int.Parse(dr["id"].ToString());
                            varclsarchitecture_OS.Designation = dr["designation"].ToString();
                            varclsarchitecture_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsarchitecture_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsarchitecture_OS.Add(varclsarchitecture_OS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsarchitecture_OS;
        }

        public List<clsarchitecture_OS> getAllClsarchitecture_OS()
        {
            List<clsarchitecture_OS> lstclsarchitecture_OS = new List<clsarchitecture_OS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM architecture_OS ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsarchitecture_OS varclsarchitecture_OS = null;
                        while (dr.Read())
                        {

                            varclsarchitecture_OS = new clsarchitecture_OS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsarchitecture_OS.Id = int.Parse(dr["id"].ToString());
                            varclsarchitecture_OS.Designation = dr["designation"].ToString();
                            varclsarchitecture_OS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsarchitecture_OS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsarchitecture_OS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsarchitecture_OS.Add(varclsarchitecture_OS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsarchitecture_OS;
        }

        public int insertClsarchitecture_OS(clsarchitecture_OS varclsarchitecture_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO architecture_OS ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsarchitecture_OS.Id));
                    if (varclsarchitecture_OS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclsarchitecture_OS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclsarchitecture_OS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsarchitecture_OS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsarchitecture_OS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsarchitecture_OS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsarchitecture_OS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsarchitecture_OS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsarchitecture_OS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsarchitecture_OS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsarchitecture_OS(clsarchitecture_OS varclsarchitecture_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE architecture_OS  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsarchitecture_OS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclsarchitecture_OS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclsarchitecture_OS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsarchitecture_OS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsarchitecture_OS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsarchitecture_OS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsarchitecture_OS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsarchitecture_OS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsarchitecture_OS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsarchitecture_OS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsarchitecture_OS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsarchitecture_OS(clsarchitecture_OS varclsarchitecture_OS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM architecture_OS  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsarchitecture_OS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'architecture_OS' avec la classe 'clsarchitecture_OS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSARCHITECTURE_OS 
        #region  CLSOS
        public clsOS getClsOS(object intid)
        {
            clsOS varclsOS = new clsOS();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM OS WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsOS.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_type_OS"].ToString().Trim().Equals("")) varclsOS.Id_type_os = int.Parse(dr["id_type_OS"].ToString());
                            if (!dr["id_architecture_OS"].ToString().Trim().Equals("")) varclsOS.Id_architecture_os = int.Parse(dr["id_architecture_OS"].ToString());
                            varclsOS.Designation = dr["designation"].ToString();
                            varclsOS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsOS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsOS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsOS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'OS' avec la classe 'clsOS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsOS;
        }

        public List<clsOS> getAllClsOS(string criteria)
        {
            List<clsOS> lstclsOS = new List<clsOS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM OS  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsOS varclsOS = null;
                        while (dr.Read())
                        {

                            varclsOS = new clsOS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsOS.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_type_OS"].ToString().Trim().Equals("")) varclsOS.Id_type_os = int.Parse(dr["id_type_OS"].ToString());
                            if (!dr["id_architecture_OS"].ToString().Trim().Equals("")) varclsOS.Id_architecture_os = int.Parse(dr["id_architecture_OS"].ToString());
                            varclsOS.Designation = dr["designation"].ToString();
                            varclsOS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsOS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsOS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsOS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsOS.Add(varclsOS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'OS' avec la classe 'clsOS' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsOS;
        }

        public List<clsOS> getAllClsOS()
        {
            List<clsOS> lstclsOS = new List<clsOS>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM OS ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsOS varclsOS = null;
                        while (dr.Read())
                        {

                            varclsOS = new clsOS();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsOS.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_type_OS"].ToString().Trim().Equals("")) varclsOS.Id_type_os = int.Parse(dr["id_type_OS"].ToString());
                            if (!dr["id_architecture_OS"].ToString().Trim().Equals("")) varclsOS.Id_architecture_os = int.Parse(dr["id_architecture_OS"].ToString());
                            varclsOS.Designation = dr["designation"].ToString();
                            varclsOS.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsOS.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsOS.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsOS.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsOS.Add(varclsOS);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'OS' avec la classe 'clsOS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsOS;
        }

        public int insertClsOS(clsOS varclsOS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO OS ( id,id_type_OS,id_architecture_OS,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@id_type_OS,@id_architecture_OS,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsOS.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@id_type_OS", DbType.Int32, 4, varclsOS.Id_type_os));
                    cmd.Parameters.Add(getParameter(cmd, "@id_architecture_OS", DbType.Int32, 4, varclsOS.Id_architecture_os));
                    if (varclsOS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsOS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsOS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsOS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsOS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsOS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsOS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsOS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsOS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsOS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'OS' avec la classe 'clsOS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsOS(clsOS varclsOS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE OS  SET id_type_OS=@id_type_OS,id_architecture_OS=@id_architecture_OS,designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id_type_OS", DbType.Int32, 4, varclsOS.Id_type_os));
                    cmd.Parameters.Add(getParameter(cmd, "@id_architecture_OS", DbType.Int32, 4, varclsOS.Id_architecture_os));
                    if (varclsOS.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsOS.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsOS.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsOS.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsOS.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsOS.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsOS.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsOS.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsOS.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsOS.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsOS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'OS' avec la classe 'clsOS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsOS(clsOS varclsOS)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM OS  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsOS.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'OS' avec la classe 'clsOS' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSOS 
        #region  CLSVERSION_IOS
        public clsversion_ios getClsversion_ios(object intid)
        {
            clsversion_ios varclsversion_ios = new clsversion_ios();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM version_ios WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsversion_ios.Id = int.Parse(dr["id"].ToString());
                            varclsversion_ios.Designation = dr["designation"].ToString();
                            varclsversion_ios.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsversion_ios.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsversion_ios.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsversion_ios.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'version_ios' avec la classe 'clsversion_ios' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsversion_ios;
        }

        public List<clsversion_ios> getAllClsversion_ios(string criteria)
        {
            List<clsversion_ios> lstclsversion_ios = new List<clsversion_ios>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM version_ios  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsversion_ios varclsversion_ios = null;
                        while (dr.Read())
                        {

                            varclsversion_ios = new clsversion_ios();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsversion_ios.Id = int.Parse(dr["id"].ToString());
                            varclsversion_ios.Designation = dr["designation"].ToString();
                            varclsversion_ios.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsversion_ios.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsversion_ios.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsversion_ios.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsversion_ios.Add(varclsversion_ios);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'version_ios' avec la classe 'clsversion_ios' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsversion_ios;
        }

        public List<clsversion_ios> getAllClsversion_ios()
        {
            List<clsversion_ios> lstclsversion_ios = new List<clsversion_ios>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM version_ios ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsversion_ios varclsversion_ios = null;
                        while (dr.Read())
                        {

                            varclsversion_ios = new clsversion_ios();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsversion_ios.Id = int.Parse(dr["id"].ToString());
                            varclsversion_ios.Designation = dr["designation"].ToString();
                            varclsversion_ios.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsversion_ios.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsversion_ios.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsversion_ios.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsversion_ios.Add(varclsversion_ios);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'version_ios' avec la classe 'clsversion_ios' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsversion_ios;
        }

        public int insertClsversion_ios(clsversion_ios varclsversion_ios)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO version_ios ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsversion_ios.Id));
                    if (varclsversion_ios.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclsversion_ios.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclsversion_ios.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsversion_ios.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsversion_ios.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsversion_ios.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsversion_ios.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsversion_ios.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsversion_ios.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsversion_ios.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'version_ios' avec la classe 'clsversion_ios' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsversion_ios(clsversion_ios varclsversion_ios)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE version_ios  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsversion_ios.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, varclsversion_ios.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 10, DBNull.Value));
                    if (varclsversion_ios.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsversion_ios.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsversion_ios.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsversion_ios.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsversion_ios.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsversion_ios.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsversion_ios.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsversion_ios.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsversion_ios.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'version_ios' avec la classe 'clsversion_ios' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsversion_ios(clsversion_ios varclsversion_ios)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM version_ios  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsversion_ios.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'version_ios' avec la classe 'clsversion_ios' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSVERSION_IOS 
        #region  CLSNETETTE
        public clsnetette getClsnetette(object intid)
        {
            clsnetette varclsnetette = new clsnetette();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM netette WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsnetette.Id = int.Parse(dr["id"].ToString());
                            varclsnetette.Designation = dr["designation"].ToString();
                            varclsnetette.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsnetette.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsnetette.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsnetette.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'netette' avec la classe 'clsnetette' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsnetette;
        }

        public List<clsnetette> getAllClsnetette(string criteria)
        {
            List<clsnetette> lstclsnetette = new List<clsnetette>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM netette  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsnetette varclsnetette = null;
                        while (dr.Read())
                        {

                            varclsnetette = new clsnetette();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsnetette.Id = int.Parse(dr["id"].ToString());
                            varclsnetette.Designation = dr["designation"].ToString();
                            varclsnetette.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsnetette.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsnetette.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsnetette.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsnetette.Add(varclsnetette);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'netette' avec la classe 'clsnetette' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsnetette;
        }

        public List<clsnetette> getAllClsnetette()
        {
            List<clsnetette> lstclsnetette = new List<clsnetette>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM netette ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsnetette varclsnetette = null;
                        while (dr.Read())
                        {

                            varclsnetette = new clsnetette();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsnetette.Id = int.Parse(dr["id"].ToString());
                            varclsnetette.Designation = dr["designation"].ToString();
                            varclsnetette.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsnetette.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsnetette.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsnetette.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsnetette.Add(varclsnetette);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'netette' avec la classe 'clsnetette' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsnetette;
        }

        public int insertClsnetette(clsnetette varclsnetette)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO netette ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsnetette.Id));
                    if (varclsnetette.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, varclsnetette.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, DBNull.Value));
                    if (varclsnetette.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsnetette.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsnetette.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsnetette.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsnetette.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsnetette.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsnetette.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsnetette.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'netette' avec la classe 'clsnetette' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsnetette(clsnetette varclsnetette)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE netette  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsnetette.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, varclsnetette.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 20, DBNull.Value));
                    if (varclsnetette.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsnetette.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsnetette.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsnetette.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsnetette.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsnetette.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsnetette.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsnetette.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsnetette.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'netette' avec la classe 'clsnetette' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsnetette(clsnetette varclsnetette)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM netette  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsnetette.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'netette' avec la classe 'clsnetette' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSNETETTE 
        #region  CLSMATERIEL
        public clsmateriel getClsmateriel(object intid)
        {
            clsmateriel varclsmateriel = new clsmateriel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsmateriel.Id = int.Parse(dr["id"].ToString());
                            varclsmateriel.Code_str = dr["code_str"].ToString();
                            if (!dr["id_categorie_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_categorie_materiel = int.Parse(dr["id_categorie_materiel"].ToString());
                            if (!dr["id_compte"].ToString().Trim().Equals("")) varclsmateriel.Id_compte = int.Parse(dr["id_compte"].ToString());
                            varclsmateriel.Qrcode = dr["qrcode"].ToString();
                            if (!dr["date_acquisition"].ToString().Trim().Equals("")) varclsmateriel.Date_acquisition = DateTime.Parse(dr["date_acquisition"].ToString());
                            if (!dr["guarantie"].ToString().Trim().Equals("")) varclsmateriel.Guarantie = int.Parse(dr["guarantie"].ToString());
                            if (!dr["id_marque"].ToString().Trim().Equals("")) varclsmateriel.Id_marque = int.Parse(dr["id_marque"].ToString());
                            if (!dr["id_modele"].ToString().Trim().Equals("")) varclsmateriel.Id_modele = int.Parse(dr["id_modele"].ToString());
                            if (!dr["id_couleur"].ToString().Trim().Equals("")) varclsmateriel.Id_couleur = int.Parse(dr["id_couleur"].ToString());
                            if (!dr["id_poids"].ToString().Trim().Equals("")) varclsmateriel.Id_poids = int.Parse(dr["id_poids"].ToString());
                            if (!dr["id_etat_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_etat_materiel = int.Parse(dr["id_etat_materiel"].ToString());
                            varclsmateriel.Photo1 = dr["photo1"].ToString();
                            varclsmateriel.Photo2 = dr["photo2"].ToString();
                            varclsmateriel.Photo3 = dr["photo3"].ToString();
                            varclsmateriel.Label = dr["label"].ToString();
                            varclsmateriel.Mac_adresse1 = dr["mac_adresse1"].ToString();
                            varclsmateriel.Mac_adresse2 = dr["mac_adresse2"].ToString();
                            varclsmateriel.Commentaire = dr["commentaire"].ToString();
                            varclsmateriel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmateriel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmateriel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmateriel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            if (!dr["id_type_ordinateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ordinateur = int.Parse(dr["id_type_ordinateur"].ToString());
                            if (!dr["id_type_clavier"].ToString().Trim().Equals("")) varclsmateriel.Id_type_clavier = int.Parse(dr["id_type_clavier"].ToString());
                            if (!dr["id_OS"].ToString().Trim().Equals("")) varclsmateriel.Id_os = int.Parse(dr["id_OS"].ToString());
                            if (!dr["ram"].ToString().Trim().Equals("")) varclsmateriel.Ram = double.Parse(dr["ram"].ToString());
                            if (!dr["processeur"].ToString().Trim().Equals("")) varclsmateriel.Processeur = double.Parse(dr["processeur"].ToString());
                            if (!dr["nombre_coeur_processeur"].ToString().Trim().Equals("")) varclsmateriel.Nombre_coeur_processeur = int.Parse(dr["nombre_coeur_processeur"].ToString());
                            if (!dr["nombre_hdd"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdd = int.Parse(dr["nombre_hdd"].ToString());
                            if (!dr["capacite_hdd"].ToString().Trim().Equals("")) varclsmateriel.Capacite_hdd = int.Parse(dr["capacite_hdd"].ToString());
                            if (!dr["indice_performance"].ToString().Trim().Equals("")) varclsmateriel.Indice_performance = int.Parse(dr["indice_performance"].ToString());
                            if (!dr["pouce"].ToString().Trim().Equals("")) varclsmateriel.Pouce = int.Parse(dr["pouce"].ToString());
                            if (!dr["nombre_usb2"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb2 = int.Parse(dr["nombre_usb2"].ToString());
                            if (!dr["nombre_usb3"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb3 = int.Parse(dr["nombre_usb3"].ToString());
                            if (!dr["nombre_hdmi"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdmi = int.Parse(dr["nombre_hdmi"].ToString());
                            if (!dr["nombre_vga"].ToString().Trim().Equals("")) varclsmateriel.Nombre_vga = int.Parse(dr["nombre_vga"].ToString());
                            if (!dr["tension_batterie"].ToString().Trim().Equals("")) varclsmateriel.Tension_batterie = double.Parse(dr["tension_batterie"].ToString());
                            if (!dr["tension_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Tension_adaptateur = double.Parse(dr["tension_adaptateur"].ToString());
                            if (!dr["puissance_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Puissance_adaptateur = double.Parse(dr["puissance_adaptateur"].ToString());
                            if (!dr["intensite_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Intensite_adaptateur = double.Parse(dr["intensite_adaptateur"].ToString());
                            if (!dr["numero_cle"].ToString().Trim().Equals("")) varclsmateriel.Numero_cle = int.Parse(dr["numero_cle"].ToString());
                            if (!dr["id_type_imprimante"].ToString().Trim().Equals("")) varclsmateriel.Id_type_imprimante = int.Parse(dr["id_type_imprimante"].ToString());
                            if (!dr["puissance"].ToString().Trim().Equals("")) varclsmateriel.Puissance = double.Parse(dr["puissance"].ToString());
                            if (!dr["intensite"].ToString().Trim().Equals("")) varclsmateriel.Intensite = double.Parse(dr["intensite"].ToString());
                            if (!dr["nombre_page_par_minute"].ToString().Trim().Equals("")) varclsmateriel.Nombre_page_par_minute = double.Parse(dr["nombre_page_par_minute"].ToString());
                            if (!dr["id_type_amplificateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_amplificateur = int.Parse(dr["id_type_amplificateur"].ToString());
                            if (!dr["tension_alimentation"].ToString().Trim().Equals("")) varclsmateriel.Tension_alimentation = int.Parse(dr["tension_alimentation"].ToString());
                            if (!dr["nombre_usb"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb = int.Parse(dr["nombre_usb"].ToString());
                            if (!dr["nombre_memoire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_memoire = int.Parse(dr["nombre_memoire"].ToString());
                            if (!dr["nombre_sorties_audio"].ToString().Trim().Equals("")) varclsmateriel.Nombre_sorties_audio = int.Parse(dr["nombre_sorties_audio"].ToString());
                            if (!dr["nombre_microphone"].ToString().Trim().Equals("")) varclsmateriel.Nombre_microphone = int.Parse(dr["nombre_microphone"].ToString());
                            if (!dr["gain"].ToString().Trim().Equals("")) varclsmateriel.Gain = double.Parse(dr["gain"].ToString());
                            if (!dr["id_type_routeur_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_routeur_ap = int.Parse(dr["id_type_routeur_AP"].ToString());
                            if (!dr["id_version_ios"].ToString().Trim().Equals("")) varclsmateriel.Id_version_ios = int.Parse(dr["id_version_ios"].ToString());
                            if (!dr["nombre_gbe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_gbe = int.Parse(dr["nombre_gbe"].ToString());
                            if (!dr["nombre_fe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fe = int.Parse(dr["nombre_fe"].ToString());
                            if (!dr["nombre_fo"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fo = int.Parse(dr["nombre_fo"].ToString());
                            if (!dr["nombre_serial"].ToString().Trim().Equals("")) varclsmateriel.Nombre_serial = int.Parse(dr["nombre_serial"].ToString());
                            if (!dr["capable_usb"].ToString().Trim().Equals("")) varclsmateriel.Capable_usb = bool.Parse(dr["capable_usb"].ToString());
                            varclsmateriel.Motpasse_defaut = dr["motpasse_defaut"].ToString();
                            varclsmateriel.Default_ip = dr["default_IP"].ToString();
                            if (!dr["nombre_console"].ToString().Trim().Equals("")) varclsmateriel.Nombre_console = int.Parse(dr["nombre_console"].ToString());
                            if (!dr["nombre_auxiliaire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_auxiliaire = int.Parse(dr["nombre_auxiliaire"].ToString());
                            if (!dr["id_type_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ap = int.Parse(dr["id_type_AP"].ToString());
                            if (!dr["id_type_switch"].ToString().Trim().Equals("")) varclsmateriel.Id_type_switch = int.Parse(dr["id_type_switch"].ToString());
                            if (!dr["frequence"].ToString().Trim().Equals("")) varclsmateriel.Frequence = double.Parse(dr["frequence"].ToString());
                            varclsmateriel.Alimentation = dr["alimentation"].ToString();
                            if (!dr["nombre_antenne"].ToString().Trim().Equals("")) varclsmateriel.Nombre_antenne = int.Parse(dr["nombre_antenne"].ToString());
                            if (!dr["id_netette"].ToString().Trim().Equals("")) varclsmateriel.Id_netette = int.Parse(dr["id_netette"].ToString());
                            if (!dr["compatible_wifi"].ToString().Trim().Equals("")) varclsmateriel.Compatible_wifi = bool.Parse(dr["compatible_wifi"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'materiel' avec la classe 'clsmateriel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsmateriel;
        }

        public List<clsmateriel> getAllClsmateriel(string criteria)
        {
            List<clsmateriel> lstclsmateriel = new List<clsmateriel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM materiel  WHERE";
                    sql += "  code_str LIKE '%" + criteria + "%'";
                    sql += "  OR   qrcode LIKE '%" + criteria + "%'";
                    sql += "  OR   photo1 LIKE '%" + criteria + "%'";
                    sql += "  OR   photo2 LIKE '%" + criteria + "%'";
                    sql += "  OR   photo3 LIKE '%" + criteria + "%'";
                    sql += "  OR   label LIKE '%" + criteria + "%'";
                    sql += "  OR   mac_adresse1 LIKE '%" + criteria + "%'";
                    sql += "  OR   mac_adresse2 LIKE '%" + criteria + "%'";
                    sql += "  OR   commentaire LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    sql += "  OR   motpasse_defaut LIKE '%" + criteria + "%'";
                    sql += "  OR   default_IP LIKE '%" + criteria + "%'";
                    sql += "  OR   alimentation LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmateriel varclsmateriel = null;
                        while (dr.Read())
                        {

                            varclsmateriel = new clsmateriel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmateriel.Id = int.Parse(dr["id"].ToString());
                            varclsmateriel.Code_str = dr["code_str"].ToString();
                            if (!dr["id_categorie_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_categorie_materiel = int.Parse(dr["id_categorie_materiel"].ToString());
                            if (!dr["id_compte"].ToString().Trim().Equals("")) varclsmateriel.Id_compte = int.Parse(dr["id_compte"].ToString());
                            varclsmateriel.Qrcode = dr["qrcode"].ToString();
                            if (!dr["date_acquisition"].ToString().Trim().Equals("")) varclsmateriel.Date_acquisition = DateTime.Parse(dr["date_acquisition"].ToString());
                            if (!dr["guarantie"].ToString().Trim().Equals("")) varclsmateriel.Guarantie = int.Parse(dr["guarantie"].ToString());
                            if (!dr["id_marque"].ToString().Trim().Equals("")) varclsmateriel.Id_marque = int.Parse(dr["id_marque"].ToString());
                            if (!dr["id_modele"].ToString().Trim().Equals("")) varclsmateriel.Id_modele = int.Parse(dr["id_modele"].ToString());
                            if (!dr["id_couleur"].ToString().Trim().Equals("")) varclsmateriel.Id_couleur = int.Parse(dr["id_couleur"].ToString());
                            if (!dr["id_poids"].ToString().Trim().Equals("")) varclsmateriel.Id_poids = int.Parse(dr["id_poids"].ToString());
                            if (!dr["id_etat_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_etat_materiel = int.Parse(dr["id_etat_materiel"].ToString());
                            varclsmateriel.Photo1 = dr["photo1"].ToString();
                            varclsmateriel.Photo2 = dr["photo2"].ToString();
                            varclsmateriel.Photo3 = dr["photo3"].ToString();
                            varclsmateriel.Label = dr["label"].ToString();
                            varclsmateriel.Mac_adresse1 = dr["mac_adresse1"].ToString();
                            varclsmateriel.Mac_adresse2 = dr["mac_adresse2"].ToString();
                            varclsmateriel.Commentaire = dr["commentaire"].ToString();
                            varclsmateriel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmateriel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmateriel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmateriel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            if (!dr["id_type_ordinateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ordinateur = int.Parse(dr["id_type_ordinateur"].ToString());
                            if (!dr["id_type_clavier"].ToString().Trim().Equals("")) varclsmateriel.Id_type_clavier = int.Parse(dr["id_type_clavier"].ToString());
                            if (!dr["id_OS"].ToString().Trim().Equals("")) varclsmateriel.Id_os = int.Parse(dr["id_OS"].ToString());
                            if (!dr["ram"].ToString().Trim().Equals("")) varclsmateriel.Ram = double.Parse(dr["ram"].ToString());
                            if (!dr["processeur"].ToString().Trim().Equals("")) varclsmateriel.Processeur = double.Parse(dr["processeur"].ToString());
                            if (!dr["nombre_coeur_processeur"].ToString().Trim().Equals("")) varclsmateriel.Nombre_coeur_processeur = int.Parse(dr["nombre_coeur_processeur"].ToString());
                            if (!dr["nombre_hdd"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdd = int.Parse(dr["nombre_hdd"].ToString());
                            if (!dr["capacite_hdd"].ToString().Trim().Equals("")) varclsmateriel.Capacite_hdd = int.Parse(dr["capacite_hdd"].ToString());
                            if (!dr["indice_performance"].ToString().Trim().Equals("")) varclsmateriel.Indice_performance = int.Parse(dr["indice_performance"].ToString());
                            if (!dr["pouce"].ToString().Trim().Equals("")) varclsmateriel.Pouce = int.Parse(dr["pouce"].ToString());
                            if (!dr["nombre_usb2"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb2 = int.Parse(dr["nombre_usb2"].ToString());
                            if (!dr["nombre_usb3"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb3 = int.Parse(dr["nombre_usb3"].ToString());
                            if (!dr["nombre_hdmi"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdmi = int.Parse(dr["nombre_hdmi"].ToString());
                            if (!dr["nombre_vga"].ToString().Trim().Equals("")) varclsmateriel.Nombre_vga = int.Parse(dr["nombre_vga"].ToString());
                            if (!dr["tension_batterie"].ToString().Trim().Equals("")) varclsmateriel.Tension_batterie = double.Parse(dr["tension_batterie"].ToString());
                            if (!dr["tension_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Tension_adaptateur = double.Parse(dr["tension_adaptateur"].ToString());
                            if (!dr["puissance_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Puissance_adaptateur = double.Parse(dr["puissance_adaptateur"].ToString());
                            if (!dr["intensite_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Intensite_adaptateur = double.Parse(dr["intensite_adaptateur"].ToString());
                            if (!dr["numero_cle"].ToString().Trim().Equals("")) varclsmateriel.Numero_cle = int.Parse(dr["numero_cle"].ToString());
                            if (!dr["id_type_imprimante"].ToString().Trim().Equals("")) varclsmateriel.Id_type_imprimante = int.Parse(dr["id_type_imprimante"].ToString());
                            if (!dr["puissance"].ToString().Trim().Equals("")) varclsmateriel.Puissance = double.Parse(dr["puissance"].ToString());
                            if (!dr["intensite"].ToString().Trim().Equals("")) varclsmateriel.Intensite = double.Parse(dr["intensite"].ToString());
                            if (!dr["nombre_page_par_minute"].ToString().Trim().Equals("")) varclsmateriel.Nombre_page_par_minute = double.Parse(dr["nombre_page_par_minute"].ToString());
                            if (!dr["id_type_amplificateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_amplificateur = int.Parse(dr["id_type_amplificateur"].ToString());
                            if (!dr["tension_alimentation"].ToString().Trim().Equals("")) varclsmateriel.Tension_alimentation = int.Parse(dr["tension_alimentation"].ToString());
                            if (!dr["nombre_usb"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb = int.Parse(dr["nombre_usb"].ToString());
                            if (!dr["nombre_memoire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_memoire = int.Parse(dr["nombre_memoire"].ToString());
                            if (!dr["nombre_sorties_audio"].ToString().Trim().Equals("")) varclsmateriel.Nombre_sorties_audio = int.Parse(dr["nombre_sorties_audio"].ToString());
                            if (!dr["nombre_microphone"].ToString().Trim().Equals("")) varclsmateriel.Nombre_microphone = int.Parse(dr["nombre_microphone"].ToString());
                            if (!dr["gain"].ToString().Trim().Equals("")) varclsmateriel.Gain = double.Parse(dr["gain"].ToString());
                            if (!dr["id_type_routeur_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_routeur_ap = int.Parse(dr["id_type_routeur_AP"].ToString());
                            if (!dr["id_version_ios"].ToString().Trim().Equals("")) varclsmateriel.Id_version_ios = int.Parse(dr["id_version_ios"].ToString());
                            if (!dr["nombre_gbe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_gbe = int.Parse(dr["nombre_gbe"].ToString());
                            if (!dr["nombre_fe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fe = int.Parse(dr["nombre_fe"].ToString());
                            if (!dr["nombre_fo"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fo = int.Parse(dr["nombre_fo"].ToString());
                            if (!dr["nombre_serial"].ToString().Trim().Equals("")) varclsmateriel.Nombre_serial = int.Parse(dr["nombre_serial"].ToString());
                            if (!dr["capable_usb"].ToString().Trim().Equals("")) varclsmateriel.Capable_usb = bool.Parse(dr["capable_usb"].ToString());
                            varclsmateriel.Motpasse_defaut = dr["motpasse_defaut"].ToString();
                            varclsmateriel.Default_ip = dr["default_IP"].ToString();
                            if (!dr["nombre_console"].ToString().Trim().Equals("")) varclsmateriel.Nombre_console = int.Parse(dr["nombre_console"].ToString());
                            if (!dr["nombre_auxiliaire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_auxiliaire = int.Parse(dr["nombre_auxiliaire"].ToString());
                            if (!dr["id_type_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ap = int.Parse(dr["id_type_AP"].ToString());
                            if (!dr["id_type_switch"].ToString().Trim().Equals("")) varclsmateriel.Id_type_switch = int.Parse(dr["id_type_switch"].ToString());
                            if (!dr["frequence"].ToString().Trim().Equals("")) varclsmateriel.Frequence = double.Parse(dr["frequence"].ToString());
                            varclsmateriel.Alimentation = dr["alimentation"].ToString();
                            if (!dr["nombre_antenne"].ToString().Trim().Equals("")) varclsmateriel.Nombre_antenne = int.Parse(dr["nombre_antenne"].ToString());
                            if (!dr["id_netette"].ToString().Trim().Equals("")) varclsmateriel.Id_netette = int.Parse(dr["id_netette"].ToString());
                            if (!dr["compatible_wifi"].ToString().Trim().Equals("")) varclsmateriel.Compatible_wifi = bool.Parse(dr["compatible_wifi"].ToString());
                            lstclsmateriel.Add(varclsmateriel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'materiel' avec la classe 'clsmateriel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmateriel;
        }

        public List<clsmateriel> getAllClsmateriel()
        {
            List<clsmateriel> lstclsmateriel = new List<clsmateriel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM materiel ORDER BY code_str ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsmateriel varclsmateriel = null;
                        while (dr.Read())
                        {

                            varclsmateriel = new clsmateriel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsmateriel.Id = int.Parse(dr["id"].ToString());
                            varclsmateriel.Code_str = dr["code_str"].ToString();
                            if (!dr["id_categorie_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_categorie_materiel = int.Parse(dr["id_categorie_materiel"].ToString());
                            if (!dr["id_compte"].ToString().Trim().Equals("")) varclsmateriel.Id_compte = int.Parse(dr["id_compte"].ToString());
                            varclsmateriel.Qrcode = dr["qrcode"].ToString();
                            if (!dr["date_acquisition"].ToString().Trim().Equals("")) varclsmateriel.Date_acquisition = DateTime.Parse(dr["date_acquisition"].ToString());
                            if (!dr["guarantie"].ToString().Trim().Equals("")) varclsmateriel.Guarantie = int.Parse(dr["guarantie"].ToString());
                            if (!dr["id_marque"].ToString().Trim().Equals("")) varclsmateriel.Id_marque = int.Parse(dr["id_marque"].ToString());
                            if (!dr["id_modele"].ToString().Trim().Equals("")) varclsmateriel.Id_modele = int.Parse(dr["id_modele"].ToString());
                            if (!dr["id_couleur"].ToString().Trim().Equals("")) varclsmateriel.Id_couleur = int.Parse(dr["id_couleur"].ToString());
                            if (!dr["id_poids"].ToString().Trim().Equals("")) varclsmateriel.Id_poids = int.Parse(dr["id_poids"].ToString());
                            if (!dr["id_etat_materiel"].ToString().Trim().Equals("")) varclsmateriel.Id_etat_materiel = int.Parse(dr["id_etat_materiel"].ToString());
                            varclsmateriel.Photo1 = dr["photo1"].ToString();
                            varclsmateriel.Photo2 = dr["photo2"].ToString();
                            varclsmateriel.Photo3 = dr["photo3"].ToString();
                            varclsmateriel.Label = dr["label"].ToString();
                            varclsmateriel.Mac_adresse1 = dr["mac_adresse1"].ToString();
                            varclsmateriel.Mac_adresse2 = dr["mac_adresse2"].ToString();
                            varclsmateriel.Commentaire = dr["commentaire"].ToString();
                            varclsmateriel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsmateriel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsmateriel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsmateriel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            if (!dr["id_type_ordinateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ordinateur = int.Parse(dr["id_type_ordinateur"].ToString());
                            if (!dr["id_type_clavier"].ToString().Trim().Equals("")) varclsmateriel.Id_type_clavier = int.Parse(dr["id_type_clavier"].ToString());
                            if (!dr["id_OS"].ToString().Trim().Equals("")) varclsmateriel.Id_os = int.Parse(dr["id_OS"].ToString());
                            if (!dr["ram"].ToString().Trim().Equals("")) varclsmateriel.Ram = double.Parse(dr["ram"].ToString());
                            if (!dr["processeur"].ToString().Trim().Equals("")) varclsmateriel.Processeur = double.Parse(dr["processeur"].ToString());
                            if (!dr["nombre_coeur_processeur"].ToString().Trim().Equals("")) varclsmateriel.Nombre_coeur_processeur = int.Parse(dr["nombre_coeur_processeur"].ToString());
                            if (!dr["nombre_hdd"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdd = int.Parse(dr["nombre_hdd"].ToString());
                            if (!dr["capacite_hdd"].ToString().Trim().Equals("")) varclsmateriel.Capacite_hdd = int.Parse(dr["capacite_hdd"].ToString());
                            if (!dr["indice_performance"].ToString().Trim().Equals("")) varclsmateriel.Indice_performance = int.Parse(dr["indice_performance"].ToString());
                            if (!dr["pouce"].ToString().Trim().Equals("")) varclsmateriel.Pouce = int.Parse(dr["pouce"].ToString());
                            if (!dr["nombre_usb2"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb2 = int.Parse(dr["nombre_usb2"].ToString());
                            if (!dr["nombre_usb3"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb3 = int.Parse(dr["nombre_usb3"].ToString());
                            if (!dr["nombre_hdmi"].ToString().Trim().Equals("")) varclsmateriel.Nombre_hdmi = int.Parse(dr["nombre_hdmi"].ToString());
                            if (!dr["nombre_vga"].ToString().Trim().Equals("")) varclsmateriel.Nombre_vga = int.Parse(dr["nombre_vga"].ToString());
                            if (!dr["tension_batterie"].ToString().Trim().Equals("")) varclsmateriel.Tension_batterie = double.Parse(dr["tension_batterie"].ToString());
                            if (!dr["tension_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Tension_adaptateur = double.Parse(dr["tension_adaptateur"].ToString());
                            if (!dr["puissance_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Puissance_adaptateur = double.Parse(dr["puissance_adaptateur"].ToString());
                            if (!dr["intensite_adaptateur"].ToString().Trim().Equals("")) varclsmateriel.Intensite_adaptateur = double.Parse(dr["intensite_adaptateur"].ToString());
                            if (!dr["numero_cle"].ToString().Trim().Equals("")) varclsmateriel.Numero_cle = int.Parse(dr["numero_cle"].ToString());
                            if (!dr["id_type_imprimante"].ToString().Trim().Equals("")) varclsmateriel.Id_type_imprimante = int.Parse(dr["id_type_imprimante"].ToString());
                            if (!dr["puissance"].ToString().Trim().Equals("")) varclsmateriel.Puissance = double.Parse(dr["puissance"].ToString());
                            if (!dr["intensite"].ToString().Trim().Equals("")) varclsmateriel.Intensite = double.Parse(dr["intensite"].ToString());
                            if (!dr["nombre_page_par_minute"].ToString().Trim().Equals("")) varclsmateriel.Nombre_page_par_minute = double.Parse(dr["nombre_page_par_minute"].ToString());
                            if (!dr["id_type_amplificateur"].ToString().Trim().Equals("")) varclsmateriel.Id_type_amplificateur = int.Parse(dr["id_type_amplificateur"].ToString());
                            if (!dr["tension_alimentation"].ToString().Trim().Equals("")) varclsmateriel.Tension_alimentation = int.Parse(dr["tension_alimentation"].ToString());
                            if (!dr["nombre_usb"].ToString().Trim().Equals("")) varclsmateriel.Nombre_usb = int.Parse(dr["nombre_usb"].ToString());
                            if (!dr["nombre_memoire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_memoire = int.Parse(dr["nombre_memoire"].ToString());
                            if (!dr["nombre_sorties_audio"].ToString().Trim().Equals("")) varclsmateriel.Nombre_sorties_audio = int.Parse(dr["nombre_sorties_audio"].ToString());
                            if (!dr["nombre_microphone"].ToString().Trim().Equals("")) varclsmateriel.Nombre_microphone = int.Parse(dr["nombre_microphone"].ToString());
                            if (!dr["gain"].ToString().Trim().Equals("")) varclsmateriel.Gain = double.Parse(dr["gain"].ToString());
                            if (!dr["id_type_routeur_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_routeur_ap = int.Parse(dr["id_type_routeur_AP"].ToString());
                            if (!dr["id_version_ios"].ToString().Trim().Equals("")) varclsmateriel.Id_version_ios = int.Parse(dr["id_version_ios"].ToString());
                            if (!dr["nombre_gbe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_gbe = int.Parse(dr["nombre_gbe"].ToString());
                            if (!dr["nombre_fe"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fe = int.Parse(dr["nombre_fe"].ToString());
                            if (!dr["nombre_fo"].ToString().Trim().Equals("")) varclsmateriel.Nombre_fo = int.Parse(dr["nombre_fo"].ToString());
                            if (!dr["nombre_serial"].ToString().Trim().Equals("")) varclsmateriel.Nombre_serial = int.Parse(dr["nombre_serial"].ToString());
                            if (!dr["capable_usb"].ToString().Trim().Equals("")) varclsmateriel.Capable_usb = bool.Parse(dr["capable_usb"].ToString());
                            varclsmateriel.Motpasse_defaut = dr["motpasse_defaut"].ToString();
                            varclsmateriel.Default_ip = dr["default_IP"].ToString();
                            if (!dr["nombre_console"].ToString().Trim().Equals("")) varclsmateriel.Nombre_console = int.Parse(dr["nombre_console"].ToString());
                            if (!dr["nombre_auxiliaire"].ToString().Trim().Equals("")) varclsmateriel.Nombre_auxiliaire = int.Parse(dr["nombre_auxiliaire"].ToString());
                            if (!dr["id_type_AP"].ToString().Trim().Equals("")) varclsmateriel.Id_type_ap = int.Parse(dr["id_type_AP"].ToString());
                            if (!dr["id_type_switch"].ToString().Trim().Equals("")) varclsmateriel.Id_type_switch = int.Parse(dr["id_type_switch"].ToString());
                            if (!dr["frequence"].ToString().Trim().Equals("")) varclsmateriel.Frequence = double.Parse(dr["frequence"].ToString());
                            varclsmateriel.Alimentation = dr["alimentation"].ToString();
                            if (!dr["nombre_antenne"].ToString().Trim().Equals("")) varclsmateriel.Nombre_antenne = int.Parse(dr["nombre_antenne"].ToString());
                            if (!dr["id_netette"].ToString().Trim().Equals("")) varclsmateriel.Id_netette = int.Parse(dr["id_netette"].ToString());
                            if (!dr["compatible_wifi"].ToString().Trim().Equals("")) varclsmateriel.Compatible_wifi = bool.Parse(dr["compatible_wifi"].ToString());
                            lstclsmateriel.Add(varclsmateriel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'materiel' avec la classe 'clsmateriel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsmateriel;
        }

        public int insertClsmateriel(clsmateriel varclsmateriel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO materiel ( id,code_str,id_categorie_materiel,id_compte,qrcode,date_acquisition,guarantie,id_marque,id_modele,id_couleur,id_poids,id_etat_materiel,photo1,photo2,photo3,label,mac_adresse1,mac_adresse2,commentaire,user_created,date_created,user_modified,date_modified,id_type_ordinateur,id_type_clavier,id_OS,ram,processeur,nombre_coeur_processeur,nombre_hdd,capacite_hdd,indice_performance,pouce,nombre_usb2,nombre_usb3,nombre_hdmi,nombre_vga,tension_batterie,tension_adaptateur,puissance_adaptateur,intensite_adaptateur,numero_cle,id_type_imprimante,puissance,intensite,nombre_page_par_minute,id_type_amplificateur,tension_alimentation,nombre_usb,nombre_memoire,nombre_sorties_audio,nombre_microphone,gain,id_type_routeur_AP,id_version_ios,nombre_gbe,nombre_fe,nombre_fo,nombre_serial,capable_usb,motpasse_defaut,default_IP,nombre_console,nombre_auxiliaire,id_type_AP,id_type_switch,frequence,alimentation,nombre_antenne,id_netette,compatible_wifi ) VALUES (@id,@code_str,@id_categorie_materiel,@id_compte,@qrcode,@date_acquisition,@guarantie,@id_marque,@id_modele,@id_couleur,@id_poids,@id_etat_materiel,@photo1,@photo2,@photo3,@label,@mac_adresse1,@mac_adresse2,@commentaire,@user_created,@date_created,@user_modified,@date_modified,@id_type_ordinateur,@id_type_clavier,@id_OS,@ram,@processeur,@nombre_coeur_processeur,@nombre_hdd,@capacite_hdd,@indice_performance,@pouce,@nombre_usb2,@nombre_usb3,@nombre_hdmi,@nombre_vga,@tension_batterie,@tension_adaptateur,@puissance_adaptateur,@intensite_adaptateur,@numero_cle,@id_type_imprimante,@puissance,@intensite,@nombre_page_par_minute,@id_type_amplificateur,@tension_alimentation,@nombre_usb,@nombre_memoire,@nombre_sorties_audio,@nombre_microphone,@gain,@id_type_routeur_AP,@id_version_ios,@nombre_gbe,@nombre_fe,@nombre_fo,@nombre_serial,@capable_usb,@motpasse_defaut,@default_IP,@nombre_console,@nombre_auxiliaire,@id_type_AP,@id_type_switch,@frequence,@alimentation,@nombre_antenne,@id_netette,@compatible_wifi  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmateriel.Id));
                    if (varclsmateriel.Code_str != null) cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 10, varclsmateriel.Code_str));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 10, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_categorie_materiel", DbType.Int32, 4, varclsmateriel.Id_categorie_materiel));
                    cmd.Parameters.Add(getParameter(cmd, "@id_compte", DbType.Int32, 4, varclsmateriel.Id_compte));
                    if (varclsmateriel.Qrcode != null) cmd.Parameters.Add(getParameter(cmd, "@qrcode", DbType.Object, 2147483647, varclsmateriel.Qrcode));
                    else cmd.Parameters.Add(getParameter(cmd, "@qrcode", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Date_acquisition.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_acquisition", DbType.DateTime, 8, varclsmateriel.Date_acquisition));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_acquisition", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.Guarantie.HasValue) cmd.Parameters.Add(getParameter(cmd, "@guarantie", DbType.Int32, 4, varclsmateriel.Guarantie));
                    else cmd.Parameters.Add(getParameter(cmd, "@guarantie", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_marque", DbType.Int32, 4, varclsmateriel.Id_marque));
                    cmd.Parameters.Add(getParameter(cmd, "@id_modele", DbType.Int32, 4, varclsmateriel.Id_modele));
                    cmd.Parameters.Add(getParameter(cmd, "@id_couleur", DbType.Int32, 4, varclsmateriel.Id_couleur));
                    cmd.Parameters.Add(getParameter(cmd, "@id_poids", DbType.Int32, 4, varclsmateriel.Id_poids));
                    cmd.Parameters.Add(getParameter(cmd, "@id_etat_materiel", DbType.Int32, 4, varclsmateriel.Id_etat_materiel));
                    if (varclsmateriel.Photo1 != null) cmd.Parameters.Add(getParameter(cmd, "@photo1", DbType.Object, 2147483647, varclsmateriel.Photo1));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo1", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Photo2 != null) cmd.Parameters.Add(getParameter(cmd, "@photo2", DbType.Object, 2147483647, varclsmateriel.Photo2));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo2", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Photo3 != null) cmd.Parameters.Add(getParameter(cmd, "@photo3", DbType.Object, 2147483647, varclsmateriel.Photo3));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo3", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Label != null) cmd.Parameters.Add(getParameter(cmd, "@label", DbType.String, 20, varclsmateriel.Label));
                    else cmd.Parameters.Add(getParameter(cmd, "@label", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Mac_adresse1 != null) cmd.Parameters.Add(getParameter(cmd, "@mac_adresse1", DbType.String, 20, varclsmateriel.Mac_adresse1));
                    else cmd.Parameters.Add(getParameter(cmd, "@mac_adresse1", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Mac_adresse2 != null) cmd.Parameters.Add(getParameter(cmd, "@mac_adresse2", DbType.String, 20, varclsmateriel.Mac_adresse2));
                    else cmd.Parameters.Add(getParameter(cmd, "@mac_adresse2", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Commentaire != null) cmd.Parameters.Add(getParameter(cmd, "@commentaire", DbType.String, 400, varclsmateriel.Commentaire));
                    else cmd.Parameters.Add(getParameter(cmd, "@commentaire", DbType.String, 400, DBNull.Value));
                    if (varclsmateriel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmateriel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmateriel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmateriel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmateriel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.Id_type_ordinateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_ordinateur", DbType.Int32, 4, varclsmateriel.Id_type_ordinateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_ordinateur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_clavier.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_clavier", DbType.Int32, 4, varclsmateriel.Id_type_clavier));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_clavier", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_os.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_OS", DbType.Int32, 4, varclsmateriel.Id_os));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_OS", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Ram.HasValue) cmd.Parameters.Add(getParameter(cmd, "@ram", DbType.Single, 4, varclsmateriel.Ram));
                    else cmd.Parameters.Add(getParameter(cmd, "@ram", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Processeur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@processeur", DbType.Single, 4, varclsmateriel.Processeur));
                    else cmd.Parameters.Add(getParameter(cmd, "@processeur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_coeur_processeur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_coeur_processeur", DbType.Int32, 4, varclsmateriel.Nombre_coeur_processeur));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_coeur_processeur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_hdd.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_hdd", DbType.Int32, 4, varclsmateriel.Nombre_hdd));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_hdd", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Capacite_hdd.HasValue) cmd.Parameters.Add(getParameter(cmd, "@capacite_hdd", DbType.Int32, 4, varclsmateriel.Capacite_hdd));
                    else cmd.Parameters.Add(getParameter(cmd, "@capacite_hdd", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Indice_performance.HasValue) cmd.Parameters.Add(getParameter(cmd, "@indice_performance", DbType.Int32, 4, varclsmateriel.Indice_performance));
                    else cmd.Parameters.Add(getParameter(cmd, "@indice_performance", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Pouce.HasValue) cmd.Parameters.Add(getParameter(cmd, "@pouce", DbType.Int32, 4, varclsmateriel.Pouce));
                    else cmd.Parameters.Add(getParameter(cmd, "@pouce", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb2.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb2", DbType.Int32, 4, varclsmateriel.Nombre_usb2));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb2", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb3.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb3", DbType.Int32, 4, varclsmateriel.Nombre_usb3));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb3", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_hdmi.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_hdmi", DbType.Int32, 4, varclsmateriel.Nombre_hdmi));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_hdmi", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_vga.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_vga", DbType.Int32, 4, varclsmateriel.Nombre_vga));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_vga", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Tension_batterie.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_batterie", DbType.Single, 4, varclsmateriel.Tension_batterie));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_batterie", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Tension_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_adaptateur", DbType.Single, 4, varclsmateriel.Tension_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Puissance_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@puissance_adaptateur", DbType.Single, 4, varclsmateriel.Puissance_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@puissance_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Intensite_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@intensite_adaptateur", DbType.Single, 4, varclsmateriel.Intensite_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@intensite_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Numero_cle.HasValue) cmd.Parameters.Add(getParameter(cmd, "@numero_cle", DbType.Int32, 4, varclsmateriel.Numero_cle));
                    else cmd.Parameters.Add(getParameter(cmd, "@numero_cle", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_imprimante.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_imprimante", DbType.Int32, 4, varclsmateriel.Id_type_imprimante));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_imprimante", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Puissance.HasValue) cmd.Parameters.Add(getParameter(cmd, "@puissance", DbType.Single, 4, varclsmateriel.Puissance));
                    else cmd.Parameters.Add(getParameter(cmd, "@puissance", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Intensite.HasValue) cmd.Parameters.Add(getParameter(cmd, "@intensite", DbType.Single, 4, varclsmateriel.Intensite));
                    else cmd.Parameters.Add(getParameter(cmd, "@intensite", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_page_par_minute.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_page_par_minute", DbType.Single, 4, varclsmateriel.Nombre_page_par_minute));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_page_par_minute", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_amplificateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_amplificateur", DbType.Int32, 4, varclsmateriel.Id_type_amplificateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_amplificateur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Tension_alimentation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_alimentation", DbType.Int32, 4, varclsmateriel.Tension_alimentation));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_alimentation", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb", DbType.Int32, 4, varclsmateriel.Nombre_usb));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_memoire.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_memoire", DbType.Int32, 4, varclsmateriel.Nombre_memoire));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_memoire", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_sorties_audio.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_sorties_audio", DbType.Int32, 4, varclsmateriel.Nombre_sorties_audio));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_sorties_audio", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_microphone.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_microphone", DbType.Int32, 4, varclsmateriel.Nombre_microphone));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_microphone", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Gain.HasValue) cmd.Parameters.Add(getParameter(cmd, "@gain", DbType.Single, 4, varclsmateriel.Gain));
                    else cmd.Parameters.Add(getParameter(cmd, "@gain", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_routeur_ap.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_routeur_AP", DbType.Int32, 4, varclsmateriel.Id_type_routeur_ap));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_routeur_AP", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_version_ios.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_version_ios", DbType.Int32, 4, varclsmateriel.Id_version_ios));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_version_ios", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_gbe.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_gbe", DbType.Int32, 4, varclsmateriel.Nombre_gbe));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_gbe", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_fe.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_fe", DbType.Int32, 4, varclsmateriel.Nombre_fe));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_fe", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_fo.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_fo", DbType.Int32, 4, varclsmateriel.Nombre_fo));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_fo", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_serial.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_serial", DbType.Int32, 4, varclsmateriel.Nombre_serial));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_serial", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Capable_usb.HasValue) cmd.Parameters.Add(getParameter(cmd, "@capable_usb", DbType.Boolean, 2, varclsmateriel.Capable_usb));
                    else cmd.Parameters.Add(getParameter(cmd, "@capable_usb", DbType.Boolean, 2, DBNull.Value));
                    if (varclsmateriel.Motpasse_defaut != null) cmd.Parameters.Add(getParameter(cmd, "@motpasse_defaut", DbType.String, 20, varclsmateriel.Motpasse_defaut));
                    else cmd.Parameters.Add(getParameter(cmd, "@motpasse_defaut", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Default_ip != null) cmd.Parameters.Add(getParameter(cmd, "@default_IP", DbType.String, 50, varclsmateriel.Default_ip));
                    else cmd.Parameters.Add(getParameter(cmd, "@default_IP", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Nombre_console.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_console", DbType.Int32, 4, varclsmateriel.Nombre_console));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_console", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_auxiliaire.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_auxiliaire", DbType.Int32, 4, varclsmateriel.Nombre_auxiliaire));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_auxiliaire", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_ap.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_AP", DbType.Int32, 4, varclsmateriel.Id_type_ap));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_AP", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_switch.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_switch", DbType.Int32, 4, varclsmateriel.Id_type_switch));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_switch", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Frequence.HasValue) cmd.Parameters.Add(getParameter(cmd, "@frequence", DbType.Single, 4, varclsmateriel.Frequence));
                    else cmd.Parameters.Add(getParameter(cmd, "@frequence", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Alimentation != null) cmd.Parameters.Add(getParameter(cmd, "@alimentation", DbType.String, 20, varclsmateriel.Alimentation));
                    else cmd.Parameters.Add(getParameter(cmd, "@alimentation", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Nombre_antenne.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_antenne", DbType.Int32, 4, varclsmateriel.Nombre_antenne));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_antenne", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_netette.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_netette", DbType.Int32, 4, varclsmateriel.Id_netette));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_netette", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Compatible_wifi.HasValue) cmd.Parameters.Add(getParameter(cmd, "@compatible_wifi", DbType.Boolean, 2, varclsmateriel.Compatible_wifi));
                    else cmd.Parameters.Add(getParameter(cmd, "@compatible_wifi", DbType.Boolean, 2, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'materiel' avec la classe 'clsmateriel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsmateriel(clsmateriel varclsmateriel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE materiel  SET code_str=@code_str,id_categorie_materiel=@id_categorie_materiel,id_compte=@id_compte,qrcode=@qrcode,date_acquisition=@date_acquisition,guarantie=@guarantie,id_marque=@id_marque,id_modele=@id_modele,id_couleur=@id_couleur,id_poids=@id_poids,id_etat_materiel=@id_etat_materiel,photo1=@photo1,photo2=@photo2,photo3=@photo3,label=@label,mac_adresse1=@mac_adresse1,mac_adresse2=@mac_adresse2,commentaire=@commentaire,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified,id_type_ordinateur=@id_type_ordinateur,id_type_clavier=@id_type_clavier,id_OS=@id_OS,ram=@ram,processeur=@processeur,nombre_coeur_processeur=@nombre_coeur_processeur,nombre_hdd=@nombre_hdd,capacite_hdd=@capacite_hdd,indice_performance=@indice_performance,pouce=@pouce,nombre_usb2=@nombre_usb2,nombre_usb3=@nombre_usb3,nombre_hdmi=@nombre_hdmi,nombre_vga=@nombre_vga,tension_batterie=@tension_batterie,tension_adaptateur=@tension_adaptateur,puissance_adaptateur=@puissance_adaptateur,intensite_adaptateur=@intensite_adaptateur,numero_cle=@numero_cle,id_type_imprimante=@id_type_imprimante,puissance=@puissance,intensite=@intensite,nombre_page_par_minute=@nombre_page_par_minute,id_type_amplificateur=@id_type_amplificateur,tension_alimentation=@tension_alimentation,nombre_usb=@nombre_usb,nombre_memoire=@nombre_memoire,nombre_sorties_audio=@nombre_sorties_audio,nombre_microphone=@nombre_microphone,gain=@gain,id_type_routeur_AP=@id_type_routeur_AP,id_version_ios=@id_version_ios,nombre_gbe=@nombre_gbe,nombre_fe=@nombre_fe,nombre_fo=@nombre_fo,nombre_serial=@nombre_serial,capable_usb=@capable_usb,motpasse_defaut=@motpasse_defaut,default_IP=@default_IP,nombre_console=@nombre_console,nombre_auxiliaire=@nombre_auxiliaire,id_type_AP=@id_type_AP,id_type_switch=@id_type_switch,frequence=@frequence,alimentation=@alimentation,nombre_antenne=@nombre_antenne,id_netette=@id_netette,compatible_wifi=@compatible_wifi  WHERE 1=1  AND id=@id ");
                    if (varclsmateriel.Code_str != null) cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 10, varclsmateriel.Code_str));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 10, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_categorie_materiel", DbType.Int32, 4, varclsmateriel.Id_categorie_materiel));
                    cmd.Parameters.Add(getParameter(cmd, "@id_compte", DbType.Int32, 4, varclsmateriel.Id_compte));
                    if (varclsmateriel.Qrcode != null) cmd.Parameters.Add(getParameter(cmd, "@qrcode", DbType.Object, 2147483647, varclsmateriel.Qrcode));
                    else cmd.Parameters.Add(getParameter(cmd, "@qrcode", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Date_acquisition.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_acquisition", DbType.DateTime, 8, varclsmateriel.Date_acquisition));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_acquisition", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.Guarantie.HasValue) cmd.Parameters.Add(getParameter(cmd, "@guarantie", DbType.Int32, 4, varclsmateriel.Guarantie));
                    else cmd.Parameters.Add(getParameter(cmd, "@guarantie", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_marque", DbType.Int32, 4, varclsmateriel.Id_marque));
                    cmd.Parameters.Add(getParameter(cmd, "@id_modele", DbType.Int32, 4, varclsmateriel.Id_modele));
                    cmd.Parameters.Add(getParameter(cmd, "@id_couleur", DbType.Int32, 4, varclsmateriel.Id_couleur));
                    cmd.Parameters.Add(getParameter(cmd, "@id_poids", DbType.Int32, 4, varclsmateriel.Id_poids));
                    cmd.Parameters.Add(getParameter(cmd, "@id_etat_materiel", DbType.Int32, 4, varclsmateriel.Id_etat_materiel));
                    if (varclsmateriel.Photo1 != null) cmd.Parameters.Add(getParameter(cmd, "@photo1", DbType.Object, 2147483647, varclsmateriel.Photo1));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo1", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Photo2 != null) cmd.Parameters.Add(getParameter(cmd, "@photo2", DbType.Object, 2147483647, varclsmateriel.Photo2));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo2", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Photo3 != null) cmd.Parameters.Add(getParameter(cmd, "@photo3", DbType.Object, 2147483647, varclsmateriel.Photo3));
                    else cmd.Parameters.Add(getParameter(cmd, "@photo3", DbType.Object, 2147483647, DBNull.Value));
                    if (varclsmateriel.Label != null) cmd.Parameters.Add(getParameter(cmd, "@label", DbType.String, 20, varclsmateriel.Label));
                    else cmd.Parameters.Add(getParameter(cmd, "@label", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Mac_adresse1 != null) cmd.Parameters.Add(getParameter(cmd, "@mac_adresse1", DbType.String, 20, varclsmateriel.Mac_adresse1));
                    else cmd.Parameters.Add(getParameter(cmd, "@mac_adresse1", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Mac_adresse2 != null) cmd.Parameters.Add(getParameter(cmd, "@mac_adresse2", DbType.String, 20, varclsmateriel.Mac_adresse2));
                    else cmd.Parameters.Add(getParameter(cmd, "@mac_adresse2", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Commentaire != null) cmd.Parameters.Add(getParameter(cmd, "@commentaire", DbType.String, 400, varclsmateriel.Commentaire));
                    else cmd.Parameters.Add(getParameter(cmd, "@commentaire", DbType.String, 400, DBNull.Value));
                    if (varclsmateriel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsmateriel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsmateriel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsmateriel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsmateriel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    if (varclsmateriel.Id_type_ordinateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_ordinateur", DbType.Int32, 4, varclsmateriel.Id_type_ordinateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_ordinateur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_clavier.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_clavier", DbType.Int32, 4, varclsmateriel.Id_type_clavier));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_clavier", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_os.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_OS", DbType.Int32, 4, varclsmateriel.Id_os));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_OS", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Ram.HasValue) cmd.Parameters.Add(getParameter(cmd, "@ram", DbType.Single, 4, varclsmateriel.Ram));
                    else cmd.Parameters.Add(getParameter(cmd, "@ram", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Processeur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@processeur", DbType.Single, 4, varclsmateriel.Processeur));
                    else cmd.Parameters.Add(getParameter(cmd, "@processeur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_coeur_processeur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_coeur_processeur", DbType.Int32, 4, varclsmateriel.Nombre_coeur_processeur));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_coeur_processeur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_hdd.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_hdd", DbType.Int32, 4, varclsmateriel.Nombre_hdd));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_hdd", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Capacite_hdd.HasValue) cmd.Parameters.Add(getParameter(cmd, "@capacite_hdd", DbType.Int32, 4, varclsmateriel.Capacite_hdd));
                    else cmd.Parameters.Add(getParameter(cmd, "@capacite_hdd", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Indice_performance.HasValue) cmd.Parameters.Add(getParameter(cmd, "@indice_performance", DbType.Int32, 4, varclsmateriel.Indice_performance));
                    else cmd.Parameters.Add(getParameter(cmd, "@indice_performance", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Pouce.HasValue) cmd.Parameters.Add(getParameter(cmd, "@pouce", DbType.Int32, 4, varclsmateriel.Pouce));
                    else cmd.Parameters.Add(getParameter(cmd, "@pouce", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb2.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb2", DbType.Int32, 4, varclsmateriel.Nombre_usb2));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb2", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb3.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb3", DbType.Int32, 4, varclsmateriel.Nombre_usb3));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb3", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_hdmi.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_hdmi", DbType.Int32, 4, varclsmateriel.Nombre_hdmi));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_hdmi", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_vga.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_vga", DbType.Int32, 4, varclsmateriel.Nombre_vga));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_vga", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Tension_batterie.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_batterie", DbType.Single, 4, varclsmateriel.Tension_batterie));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_batterie", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Tension_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_adaptateur", DbType.Single, 4, varclsmateriel.Tension_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Puissance_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@puissance_adaptateur", DbType.Single, 4, varclsmateriel.Puissance_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@puissance_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Intensite_adaptateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@intensite_adaptateur", DbType.Single, 4, varclsmateriel.Intensite_adaptateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@intensite_adaptateur", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Numero_cle.HasValue) cmd.Parameters.Add(getParameter(cmd, "@numero_cle", DbType.Int32, 4, varclsmateriel.Numero_cle));
                    else cmd.Parameters.Add(getParameter(cmd, "@numero_cle", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_imprimante.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_imprimante", DbType.Int32, 4, varclsmateriel.Id_type_imprimante));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_imprimante", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Puissance.HasValue) cmd.Parameters.Add(getParameter(cmd, "@puissance", DbType.Single, 4, varclsmateriel.Puissance));
                    else cmd.Parameters.Add(getParameter(cmd, "@puissance", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Intensite.HasValue) cmd.Parameters.Add(getParameter(cmd, "@intensite", DbType.Single, 4, varclsmateriel.Intensite));
                    else cmd.Parameters.Add(getParameter(cmd, "@intensite", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_page_par_minute.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_page_par_minute", DbType.Single, 4, varclsmateriel.Nombre_page_par_minute));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_page_par_minute", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_amplificateur.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_amplificateur", DbType.Int32, 4, varclsmateriel.Id_type_amplificateur));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_amplificateur", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Tension_alimentation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@tension_alimentation", DbType.Int32, 4, varclsmateriel.Tension_alimentation));
                    else cmd.Parameters.Add(getParameter(cmd, "@tension_alimentation", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_usb.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_usb", DbType.Int32, 4, varclsmateriel.Nombre_usb));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_usb", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_memoire.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_memoire", DbType.Int32, 4, varclsmateriel.Nombre_memoire));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_memoire", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_sorties_audio.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_sorties_audio", DbType.Int32, 4, varclsmateriel.Nombre_sorties_audio));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_sorties_audio", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_microphone.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_microphone", DbType.Int32, 4, varclsmateriel.Nombre_microphone));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_microphone", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Gain.HasValue) cmd.Parameters.Add(getParameter(cmd, "@gain", DbType.Single, 4, varclsmateriel.Gain));
                    else cmd.Parameters.Add(getParameter(cmd, "@gain", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_routeur_ap.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_routeur_AP", DbType.Int32, 4, varclsmateriel.Id_type_routeur_ap));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_routeur_AP", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_version_ios.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_version_ios", DbType.Int32, 4, varclsmateriel.Id_version_ios));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_version_ios", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_gbe.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_gbe", DbType.Int32, 4, varclsmateriel.Nombre_gbe));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_gbe", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_fe.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_fe", DbType.Int32, 4, varclsmateriel.Nombre_fe));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_fe", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_fo.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_fo", DbType.Int32, 4, varclsmateriel.Nombre_fo));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_fo", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_serial.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_serial", DbType.Int32, 4, varclsmateriel.Nombre_serial));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_serial", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Capable_usb.HasValue) cmd.Parameters.Add(getParameter(cmd, "@capable_usb", DbType.Boolean, 2, varclsmateriel.Capable_usb));
                    else cmd.Parameters.Add(getParameter(cmd, "@capable_usb", DbType.Boolean, 2, DBNull.Value));
                    if (varclsmateriel.Motpasse_defaut != null) cmd.Parameters.Add(getParameter(cmd, "@motpasse_defaut", DbType.String, 20, varclsmateriel.Motpasse_defaut));
                    else cmd.Parameters.Add(getParameter(cmd, "@motpasse_defaut", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Default_ip != null) cmd.Parameters.Add(getParameter(cmd, "@default_IP", DbType.String, 50, varclsmateriel.Default_ip));
                    else cmd.Parameters.Add(getParameter(cmd, "@default_IP", DbType.String, 50, DBNull.Value));
                    if (varclsmateriel.Nombre_console.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_console", DbType.Int32, 4, varclsmateriel.Nombre_console));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_console", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Nombre_auxiliaire.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_auxiliaire", DbType.Int32, 4, varclsmateriel.Nombre_auxiliaire));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_auxiliaire", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_ap.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_AP", DbType.Int32, 4, varclsmateriel.Id_type_ap));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_AP", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_type_switch.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_type_switch", DbType.Int32, 4, varclsmateriel.Id_type_switch));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_type_switch", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Frequence.HasValue) cmd.Parameters.Add(getParameter(cmd, "@frequence", DbType.Single, 4, varclsmateriel.Frequence));
                    else cmd.Parameters.Add(getParameter(cmd, "@frequence", DbType.Single, 4, DBNull.Value));
                    if (varclsmateriel.Alimentation != null) cmd.Parameters.Add(getParameter(cmd, "@alimentation", DbType.String, 20, varclsmateriel.Alimentation));
                    else cmd.Parameters.Add(getParameter(cmd, "@alimentation", DbType.String, 20, DBNull.Value));
                    if (varclsmateriel.Nombre_antenne.HasValue) cmd.Parameters.Add(getParameter(cmd, "@nombre_antenne", DbType.Int32, 4, varclsmateriel.Nombre_antenne));
                    else cmd.Parameters.Add(getParameter(cmd, "@nombre_antenne", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Id_netette.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_netette", DbType.Int32, 4, varclsmateriel.Id_netette));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_netette", DbType.Int32, 4, DBNull.Value));
                    if (varclsmateriel.Compatible_wifi.HasValue) cmd.Parameters.Add(getParameter(cmd, "@compatible_wifi", DbType.Boolean, 2, varclsmateriel.Compatible_wifi));
                    else cmd.Parameters.Add(getParameter(cmd, "@compatible_wifi", DbType.Boolean, 2, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmateriel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'materiel' avec la classe 'clsmateriel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsmateriel(clsmateriel varclsmateriel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsmateriel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'materiel' avec la classe 'clsmateriel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSMATERIEL 
        #region  CLSGRADE
        public clsgrade getClsgrade(object intid)
        {
            clsgrade varclsgrade = new clsgrade();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM grade WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsgrade.Id = int.Parse(dr["id"].ToString());
                            varclsgrade.Designation = dr["designation"].ToString();
                            varclsgrade.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsgrade.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsgrade.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsgrade.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'grade' avec la classe 'clsgrade' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsgrade;
        }

        public List<clsgrade> getAllClsgrade(string criteria)
        {
            List<clsgrade> lstclsgrade = new List<clsgrade>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM grade  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsgrade varclsgrade = null;
                        while (dr.Read())
                        {

                            varclsgrade = new clsgrade();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsgrade.Id = int.Parse(dr["id"].ToString());
                            varclsgrade.Designation = dr["designation"].ToString();
                            varclsgrade.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsgrade.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsgrade.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsgrade.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsgrade.Add(varclsgrade);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'grade' avec la classe 'clsgrade' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsgrade;
        }

        public List<clsgrade> getAllClsgrade()
        {
            List<clsgrade> lstclsgrade = new List<clsgrade>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM grade ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsgrade varclsgrade = null;
                        while (dr.Read())
                        {

                            varclsgrade = new clsgrade();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsgrade.Id = int.Parse(dr["id"].ToString());
                            varclsgrade.Designation = dr["designation"].ToString();
                            varclsgrade.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsgrade.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsgrade.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsgrade.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsgrade.Add(varclsgrade);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'grade' avec la classe 'clsgrade' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsgrade;
        }

        public int insertClsgrade(clsgrade varclsgrade)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO grade ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgrade.Id));
                    if (varclsgrade.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsgrade.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsgrade.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsgrade.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsgrade.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsgrade.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsgrade.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'grade' avec la classe 'clsgrade' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsgrade(clsgrade varclsgrade)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE grade  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsgrade.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsgrade.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsgrade.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsgrade.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsgrade.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsgrade.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsgrade.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsgrade.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgrade.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'grade' avec la classe 'clsgrade' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsgrade(clsgrade varclsgrade)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM grade  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsgrade.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'grade' avec la classe 'clsgrade' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSGRADE 
        #region  CLSPERSONNE
        public clspersonne getClspersonne(object intid)
        {
            clspersonne varclspersonne = new clspersonne();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM personne WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclspersonne.Id = int.Parse(dr["id"].ToString());
                            varclspersonne.Nom = dr["nom"].ToString();
                            varclspersonne.Postnom = dr["postnom"].ToString();
                            varclspersonne.Prenom = dr["prenom"].ToString();
                            if (!dr["id_grade"].ToString().Trim().Equals("")) varclspersonne.Id_grade = int.Parse(dr["id_grade"].ToString());
                            if (!dr["isenseignant"].ToString().Trim().Equals("")) varclspersonne.Isenseignant = bool.Parse(dr["isenseignant"].ToString());
                            if (!dr["isagent"].ToString().Trim().Equals("")) varclspersonne.Isagent = bool.Parse(dr["isagent"].ToString());
                            if (!dr["isetudiant"].ToString().Trim().Equals("")) varclspersonne.Isetudiant = bool.Parse(dr["isetudiant"].ToString());
                            varclspersonne.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspersonne.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspersonne.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspersonne.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'personne' avec la classe 'clspersonne' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclspersonne;
        }

        public List<clspersonne> getAllClspersonne(string criteria)
        {
            List<clspersonne> lstclspersonne = new List<clspersonne>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM personne  WHERE";
                    sql += "   nom LIKE '%" + criteria + "%'";
                    sql += "  OR   postnom LIKE '%" + criteria + "%'";
                    sql += "  OR   prenom LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clspersonne varclspersonne = null;
                        while (dr.Read())
                        {

                            varclspersonne = new clspersonne();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspersonne.Id = int.Parse(dr["id"].ToString());
                            varclspersonne.Nom = dr["nom"].ToString();
                            varclspersonne.Postnom = dr["postnom"].ToString();
                            varclspersonne.Prenom = dr["prenom"].ToString();
                            if (!dr["id_grade"].ToString().Trim().Equals("")) varclspersonne.Id_grade = int.Parse(dr["id_grade"].ToString());
                            if (!dr["isenseignant"].ToString().Trim().Equals("")) varclspersonne.Isenseignant = bool.Parse(dr["isenseignant"].ToString());
                            if (!dr["isagent"].ToString().Trim().Equals("")) varclspersonne.Isagent = bool.Parse(dr["isagent"].ToString());
                            if (!dr["isetudiant"].ToString().Trim().Equals("")) varclspersonne.Isetudiant = bool.Parse(dr["isetudiant"].ToString());
                            varclspersonne.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspersonne.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspersonne.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspersonne.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspersonne.Add(varclspersonne);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'personne' avec la classe 'clspersonne' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspersonne;
        }

        public List<clspersonne> getAllClspersonne()
        {
            List<clspersonne> lstclspersonne = new List<clspersonne>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM personne ORDER BY nom ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clspersonne varclspersonne = null;
                        while (dr.Read())
                        {
                            varclspersonne = new clspersonne();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspersonne.Id = int.Parse(dr["id"].ToString());
                            varclspersonne.Nom = dr["nom"].ToString();
                            varclspersonne.Postnom = dr["postnom"].ToString();
                            varclspersonne.Prenom = dr["prenom"].ToString();
                            if (!dr["id_grade"].ToString().Trim().Equals("")) varclspersonne.Id_grade = int.Parse(dr["id_grade"].ToString());
                            if (!dr["isenseignant"].ToString().Trim().Equals("")) varclspersonne.Isenseignant = bool.Parse(dr["isenseignant"].ToString());
                            if (!dr["isagent"].ToString().Trim().Equals("")) varclspersonne.Isagent = bool.Parse(dr["isagent"].ToString());
                            if (!dr["isetudiant"].ToString().Trim().Equals("")) varclspersonne.Isetudiant = bool.Parse(dr["isetudiant"].ToString());
                            varclspersonne.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspersonne.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspersonne.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspersonne.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspersonne.Add(varclspersonne);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'personne' avec la classe 'clspersonne' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspersonne;
        }

        public int insertClspersonne(clspersonne varclspersonne)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO personne ( id,nom,postnom,prenom,id_grade,isenseignant,isagent,isetudiant,user_created,date_created,user_modified,date_modified ) VALUES (@id,@nom,@postnom,@prenom,@id_grade,@isenseignant,@isagent,@isetudiant,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspersonne.Id));
                    if (varclspersonne.Nom != null) cmd.Parameters.Add(getParameter(cmd, "@nom", DbType.String, 50, varclspersonne.Nom));
                    else cmd.Parameters.Add(getParameter(cmd, "@nom", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Postnom != null) cmd.Parameters.Add(getParameter(cmd, "@postnom", DbType.String, 30, varclspersonne.Postnom));
                    else cmd.Parameters.Add(getParameter(cmd, "@postnom", DbType.String, 30, DBNull.Value));
                    if (varclspersonne.Prenom != null) cmd.Parameters.Add(getParameter(cmd, "@prenom", DbType.String, 30, varclspersonne.Prenom));
                    else cmd.Parameters.Add(getParameter(cmd, "@prenom", DbType.String, 30, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_grade", DbType.Int32, 4, varclspersonne.Id_grade));
                    if (varclspersonne.Isenseignant.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isenseignant", DbType.Boolean, 2, varclspersonne.Isenseignant));
                    else cmd.Parameters.Add(getParameter(cmd, "@isenseignant", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.Isagent.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isagent", DbType.Boolean, 2, varclspersonne.Isagent));
                    else cmd.Parameters.Add(getParameter(cmd, "@isagent", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.Isetudiant.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isetudiant", DbType.Boolean, 2, varclspersonne.Isetudiant));
                    else cmd.Parameters.Add(getParameter(cmd, "@isetudiant", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspersonne.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspersonne.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspersonne.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspersonne.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspersonne.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'personne' avec la classe 'clspersonne' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClspersonne(clspersonne varclspersonne)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE personne  SET nom=@nom,postnom=@postnom,prenom=@prenom,id_grade=@id_grade,isenseignant=@isenseignant,isagent=@isagent,isetudiant=@isetudiant,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclspersonne.Nom != null) cmd.Parameters.Add(getParameter(cmd, "@nom", DbType.String, 50, varclspersonne.Nom));
                    else cmd.Parameters.Add(getParameter(cmd, "@nom", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Postnom != null) cmd.Parameters.Add(getParameter(cmd, "@postnom", DbType.String, 30, varclspersonne.Postnom));
                    else cmd.Parameters.Add(getParameter(cmd, "@postnom", DbType.String, 30, DBNull.Value));
                    if (varclspersonne.Prenom != null) cmd.Parameters.Add(getParameter(cmd, "@prenom", DbType.String, 30, varclspersonne.Prenom));
                    else cmd.Parameters.Add(getParameter(cmd, "@prenom", DbType.String, 30, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_grade", DbType.Int32, 4, varclspersonne.Id_grade));
                    if (varclspersonne.Isenseignant.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isenseignant", DbType.Boolean, 2, varclspersonne.Isenseignant));
                    else cmd.Parameters.Add(getParameter(cmd, "@isenseignant", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.Isagent.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isagent", DbType.Boolean, 2, varclspersonne.Isagent));
                    else cmd.Parameters.Add(getParameter(cmd, "@isagent", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.Isetudiant.HasValue) cmd.Parameters.Add(getParameter(cmd, "@isetudiant", DbType.Boolean, 2, varclspersonne.Isetudiant));
                    else cmd.Parameters.Add(getParameter(cmd, "@isetudiant", DbType.Boolean, 2, DBNull.Value));
                    if (varclspersonne.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspersonne.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspersonne.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspersonne.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspersonne.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspersonne.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspersonne.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspersonne.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'personne' avec la classe 'clspersonne' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClspersonne(clspersonne varclspersonne)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM personne  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspersonne.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'personne' avec la classe 'clspersonne' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSPERSONNE 
        #region  CLSTYPE_LIEU_AFFECTATION
        public clstype_lieu_affectation getClstype_lieu_affectation(object intid)
        {
            clstype_lieu_affectation varclstype_lieu_affectation = new clstype_lieu_affectation();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_lieu_affectation WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclstype_lieu_affectation.Designation = dr["designation"].ToString();
                            varclstype_lieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_lieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclstype_lieu_affectation;
        }

        public List<clstype_lieu_affectation> getAllClstype_lieu_affectation(string criteria)
        {
            List<clstype_lieu_affectation> lstclstype_lieu_affectation = new List<clstype_lieu_affectation>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM type_lieu_affectation  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_lieu_affectation varclstype_lieu_affectation = null;
                        while (dr.Read())
                        {

                            varclstype_lieu_affectation = new clstype_lieu_affectation();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclstype_lieu_affectation.Designation = dr["designation"].ToString();
                            varclstype_lieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_lieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_lieu_affectation.Add(varclstype_lieu_affectation);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_lieu_affectation;
        }

        public List<clstype_lieu_affectation> getAllClstype_lieu_affectation()
        {
            List<clstype_lieu_affectation> lstclstype_lieu_affectation = new List<clstype_lieu_affectation>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM type_lieu_affectation ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clstype_lieu_affectation varclstype_lieu_affectation = null;
                        while (dr.Read())
                        {

                            varclstype_lieu_affectation = new clstype_lieu_affectation();
                            if (!dr["id"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclstype_lieu_affectation.Designation = dr["designation"].ToString();
                            varclstype_lieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclstype_lieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclstype_lieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclstype_lieu_affectation.Add(varclstype_lieu_affectation);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstype_lieu_affectation;
        }

        public int insertClstype_lieu_affectation(clstype_lieu_affectation varclstype_lieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO type_lieu_affectation ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_lieu_affectation.Id));
                    if (varclstype_lieu_affectation.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_lieu_affectation.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_lieu_affectation.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_lieu_affectation.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_lieu_affectation.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_lieu_affectation.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_lieu_affectation.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClstype_lieu_affectation(clstype_lieu_affectation varclstype_lieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE type_lieu_affectation  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclstype_lieu_affectation.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclstype_lieu_affectation.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclstype_lieu_affectation.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclstype_lieu_affectation.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclstype_lieu_affectation.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclstype_lieu_affectation.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclstype_lieu_affectation.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclstype_lieu_affectation.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_lieu_affectation.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClstype_lieu_affectation(clstype_lieu_affectation varclstype_lieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM type_lieu_affectation  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclstype_lieu_affectation.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'type_lieu_affectation' avec la classe 'clstype_lieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSTYPE_LIEU_AFFECTATION 
        #region  CLSAC
        public clsAC getClsAC(object intid)
        {
            clsAC varclsAC = new clsAC();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM AC WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsAC.Id = int.Parse(dr["id"].ToString());
                            varclsAC.Code_str = dr["code_str"].ToString();
                            varclsAC.Designation = dr["designation"].ToString();
                            varclsAC.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsAC.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsAC.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsAC.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'AC' avec la classe 'clsAC' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsAC;
        }

        public List<clsAC> getAllClsAC(string criteria)
        {
            List<clsAC> lstclsAC = new List<clsAC>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM AC  WHERE";
                    sql += "  code_str LIKE '%" + criteria + "%'";
                    sql += "  OR   designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsAC varclsAC = null;
                        while (dr.Read())
                        {

                            varclsAC = new clsAC();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsAC.Id = int.Parse(dr["id"].ToString());
                            varclsAC.Code_str = dr["code_str"].ToString();
                            varclsAC.Designation = dr["designation"].ToString();
                            varclsAC.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsAC.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsAC.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsAC.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsAC.Add(varclsAC);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'AC' avec la classe 'clsAC' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsAC;
        }

        public List<clsAC> getAllClsAC()
        {
            List<clsAC> lstclsAC = new List<clsAC>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM AC ORDER BY designation DESC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsAC varclsAC = null;
                        while (dr.Read())
                        {

                            varclsAC = new clsAC();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsAC.Id = int.Parse(dr["id"].ToString());
                            varclsAC.Code_str = dr["code_str"].ToString();
                            varclsAC.Designation = dr["designation"].ToString();
                            varclsAC.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsAC.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsAC.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsAC.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsAC.Add(varclsAC);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'AC' avec la classe 'clsAC' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsAC;
        }

        public int insertClsAC(clsAC varclsAC)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO AC ( id,code_str,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@code_str,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsAC.Id));
                    if (varclsAC.Code_str != null) cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 50, varclsAC.Code_str));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsAC.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsAC.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsAC.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsAC.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsAC.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsAC.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsAC.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'AC' avec la classe 'clsAC' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsAC(clsAC varclsAC)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE AC  SET code_str=@code_str,designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsAC.Code_str != null) cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 50, varclsAC.Code_str));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_str", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsAC.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsAC.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsAC.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsAC.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsAC.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsAC.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsAC.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsAC.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsAC.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'AC' avec la classe 'clsAC' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsAC(clsAC varclsAC)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM AC  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsAC.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'AC' avec la classe 'clsAC' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSAC 
        #region  CLSOPTIO
        public clsoptio getClsoptio(object intid)
        {
            clsoptio varclsoptio = new clsoptio();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM optio WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsoptio.Id = int.Parse(dr["id"].ToString());
                            varclsoptio.Designation = dr["designation"].ToString();
                            varclsoptio.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsoptio.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsoptio.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsoptio.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'optio' avec la classe 'clsoptio' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsoptio;
        }

        public List<clsoptio> getAllClsoptio(string criteria)
        {
            List<clsoptio> lstclsoptio = new List<clsoptio>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM optio  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsoptio varclsoptio = null;
                        while (dr.Read())
                        {

                            varclsoptio = new clsoptio();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsoptio.Id = int.Parse(dr["id"].ToString());
                            varclsoptio.Designation = dr["designation"].ToString();
                            varclsoptio.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsoptio.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsoptio.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsoptio.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsoptio.Add(varclsoptio);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'optio' avec la classe 'clsoptio' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsoptio;
        }

        public List<clsoptio> getAllClsoptio()
        {
            List<clsoptio> lstclsoptio = new List<clsoptio>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM optio ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsoptio varclsoptio = null;
                        while (dr.Read())
                        {

                            varclsoptio = new clsoptio();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsoptio.Id = int.Parse(dr["id"].ToString());
                            varclsoptio.Designation = dr["designation"].ToString();
                            varclsoptio.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsoptio.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsoptio.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsoptio.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsoptio.Add(varclsoptio);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'optio' avec la classe 'clsoptio' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsoptio;
        }

        public int insertClsoptio(clsoptio varclsoptio)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO optio ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsoptio.Id));
                    if (varclsoptio.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsoptio.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsoptio.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsoptio.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsoptio.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsoptio.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsoptio.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'optio' avec la classe 'clsoptio' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsoptio(clsoptio varclsoptio)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE optio  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsoptio.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsoptio.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsoptio.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsoptio.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsoptio.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsoptio.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsoptio.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsoptio.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsoptio.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'optio' avec la classe 'clsoptio' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsoptio(clsoptio varclsoptio)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM optio  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsoptio.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'optio' avec la classe 'clsoptio' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSOPTIO 
        #region  CLSPROMOTION
        public clspromotion getClspromotion(object intid)
        {
            clspromotion varclspromotion = new clspromotion();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM promotion WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclspromotion.Id = int.Parse(dr["id"].ToString());
                            varclspromotion.Designation = dr["designation"].ToString();
                            varclspromotion.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspromotion.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspromotion.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspromotion.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'promotion' avec la classe 'clspromotion' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclspromotion;
        }

        public List<clspromotion> getAllClspromotion(string criteria)
        {
            List<clspromotion> lstclspromotion = new List<clspromotion>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM promotion  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clspromotion varclspromotion = null;
                        while (dr.Read())
                        {

                            varclspromotion = new clspromotion();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspromotion.Id = int.Parse(dr["id"].ToString());
                            varclspromotion.Designation = dr["designation"].ToString();
                            varclspromotion.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspromotion.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspromotion.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspromotion.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspromotion.Add(varclspromotion);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'promotion' avec la classe 'clspromotion' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspromotion;
        }

        public List<clspromotion> getAllClspromotion()
        {
            List<clspromotion> lstclspromotion = new List<clspromotion>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM promotion ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clspromotion varclspromotion = null;
                        while (dr.Read())
                        {

                            varclspromotion = new clspromotion();
                            if (!dr["id"].ToString().Trim().Equals("")) varclspromotion.Id = int.Parse(dr["id"].ToString());
                            varclspromotion.Designation = dr["designation"].ToString();
                            varclspromotion.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclspromotion.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclspromotion.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclspromotion.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclspromotion.Add(varclspromotion);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'promotion' avec la classe 'clspromotion' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclspromotion;
        }

        public int insertClspromotion(clspromotion varclspromotion)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO promotion ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspromotion.Id));
                    if (varclspromotion.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclspromotion.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspromotion.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspromotion.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspromotion.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspromotion.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspromotion.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'promotion' avec la classe 'clspromotion' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClspromotion(clspromotion varclspromotion)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE promotion  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclspromotion.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclspromotion.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclspromotion.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclspromotion.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclspromotion.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclspromotion.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclspromotion.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclspromotion.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspromotion.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'promotion' avec la classe 'clspromotion' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClspromotion(clspromotion varclspromotion)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM promotion  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclspromotion.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'promotion' avec la classe 'clspromotion' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSPROMOTION 
        #region  CLSSECTION
        public clssection getClssection(object intid)
        {
            clssection varclssection = new clssection();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM section WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclssection.Id = int.Parse(dr["id"].ToString());
                            varclssection.Designation1 = dr["designation1"].ToString();
                            varclssection.Designation2 = dr["designation2"].ToString();
                            varclssection.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssection.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssection.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssection.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'section' avec la classe 'clssection' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclssection;
        }

        public List<clssection> getAllClssection(string criteria)
        {
            List<clssection> lstclssection = new List<clssection>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM section  WHERE";
                    sql += "  designation1 LIKE '%" + criteria + "%'";
                    sql += "  OR   designation2 LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssection varclssection = null;
                        while (dr.Read())
                        {

                            varclssection = new clssection();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssection.Id = int.Parse(dr["id"].ToString());
                            varclssection.Designation1 = dr["designation1"].ToString();
                            varclssection.Designation2 = dr["designation2"].ToString();
                            varclssection.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssection.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssection.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssection.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssection.Add(varclssection);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'section' avec la classe 'clssection' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssection;
        }

        public List<clssection> getAllClssection()
        {
            List<clssection> lstclssection = new List<clssection>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM section ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssection varclssection = null;
                        while (dr.Read())
                        {

                            varclssection = new clssection();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssection.Id = int.Parse(dr["id"].ToString());
                            varclssection.Designation1 = dr["designation1"].ToString();
                            varclssection.Designation2 = dr["designation2"].ToString();
                            varclssection.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssection.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssection.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssection.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssection.Add(varclssection);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'section' avec la classe 'clssection' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssection;
        }

        public int insertClssection(clssection varclssection)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO section ( id,designation1,designation2,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation1,@designation2,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssection.Id));
                    if (varclssection.Designation1 != null) cmd.Parameters.Add(getParameter(cmd, "@designation1", DbType.String, 5, varclssection.Designation1));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation1", DbType.String, 5, DBNull.Value));
                    if (varclssection.Designation2 != null) cmd.Parameters.Add(getParameter(cmd, "@designation2", DbType.String, 30, varclssection.Designation2));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation2", DbType.String, 30, DBNull.Value));
                    if (varclssection.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssection.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssection.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssection.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssection.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssection.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssection.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssection.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'section' avec la classe 'clssection' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClssection(clssection varclssection)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE section  SET designation1=@designation1,designation2=@designation2,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclssection.Designation1 != null) cmd.Parameters.Add(getParameter(cmd, "@designation1", DbType.String, 5, varclssection.Designation1));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation1", DbType.String, 5, DBNull.Value));
                    if (varclssection.Designation2 != null) cmd.Parameters.Add(getParameter(cmd, "@designation2", DbType.String, 30, varclssection.Designation2));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation2", DbType.String, 30, DBNull.Value));
                    if (varclssection.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssection.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssection.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssection.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssection.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssection.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssection.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssection.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssection.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'section' avec la classe 'clssection' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClssection(clssection varclssection)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM section  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssection.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'section' avec la classe 'clssection' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSSECTION 
        #region  CLSRETRAIT_MATERIEL
        public clsretrait_materiel getClsretrait_materiel(object intid)
        {
            clsretrait_materiel varclsretrait_materiel = new clsretrait_materiel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM retrait_materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsretrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsretrait_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_optio"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_optio = int.Parse(dr["id_optio"].ToString());
                            if (!dr["id_promotion"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_promotion = int.Parse(dr["id_promotion"].ToString());
                            if (!dr["id_section"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_section = int.Parse(dr["id_section"].ToString());
                            if (!dr["date_retrait"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_retrait = DateTime.Parse(dr["date_retrait"].ToString());
                            if (!dr["retirer"].ToString().Trim().Equals("")) varclsretrait_materiel.Retirer = bool.Parse(dr["retirer"].ToString());
                            if (!dr["deposer"].ToString().Trim().Equals("")) varclsretrait_materiel.Deposer = bool.Parse(dr["deposer"].ToString());
                            varclsretrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsretrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsretrait_materiel;
        }

        public List<clsretrait_materiel> getAllClsretrait_materiel(string criteria)
        {
            List<clsretrait_materiel> lstclsretrait_materiel = new List<clsretrait_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM retrait_materiel  WHERE";
                    sql += "  code_AC LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsretrait_materiel varclsretrait_materiel = null;
                        while (dr.Read())
                        {

                            varclsretrait_materiel = new clsretrait_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsretrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsretrait_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_optio"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_optio = int.Parse(dr["id_optio"].ToString());
                            if (!dr["id_promotion"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_promotion = int.Parse(dr["id_promotion"].ToString());
                            if (!dr["id_section"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_section = int.Parse(dr["id_section"].ToString());
                            if (!dr["date_retrait"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_retrait = DateTime.Parse(dr["date_retrait"].ToString());
                            if (!dr["retirer"].ToString().Trim().Equals("")) varclsretrait_materiel.Retirer = bool.Parse(dr["retirer"].ToString());
                            if (!dr["deposer"].ToString().Trim().Equals("")) varclsretrait_materiel.Deposer = bool.Parse(dr["deposer"].ToString());
                            varclsretrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsretrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsretrait_materiel.Add(varclsretrait_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsretrait_materiel;
        }

        public List<clsretrait_materiel> getAllClsretrait_materiel()
        {
            List<clsretrait_materiel> lstclsretrait_materiel = new List<clsretrait_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM retrait_materiel ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsretrait_materiel varclsretrait_materiel = null;
                        while (dr.Read())
                        {

                            varclsretrait_materiel = new clsretrait_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsretrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsretrait_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_optio"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_optio = int.Parse(dr["id_optio"].ToString());
                            if (!dr["id_promotion"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_promotion = int.Parse(dr["id_promotion"].ToString());
                            if (!dr["id_section"].ToString().Trim().Equals("")) varclsretrait_materiel.Id_section = int.Parse(dr["id_section"].ToString());
                            if (!dr["date_retrait"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_retrait = DateTime.Parse(dr["date_retrait"].ToString());
                            if (!dr["retirer"].ToString().Trim().Equals("")) varclsretrait_materiel.Retirer = bool.Parse(dr["retirer"].ToString());
                            if (!dr["deposer"].ToString().Trim().Equals("")) varclsretrait_materiel.Deposer = bool.Parse(dr["deposer"].ToString());
                            varclsretrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsretrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsretrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsretrait_materiel.Add(varclsretrait_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsretrait_materiel;
        }

        public int insertClsretrait_materiel(clsretrait_materiel varclsretrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO retrait_materiel ( id,id_personne,code_AC,id_optio,id_promotion,id_section,date_retrait,retirer,deposer,user_created,date_created,user_modified,date_modified ) VALUES (@id,@id_personne,@code_AC,@id_optio,@id_promotion,@id_section,@date_retrait,@retirer,@deposer,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsretrait_materiel.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsretrait_materiel.Id_personne));
                    if (varclsretrait_materiel.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclsretrait_materiel.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Id_optio.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_optio", DbType.Int32, 4, varclsretrait_materiel.Id_optio));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_optio", DbType.Int32, 4, DBNull.Value));
                    if (varclsretrait_materiel.Id_promotion.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_promotion", DbType.Int32, 4, varclsretrait_materiel.Id_promotion));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_promotion", DbType.Int32, 4, DBNull.Value));
                    if (varclsretrait_materiel.Id_section.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_section", DbType.Int32, 4, varclsretrait_materiel.Id_section));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_section", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_retrait", DbType.DateTime, 8, varclsretrait_materiel.Date_retrait));
                    cmd.Parameters.Add(getParameter(cmd, "@retirer", DbType.Boolean, 2, varclsretrait_materiel.Retirer));
                    if (varclsretrait_materiel.Deposer.HasValue) cmd.Parameters.Add(getParameter(cmd, "@deposer", DbType.Boolean, 2, varclsretrait_materiel.Deposer));
                    else cmd.Parameters.Add(getParameter(cmd, "@deposer", DbType.Boolean, 2, DBNull.Value));
                    if (varclsretrait_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsretrait_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsretrait_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsretrait_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsretrait_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsretrait_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsretrait_materiel(clsretrait_materiel varclsretrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE retrait_materiel  SET id_personne=@id_personne,code_AC=@code_AC,id_optio=@id_optio,id_promotion=@id_promotion,id_section=@id_section,date_retrait=@date_retrait,retirer=@retirer,deposer=@deposer,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsretrait_materiel.Id_personne));
                    if (varclsretrait_materiel.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclsretrait_materiel.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Id_optio.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_optio", DbType.Int32, 4, varclsretrait_materiel.Id_optio));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_optio", DbType.Int32, 4, DBNull.Value));
                    if (varclsretrait_materiel.Id_promotion.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_promotion", DbType.Int32, 4, varclsretrait_materiel.Id_promotion));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_promotion", DbType.Int32, 4, DBNull.Value));
                    if (varclsretrait_materiel.Id_section.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_section", DbType.Int32, 4, varclsretrait_materiel.Id_section));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_section", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_retrait", DbType.DateTime, 8, varclsretrait_materiel.Date_retrait));
                    cmd.Parameters.Add(getParameter(cmd, "@retirer", DbType.Boolean, 2, varclsretrait_materiel.Retirer));
                    if (varclsretrait_materiel.Deposer.HasValue) cmd.Parameters.Add(getParameter(cmd, "@deposer", DbType.Boolean, 2, varclsretrait_materiel.Deposer));
                    else cmd.Parameters.Add(getParameter(cmd, "@deposer", DbType.Boolean, 2, DBNull.Value));
                    if (varclsretrait_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsretrait_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsretrait_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsretrait_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsretrait_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsretrait_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsretrait_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsretrait_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsretrait_materiel(clsretrait_materiel varclsretrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM retrait_materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsretrait_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'retrait_materiel' avec la classe 'clsretrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRETRAIT_MATERIEL 
        #region  CLSDETAIL_RETRAIT_MATERIEL
        public clsdetail_retrait_materiel getClsdetail_retrait_materiel(object intid)
        {
            clsdetail_retrait_materiel varclsdetail_retrait_materiel = new clsdetail_retrait_materiel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM detail_retrait_materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_retrait_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_retrait_materiel = int.Parse(dr["id_retrait_materiel"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            varclsdetail_retrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsdetail_retrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsdetail_retrait_materiel;
        }

        public List<clsdetail_retrait_materiel> getAllClsdetail_retrait_materiel(string criteria)
        {
            List<clsdetail_retrait_materiel> lstclsdetail_retrait_materiel = new List<clsdetail_retrait_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM detail_retrait_materiel  WHERE";
                    sql += "  user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsdetail_retrait_materiel varclsdetail_retrait_materiel = null;
                        while (dr.Read())
                        {

                            varclsdetail_retrait_materiel = new clsdetail_retrait_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_retrait_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_retrait_materiel = int.Parse(dr["id_retrait_materiel"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            varclsdetail_retrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsdetail_retrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsdetail_retrait_materiel.Add(varclsdetail_retrait_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsdetail_retrait_materiel;
        }

        public List<clsdetail_retrait_materiel> getAllClsdetail_retrait_materiel()
        {
            List<clsdetail_retrait_materiel> lstclsdetail_retrait_materiel = new List<clsdetail_retrait_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM detail_retrait_materiel ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsdetail_retrait_materiel varclsdetail_retrait_materiel = null;
                        while (dr.Read())
                        {

                            varclsdetail_retrait_materiel = new clsdetail_retrait_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_retrait_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_retrait_materiel = int.Parse(dr["id_retrait_materiel"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            varclsdetail_retrait_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsdetail_retrait_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsdetail_retrait_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsdetail_retrait_materiel.Add(varclsdetail_retrait_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsdetail_retrait_materiel;
        }

        public int insertClsdetail_retrait_materiel(clsdetail_retrait_materiel varclsdetail_retrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO detail_retrait_materiel ( id,id_retrait_materiel,id_materiel,user_created,date_created,user_modified,date_modified ) VALUES (@id,@id_retrait_materiel,@id_materiel,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsdetail_retrait_materiel.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@id_retrait_materiel", DbType.Int32, 4, varclsdetail_retrait_materiel.Id_retrait_materiel));
                    cmd.Parameters.Add(getParameter(cmd, "@id_materiel", DbType.Int32, 4, varclsdetail_retrait_materiel.Id_materiel));
                    if (varclsdetail_retrait_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsdetail_retrait_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsdetail_retrait_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsdetail_retrait_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsdetail_retrait_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsdetail_retrait_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsdetail_retrait_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsdetail_retrait_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsdetail_retrait_materiel(clsdetail_retrait_materiel varclsdetail_retrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE detail_retrait_materiel  SET id_retrait_materiel=@id_retrait_materiel,id_materiel=@id_materiel,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id_retrait_materiel", DbType.Int32, 4, varclsdetail_retrait_materiel.Id_retrait_materiel));
                    cmd.Parameters.Add(getParameter(cmd, "@id_materiel", DbType.Int32, 4, varclsdetail_retrait_materiel.Id_materiel));
                    if (varclsdetail_retrait_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsdetail_retrait_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsdetail_retrait_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsdetail_retrait_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsdetail_retrait_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsdetail_retrait_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsdetail_retrait_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsdetail_retrait_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsdetail_retrait_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsdetail_retrait_materiel(clsdetail_retrait_materiel varclsdetail_retrait_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM detail_retrait_materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsdetail_retrait_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'detail_retrait_materiel' avec la classe 'clsdetail_retrait_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSDETAIL_RETRAIT_MATERIEL 
        #region  CLSSIGNATAIRE
        public clssignataire getClssignataire(object intid)
        {
            clssignataire varclssignataire = new clssignataire();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM signataire WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclssignataire.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclssignataire.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclssignataire.Code_ac = dr["code_AC"].ToString();
                            varclssignataire.Signature_specimen = dr["signature_specimen"].ToString();
                            varclssignataire.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssignataire.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssignataire.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssignataire.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'signataire' avec la classe 'clssignataire' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclssignataire;
        }

        public List<clssignataire> getAllClssignataire(string criteria)
        {
            List<clssignataire> lstclssignataire = new List<clssignataire>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM signataire  WHERE";
                    sql += "  code_AC LIKE '%" + criteria + "%'";
                    sql += "  OR   signature_specimen LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssignataire varclssignataire = null;
                        while (dr.Read())
                        {

                            varclssignataire = new clssignataire();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssignataire.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclssignataire.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclssignataire.Code_ac = dr["code_AC"].ToString();
                            varclssignataire.Signature_specimen = dr["signature_specimen"].ToString();
                            varclssignataire.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssignataire.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssignataire.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssignataire.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssignataire.Add(varclssignataire);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'signataire' avec la classe 'clssignataire' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssignataire;
        }

        public List<clssignataire> getAllClssignataire()
        {
            List<clssignataire> lstclssignataire = new List<clssignataire>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM signataire ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssignataire varclssignataire = null;
                        while (dr.Read())
                        {

                            varclssignataire = new clssignataire();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssignataire.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclssignataire.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclssignataire.Code_ac = dr["code_AC"].ToString();
                            varclssignataire.Signature_specimen = dr["signature_specimen"].ToString();
                            varclssignataire.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssignataire.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssignataire.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssignataire.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssignataire.Add(varclssignataire);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'signataire' avec la classe 'clssignataire' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssignataire;
        }

        public int insertClssignataire(clssignataire varclssignataire)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO signataire ( id,id_personne,code_AC,signature_specimen,user_created,date_created,user_modified,date_modified ) VALUES (@id,@id_personne,@code_AC,@signature_specimen,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssignataire.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclssignataire.Id_personne));
                    if (varclssignataire.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclssignataire.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Signature_specimen != null) cmd.Parameters.Add(getParameter(cmd, "@signature_specimen", DbType.Object, 2147483647, varclssignataire.Signature_specimen));
                    else cmd.Parameters.Add(getParameter(cmd, "@signature_specimen", DbType.Object, 2147483647, DBNull.Value));
                    if (varclssignataire.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssignataire.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssignataire.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssignataire.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssignataire.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssignataire.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'signataire' avec la classe 'clssignataire' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClssignataire(clssignataire varclssignataire)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE signataire  SET id_personne=@id_personne,code_AC=@code_AC,signature_specimen=@signature_specimen,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclssignataire.Id_personne));
                    if (varclssignataire.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclssignataire.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Signature_specimen != null) cmd.Parameters.Add(getParameter(cmd, "@signature_specimen", DbType.Object, 2147483647, varclssignataire.Signature_specimen));
                    else cmd.Parameters.Add(getParameter(cmd, "@signature_specimen", DbType.Object, 2147483647, DBNull.Value));
                    if (varclssignataire.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssignataire.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssignataire.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssignataire.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssignataire.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssignataire.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssignataire.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssignataire.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'signataire' avec la classe 'clssignataire' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClssignataire(clssignataire varclssignataire)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM signataire  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssignataire.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'signataire' avec la classe 'clssignataire' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSSIGNATAIRE 
        #region  CLSSALLE
        public clssalle getClssalle(object intid)
        {
            clssalle varclssalle = new clssalle();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM salle WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclssalle.Id = int.Parse(dr["id"].ToString());
                            varclssalle.Designation = dr["designation"].ToString();
                            varclssalle.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssalle.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssalle.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssalle.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'salle' avec la classe 'clssalle' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclssalle;
        }

        public List<clssalle> getAllClssalle(string criteria)
        {
            List<clssalle> lstclssalle = new List<clssalle>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM salle  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssalle varclssalle = null;
                        while (dr.Read())
                        {

                            varclssalle = new clssalle();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssalle.Id = int.Parse(dr["id"].ToString());
                            varclssalle.Designation = dr["designation"].ToString();
                            varclssalle.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssalle.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssalle.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssalle.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssalle.Add(varclssalle);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'salle' avec la classe 'clssalle' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssalle;
        }

        public List<clssalle> getAllClssalle()
        {
            List<clssalle> lstclssalle = new List<clssalle>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM salle ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clssalle varclssalle = null;
                        while (dr.Read())
                        {

                            varclssalle = new clssalle();
                            if (!dr["id"].ToString().Trim().Equals("")) varclssalle.Id = int.Parse(dr["id"].ToString());
                            varclssalle.Designation = dr["designation"].ToString();
                            varclssalle.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclssalle.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclssalle.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclssalle.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclssalle.Add(varclssalle);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'salle' avec la classe 'clssalle' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclssalle;
        }

        public int insertClssalle(clssalle varclssalle)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO salle ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssalle.Id));
                    if (varclssalle.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclssalle.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclssalle.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssalle.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssalle.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssalle.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssalle.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssalle.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssalle.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssalle.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'salle' avec la classe 'clssalle' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClssalle(clssalle varclssalle)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE salle  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclssalle.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclssalle.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclssalle.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclssalle.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclssalle.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclssalle.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclssalle.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclssalle.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclssalle.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclssalle.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssalle.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'salle' avec la classe 'clssalle' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClssalle(clssalle varclssalle)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM salle  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclssalle.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'salle' avec la classe 'clssalle' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSSALLE 
        #region  CLSFONCTION
        public clsfonction getClsfonction(object intid)
        {
            clsfonction varclsfonction = new clsfonction();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM fonction WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsfonction.Id = int.Parse(dr["id"].ToString());
                            varclsfonction.Designation = dr["designation"].ToString();
                            varclsfonction.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsfonction.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsfonction.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsfonction.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'fonction' avec la classe 'clsfonction' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsfonction;
        }

        public List<clsfonction> getAllClsfonction(string criteria)
        {
            List<clsfonction> lstclsfonction = new List<clsfonction>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM fonction  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsfonction varclsfonction = null;
                        while (dr.Read())
                        {

                            varclsfonction = new clsfonction();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsfonction.Id = int.Parse(dr["id"].ToString());
                            varclsfonction.Designation = dr["designation"].ToString();
                            varclsfonction.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsfonction.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsfonction.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsfonction.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsfonction.Add(varclsfonction);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'fonction' avec la classe 'clsfonction' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsfonction;
        }

        public List<clsfonction> getAllClsfonction()
        {
            List<clsfonction> lstclsfonction = new List<clsfonction>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM fonction ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsfonction varclsfonction = null;
                        while (dr.Read())
                        {

                            varclsfonction = new clsfonction();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsfonction.Id = int.Parse(dr["id"].ToString());
                            varclsfonction.Designation = dr["designation"].ToString();
                            varclsfonction.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsfonction.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsfonction.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsfonction.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsfonction.Add(varclsfonction);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'fonction' avec la classe 'clsfonction' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsfonction;
        }

        public int insertClsfonction(clsfonction varclsfonction)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO fonction ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsfonction.Id));
                    if (varclsfonction.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsfonction.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsfonction.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsfonction.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsfonction.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsfonction.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsfonction.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'fonction' avec la classe 'clsfonction' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsfonction(clsfonction varclsfonction)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE fonction  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsfonction.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclsfonction.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsfonction.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsfonction.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsfonction.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsfonction.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsfonction.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsfonction.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsfonction.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'fonction' avec la classe 'clsfonction' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsfonction(clsfonction varclsfonction)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM fonction  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsfonction.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'fonction' avec la classe 'clsfonction' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSFONCTION 
        #region  CLSLIEU_AFFECTATION
        public clslieu_affectation getClslieu_affectation(object intid)
        {
            clslieu_affectation varclslieu_affectation = new clslieu_affectation();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM lieu_affectation WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclslieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclslieu_affectation.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_type_lieu_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Id_type_lieu_affectation = int.Parse(dr["id_type_lieu_affectation"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclslieu_affectation.Id_personne = int.Parse(dr["id_personne"].ToString());
                            if (!dr["id_fonction"].ToString().Trim().Equals("")) varclslieu_affectation.Id_fonction = int.Parse(dr["id_fonction"].ToString());
                            varclslieu_affectation.Designation = dr["designation"].ToString();
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclslieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclslieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclslieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclslieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclslieu_affectation;
        }

        public List<clslieu_affectation> getAllClslieu_affectation(string criteria)
        {
            List<clslieu_affectation> lstclslieu_affectation = new List<clslieu_affectation>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM lieu_affectation  WHERE";
                    sql += "  code_AC LIKE '%" + criteria + "%'";
                    sql += "  OR   designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clslieu_affectation varclslieu_affectation = null;
                        while (dr.Read())
                        {

                            varclslieu_affectation = new clslieu_affectation();
                            if (!dr["id"].ToString().Trim().Equals("")) varclslieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclslieu_affectation.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_type_lieu_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Id_type_lieu_affectation = int.Parse(dr["id_type_lieu_affectation"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclslieu_affectation.Id_personne = int.Parse(dr["id_personne"].ToString());
                            if (!dr["id_fonction"].ToString().Trim().Equals("")) varclslieu_affectation.Id_fonction = int.Parse(dr["id_fonction"].ToString());
                            varclslieu_affectation.Designation = dr["designation"].ToString();
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclslieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclslieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclslieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclslieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclslieu_affectation.Add(varclslieu_affectation);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclslieu_affectation;
        }

        public List<clslieu_affectation> getAllClslieu_affectation()
        {
            List<clslieu_affectation> lstclslieu_affectation = new List<clslieu_affectation>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM lieu_affectation ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clslieu_affectation varclslieu_affectation = null;
                        while (dr.Read())
                        {

                            varclslieu_affectation = new clslieu_affectation();
                            if (!dr["id"].ToString().Trim().Equals("")) varclslieu_affectation.Id = int.Parse(dr["id"].ToString());
                            varclslieu_affectation.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_type_lieu_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Id_type_lieu_affectation = int.Parse(dr["id_type_lieu_affectation"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclslieu_affectation.Id_personne = int.Parse(dr["id_personne"].ToString());
                            if (!dr["id_fonction"].ToString().Trim().Equals("")) varclslieu_affectation.Id_fonction = int.Parse(dr["id_fonction"].ToString());
                            varclslieu_affectation.Designation = dr["designation"].ToString();
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclslieu_affectation.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclslieu_affectation.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclslieu_affectation.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclslieu_affectation.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclslieu_affectation.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclslieu_affectation.Add(varclslieu_affectation);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclslieu_affectation;
        }

        public int insertClslieu_affectation(clslieu_affectation varclslieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO lieu_affectation ( id,code_AC,id_type_lieu_affectation,id_personne,id_fonction,designation,date_affectation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@code_AC,@id_type_lieu_affectation,@id_personne,@id_fonction,@designation,@date_affectation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclslieu_affectation.Id));
                    if (varclslieu_affectation.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclslieu_affectation.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_type_lieu_affectation", DbType.Int32, 4, varclslieu_affectation.Id_type_lieu_affectation));
                    if (varclslieu_affectation.Id_personne.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclslieu_affectation.Id_personne));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, DBNull.Value));
                    if (varclslieu_affectation.Id_fonction.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_fonction", DbType.Int32, 4, varclslieu_affectation.Id_fonction));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_fonction", DbType.Int32, 4, DBNull.Value));
                    if (varclslieu_affectation.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclslieu_affectation.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_affectation", DbType.DateTime, 8, varclslieu_affectation.Date_affectation));
                    if (varclslieu_affectation.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclslieu_affectation.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclslieu_affectation.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclslieu_affectation.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclslieu_affectation.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclslieu_affectation.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclslieu_affectation.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclslieu_affectation.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClslieu_affectation(clslieu_affectation varclslieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE lieu_affectation  SET code_AC=@code_AC,id_type_lieu_affectation=@id_type_lieu_affectation,id_personne=@id_personne,id_fonction=@id_fonction,designation=@designation,date_affectation=@date_affectation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclslieu_affectation.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclslieu_affectation.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_type_lieu_affectation", DbType.Int32, 4, varclslieu_affectation.Id_type_lieu_affectation));
                    if (varclslieu_affectation.Id_personne.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclslieu_affectation.Id_personne));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, DBNull.Value));
                    if (varclslieu_affectation.Id_fonction.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_fonction", DbType.Int32, 4, varclslieu_affectation.Id_fonction));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_fonction", DbType.Int32, 4, DBNull.Value));
                    if (varclslieu_affectation.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclslieu_affectation.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_affectation", DbType.DateTime, 8, varclslieu_affectation.Date_affectation));
                    if (varclslieu_affectation.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclslieu_affectation.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclslieu_affectation.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclslieu_affectation.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclslieu_affectation.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclslieu_affectation.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclslieu_affectation.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclslieu_affectation.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclslieu_affectation.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClslieu_affectation(clslieu_affectation varclslieu_affectation)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM lieu_affectation  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclslieu_affectation.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'lieu_affectation' avec la classe 'clslieu_affectation' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSLIEU_AFFECTATION 
        #region  CLSAFFECTATION_MATERIEL
        public clsaffectation_materiel getClsaffectation_materiel(object intid)
        {
            clsaffectation_materiel varclsaffectation_materiel = new clsaffectation_materiel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM affectation_materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsaffectation_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_lieu_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_lieu_affectation = int.Parse(dr["id_lieu_affectation"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            if (!dr["id_salle"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_salle = int.Parse(dr["id_salle"].ToString());
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclsaffectation_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsaffectation_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsaffectation_materiel;
        }

        public List<clsaffectation_materiel> getAllClsaffectation_materiel(string criteria)
        {
            List<clsaffectation_materiel> lstclsaffectation_materiel = new List<clsaffectation_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM affectation_materiel  WHERE";
                    sql += "  code_AC LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsaffectation_materiel varclsaffectation_materiel = null;
                        while (dr.Read())
                        {

                            varclsaffectation_materiel = new clsaffectation_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsaffectation_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_lieu_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_lieu_affectation = int.Parse(dr["id_lieu_affectation"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            if (!dr["id_salle"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_salle = int.Parse(dr["id_salle"].ToString());
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclsaffectation_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsaffectation_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsaffectation_materiel.Add(varclsaffectation_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsaffectation_materiel;
        }

        public List<clsaffectation_materiel> getAllClsaffectation_materiel()
        {
            List<clsaffectation_materiel> lstclsaffectation_materiel = new List<clsaffectation_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM affectation_materiel ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsaffectation_materiel varclsaffectation_materiel = null;
                        while (dr.Read())
                        {

                            varclsaffectation_materiel = new clsaffectation_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id = int.Parse(dr["id"].ToString());
                            varclsaffectation_materiel.Code_ac = dr["code_AC"].ToString();
                            if (!dr["id_lieu_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_lieu_affectation = int.Parse(dr["id_lieu_affectation"].ToString());
                            if (!dr["id_materiel"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_materiel = int.Parse(dr["id_materiel"].ToString());
                            if (!dr["id_salle"].ToString().Trim().Equals("")) varclsaffectation_materiel.Id_salle = int.Parse(dr["id_salle"].ToString());
                            if (!dr["date_affectation"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_affectation = DateTime.Parse(dr["date_affectation"].ToString());
                            varclsaffectation_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclsaffectation_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclsaffectation_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclsaffectation_materiel.Add(varclsaffectation_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsaffectation_materiel;
        }

        public int insertClsaffectation_materiel(clsaffectation_materiel varclsaffectation_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO affectation_materiel ( id,code_AC,id_lieu_affectation,id_materiel,id_salle,date_affectation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@code_AC,@id_lieu_affectation,@id_materiel,@id_salle,@date_affectation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsaffectation_materiel.Id));
                    if (varclsaffectation_materiel.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclsaffectation_materiel.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_lieu_affectation", DbType.Int32, 4, varclsaffectation_materiel.Id_lieu_affectation));
                    cmd.Parameters.Add(getParameter(cmd, "@id_materiel", DbType.Int32, 4, varclsaffectation_materiel.Id_materiel));
                    if (varclsaffectation_materiel.Id_salle.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_salle", DbType.Int32, 4, varclsaffectation_materiel.Id_salle));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_salle", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_affectation", DbType.DateTime, 8, varclsaffectation_materiel.Date_affectation));
                    if (varclsaffectation_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsaffectation_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsaffectation_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsaffectation_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsaffectation_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsaffectation_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsaffectation_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsaffectation_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsaffectation_materiel(clsaffectation_materiel varclsaffectation_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE affectation_materiel  SET code_AC=@code_AC,id_lieu_affectation=@id_lieu_affectation,id_materiel=@id_materiel,id_salle=@id_salle,date_affectation=@date_affectation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclsaffectation_materiel.Code_ac != null) cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, varclsaffectation_materiel.Code_ac));
                    else cmd.Parameters.Add(getParameter(cmd, "@code_AC", DbType.String, 50, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id_lieu_affectation", DbType.Int32, 4, varclsaffectation_materiel.Id_lieu_affectation));
                    cmd.Parameters.Add(getParameter(cmd, "@id_materiel", DbType.Int32, 4, varclsaffectation_materiel.Id_materiel));
                    if (varclsaffectation_materiel.Id_salle.HasValue) cmd.Parameters.Add(getParameter(cmd, "@id_salle", DbType.Int32, 4, varclsaffectation_materiel.Id_salle));
                    else cmd.Parameters.Add(getParameter(cmd, "@id_salle", DbType.Int32, 4, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@date_affectation", DbType.DateTime, 8, varclsaffectation_materiel.Date_affectation));
                    if (varclsaffectation_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclsaffectation_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclsaffectation_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclsaffectation_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclsaffectation_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclsaffectation_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclsaffectation_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclsaffectation_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsaffectation_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsaffectation_materiel(clsaffectation_materiel varclsaffectation_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM affectation_materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsaffectation_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'affectation_materiel' avec la classe 'clsaffectation_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSAFFECTATION_MATERIEL 
        #region  CLSUTILISATEUR
        public clsutilisateur getClsutilisateur(object intid)
        {
            clsutilisateur varclsutilisateur = new clsutilisateur();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM utilisateur WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclsutilisateur.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsutilisateur.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsutilisateur.Nomuser = dr["nomuser"].ToString();
                            varclsutilisateur.Motpass = dr["motpass"].ToString();
                            varclsutilisateur.Schema_user = dr["schema_user"].ToString();
                            varclsutilisateur.Droits = dr["droits"].ToString();
                            if (!dr["activation"].ToString().Trim().Equals("")) varclsutilisateur.Activation = bool.Parse(dr["activation"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsutilisateur;
        }

        public List<clsutilisateur> getAllClsutilisateur(string criteria)
        {
            List<clsutilisateur> lstclsutilisateur = new List<clsutilisateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM utilisateur  WHERE";
                    sql += "  nomuser LIKE '%" + criteria + "%'";
                    sql += "  OR   motpass LIKE '%" + criteria + "%'";
                    sql += "  OR   schema_user LIKE '%" + criteria + "%'";
                    sql += "  OR   droits LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsutilisateur varclsutilisateur = null;
                        while (dr.Read())
                        {

                            varclsutilisateur = new clsutilisateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsutilisateur.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsutilisateur.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsutilisateur.Nomuser = dr["nomuser"].ToString();
                            varclsutilisateur.Motpass = dr["motpass"].ToString();
                            varclsutilisateur.Schema_user = dr["schema_user"].ToString();
                            varclsutilisateur.Droits = dr["droits"].ToString();
                            if (!dr["activation"].ToString().Trim().Equals("")) varclsutilisateur.Activation = bool.Parse(dr["activation"].ToString());
                            lstclsutilisateur.Add(varclsutilisateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'utilisateur' avec la classe 'clsutilisateur' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsutilisateur;
        }

        public List<clsutilisateur> getAllClsutilisateur()
        {
            List<clsutilisateur> lstclsutilisateur = new List<clsutilisateur>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM utilisateur ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsutilisateur varclsutilisateur = null;
                        while (dr.Read())
                        {

                            varclsutilisateur = new clsutilisateur();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsutilisateur.Id = int.Parse(dr["id"].ToString());
                            if (!dr["id_personne"].ToString().Trim().Equals("")) varclsutilisateur.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsutilisateur.Nomuser = dr["nomuser"].ToString();
                            varclsutilisateur.Motpass = dr["motpass"].ToString();
                            varclsutilisateur.Schema_user = dr["schema_user"].ToString();
                            varclsutilisateur.Droits = dr["droits"].ToString();
                            if (!dr["activation"].ToString().Trim().Equals("")) varclsutilisateur.Activation = bool.Parse(dr["activation"].ToString());
                            lstclsutilisateur.Add(varclsutilisateur);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclsutilisateur;
        }

        public int insertClsutilisateur1(clsutilisateur varclsutilisateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO utilisateur (id,id_personne,nomuser,motpass,schema_user,droits,activation ) VALUES (@id,@id_personne,@nomuser,@motpass,@schema_user,@droits,@activation  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsutilisateur.Id_personne));
                    if (varclsutilisateur.Nomuser != null) cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, varclsutilisateur.Nomuser));
                    else cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, DBNull.Value));
                    if (varclsutilisateur.Motpass != null) cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, varclsutilisateur.Motpass));
                    else cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, DBNull.Value));
                    if (varclsutilisateur.Schema_user != null) cmd.Parameters.Add(getParameter(cmd, "@schema_user", DbType.String, 20, varclsutilisateur.Schema_user));
                    else cmd.Parameters.Add(getParameter(cmd, "@schema_user", DbType.String, 20, DBNull.Value));
                    if (varclsutilisateur.Droits != null) cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 300, varclsutilisateur.Droits));
                    else cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 300, DBNull.Value));
                    if (varclsutilisateur.Activation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                    else cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsutilisateur1(clsutilisateur varclsutilisateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE utilisateur  SET id_personne=@id_personne,nomuser=@nomuser,motpass=@motpass,schema_user=@schema_user,droits=@droits,activation=@activation  WHERE 1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsutilisateur.Id_personne));
                    if (varclsutilisateur.Nomuser != null) cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, varclsutilisateur.Nomuser));
                    else cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, DBNull.Value));
                    if (varclsutilisateur.Motpass != null) cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, varclsutilisateur.Motpass));
                    else cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, DBNull.Value));
                    if (varclsutilisateur.Schema_user != null) cmd.Parameters.Add(getParameter(cmd, "@schema_user", DbType.String, 20, varclsutilisateur.Schema_user));
                    else cmd.Parameters.Add(getParameter(cmd, "@schema_user", DbType.String, 20, DBNull.Value));
                    if (varclsutilisateur.Droits != null) cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 300, varclsutilisateur.Droits));
                    else cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 300, DBNull.Value));
                    if (varclsutilisateur.Activation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                    else cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsutilisateur1(clsutilisateur varclsutilisateur)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM utilisateur  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #region BEGIN ADD UTILISATEUR
        public int GenerateLastID(string table_name)
        {
            int lastID = 0;
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT MAX(id) AS lastID From {0}", table_name);
                    IDataReader rd = cmd.ExecuteReader();

                    if (rd.Read())
                    {
                        if (rd["lastID"] == DBNull.Value)
                            lastID = 1;
                        else if (Convert.ToInt32(rd["lastID"].ToString()) == 0)
                            lastID = 1;
                        else
                            lastID = Convert.ToInt32(rd["lastID"].ToString()) + 1;
                    }

                    rd.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            finally
            {
                conn.Close();
            }
            return lastID;
        }

        //Bonne partie pour Gestion users
        public int insertClsutilisateur(clsutilisateur varclsutilisateur)
        {
            //On crée d'abord le user en déhors de la transaction car les procedures stocke ont un traitement 
            //transactionnel par defaut
            bool echec_create = true;
            string message_erreur_user = "";

            try
            {
                //Avant tous on commence par creer l'ID qui sera inseree dans la table des users
                varclsutilisateur.Id = this.GenerateLastID("utilisateur");
                //Avant de faire l'insertion dans la table utilisateur, on commence par créer le login et le user de la BD
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"exec sp_addlogin '" + varclsutilisateur.Nomuser + "','" + varclsutilisateur.Motpass + "','" + clsMetier.bdEnCours + @"'                                               
                                                      exec sp_grantdbaccess '" + varclsutilisateur.Nomuser + @"'
                                                 ");
                    int j = cmd.ExecuteNonQuery();
                    echec_create = false;
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                message_erreur_user = exc.Message;
                conn.Close();
                throw new Exception(exc.Message);
            }

            //Dans la transaction on fait le reste
            IDbTransaction transaction = null;

            int i = 0;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                if (echec_create)
                    throw new Exception(message_erreur_user);//Si la création du user a échoué, on fait échoué le reste

                //Si l'on à cocher la case à cocher d'activation de l'utilisateur on doit le donner accès à se connecter ou non
                if (!(bool)varclsutilisateur.Activation)
                {
                    using (IDbCommand cmd2 = conn.CreateCommand())
                    {
                        cmd2.CommandText = string.Format(@"revoke connect to " + varclsutilisateur.Nomuser); //On interdit à l'utilisateur de se connecter au serveur
                        cmd2.Transaction = transaction;

                        i = cmd2.ExecuteNonQuery();
                    }
                }

                //Insertion de l'utilisateur créé dans la table des user sans droits
                using (IDbCommand cmd3 = conn.CreateCommand())
                {
                    cmd3.CommandText = string.Format("INSERT INTO utilisateur (id,id_personne,nomuser,motpass,schema_user,activation ) VALUES (@id,@id_personne,@nomuser,@motpass,@schema_user,@activation)");
                    cmd3.Parameters.Add(getParameter(cmd3, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                    cmd3.Parameters.Add(getParameter(cmd3, "@id_personne", DbType.Int32, 4, varclsutilisateur.Id_personne));
                    if (varclsutilisateur.Nomuser != null) cmd3.Parameters.Add(getParameter(cmd3, "@nomuser", DbType.String, 30, varclsutilisateur.Nomuser));
                    else cmd3.Parameters.Add(getParameter(cmd3, "@nomuser", DbType.String, 30, DBNull.Value));
                    if (varclsutilisateur.Motpass != null) cmd3.Parameters.Add(getParameter(cmd3, "@motpass", DbType.String, 1000, ImplementChiffer.Instance.Cipher(varclsutilisateur.Motpass, "Jos@mRootP@ss")));//On chiffre le password a mettre dans la BD
                    else cmd3.Parameters.Add(getParameter(cmd3, "@motpass", DbType.String, 1000, DBNull.Value));
                    varclsutilisateur.Schema_user = varclsutilisateur.Nomuser;
                    if (varclsutilisateur.Schema_user != null) cmd3.Parameters.Add(getParameter(cmd3, "@schema_user", DbType.String, 20, varclsutilisateur.Schema_user));
                    else cmd3.Parameters.Add(getParameter(cmd3, "@schema_user", DbType.String, 20, DBNull.Value));
                    if (varclsutilisateur.Activation.HasValue) cmd3.Parameters.Add(getParameter(cmd3, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                    else cmd3.Parameters.Add(getParameter(cmd3, "@activation", DbType.Boolean, 2, DBNull.Value));
                    cmd3.Transaction = transaction;
                    i = cmd3.ExecuteNonQuery();

                    transaction.Commit();
                }

                conn.Close();
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();

                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec lors de la création de l'utilisateur  : " + message_erreur_user + " => " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                    throw new Exception(exc.Message);
                }
                conn.Close();
            }
            return i;
        }

        public int updateClsutilisateur(clsutilisateur varclsutilisateur)
        {
            IDbTransaction transaction = null;
            int i = 0;
            bool ok = false;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                if (clsTools.etat_modification_user == 4)
                {
                    varclsutilisateur.Activation = clsTools.activationUser;

                    if (conn.State != ConnectionState.Open) conn.Open();

                    if ((bool)varclsutilisateur.Activation)
                    {
                        using (IDbCommand cmd3 = conn.CreateCommand())
                        {
                            cmd3.CommandText = string.Format(@"grant connect to " + varclsutilisateur.Nomuser); //On interdit à l'utilisateur de se connecter au serveur
                            cmd3.Transaction = transaction;
                            i = cmd3.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (IDbCommand cmd3 = conn.CreateCommand())
                        {
                            cmd3.CommandText = string.Format(@"revoke connect to " + varclsutilisateur.Nomuser); //On interdit à l'utilisateur de se connecter au serveur
                            cmd3.Transaction = transaction;
                            i = cmd3.ExecuteNonQuery();
                        }
                    }

                    using (IDbCommand cmd4 = conn.CreateCommand())
                    {
                        cmd4.CommandText = string.Format("UPDATE utilisateur SET activation=@activation  WHERE 1=1  AND id=@id ");
                        cmd4.Parameters.Add(getParameter(cmd4, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                        cmd4.Parameters.Add(getParameter(cmd4, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                        cmd4.Transaction = transaction;

                        i = cmd4.ExecuteNonQuery();
                    }
                }
                else if (clsTools.etat_modification_user == 1)
                {
                    //Modification du nom user seulement

                    //Avant de modifier l'utilisateur dans la table, on modifie le user de la bd
                    using (IDbCommand cmd1 = conn.CreateCommand())
                    {
                        varclsutilisateur.Nomuser = clsTools.newUser;
                        //varclstbl_utilisateur.Motpass = clsTools.oldPassword;
                        cmd1.CommandText = string.Format("alter login " + clsTools.oldUser + " with name=" + varclsutilisateur.Nomuser); //On modifie le login de l'utilisateur pour changer son mode de connexion
                        cmd1.Transaction = transaction;
                        i = cmd1.ExecuteNonQuery();
                    }
                }
                else if (clsTools.etat_modification_user == 2)
                {
                    //Modification du mot de passe seulement

                    //Avant de modifier l'utilisateur dans la table, on modifie le user de la bd
                    using (IDbCommand cmd1 = conn.CreateCommand())
                    {
                        varclsutilisateur.Motpass = clsTools.newPassword;
                        cmd1.CommandText = string.Format("alter LOGIN " + varclsutilisateur.Nomuser + " WITH PASSWORD='" + ImplementChiffer.Instance.Decipher(clsTools.newPassword, "Jos@mRootP@ss") + "'"); //On modifie le login de l'utilisateur pour changer son mot de passe de connexion
                        cmd1.Transaction = transaction;
                        i = cmd1.ExecuteNonQuery();
                    }
                }
                else if (clsTools.etat_modification_user == 3)
                {
                    //Modification du nom d'utilisateur et du mot de passe

                    //Avant de modifier l'utilisateur dans la table, on modifie le user de la bd
                    using (IDbCommand cmd1 = conn.CreateCommand())
                    {
                        varclsutilisateur.Nomuser = clsTools.newUser;
                        varclsutilisateur.Motpass = clsTools.newPassword;
                        cmd1.CommandText = string.Format("ALTER LOGIN " + clsTools.oldUser + " WITH PASSWORD='" + ImplementChiffer.Instance.Decipher(clsTools.newPassword, "Jos@mRootP@ss") + "'" + @"
                                                          ALTER LOGIN " + clsTools.oldUser + " WITH NAME=" + varclsutilisateur.Nomuser); //On modifie le login de l'utilisateur pour changer son mot de passe de connexion, puis on modifie son nom de login
                        cmd1.Transaction = transaction;
                        i = cmd1.ExecuteNonQuery();
                    }
                }

                if (clsTools.etat_modification_user == 1)
                {
                    //Modification de l'utilisateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("UPDATE utilisateur  SET id_personne=@id_personne,nomuser=@nomuser,activation=@activation  WHERE 1=1  AND id=@id ");
                        cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsutilisateur.Id_personne));
                        if (varclsutilisateur.Nomuser != null) cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, varclsutilisateur.Nomuser));
                        else cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, DBNull.Value));
                        if (varclsutilisateur.Activation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                        else cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, DBNull.Value));
                        cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                        cmd.Transaction = transaction;
                        i = cmd.ExecuteNonQuery();
                        ok = true;
                    }
                }
                else if (clsTools.etat_modification_user == 2 || clsTools.etat_modification_user == 3)
                {
                    //Modification de l'utilisateur
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("UPDATE utilisateur  SET id_personne=@id_personne,nomuser=@nomuser,motpass=@motpass,activation=@activation  WHERE 1=1  AND id=@id ");
                        cmd.Parameters.Add(getParameter(cmd, "@id_personne", DbType.Int32, 4, varclsutilisateur.Id_personne));
                        if (varclsutilisateur.Nomuser != null) cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, varclsutilisateur.Nomuser));
                        else cmd.Parameters.Add(getParameter(cmd, "@nomuser", DbType.String, 30, DBNull.Value));
                        if (varclsutilisateur.Motpass != null) cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, varclsutilisateur.Motpass));
                        else cmd.Parameters.Add(getParameter(cmd, "@motpass", DbType.String, 1000, DBNull.Value));
                        if (varclsutilisateur.Activation.HasValue) cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, varclsutilisateur.Activation));
                        else cmd.Parameters.Add(getParameter(cmd, "@activation", DbType.Boolean, 2, DBNull.Value));
                        cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                        cmd.Transaction = transaction;
                        i = cmd.ExecuteNonQuery();
                        ok = true;
                    }
                }

                if (!ok) conn.Close();

                if (ok)
                {
                    if (clsTools.etat_modification_user == 1 || clsTools.etat_modification_user == 2 || clsTools.etat_modification_user == 3)
                    {
                        if (conn.State != ConnectionState.Open) conn.Open();

                        //On récupère le nom de l'utilisateur qui correspond au premier qui a été créé à la première fois
                        //et qui est équivalente au nom du schema de ce dernier

                        using (IDbCommand cmd2 = conn.CreateCommand())
                        {
                            cmd2.CommandText = string.Format(@"SELECT utilisateur.schema_user FROM utilisateur WHERE utilisateur.id=" + varclsutilisateur.Id);
                            cmd2.Transaction = transaction;

                            using (IDataReader dr = cmd2.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    varclsutilisateur.Nomuser = dr["schema_user"].ToString();
                                }
                            }
                        }

                        //Si l'on à cocher la case à cocher d'activation de l'utilisateur on doit le donner accès à se connecter ou non
                        if ((bool)varclsutilisateur.Activation)
                        {
                            using (IDbCommand cmd3 = conn.CreateCommand())
                            {
                                cmd3.CommandText = string.Format(@"grant connect to " + varclsutilisateur.Nomuser); //On interdit à l'utilisateur de se connecter au serveur
                                cmd3.Transaction = transaction;
                                i = cmd3.ExecuteNonQuery();
                                transaction.Commit();
                                conn.Close();
                            }
                        }
                        else
                        {
                            using (IDbCommand cmd3 = conn.CreateCommand())
                            {
                                cmd3.CommandText = string.Format(@"revoke connect to " + varclsutilisateur.Nomuser); //On interdit à l'utilisateur de se connecter au serveur
                                cmd3.Transaction = transaction;
                                i = cmd3.ExecuteNonQuery();
                                transaction.Commit();
                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();

                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec lors de la modification de l'utilisateur, " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                    throw new Exception(exc.Message);
                }

                conn.Close();
            }
            clsTools.etat_modification_user = -1;
            return i;
        }

        public int deleteClsutilisateur(clsutilisateur varclsutilisateur)
        {
            int i = 0;
            IDbTransaction transaction = null;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                using (IDbCommand cmd1 = conn.CreateCommand())
                {
                    cmd1.CommandText = string.Format(@"SELECT utilisateur.schema_user FROM utilisateur WHERE utilisateur.id=" + varclsutilisateur.Id);
                    cmd1.Transaction = transaction;
                    using (IDataReader dr = cmd1.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["schema_user"].ToString().Trim().Equals("")) varclsutilisateur.Schema_user = dr["schema_user"].ToString();
                        }
                    }
                }

                //Avant de supprimer l'utilisateur dans la table, on supprime son schema qui correspond au premier nom d'utilisateur crée
                //puis on supprime son nom d'utilisateur et enfin on supprime son login
                using (IDbCommand cmd2 = conn.CreateCommand())
                {
                    cmd2.CommandText = string.Format("DROP SCHEMA " + varclsutilisateur.Schema_user + @" 
                                                      DROP USER " + varclsutilisateur.Schema_user + @"
                                                      DROP LOGIN " + varclsutilisateur.Nomuser);
                    cmd2.Transaction = transaction;
                    i = cmd2.ExecuteNonQuery();
                }

                //Enfin on supprime l'utilisateur dans la table des utilisateurs
                using (IDbCommand cmd3 = conn.CreateCommand())
                {
                    cmd3.CommandText = string.Format("DELETE FROM utilisateur WHERE  1=1  AND id=@id ");
                    cmd3.Parameters.Add(getParameter(cmd3, "@id", DbType.Int32, 4, varclsutilisateur.Id));
                    cmd3.Transaction = transaction;
                    i = cmd3.ExecuteNonQuery();
                    transaction.Commit();
                }

                conn.Close();
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();

                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec lors de la suppression de l'utilisateur, " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                    throw new Exception(exc.Message);
                }
            }
            return i;
        }

        /// <summary>
        /// Permet de verifier les paramètres de connexion de l'utilisateur, donc username et password
        /// et retourne un tableau contenant successivement l'Id de l'Agent, son nom et ses droits qui determinent son niveau
        /// </summary>
        /// <param name="String nom d'utilisateur"></param>
        /// <param name="String mot de passe"></param>
        /// <returns>ArrayList</returns>
        public ArrayList verifieLoginUser(string username, string password)
        {
            ArrayList lstValue = new ArrayList();
            bool okActivateUser = false;

            //Echec de la connexion en superAdministrateur alors on peut se connecte en Administrateur 
            //ou en invite (User)
            ////if (username.ToLower().Equals("sa"))
            ////{
            ////    throw new Exception("L'utilisateur 'SA' a été désactivé dans cette application pour raisons de sécurité, veuillez contacter votre Administrateur");
            ////}
            ////else 
            if (username.ToLower().Equals("sa"))//Super Administrateur par defaut
            {
                //Super utilisateur de la BD
                lstValue.Add("0");
                lstValue.Add("Superutilisateur de la BD");
                lstValue.Add("Administrateur");
                lstValue.Add(true);
            }
            else
            {
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    //On commence par recuperer le password chiffre dans la BD pour la comparer avec celui que 
                    //le user a saisi
                    string strBDdecipherPasswor = "", strBDCipher = "";
                    bool ok = false;

                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format("SELECT motpass from utilisateur WHERE nomuser='{0}'", username);
                        IDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            strBDCipher = (dr["motpass"]).ToString();
                            strBDdecipherPasswor = ImplementChiffer.Instance.Decipher((dr["motpass"]).ToString(), "Jos@mRootP@ss");
                            ok = true;
                        }
                        dr.Close();
                        cmd.Dispose();
                    }


                    if (ok && strBDdecipherPasswor.CompareTo(password) == 0)
                    {
                        using (IDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = string.Format(@"SELECT personne.id AS id,ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') AS nom,utilisateur.activation AS activation,utilisateur.nomuser,utilisateur.droits AS droits,utilisateur.motpass FROM personne 
                            LEFT OUTER JOIN utilisateur ON personne.id=utilisateur.id_personne WHERE utilisateur.nomuser='{0}' AND utilisateur.motpass='{1}'", username, strBDCipher);

                            using (IDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    lstValue.Add(dr["id"].ToString());
                                    lstValue.Add(Convert.ToString(dr["nom"]));
                                    lstValue.Add(Convert.ToString(dr["droits"]));//Tous les droit de l'utilisateur
                                                                                 /*Ces droits sont:0->Administrateur : Administrateur de la BD avec tous les droits
                                                                                                   1->Admin          : Adminikstrateur local de l'application avec certaines restrictions
                                                                                                     comme la non possibilite de suppression de users ou des enregistrements
                                                                                                   2->User           : Utilisateur simple avec beaucoup des restrictions
                                                                                 */

                                    okActivateUser = Convert.ToBoolean(dr["activation"]);

                                    //Recuperation du nombre des droits de l'utilisateur
                                    int nbr = 0;

                                    if (!string.IsNullOrEmpty(lstValue[2].ToString()))
                                    {
                                        string[] nbdroit = lstValue[2].ToString().Split(',');
                                        foreach (string str in nbdroit)
                                            nbr++;
                                        clsTools.nombre_droit = nbr;//Nombre total des droits de l'utilisateur
                                    }

                                    if (okActivateUser)
                                    {
                                        if (clsTools.nombre_droit == 0)
                                        {
                                            lstValue.Add(false);
                                            throw new Exception("Cet utilisateur est activé mais n'a encore aucun droit");
                                        }
                                        else
                                        {
                                            //Utilisateur valide
                                            lstValue.Add(true);
                                        }
                                    }
                                    else
                                    {
                                        lstValue.Add(false);

                                        if (clsTools.nombre_droit == 0)
                                        {
                                            throw new Exception("Cet utilisateur est désactivé et n'a aucun droit");
                                        }
                                        else
                                        {
                                            throw new Exception("Cet utilisateur est désactivé mais a des droits d'accès");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        lstValue.Add(false);
                        throw new Exception("Nom d'utilisateur ou mot de passe invalide, contacter votre administrateur");
                    }
                    conn.Close();
                }
                catch (Exception exc)
                {
                    conn.Close();
                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Vérification des paramètres de connexion de l'utilisateur : username et password : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                    throw new Exception(exc.Message);
                }
            }
            return lstValue;
        }

        public DataTable getAllClsutilisateur_Agent()
        {
            DataTable lstclstbl_utilisateur = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"SELECT utilisateur.id,utilisateur.id_personne,utilisateur.nomuser,utilisateur.motpass,utilisateur.schema_user,utilisateur.droits,utilisateur.activation, ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') AS nom FROM utilisateur 
                    INNER JOIN personne ON personne.id = utilisateur.id_personne ORDER BY utilisateur.nomuser ASC");

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclstbl_utilisateur.Load(dr);
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstbl_utilisateur;
        }

        public DataTable getAllClsutilisateur_Agent2(int intid)
        {
            DataTable lstclstbl_utilisateur = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"SELECT utilisateur.id,utilisateur.id_personne,utilisateur.nomuser,utilisateur.motpass,utilisateur.schema_user,
                    utilisateur.droits,utilisateur.activation, ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'') AS nom FROM utilisateur 
                    INNER JOIN personne ON personne.id = utilisateur.id_personne WHERE utilisateur.id=" + intid);

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclstbl_utilisateur.Load(dr);
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclstbl_utilisateur;
        }

        public clsutilisateur getClsutilisateurUser(string nom_user)
        {
            clsutilisateur varclsutilisateur = new clsutilisateur();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"SELECT utilisateur.id AS idUser,utilisateur.id_personne,utilisateur.nomuser,utilisateur.motpass,
                    utilisateur.schema_user,utilisateur.droits,utilisateur.activation, ISNULL(personne.nom,'') + ' ' + ISNULL(personne.postnom,'') + ' ' + ISNULL(personne.prenom,'')  AS nom FROM utilisateur 
                    INNER JOIN personne ON personne.id = utilisateur.id_personne WHERE utilisateur.nomuser='{0}'", nom_user);

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            //Utilisateur
                            if (!dr["idUser"].ToString().Trim().Equals("")) varclsutilisateur.Id = int.Parse(dr["idUser"].ToString());
                            varclsutilisateur.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsutilisateur.Nomuser = dr["nomuser"].ToString();
                            varclsutilisateur.Motpass = ImplementChiffer.Instance.Decipher(dr["motpass"].ToString(), "Jos@mRootP@ss");
                            varclsutilisateur.Schema_user = dr["schema_user"].ToString();
                            varclsutilisateur.Droits = dr["droits"].ToString();
                            if (!dr["activation"].ToString().Trim().Equals("")) varclsutilisateur.Activation = bool.Parse(dr["activation"].ToString());

                            //Personne
                            varclsutilisateur.Id_personne = int.Parse(dr["id_personne"].ToString());
                            varclsutilisateur.Nom = dr["nom"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Vérification des paramètres de connexion de l'utilisateur : username et password : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclsutilisateur;
        }

        public int updateClsutilisateur_droit(int id_user, string droits)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                //Modification de l'utilisateur
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE utilisateur  SET droits=@droits  WHERE 1=1  AND id=@id ");
                    if (droits != null) cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 100, droits));
                    else cmd.Parameters.Add(getParameter(cmd, "@droits", DbType.String, 100, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, id_user));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'utilisateur' avec la classe 'clsutilisateur' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion END ADD UTILISATEUR
        #region GESTION DES DROITS D'ACCES SUR LES TABLES POUR LES UTILISATEUR
        public string[] getLogin_SchemaUser(int id_user)
        {
            string[] schema = new string[2];

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT nomuser,schema_user  FROM utilisateur WHERE id=" + id_user);

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            schema[0] = dr["nomuser"].ToString();
                            schema[1] = dr["schema_user"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'tbl_fiche_menage' avec la classe 'clstbl_fiche_menage' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return schema;
        }

        public List<int> getDroitsUser(int id_user)
        {
            List<int> droits = new List<int>();

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT droits FROM utilisateur WHERE id=" + id_user);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string[] temp = dr["droits"].ToString().Split(',');
                            int taille = temp.Length;

                            foreach (string str in temp)
                            {
                                if (str.ToString().Equals("Administrateur")) droits.Add(0);
                                else if (str.Equals("Admin")) droits.Add(1);
                                else if (str.Equals("User")) droits.Add(2);
                            }
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'tbl_fiche_menage' avec la classe 'clstbl_fiche_menage' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return droits;
        }

        public void grantPermission(List<int> permission, string nom_login, string nom_utilisateur)
        {
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                //On tourne dans la boucle qui tournera tant qu'il ya encore un groupe de permission à accordé
                foreach (int droit in permission)
                {
                    if (droit == 0)
                    {
                        #region Droit pour administrateur (Ce dernier a tous les droits sur le systeme)
                        string requete = @"exec sp_addsrvrolemember '" + nom_login + @"','sysadmin' 
                        exec sp_addsrvrolemember '" + nom_login + @"','securityadmin' 
                        exec sp_addsrvrolemember '" + nom_login + @"','dbcreator' 
                        exec sp_addrolemember 'db_owner','" + nom_utilisateur + @"'
                        exec sp_addrolemember 'db_ddladmin','" + nom_utilisateur + @"'
                        exec sp_addrolemember 'db_accessadmin','" + nom_utilisateur + @"'";

                        using (IDbCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = string.Format(requete);
                            cmd.ExecuteNonQuery();
                        }
                        #endregion
                    }
                    else if (droit == 1)
                    {
                        #region Droit pour Admin (Ce dernier est aussi administrateur mais avec certaines limites comme suppressionm etc)
                        string requete = @"grant select,insert,update on compte to " + nom_utilisateur + @" 
                        grant select,insert,update on marque  to " + nom_utilisateur + @"
                        grant select,insert,update on modele  to " + nom_utilisateur + @"
                        grant select,insert,update on couleur  to " + nom_utilisateur + @"
                        grant select,insert,update on poids  to " + nom_utilisateur + @"
                        grant select,insert,update on type_ordinateur  to " + nom_utilisateur + @"
                        grant select,insert,update on type_imprimante  to " + nom_utilisateur + @"
                        grant select,insert,update on type_amplificateur  to " + nom_utilisateur + @"
                        grant select,insert,update on type_routeur_AP  to " + nom_utilisateur + @"
                        grant select,insert,update on type_AP  to " + nom_utilisateur + @"
                        grant select,insert,update on type_switch  to " + nom_utilisateur + @"
                        grant select,insert,update on type_clavier  to " + nom_utilisateur + @"
                        grant select,insert,update on etat_materiel  to " + nom_utilisateur + @"
                        grant select,insert,update on type_OS  to " + nom_utilisateur + @"
                        grant select,insert,update on architecture_OS  to " + nom_utilisateur + @"
                        grant select,insert,update on OS  to " + nom_utilisateur + @"
                        grant select,insert,update on version_ios  to " + nom_utilisateur + @"
                        grant select,insert,update on netette  to " + nom_utilisateur + @"
                        grant select,insert,update on materiel  to " + nom_utilisateur + @"
                        grant select,insert,update on grade  to " + nom_utilisateur + @"
                        grant select,insert,update on personne  to " + nom_utilisateur + @"
                        grant select,insert,update on type_lieu_affectation  to " + nom_utilisateur + @"
                        grant select,insert,update on AC  to " + nom_utilisateur + @"
                        grant select,insert,update on optio  to " + nom_utilisateur + @"
                        grant select,insert,update on promotion  to " + nom_utilisateur + @"
                        grant select,insert,update on section  to " + nom_utilisateur + @"
                        grant select,insert,update on retrait_materiel  to " + nom_utilisateur + @"
                        grant select,insert,update on detail_retrait_materiel  to " + nom_utilisateur + @"
                        grant select,insert,update on signataire  to " + nom_utilisateur + @"
                        grant select,insert,update on salle  to " + nom_utilisateur + @"
                        grant select,insert,update on fonction  to " + nom_utilisateur + @"
                        grant select,insert,update on lieu_affectation  to " + nom_utilisateur + @"
                        grant select,insert,update on affectation_materiel  to " + nom_utilisateur + @"
                        grant select on utilisateur  to " + nom_utilisateur + @"
                        grant select on groupe  to " + nom_utilisateur;

                        using (IDbCommand cmd1 = conn.CreateCommand())
                        {
                            cmd1.CommandText = string.Format(requete);
                            cmd1.ExecuteNonQuery();
                        }
                        #endregion
                    }
                    else if (droit == 2)
                    {
                        #region Droit pour User
                        string requete = @"grant select on compte to " + nom_utilisateur + @" 
                        grant select on marque  to " + nom_utilisateur + @"
                        grant select on modele  to " + nom_utilisateur + @"
                        grant select on couleur  to " + nom_utilisateur + @"
                        grant select on poids  to " + nom_utilisateur + @"
                        grant select on type_ordinateur  to " + nom_utilisateur + @"
                        grant select on type_imprimante  to " + nom_utilisateur + @"
                        grant select on type_amplificateur  to " + nom_utilisateur + @"
                        grant select on type_routeur_AP  to " + nom_utilisateur + @"
                        grant select on type_AP  to " + nom_utilisateur + @"
                        grant select on type_switch  to " + nom_utilisateur + @"
                        grant select on type_clavier  to " + nom_utilisateur + @"
                        grant select on etat_materiel  to " + nom_utilisateur + @"
                        grant select on type_OS  to " + nom_utilisateur + @"
                        grant select on architecture_OS  to " + nom_utilisateur + @"
                        grant select on OS  to " + nom_utilisateur + @"
                        grant select on version_ios  to " + nom_utilisateur + @"
                        grant select on netette  to " + nom_utilisateur + @"
                        grant select on materiel  to " + nom_utilisateur + @"
                        grant select on grade  to " + nom_utilisateur + @"
                        grant select on personne  to " + nom_utilisateur + @"
                        grant select on type_lieu_affectation  to " + nom_utilisateur + @"
                        grant select on AC  to " + nom_utilisateur + @"
                        grant select on optio  to " + nom_utilisateur + @"
                        grant select on promotion  to " + nom_utilisateur + @"
                        grant select on section  to " + nom_utilisateur + @"
                        grant select on retrait_materiel  to " + nom_utilisateur + @"
                        grant select on detail_retrait_materiel  to " + nom_utilisateur + @"
                        grant select on signataire  to " + nom_utilisateur + @"
                        grant select on salle  to " + nom_utilisateur + @"
                        grant select on fonction  to " + nom_utilisateur + @"
                        grant select on lieu_affectation  to " + nom_utilisateur + @"
                        grant select on affectation_materiel  to " + nom_utilisateur + @"
                        grant select on utilisateur  to " + nom_utilisateur + @"
                        grant select on groupe  to " + nom_utilisateur;

                        using (IDbCommand cmd2 = conn.CreateCommand())
                        {
                            cmd2.CommandText = string.Format(requete);
                            cmd2.ExecuteNonQuery();
                        }
                        #endregion
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec d'attribution des droits à l'utilisateur, veuillez réessayez ultérieurement : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
        }

        public void revokePermission(List<int> permission, string nom_login, string nom_utilisateur)
        {
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                //On tourne dans la boucle qui tournera tant qu'il ya encore un groupe dde permission à accordé
                foreach (int droit in permission)
                {
                    if (droit == 0)
                    {
                        //Droit pour administrateur
                        throw new Exception("Les droits de l'administrateur ne peuvent pas être retirés à ce niveau, reportez vous au moteur de SGBD");
                    }
                    else if (droit == 1)
                    {
                        #region Droit pour Admin
                        string requete = @"revoke select,insert,update on compte to " + nom_utilisateur + @" 
                        revoke select,insert,update on marque  to " + nom_utilisateur + @"
                        revoke select,insert,update on modele  to " + nom_utilisateur + @"
                        revoke select,insert,update on couleur  to " + nom_utilisateur + @"
                        revoke select,insert,update on poids  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_ordinateur  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_imprimante  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_amplificateur  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_routeur_AP  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_AP  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_switch  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_clavier  to " + nom_utilisateur + @"
                        revoke select,insert,update on etat_materiel  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_OS  to " + nom_utilisateur + @"
                        revoke select,insert,update on architecture_OS  to " + nom_utilisateur + @"
                        revoke select,insert,update on OS  to " + nom_utilisateur + @"
                        revoke select,insert,update on version_ios  to " + nom_utilisateur + @"
                        revoke select,insert,update on netette  to " + nom_utilisateur + @"
                        revoke select,insert,update on materiel  to " + nom_utilisateur + @"
                        revoke select,insert,update on grade  to " + nom_utilisateur + @"
                        revoke select,insert,update on personne  to " + nom_utilisateur + @"
                        revoke select,insert,update on type_lieu_affectation  to " + nom_utilisateur + @"
                        revokerevoke select,insert,update on AC  to " + nom_utilisateur + @"
                        revoke select,insert,update on optio  to " + nom_utilisateur + @"
                        revoke select,insert,update on promotion  to " + nom_utilisateur + @"
                        revoke select,insert,update on section  to " + nom_utilisateur + @"
                        revoke select,insert,update on retrait_materiel  to " + nom_utilisateur + @"
                        revoke select,insert,update on detail_retrait_materiel  to " + nom_utilisateur + @"
                        revoke select,insert,update on signataire  to " + nom_utilisateur + @"
                        revoke select,insert,update on salle  to " + nom_utilisateur + @"
                        revoke select,insert,update on fonction  to " + nom_utilisateur + @"
                        revoke select,insert,update on lieu_affectation  to " + nom_utilisateur + @"
                        revoke select,insert,update on affectation_materiel  to " + nom_utilisateur + @"
                        revoke select on utilisateur  to " + nom_utilisateur + @"
                        revoke select on groupe  to " + nom_utilisateur;

                        using (IDbCommand cmd1 = conn.CreateCommand())
                        {
                            cmd1.CommandText = string.Format(requete);
                            cmd1.ExecuteNonQuery();
                        }
                        #endregion
                    }
                    else if (droit == 2)
                    {
                        #region Droit pour User
                        string requete = @"revoke select on compte to " + nom_utilisateur + @" 
                        revoke select on marque  to " + nom_utilisateur + @"
                        revoke select on modele  to " + nom_utilisateur + @"
                        revoke select on couleur  to " + nom_utilisateur + @"
                        revoke select on poids  to " + nom_utilisateur + @"
                        revoke select on type_ordinateur  to " + nom_utilisateur + @"
                        revoke select on type_imprimante  to " + nom_utilisateur + @"
                        revoke select on type_amplificateur  to " + nom_utilisateur + @"
                        revoke select on type_routeur_AP  to " + nom_utilisateur + @"
                        revoke select on type_AP  to " + nom_utilisateur + @"
                        revoke select on type_switch  to " + nom_utilisateur + @"
                        revoke select on type_clavier  to " + nom_utilisateur + @"
                        revoke select on etat_materiel  to " + nom_utilisateur + @"
                        revoke select on type_OS  to " + nom_utilisateur + @"
                        revoke select on architecture_OS  to " + nom_utilisateur + @"
                        revoke select on OS  to " + nom_utilisateur + @"
                        revoke select on version_ios  to " + nom_utilisateur + @"
                        revoke select on netette  to " + nom_utilisateur + @"
                        revoke select on materiel  to " + nom_utilisateur + @"
                        revoke select on grade  to " + nom_utilisateur + @"
                        revoke select on personne  to " + nom_utilisateur + @"
                        revoke select on type_lieu_affectation  to " + nom_utilisateur + @"
                        revoke select on AC  to " + nom_utilisateur + @"
                        revoke select on optio  to " + nom_utilisateur + @"
                        revoke select on promotion  to " + nom_utilisateur + @"
                        revoke select on section  to " + nom_utilisateur + @"
                        revoke select on retrait_materiel  to " + nom_utilisateur + @"
                        revoke select on detail_retrait_materiel  to " + nom_utilisateur + @"
                        revoke select on signataire  to " + nom_utilisateur + @"
                        revoke select on salle  to " + nom_utilisateur + @"
                        revoke select on fonction  to " + nom_utilisateur + @"
                        revoke select on lieu_affectation  to " + nom_utilisateur + @"
                        revoke select on affectation_materiel  to " + nom_utilisateur + @"
                        revoke select on utilisateur  to " + nom_utilisateur + @"
                        revoke select on groupe  to " + nom_utilisateur;

                        using (IDbCommand cmd2 = conn.CreateCommand())
                        {
                            cmd2.CommandText = string.Format(requete);
                            cmd2.ExecuteNonQuery();
                        }
                        #endregion
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec retrait des droits à l'utilisateur, veuillez réessayez ultérieurement : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
        }
        #endregion

        #endregion CLSUTILISATEUR 
        #region  CLSCATEGORIE_MATERIEL
        public clscategorie_materiel getClscategorie_materiel(object intid)
        {
            clscategorie_materiel varclscategorie_materiel = new clscategorie_materiel();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM categorie_materiel WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {

                            if (!dr["id"].ToString().Trim().Equals("")) varclscategorie_materiel.Id = int.Parse(dr["id"].ToString());
                            varclscategorie_materiel.Designation = dr["designation"].ToString();
                            varclscategorie_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscategorie_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return varclscategorie_materiel;
        }

        public List<clscategorie_materiel> getAllClscategorie_materiel(string criteria)
        {
            List<clscategorie_materiel> lstclscategorie_materiel = new List<clscategorie_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM categorie_materiel  WHERE";
                    sql += "  designation LIKE '%" + criteria + "%'";
                    sql += "  OR   user_created LIKE '%" + criteria + "%'";
                    sql += "  OR   user_modified LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscategorie_materiel varclscategorie_materiel = null;
                        while (dr.Read())
                        {

                            varclscategorie_materiel = new clscategorie_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscategorie_materiel.Id = int.Parse(dr["id"].ToString());
                            varclscategorie_materiel.Designation = dr["designation"].ToString();
                            varclscategorie_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscategorie_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscategorie_materiel.Add(varclscategorie_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscategorie_materiel;
        }

        public List<clscategorie_materiel> getAllClscategorie_materiel()
        {
            List<clscategorie_materiel> lstclscategorie_materiel = new List<clscategorie_materiel>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM categorie_materiel ORDER BY designation ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clscategorie_materiel varclscategorie_materiel = null;
                        while (dr.Read())
                        {

                            varclscategorie_materiel = new clscategorie_materiel();
                            if (!dr["id"].ToString().Trim().Equals("")) varclscategorie_materiel.Id = int.Parse(dr["id"].ToString());
                            varclscategorie_materiel.Designation = dr["designation"].ToString();
                            varclscategorie_materiel.User_created = dr["user_created"].ToString();
                            if (!dr["date_created"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_created = DateTime.Parse(dr["date_created"].ToString());
                            varclscategorie_materiel.User_modified = dr["user_modified"].ToString();
                            if (!dr["date_modified"].ToString().Trim().Equals("")) varclscategorie_materiel.Date_modified = DateTime.Parse(dr["date_modified"].ToString());
                            lstclscategorie_materiel.Add(varclscategorie_materiel);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return lstclscategorie_materiel;
        }

        public int insertClscategorie_materiel(clscategorie_materiel varclscategorie_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO categorie_materiel ( id,designation,user_created,date_created,user_modified,date_modified ) VALUES (@id,@designation,@user_created,@date_created,@user_modified,@date_modified  )");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscategorie_materiel.Id));
                    if (varclscategorie_materiel.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclscategorie_materiel.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscategorie_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscategorie_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscategorie_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscategorie_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscategorie_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClscategorie_materiel(clscategorie_materiel varclscategorie_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE categorie_materiel  SET designation=@designation,user_created=@user_created,date_created=@date_created,user_modified=@user_modified,date_modified=@date_modified  WHERE 1=1  AND id=@id ");
                    if (varclscategorie_materiel.Designation != null) cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, varclscategorie_materiel.Designation));
                    else cmd.Parameters.Add(getParameter(cmd, "@designation", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.User_created != null) cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, varclscategorie_materiel.User_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_created", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.Date_created.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, varclscategorie_materiel.Date_created));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_created", DbType.DateTime, 8, DBNull.Value));
                    if (varclscategorie_materiel.User_modified != null) cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, varclscategorie_materiel.User_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@user_modified", DbType.String, 50, DBNull.Value));
                    if (varclscategorie_materiel.Date_modified.HasValue) cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, varclscategorie_materiel.Date_modified));
                    else cmd.Parameters.Add(getParameter(cmd, "@date_modified", DbType.DateTime, 8, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscategorie_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClscategorie_materiel(clscategorie_materiel varclscategorie_materiel)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM categorie_materiel  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclscategorie_materiel.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'categorie_materiel' avec la classe 'clscategorie_materiel' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFile.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSCATEGORIE_MATERIEL 
    } //***fin class 
} //***fin namespace 
