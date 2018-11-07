using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using ManageUtilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;

namespace smartManage.RadiusStudentsModel
{
    public class clsMetier2
    {
        const string DirectoryUtilLog = "Log"; //***Les variables globales***
        private static string _ConnectionString, _host, _db, _user, _pwd;
        private static clsMetier2 Fact;
        private MySqlConnection conn;
        private static GsmCommMain comm;
        #region prerecquis
        public static clsMetier2 GetInstance()
        {
            if (Fact == null)
                Fact = new clsMetier2();
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
            conn = new MySqlConnection(ConnectionString);
        }
        public void Initialize(clsConnexion2 con)
        {
            _host = con.Serveur;// host;
            _db = con.DB; 
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", _host, _db, _user, _pwd);
            conn = new MySqlConnection(sch);
        }
        public void Initialize(clsConnexion2 con, int type)
        {
            _host = con.Serveur;// host;
            _db = con.DB; ;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}", _host, _db, _user, _pwd);

            //On garde la chaine de connexion pour utilisation avec les reports
            smartManage.RadiusStudentsModel.Properties.Settings.Default.strChaineConnexion = sch;
            conn = new MySqlConnection(sch);
        }
        public void Initialize(string host, string db, string user, string pwd)
        {
            _host = host;
            _db = db;
            _user = user;
            _pwd = pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}", _host, _db, _user, _pwd);
            conn = new MySqlConnection(sch);
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

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                List<string> lstView = new List<string>();

                //Apres ouvrerture de la connexion on verifier la vue et on la cree si besoin est
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT table_name FROM information_schema.views WHERE table_name='groupe'";

                    using (IDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lstView.Add(rd["table_name"].ToString());
                        }
                    }
                }              
                
                if(lstView.Count > 0)
                {
                    int count = 1;
                    foreach (string str in lstView)
                    {
                        if (count == lstView.Count && str.Equals("groupe"))
                            break;
                        else
                        {
                            //Creation de la vue
                            using (IDbCommand cmd1 = conn.CreateCommand())
                            {
                                cmd1.CommandText = string.Format(@"CREATE VIEW groupe 
					        AS
					        SELECT DISTINCT radgroupcheck.groupname FROM radgroupcheck");
                                cmd1.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    //Creation de la vue
                    using (IDbCommand cmd1 = conn.CreateCommand())
                    {
                        cmd1.CommandText = string.Format(@"CREATE VIEW groupe 
					        AS
					        SELECT DISTINCT radgroupcheck.groupname FROM radgroupcheck");
                        cmd1.ExecuteNonQuery();
                    }
                }

                conn.Close();
            }
            catch (Exception exc)
            {
                bl = false;
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat de la connexion à la BD sans paramètre : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusAdmin.txt");
                throw new Exception(exc.Message);
            }
            return bl;
        }
        public bool isConnect(clsConnexion2 con)
        {
            bool bl = true;
            _host = con.Serveur;// host;
            _db = con.DB;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}", _host, _db, _user, _pwd);
            conn = new MySqlConnection(sch);
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
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat connexion à la BD avec paramètre : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return bl;
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
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Récupération de toutes les bases de Données SQLServer : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
        }

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
        #endregion prerecquis
        //POUR RADIUS
        #region  CLSNAS
        public clsnas getClsnas(object intid)
        {
            clsnas varclsnas = new clsnas();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM nas WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsnas.Id = int.Parse(dr["id"].ToString());
                            varclsnas.Nasname = dr["nasname"].ToString();
                            varclsnas.Shortname = dr["shortname"].ToString();
                            varclsnas.Type = dr["type"].ToString();
                            if (!dr["ports"].ToString().Trim().Equals("")) varclsnas.Ports = int.Parse(dr["ports"].ToString());
                            varclsnas.Secret = dr["secret"].ToString();
                            varclsnas.Server = dr["server"].ToString();
                            varclsnas.Community = dr["community"].ToString();
                            varclsnas.Description = dr["description"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsnas;
        }

        public List<clsnas> getAllClsnas(string criteria)
        {
            List<clsnas> lstclsnas = new List<clsnas>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM nas  WHERE";
                    sql += "  nasname LIKE '%" + criteria + "%'";
                    sql += "  OR   shortname LIKE '%" + criteria + "%'";
                    sql += "  OR   type LIKE '%" + criteria + "%'";
                    sql += "  OR   secret LIKE '%" + criteria + "%'";
                    sql += "  OR   server LIKE '%" + criteria + "%'";
                    sql += "  OR   community LIKE '%" + criteria + "%'";
                    sql += "  OR   description LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsnas varclsnas = null;
                        while (dr.Read())
                        {
                            varclsnas = new clsnas();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsnas.Id = int.Parse(dr["id"].ToString());
                            varclsnas.Nasname = dr["nasname"].ToString();
                            varclsnas.Shortname = dr["shortname"].ToString();
                            varclsnas.Type = dr["type"].ToString();
                            if (!dr["ports"].ToString().Trim().Equals("")) varclsnas.Ports = int.Parse(dr["ports"].ToString());
                            varclsnas.Secret = dr["secret"].ToString();
                            varclsnas.Server = dr["server"].ToString();
                            varclsnas.Community = dr["community"].ToString();
                            varclsnas.Description = dr["description"].ToString();
                            lstclsnas.Add(varclsnas);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'nas' avec la classe 'clsnas' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsnas;
        }

        public List<clsnas> getAllClsnas()
        {
            List<clsnas> lstclsnas = new List<clsnas>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT nas.*,(SELECT COUNT(id) FROM nas) AS nbr_enreg FROM nas ORDER BY nasname ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsnas varclsnas = null;
                        while (dr.Read())
                        {
                            varclsnas = new clsnas();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsnas.Id = int.Parse(dr["id"].ToString());
                            varclsnas.Nasname = dr["nasname"].ToString();
                            varclsnas.Shortname = dr["shortname"].ToString();
                            varclsnas.Type = dr["type"].ToString();
                            if (!dr["ports"].ToString().Trim().Equals("")) varclsnas.Ports = int.Parse(dr["ports"].ToString());
                            varclsnas.Secret = dr["secret"].ToString();
                            varclsnas.Server = dr["server"].ToString();
                            varclsnas.Community = dr["community"].ToString();
                            varclsnas.Description = dr["description"].ToString();
                            varclsnas.Nombre_enregistrement = int.Parse(dr["nbr_enreg"].ToString());
                            lstclsnas.Add(varclsnas);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'nas' avec la classe 'clsnas' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsnas;
        }

        public int insertClsnas(clsnas varclsnas)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO nas ( nasname,shortname,type,ports,secret,server,community,description ) VALUES (@nasname,@shortname,@type,@ports,@secret,@server,@community,@description  )");
                    if (varclsnas.Nasname != null) cmd.Parameters.Add(getParameter(cmd, "@nasname", DbType.String, 128, varclsnas.Nasname));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasname", DbType.String, 128, DBNull.Value));
                    if (varclsnas.Shortname != null) cmd.Parameters.Add(getParameter(cmd, "@shortname", DbType.String, 32, varclsnas.Shortname));
                    else cmd.Parameters.Add(getParameter(cmd, "@shortname", DbType.String, 32, DBNull.Value));
                    if (varclsnas.Type != null) cmd.Parameters.Add(getParameter(cmd, "@type", DbType.String, 30, varclsnas.Type));
                    else cmd.Parameters.Add(getParameter(cmd, "@type", DbType.String, 30, DBNull.Value));
                    if (varclsnas.Ports.HasValue) cmd.Parameters.Add(getParameter(cmd, "@ports", DbType.Int32, 4, varclsnas.Ports));
                    else cmd.Parameters.Add(getParameter(cmd, "@ports", DbType.Int32, 4, DBNull.Value));
                    if (varclsnas.Secret != null) cmd.Parameters.Add(getParameter(cmd, "@secret", DbType.String, 60, varclsnas.Secret));
                    else cmd.Parameters.Add(getParameter(cmd, "@secret", DbType.String, 60, DBNull.Value));
                    if (varclsnas.Server != null) cmd.Parameters.Add(getParameter(cmd, "@server", DbType.String, 64, varclsnas.Server));
                    else cmd.Parameters.Add(getParameter(cmd, "@server", DbType.String, 64, DBNull.Value));
                    if (varclsnas.Community != null) cmd.Parameters.Add(getParameter(cmd, "@community", DbType.String, 50, varclsnas.Community));
                    else cmd.Parameters.Add(getParameter(cmd, "@community", DbType.String, 50, DBNull.Value));
                    if (varclsnas.Description != null) cmd.Parameters.Add(getParameter(cmd, "@description", DbType.String, 200, varclsnas.Description));
                    else cmd.Parameters.Add(getParameter(cmd, "@description", DbType.String, 200, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsnas(clsnas varclsnas)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE nas  SET nasname=@nasname,shortname=@shortname,type=@type,ports=@ports,secret=@secret,server=@server,community=@community,description=@description  WHERE 1=1  AND id=@id ");
                    if (varclsnas.Nasname != null) cmd.Parameters.Add(getParameter(cmd, "@nasname", DbType.String, 128, varclsnas.Nasname));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasname", DbType.String, 128, DBNull.Value));
                    if (varclsnas.Shortname != null) cmd.Parameters.Add(getParameter(cmd, "@shortname", DbType.String, 32, varclsnas.Shortname));
                    else cmd.Parameters.Add(getParameter(cmd, "@shortname", DbType.String, 32, DBNull.Value));
                    if (varclsnas.Type != null) cmd.Parameters.Add(getParameter(cmd, "@type", DbType.String, 30, varclsnas.Type));
                    else cmd.Parameters.Add(getParameter(cmd, "@type", DbType.String, 30, DBNull.Value));
                    if (varclsnas.Ports.HasValue) cmd.Parameters.Add(getParameter(cmd, "@ports", DbType.Int32, 4, varclsnas.Ports));
                    else cmd.Parameters.Add(getParameter(cmd, "@ports", DbType.Int32, 4, DBNull.Value));
                    if (varclsnas.Secret != null) cmd.Parameters.Add(getParameter(cmd, "@secret", DbType.String, 60, varclsnas.Secret));
                    else cmd.Parameters.Add(getParameter(cmd, "@secret", DbType.String, 60, DBNull.Value));
                    if (varclsnas.Server != null) cmd.Parameters.Add(getParameter(cmd, "@server", DbType.String, 64, varclsnas.Server));
                    else cmd.Parameters.Add(getParameter(cmd, "@server", DbType.String, 64, DBNull.Value));
                    if (varclsnas.Community != null) cmd.Parameters.Add(getParameter(cmd, "@community", DbType.String, 50, varclsnas.Community));
                    else cmd.Parameters.Add(getParameter(cmd, "@community", DbType.String, 50, DBNull.Value));
                    if (varclsnas.Description != null) cmd.Parameters.Add(getParameter(cmd, "@description", DbType.String, 200, varclsnas.Description));
                    else cmd.Parameters.Add(getParameter(cmd, "@description", DbType.String, 200, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsnas.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsnas(clsnas varclsnas)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM nas  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsnas.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSNAS 
        #region  CLSRADACCT
        public clsradacct getClsradacct(object intid)
        {
            clsradacct varclsradacct = new clsradacct();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radacct WHERE radacctid={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["radacctid"].ToString().Trim().Equals("")) varclsradacct.Radacctid = long.Parse(dr["radacctid"].ToString());
                            varclsradacct.Acctsessionid = dr["acctsessionid"].ToString();
                            varclsradacct.Acctuniqueid = dr["acctuniqueid"].ToString();
                            varclsradacct.Username = dr["username"].ToString();
                            varclsradacct.Groupname = dr["groupname"].ToString();
                            varclsradacct.Realm = dr["realm"].ToString();
                            varclsradacct.Nasipaddress = dr["nasipaddress"].ToString();
                            varclsradacct.Nasportid = dr["nasportid"].ToString();
                            varclsradacct.Nasporttype = dr["nasporttype"].ToString();
                            if (!dr["acctstarttime"].ToString().Trim().Equals("")) varclsradacct.Acctstarttime = DateTime.Parse(dr["acctstarttime"].ToString());
                            if (!dr["acctstoptime"].ToString().Trim().Equals("")) varclsradacct.Acctstoptime = DateTime.Parse(dr["acctstoptime"].ToString());
                            if (!dr["acctsessiontime"].ToString().Trim().Equals("")) varclsradacct.Acctsessiontime = int.Parse(dr["acctsessiontime"].ToString());
                            varclsradacct.Acctauthentic = dr["acctauthentic"].ToString();
                            varclsradacct.Connectinfo_start = dr["connectinfo_start"].ToString();
                            varclsradacct.Connectinfo_stop = dr["connectinfo_stop"].ToString();
                            if (!dr["acctinputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctinputoctets = long.Parse(dr["acctinputoctets"].ToString());
                            if (!dr["acctoutputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctoutputoctets = long.Parse(dr["acctoutputoctets"].ToString());
                            varclsradacct.Calledstationid = dr["calledstationid"].ToString();
                            varclsradacct.Callingstationid = dr["callingstationid"].ToString();
                            varclsradacct.Acctterminatecause = dr["acctterminatecause"].ToString();
                            varclsradacct.Servicetype = dr["servicetype"].ToString();
                            varclsradacct.Framedprotocol = dr["framedprotocol"].ToString();
                            varclsradacct.Framedipaddress = dr["framedipaddress"].ToString();
                            if (!dr["acctstartdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstartdelay = int.Parse(dr["acctstartdelay"].ToString());
                            if (!dr["acctstopdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstopdelay = int.Parse(dr["acctstopdelay"].ToString());
                            varclsradacct.Xascendsessionsvrkey = dr["xascendsessionsvrkey"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradacct;
        }

        public List<clsradacct> getAllClsradacct(string criteria)
        {
            List<clsradacct> lstclsradacct = new List<clsradacct>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radacct  WHERE ";
                    sql += "  acctsessionid LIKE '%" + criteria + "%'";
                    sql += "  OR   acctuniqueid LIKE '%" + criteria + "%'";
                    sql += "  OR   username LIKE '%" + criteria + "%'";
                    sql += "  OR   groupname LIKE '%" + criteria + "%'";
                    sql += "  OR   realm LIKE '%" + criteria + "%'";
                    sql += "  OR   nasipaddress LIKE '%" + criteria + "%'";
                    sql += "  OR   nasportid LIKE '%" + criteria + "%'";
                    sql += "  OR   nasporttype LIKE '%" + criteria + "%'";
                    sql += "  OR   acctauthentic LIKE '%" + criteria + "%'";
                    sql += "  OR   connectinfo_start LIKE '%" + criteria + "%'";
                    sql += "  OR   connectinfo_stop LIKE '%" + criteria + "%'";
                    sql += "  OR   calledstationid LIKE '%" + criteria + "%'";
                    sql += "  OR   callingstationid LIKE '%" + criteria + "%'";
                    sql += "  OR   acctterminatecause LIKE '%" + criteria + "%'";
                    sql += "  OR   servicetype LIKE '%" + criteria + "%'";
                    sql += "  OR   framedprotocol LIKE '%" + criteria + "%'";
                    sql += "  OR   framedipaddress LIKE '%" + criteria + "%'";
                    sql += "  OR   xascendsessionsvrkey LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradacct varclsradacct = null;
                        while (dr.Read())
                        {
                            varclsradacct = new clsradacct();
                            if (!dr["radacctid"].ToString().Trim().Equals("")) varclsradacct.Radacctid = long.Parse(dr["radacctid"].ToString());
                            varclsradacct.Acctsessionid = dr["acctsessionid"].ToString();
                            varclsradacct.Acctuniqueid = dr["acctuniqueid"].ToString();
                            varclsradacct.Username = dr["username"].ToString();
                            varclsradacct.Groupname = dr["groupname"].ToString();
                            varclsradacct.Realm = dr["realm"].ToString();
                            varclsradacct.Nasipaddress = dr["nasipaddress"].ToString();
                            varclsradacct.Nasportid = dr["nasportid"].ToString();
                            varclsradacct.Nasporttype = dr["nasporttype"].ToString();
                            if (!dr["acctstarttime"].ToString().Trim().Equals("")) varclsradacct.Acctstarttime = DateTime.Parse(dr["acctstarttime"].ToString());
                            if (!dr["acctstoptime"].ToString().Trim().Equals("")) varclsradacct.Acctstoptime = DateTime.Parse(dr["acctstoptime"].ToString());
                            if (!dr["acctsessiontime"].ToString().Trim().Equals("")) varclsradacct.Acctsessiontime = int.Parse(dr["acctsessiontime"].ToString());
                            varclsradacct.Acctauthentic = dr["acctauthentic"].ToString();
                            varclsradacct.Connectinfo_start = dr["connectinfo_start"].ToString();
                            varclsradacct.Connectinfo_stop = dr["connectinfo_stop"].ToString();
                            if (!dr["acctinputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctinputoctets = long.Parse(dr["acctinputoctets"].ToString());
                            if (!dr["acctoutputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctoutputoctets = long.Parse(dr["acctoutputoctets"].ToString());
                            varclsradacct.Calledstationid = dr["calledstationid"].ToString();
                            varclsradacct.Callingstationid = dr["callingstationid"].ToString();
                            varclsradacct.Acctterminatecause = dr["acctterminatecause"].ToString();
                            varclsradacct.Servicetype = dr["servicetype"].ToString();
                            varclsradacct.Framedprotocol = dr["framedprotocol"].ToString();
                            varclsradacct.Framedipaddress = dr["framedipaddress"].ToString();
                            if (!dr["acctstartdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstartdelay = int.Parse(dr["acctstartdelay"].ToString());
                            if (!dr["acctstopdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstopdelay = int.Parse(dr["acctstopdelay"].ToString());
                            varclsradacct.Xascendsessionsvrkey = dr["xascendsessionsvrkey"].ToString();
                            lstclsradacct.Add(varclsradacct);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radacct' avec la classe 'clsradacct' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradacct;
        }

        public List<clsradacct> getAllClsradacct()
        {
            List<clsradacct> lstclsradacct = new List<clsradacct>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT radacct.*,(SELECT COUNT(radacctid) FROM radacct) AS nbr_enreg FROM radacct ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradacct varclsradacct = null;
                        while (dr.Read())
                        {
                            varclsradacct = new clsradacct(); 
                            if (!dr["radacctid"].ToString().Trim().Equals("")) varclsradacct.Radacctid = long.Parse(dr["radacctid"].ToString());
                            varclsradacct.Acctsessionid = dr["acctsessionid"].ToString();
                            varclsradacct.Acctuniqueid = dr["acctuniqueid"].ToString();
                            varclsradacct.Username = dr["username"].ToString();
                            varclsradacct.Groupname = dr["groupname"].ToString();
                            varclsradacct.Realm = dr["realm"].ToString();
                            varclsradacct.Nasipaddress = dr["nasipaddress"].ToString();
                            varclsradacct.Nasportid = dr["nasportid"].ToString();
                            varclsradacct.Nasporttype = dr["nasporttype"].ToString();
                            if (!dr["acctstarttime"].ToString().Trim().Equals("")) varclsradacct.Acctstarttime = DateTime.Parse(dr["acctstarttime"].ToString());
                            if (!dr["acctstoptime"].ToString().Trim().Equals("")) varclsradacct.Acctstoptime = DateTime.Parse(dr["acctstoptime"].ToString());
                            if (!dr["acctsessiontime"].ToString().Trim().Equals("")) varclsradacct.Acctsessiontime = int.Parse(dr["acctsessiontime"].ToString());
                            varclsradacct.Acctauthentic = dr["acctauthentic"].ToString();
                            varclsradacct.Connectinfo_start = dr["connectinfo_start"].ToString();
                            varclsradacct.Connectinfo_stop = dr["connectinfo_stop"].ToString();
                            if (!dr["acctinputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctinputoctets = long.Parse(dr["acctinputoctets"].ToString());
                            if (!dr["acctoutputoctets"].ToString().Trim().Equals("")) varclsradacct.Acctoutputoctets = long.Parse(dr["acctoutputoctets"].ToString());
                            varclsradacct.Calledstationid = dr["calledstationid"].ToString();
                            varclsradacct.Callingstationid = dr["callingstationid"].ToString();
                            varclsradacct.Acctterminatecause = dr["acctterminatecause"].ToString();
                            varclsradacct.Servicetype = dr["servicetype"].ToString();
                            varclsradacct.Framedprotocol = dr["framedprotocol"].ToString();
                            varclsradacct.Framedipaddress = dr["framedipaddress"].ToString();
                            if (!dr["acctstartdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstartdelay = int.Parse(dr["acctstartdelay"].ToString());
                            if (!dr["acctstopdelay"].ToString().Trim().Equals("")) varclsradacct.Acctstopdelay = int.Parse(dr["acctstopdelay"].ToString());
                            varclsradacct.Xascendsessionsvrkey = dr["xascendsessionsvrkey"].ToString();
                            varclsradacct.Nombre_enregistrement = long.Parse(dr["nbr_enreg"].ToString());
                            lstclsradacct.Add(varclsradacct);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradacct;
        }

        public int insertClsradacct(clsradacct varclsradacct)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radacct ( acctsessionid,acctuniqueid,username,groupname,realm,nasipaddress,nasportid,nasporttype,acctstarttime,acctstoptime,acctsessiontime,acctauthentic,connectinfo_start,connectinfo_stop,acctinputoctets,acctoutputoctets,calledstationid,callingstationid,acctterminatecause,servicetype,framedprotocol,framedipaddress,acctstartdelay,acctstopdelay,xascendsessionsvrkey ) VALUES (@acctsessionid,@acctuniqueid,@username,@groupname,@realm,@nasipaddress,@nasportid,@nasporttype,@acctstarttime,@acctstoptime,@acctsessiontime,@acctauthentic,@connectinfo_start,@connectinfo_stop,@acctinputoctets,@acctoutputoctets,@calledstationid,@callingstationid,@acctterminatecause,@servicetype,@framedprotocol,@framedipaddress,@acctstartdelay,@acctstopdelay,@xascendsessionsvrkey  )");
                    if (varclsradacct.Acctsessionid != null) cmd.Parameters.Add(getParameter(cmd, "@acctsessionid", DbType.String, 64, varclsradacct.Acctsessionid));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctsessionid", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Acctuniqueid != null) cmd.Parameters.Add(getParameter(cmd, "@acctuniqueid", DbType.String, 32, varclsradacct.Acctuniqueid));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctuniqueid", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradacct.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradacct.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Realm != null) cmd.Parameters.Add(getParameter(cmd, "@realm", DbType.String, 64, varclsradacct.Realm));
                    else cmd.Parameters.Add(getParameter(cmd, "@realm", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Nasipaddress != null) cmd.Parameters.Add(getParameter(cmd, "@nasipaddress", DbType.String, 15, varclsradacct.Nasipaddress));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasipaddress", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Nasportid != null) cmd.Parameters.Add(getParameter(cmd, "@nasportid", DbType.String, 15, varclsradacct.Nasportid));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasportid", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Nasporttype != null) cmd.Parameters.Add(getParameter(cmd, "@nasporttype", DbType.String, 32, varclsradacct.Nasporttype));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasporttype", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Acctstarttime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstarttime", DbType.DateTime, 8, varclsradacct.Acctstarttime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstarttime", DbType.DateTime, 8, DBNull.Value));
                    if (varclsradacct.Acctstoptime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstoptime", DbType.DateTime, 8, varclsradacct.Acctstoptime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstoptime", DbType.DateTime, 8, DBNull.Value));
                    if (varclsradacct.Acctsessiontime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctsessiontime", DbType.Int32, 4, varclsradacct.Acctsessiontime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctsessiontime", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Acctauthentic != null) cmd.Parameters.Add(getParameter(cmd, "@acctauthentic", DbType.String, 32, varclsradacct.Acctauthentic));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctauthentic", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Connectinfo_start != null) cmd.Parameters.Add(getParameter(cmd, "@connectinfo_start", DbType.String, 50, varclsradacct.Connectinfo_start));
                    else cmd.Parameters.Add(getParameter(cmd, "@connectinfo_start", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Connectinfo_stop != null) cmd.Parameters.Add(getParameter(cmd, "@connectinfo_stop", DbType.String, 50, varclsradacct.Connectinfo_stop));
                    else cmd.Parameters.Add(getParameter(cmd, "@connectinfo_stop", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Acctinputoctets.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctinputoctets", DbType.Int64, 8, varclsradacct.Acctinputoctets));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctinputoctets", DbType.Int64, 8, DBNull.Value));
                    if (varclsradacct.Acctoutputoctets.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctoutputoctets", DbType.Int64, 8, varclsradacct.Acctoutputoctets));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctoutputoctets", DbType.Int64, 8, DBNull.Value));
                    if (varclsradacct.Calledstationid != null) cmd.Parameters.Add(getParameter(cmd, "@calledstationid", DbType.String, 50, varclsradacct.Calledstationid));
                    else cmd.Parameters.Add(getParameter(cmd, "@calledstationid", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Callingstationid != null) cmd.Parameters.Add(getParameter(cmd, "@callingstationid", DbType.String, 50, varclsradacct.Callingstationid));
                    else cmd.Parameters.Add(getParameter(cmd, "@callingstationid", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Acctterminatecause != null) cmd.Parameters.Add(getParameter(cmd, "@acctterminatecause", DbType.String, 32, varclsradacct.Acctterminatecause));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctterminatecause", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Servicetype != null) cmd.Parameters.Add(getParameter(cmd, "@servicetype", DbType.String, 32, varclsradacct.Servicetype));
                    else cmd.Parameters.Add(getParameter(cmd, "@servicetype", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Framedprotocol != null) cmd.Parameters.Add(getParameter(cmd, "@framedprotocol", DbType.String, 32, varclsradacct.Framedprotocol));
                    else cmd.Parameters.Add(getParameter(cmd, "@framedprotocol", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Framedipaddress != null) cmd.Parameters.Add(getParameter(cmd, "@framedipaddress", DbType.String, 15, varclsradacct.Framedipaddress));
                    else cmd.Parameters.Add(getParameter(cmd, "@framedipaddress", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Acctstartdelay.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstartdelay", DbType.Int32, 4, varclsradacct.Acctstartdelay));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstartdelay", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Acctstopdelay.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstopdelay", DbType.Int32, 4, varclsradacct.Acctstopdelay));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstopdelay", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Xascendsessionsvrkey != null) cmd.Parameters.Add(getParameter(cmd, "@xascendsessionsvrkey", DbType.String, 10, varclsradacct.Xascendsessionsvrkey));
                    else cmd.Parameters.Add(getParameter(cmd, "@xascendsessionsvrkey", DbType.String, 10, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradacct(clsradacct varclsradacct)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radacct  SET acctsessionid=@acctsessionid,acctuniqueid=@acctuniqueid,username=@username,groupname=@groupname,realm=@realm,nasipaddress=@nasipaddress,nasportid=@nasportid,nasporttype=@nasporttype,acctstarttime=@acctstarttime,acctstoptime=@acctstoptime,acctsessiontime=@acctsessiontime,acctauthentic=@acctauthentic,connectinfo_start=@connectinfo_start,connectinfo_stop=@connectinfo_stop,acctinputoctets=@acctinputoctets,acctoutputoctets=@acctoutputoctets,calledstationid=@calledstationid,callingstationid=@callingstationid,acctterminatecause=@acctterminatecause,servicetype=@servicetype,framedprotocol=@framedprotocol,framedipaddress=@framedipaddress,acctstartdelay=@acctstartdelay,acctstopdelay=@acctstopdelay,xascendsessionsvrkey=@xascendsessionsvrkey  WHERE 1=1  AND radacctid=@radacctid ");
                    if (varclsradacct.Acctsessionid != null) cmd.Parameters.Add(getParameter(cmd, "@acctsessionid", DbType.String, 64, varclsradacct.Acctsessionid));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctsessionid", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Acctuniqueid != null) cmd.Parameters.Add(getParameter(cmd, "@acctuniqueid", DbType.String, 32, varclsradacct.Acctuniqueid));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctuniqueid", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradacct.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradacct.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Realm != null) cmd.Parameters.Add(getParameter(cmd, "@realm", DbType.String, 64, varclsradacct.Realm));
                    else cmd.Parameters.Add(getParameter(cmd, "@realm", DbType.String, 64, DBNull.Value));
                    if (varclsradacct.Nasipaddress != null) cmd.Parameters.Add(getParameter(cmd, "@nasipaddress", DbType.String, 15, varclsradacct.Nasipaddress));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasipaddress", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Nasportid != null) cmd.Parameters.Add(getParameter(cmd, "@nasportid", DbType.String, 15, varclsradacct.Nasportid));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasportid", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Nasporttype != null) cmd.Parameters.Add(getParameter(cmd, "@nasporttype", DbType.String, 32, varclsradacct.Nasporttype));
                    else cmd.Parameters.Add(getParameter(cmd, "@nasporttype", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Acctstarttime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstarttime", DbType.DateTime, 8, varclsradacct.Acctstarttime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstarttime", DbType.DateTime, 8, DBNull.Value));
                    if (varclsradacct.Acctstoptime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstoptime", DbType.DateTime, 8, varclsradacct.Acctstoptime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstoptime", DbType.DateTime, 8, DBNull.Value));
                    if (varclsradacct.Acctsessiontime.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctsessiontime", DbType.Int32, 4, varclsradacct.Acctsessiontime));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctsessiontime", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Acctauthentic != null) cmd.Parameters.Add(getParameter(cmd, "@acctauthentic", DbType.String, 32, varclsradacct.Acctauthentic));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctauthentic", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Connectinfo_start != null) cmd.Parameters.Add(getParameter(cmd, "@connectinfo_start", DbType.String, 50, varclsradacct.Connectinfo_start));
                    else cmd.Parameters.Add(getParameter(cmd, "@connectinfo_start", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Connectinfo_stop != null) cmd.Parameters.Add(getParameter(cmd, "@connectinfo_stop", DbType.String, 50, varclsradacct.Connectinfo_stop));
                    else cmd.Parameters.Add(getParameter(cmd, "@connectinfo_stop", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Acctinputoctets.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctinputoctets", DbType.Int64, 8, varclsradacct.Acctinputoctets));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctinputoctets", DbType.Int64, 8, DBNull.Value));
                    if (varclsradacct.Acctoutputoctets.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctoutputoctets", DbType.Int64, 8, varclsradacct.Acctoutputoctets));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctoutputoctets", DbType.Int64, 8, DBNull.Value));
                    if (varclsradacct.Calledstationid != null) cmd.Parameters.Add(getParameter(cmd, "@calledstationid", DbType.String, 50, varclsradacct.Calledstationid));
                    else cmd.Parameters.Add(getParameter(cmd, "@calledstationid", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Callingstationid != null) cmd.Parameters.Add(getParameter(cmd, "@callingstationid", DbType.String, 50, varclsradacct.Callingstationid));
                    else cmd.Parameters.Add(getParameter(cmd, "@callingstationid", DbType.String, 50, DBNull.Value));
                    if (varclsradacct.Acctterminatecause != null) cmd.Parameters.Add(getParameter(cmd, "@acctterminatecause", DbType.String, 32, varclsradacct.Acctterminatecause));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctterminatecause", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Servicetype != null) cmd.Parameters.Add(getParameter(cmd, "@servicetype", DbType.String, 32, varclsradacct.Servicetype));
                    else cmd.Parameters.Add(getParameter(cmd, "@servicetype", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Framedprotocol != null) cmd.Parameters.Add(getParameter(cmd, "@framedprotocol", DbType.String, 32, varclsradacct.Framedprotocol));
                    else cmd.Parameters.Add(getParameter(cmd, "@framedprotocol", DbType.String, 32, DBNull.Value));
                    if (varclsradacct.Framedipaddress != null) cmd.Parameters.Add(getParameter(cmd, "@framedipaddress", DbType.String, 15, varclsradacct.Framedipaddress));
                    else cmd.Parameters.Add(getParameter(cmd, "@framedipaddress", DbType.String, 15, DBNull.Value));
                    if (varclsradacct.Acctstartdelay.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstartdelay", DbType.Int32, 4, varclsradacct.Acctstartdelay));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstartdelay", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Acctstopdelay.HasValue) cmd.Parameters.Add(getParameter(cmd, "@acctstopdelay", DbType.Int32, 4, varclsradacct.Acctstopdelay));
                    else cmd.Parameters.Add(getParameter(cmd, "@acctstopdelay", DbType.Int32, 4, DBNull.Value));
                    if (varclsradacct.Xascendsessionsvrkey != null) cmd.Parameters.Add(getParameter(cmd, "@xascendsessionsvrkey", DbType.String, 10, varclsradacct.Xascendsessionsvrkey));
                    else cmd.Parameters.Add(getParameter(cmd, "@xascendsessionsvrkey", DbType.String, 10, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@radacctid", DbType.Int64, 8, varclsradacct.Radacctid));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradacct(clsradacct varclsradacct)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radacct  WHERE  1=1  AND radacctid=@radacctid ");
                    cmd.Parameters.Add(getParameter(cmd, "@radacctid", DbType.Int64, 8, varclsradacct.Radacctid));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradacct()
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radacct");
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADACCT 
        #region  CLSRADCHECK
        public clsradcheck getClsradcheck(object intid)
        {
            clsradcheck varclsradcheck = new clsradcheck();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radcheck WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradcheck.Username = dr["username"].ToString();
                            varclsradcheck.Attribute = dr["attribute"].ToString();
                            varclsradcheck.Op = dr["op"].ToString();
                            varclsradcheck.Value = dr["value"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradcheck;
        }

        public List<clsradcheck> getAllClsradcheck(string criteria)
        {
            List<clsradcheck> lstclsradcheck = new List<clsradcheck>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radcheck  WHERE";
                    sql += "  username LIKE '%" + criteria + "%'";
                    sql += "  OR   attribute LIKE '%" + criteria + "%'";
                    sql += "  OR   op LIKE '%" + criteria + "%'";
                    sql += "  OR   value LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradcheck varclsradcheck = null;
                        while (dr.Read())
                        {
                            varclsradcheck = new clsradcheck();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradcheck.Username = dr["username"].ToString();
                            varclsradcheck.Attribute = dr["attribute"].ToString();
                            varclsradcheck.Op = dr["op"].ToString();
                            varclsradcheck.Value = dr["value"].ToString();
                            lstclsradcheck.Add(varclsradcheck);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradcheck;
        }

        public List<clsradcheck> getAllClsradcheck()
        {
            List<clsradcheck> lstclsradcheck = new List<clsradcheck>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT radcheck.*,(SELECT COUNT(id) FROM radcheck) AS nbr_enreg FROM radcheck ORDER BY username ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradcheck varclsradcheck = null;
                        while (dr.Read())
                        {
                            varclsradcheck = new clsradcheck();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradcheck.Username = dr["username"].ToString();
                            varclsradcheck.Attribute = dr["attribute"].ToString();
                            varclsradcheck.Op = dr["op"].ToString();
                            varclsradcheck.Value = dr["value"].ToString();
                            varclsradcheck.Nombre_enregistrement = long.Parse(dr["nbr_enreg"].ToString());
                            lstclsradcheck.Add(varclsradcheck);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradcheck;
        }

        public DataTable getAllClsradcheck_dt()
        {
            DataTable lstclsradcheck = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"SELECT radcheck.id,radcheck.username,radcheck.attribute,radcheck.op,radcheck.value,radusergroup.priority,(SELECT COUNT(radcheck.id) FROM radcheck) AS nbr_enreg,groupe.groupname FROM radcheck
                    LEFT OUTER JOIN radusergroup ON radcheck.username = radusergroup.username
                    LEFT OUTER JOIN groupe ON radusergroup.groupname = groupe.groupname ORDER BY radcheck.username ASC");

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradcheck.Load(dr);
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradcheck;
        }

        public DataTable getAllClsradcheck_dt(string criteria)
        {
            DataTable lstclsradcheck = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(@"SELECT radcheck.id,radcheck.username,radcheck.attribute,radcheck.op,radcheck.value,radusergroup.priority,(SELECT COUNT(radcheck.id) FROM radcheck) AS nbr_enreg,groupe.groupname FROM radcheck
                    LEFT OUTER JOIN radusergroup ON radcheck.username = radusergroup.username
                    LEFT OUTER JOIN groupe ON radusergroup.groupname = groupe.groupname WHERE radcheck.username LIKE '%" + criteria + "%' OR radcheck.attribute LIKE '%" + criteria + "%' OR radcheck.op LIKE '%" + criteria + "%'" +
                    " OR radcheck.value LIKE '%" + criteria + "%' OR radusergroup.priority LIKE '%" + criteria + "%' OR groupe.groupname LIKE '%" + criteria + "%' ORDER BY radcheck.username ASC");

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradcheck.Load(dr);
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradcheck;
        }

        public int insertClsradcheck(clsradcheck varclsradcheck)
        {
            int i = 0;
            IDbTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                //Insert in radcheck
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radcheck ( username,attribute,op,value ) VALUES (@username,@attribute,@op,@value  )");
                    if (varclsradcheck.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradcheck.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradcheck.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradcheck.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradcheck.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradcheck.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradcheck.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradcheck.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    cmd.Transaction = transaction;
                    i = cmd.ExecuteNonQuery();
                }

                using (IDbCommand cmd1 = conn.CreateCommand())
                {
                    cmd1.CommandText = string.Format("INSERT INTO radusergroup (username,groupname,priority ) VALUES (@username,@groupname,@priority  )");
                    if (varclsradcheck.Username != null) cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, varclsradcheck.Username));
                    else cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradcheck.Groupname != null) cmd1.Parameters.Add(getParameter(cmd1, "@groupname", DbType.String, 64, varclsradcheck.Groupname));
                    else cmd1.Parameters.Add(getParameter(cmd1, "@groupname", DbType.String, 64, DBNull.Value));
                    cmd1.Parameters.Add(getParameter(cmd1, "@priority", DbType.Int32, 4, varclsradcheck.Priority));
                    cmd1.Transaction = transaction;
                    i = cmd1.ExecuteNonQuery();

                    transaction.Commit();
                    conn.Close();
                    transaction.Dispose();
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                    transaction.Dispose();
                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                    throw new Exception(exc.Message);
                }
            }
            return i;
        }

        public int updateClsradcheck(clsradcheck varclsradcheck)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radcheck  SET username=@username,attribute=@attribute,op=@op,value=@value  WHERE 1=1  AND id=@id ");
                    if (varclsradcheck.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradcheck.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradcheck.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradcheck.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradcheck.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradcheck.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradcheck.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradcheck.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradcheck.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradcheck(clsradcheck varclsradcheck)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radcheck  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradcheck.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #region USING DATAROWVIEW
        public int insertClsradcheck_dtrowv(DataRowView varclsradcheck)
        {
            int i = 0;
            IDbTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                //Insert in radcheck
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radcheck ( username,attribute,op,value ) VALUES (@username,@attribute,@op,@value  )");
                    cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradcheck["username"]));
                    cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradcheck["attribute"]));
                    cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradcheck["op"]));
                    cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradcheck["value"]));
                    cmd.Transaction = transaction;
                    i = cmd.ExecuteNonQuery();
                }

                //Insert in radusergroup
                using (IDbCommand cmd1 = conn.CreateCommand())
                {
                    cmd1.CommandText = string.Format("INSERT INTO radusergroup (username,groupname,priority ) VALUES (@username,@groupname,@priority  )");
                    cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, varclsradcheck["username"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@groupname", DbType.String, 64, varclsradcheck["groupname"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@priority", DbType.Int32, 4, varclsradcheck["priority"]));
                    cmd1.Transaction = transaction;
                    i = cmd1.ExecuteNonQuery();

                    transaction.Commit();
                    conn.Close();
                    transaction.Dispose();
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                    transaction.Dispose();
                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                    throw new Exception(exc.Message);
                }
            }
            return i;
        }

        public int updateClsradcheck_dtrowv(DataRowView varclsradcheck)
        {
            int i = 0;
            IDbTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);
                string oldUsername = null;

                //On commence par récupéré l'ancien useranme en cas de modification de ce dernier
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT username FROM radcheck WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradcheck["id"]));
                    cmd.Transaction = transaction;

                    using (IDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                            oldUsername = rd["username"].ToString();
                    }
                }

                //Update in radcheck
                using (IDbCommand cmd1 = conn.CreateCommand())
                {
                    cmd1.CommandText = string.Format("UPDATE radcheck  SET username=@username,attribute=@attribute,op=@op,value=@value  WHERE id=@id ");
                    cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, varclsradcheck["username"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@attribute", DbType.String, 64, varclsradcheck["attribute"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@op", DbType.String, 2, varclsradcheck["op"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@value", DbType.String, 253, varclsradcheck["value"]));
                    cmd1.Parameters.Add(getParameter(cmd1, "@id", DbType.Int32, 4, varclsradcheck["id"]));
                    cmd1.Transaction = transaction;
                    i = cmd1.ExecuteNonQuery();
                }

                //Update in radusergroup
                using (IDbCommand cmd2 = conn.CreateCommand())
                {
                    cmd2.CommandText = string.Format("UPDATE radusergroup  SET username=@newusername,groupname=@groupname,priority=@priority  WHERE username=@oldusername  ");
                    cmd2.Parameters.Add(getParameter(cmd2, "@groupname", DbType.String, 64, varclsradcheck["groupname"]));
                    cmd2.Parameters.Add(getParameter(cmd2, "@priority", DbType.Int32, 4, varclsradcheck["priority"]));
                    cmd2.Parameters.Add(getParameter(cmd2, "@newusername", DbType.String, 64, varclsradcheck["username"]));
                    cmd2.Parameters.Add(getParameter(cmd2, "@oldusername", DbType.String, 64, oldUsername));
                    cmd2.Transaction = transaction;
                    i = cmd2.ExecuteNonQuery();

                    transaction.Commit();
                    conn.Close();
                    transaction.Dispose();
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                    transaction.Dispose();
                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusAdmin.txt");
                    throw new Exception(exc.Message);
                }
            }
            return i;
        }

        public int deleteClsradcheck_dtrowv(DataRowView varclsradcheck)
        {
            int i = 0;
            IDbTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                //Delete in radcheck
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radcheck  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradcheck["id"]));
                    cmd.Transaction = transaction;
                    i = cmd.ExecuteNonQuery();
                }

                //Delete in radusergroup
                using (IDbCommand cmd1 = conn.CreateCommand())
                {
                    cmd1.CommandText = string.Format("DELETE FROM radusergroup WHERE username=@username  ");
                    cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, varclsradcheck["username"]));
                    cmd1.Transaction = transaction;
                    i = cmd1.ExecuteNonQuery();

                    transaction.Commit();
                    conn.Close();
                    transaction.Dispose();
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    conn.Close();
                    transaction.Dispose();
                    string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                    ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                    throw new Exception(exc.Message);
                }
            }
            return i;
        }
        #endregion

        #endregion CLSRADCHECK 
        #region  CLSRADGROUPCHECK
        public clsradgroupcheck getClsradgroupcheck(object intid)
        {
            clsradgroupcheck varclsradgroupcheck = new clsradgroupcheck();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radgroupcheck WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupcheck.Groupname = dr["groupname"].ToString();
                            varclsradgroupcheck.Attribute = dr["attribute"].ToString();
                            varclsradgroupcheck.Op = dr["op"].ToString();
                            varclsradgroupcheck.Value = dr["value"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradgroupcheck;
        }

        public List<clsradgroupcheck> getAllClsradgroupcheck(string criteria)
        {
            List<clsradgroupcheck> lstclsradgroupcheck = new List<clsradgroupcheck>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radgroupcheck  WHERE";
                    sql += "  groupname LIKE '%" + criteria + "%'";
                    sql += "  OR   attribute LIKE '%" + criteria + "%'";
                    sql += "  OR   op LIKE '%" + criteria + "%'";
                    sql += "  OR   value LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradgroupcheck varclsradgroupcheck = null;
                        while (dr.Read())
                        {
                            varclsradgroupcheck = new clsradgroupcheck();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupcheck.Groupname = dr["groupname"].ToString();
                            varclsradgroupcheck.Attribute = dr["attribute"].ToString();
                            varclsradgroupcheck.Op = dr["op"].ToString();
                            varclsradgroupcheck.Value = dr["value"].ToString();
                            lstclsradgroupcheck.Add(varclsradgroupcheck);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradgroupcheck;
        }

        public List<clsradgroupcheck> getAllClsradgroupcheck()
        {
            List<clsradgroupcheck> lstclsradgroupcheck = new List<clsradgroupcheck>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT radgroupcheck.*,(SELECT COUNT(id) FROM radgroupcheck) AS nbr_enreg FROM radgroupcheck ORDER BY groupname ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradgroupcheck varclsradgroupcheck = null;
                        while (dr.Read())
                        {
                            varclsradgroupcheck = new clsradgroupcheck();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupcheck.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupcheck.Groupname = dr["groupname"].ToString();
                            varclsradgroupcheck.Attribute = dr["attribute"].ToString();
                            varclsradgroupcheck.Op = dr["op"].ToString();
                            varclsradgroupcheck.Value = dr["value"].ToString();
                            varclsradgroupcheck.Nombre_enregistrement = int.Parse(dr["nbr_enreg"].ToString());
                            lstclsradgroupcheck.Add(varclsradgroupcheck);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradgroupcheck;
        }

        public DataTable getAllClsradgroupcheck_dt()
        {
            DataTable lstclsradgroupcheck = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT DISTINCT radgroupcheck.groupname,(SELECT COUNT(radgroupcheck.id) FROM radgroupcheck) AS nbr_enreg FROM radgroupcheck ORDER BY groupname ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradgroupcheck.Load(dr);
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradgroupcheck;
        }

        public int insertClsradgroupcheck(clsradgroupcheck varclsradgroupcheck)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radgroupcheck ( groupname,attribute,op,value ) VALUES (@groupname,@attribute,@op,@value  )");
                    if (varclsradgroupcheck.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradgroupcheck.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupcheck.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradgroupcheck.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupcheck.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradgroupcheck.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradgroupcheck.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradgroupcheck.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradgroupcheck(clsradgroupcheck varclsradgroupcheck)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radgroupcheck  SET groupname=@groupname,attribute=@attribute,op=@op,value=@value  WHERE 1=1  AND id=@id ");
                    if (varclsradgroupcheck.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradgroupcheck.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupcheck.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradgroupcheck.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupcheck.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradgroupcheck.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradgroupcheck.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradgroupcheck.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradgroupcheck.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradgroupcheck(clsradgroupcheck varclsradgroupcheck)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radgroupcheck  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradgroupcheck.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADGROUPCHECK 
        #region  CLSRADGROUPREPLY
        public clsradgroupreply getClsradgroupreply(object intid)
        {
            clsradgroupreply varclsradgroupreply = new clsradgroupreply();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radgroupreply WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupreply.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupreply.Groupname = dr["groupname"].ToString();
                            varclsradgroupreply.Attribute = dr["attribute"].ToString();
                            varclsradgroupreply.Op = dr["op"].ToString();
                            varclsradgroupreply.Value = dr["value"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradgroupreply;
        }

        public List<clsradgroupreply> getAllClsradgroupreply(string criteria)
        {
            List<clsradgroupreply> lstclsradgroupreply = new List<clsradgroupreply>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radgroupreply  WHERE";
                    sql += "  groupname LIKE '%" + criteria + "%'";
                    sql += "  OR   attribute LIKE '%" + criteria + "%'";
                    sql += "  OR   op LIKE '%" + criteria + "%'";
                    sql += "  OR   value LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradgroupreply varclsradgroupreply = null;
                        while (dr.Read())
                        {
                            varclsradgroupreply = new clsradgroupreply();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupreply.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupreply.Groupname = dr["groupname"].ToString();
                            varclsradgroupreply.Attribute = dr["attribute"].ToString();
                            varclsradgroupreply.Op = dr["op"].ToString();
                            varclsradgroupreply.Value = dr["value"].ToString();
                            lstclsradgroupreply.Add(varclsradgroupreply);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radgroupreply' avec la classe 'clsradgroupreply' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradgroupreply;
        }

        public List<clsradgroupreply> getAllClsradgroupreply()
        {
            List<clsradgroupreply> lstclsradgroupreply = new List<clsradgroupreply>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radgroupreply ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradgroupreply varclsradgroupreply = null;
                        while (dr.Read())
                        {
                            varclsradgroupreply = new clsradgroupreply();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradgroupreply.Id = int.Parse(dr["id"].ToString());
                            varclsradgroupreply.Groupname = dr["groupname"].ToString();
                            varclsradgroupreply.Attribute = dr["attribute"].ToString();
                            varclsradgroupreply.Op = dr["op"].ToString();
                            varclsradgroupreply.Value = dr["value"].ToString();
                            lstclsradgroupreply.Add(varclsradgroupreply);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradgroupreply;
        }

        public int insertClsradgroupreply(clsradgroupreply varclsradgroupreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radgroupreply ( groupname,attribute,op,value ) VALUES (@groupname,@attribute,@op,@value  )");
                    if (varclsradgroupreply.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradgroupreply.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupreply.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradgroupreply.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupreply.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradgroupreply.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradgroupreply.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradgroupreply.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradgroupreply(clsradgroupreply varclsradgroupreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radgroupreply  SET groupname=@groupname,attribute=@attribute,op=@op,value=@value  WHERE 1=1  AND id=@id ");
                    if (varclsradgroupreply.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradgroupreply.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupreply.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradgroupreply.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradgroupreply.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradgroupreply.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradgroupreply.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradgroupreply.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradgroupreply.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradgroupreply(clsradgroupreply varclsradgroupreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radgroupreply  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradgroupreply.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADGROUPREPLY 
        #region  CLSRADPOSTAUTH
        public clsradpostauth getClsradpostauth(object intid)
        {
            clsradpostauth varclsradpostauth = new clsradpostauth();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radpostauth WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradpostauth.Id = int.Parse(dr["id"].ToString());
                            varclsradpostauth.Username = dr["username"].ToString();
                            varclsradpostauth.Pass = dr["pass"].ToString();
                            varclsradpostauth.Reply = dr["reply"].ToString();
                            if (!dr["authdate"].ToString().Trim().Equals("")) varclsradpostauth.Authdate = DateTime.Parse(dr["authdate"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradpostauth;
        }

        public List<clsradpostauth> getAllClsradpostauth(string criteria)
        {
            List<clsradpostauth> lstclsradpostauth = new List<clsradpostauth>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radpostauth  WHERE";
                    sql += "  username LIKE '%" + criteria + "%'";
                    sql += "  OR   pass LIKE '%" + criteria + "%'";
                    sql += "  OR   reply LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradpostauth varclsradpostauth = null;
                        while (dr.Read())
                        {
                            varclsradpostauth = new clsradpostauth();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradpostauth.Id = int.Parse(dr["id"].ToString());
                            varclsradpostauth.Username = dr["username"].ToString();
                            varclsradpostauth.Pass = dr["pass"].ToString();
                            varclsradpostauth.Reply = dr["reply"].ToString();
                            if (!dr["authdate"].ToString().Trim().Equals("")) varclsradpostauth.Authdate = DateTime.Parse(dr["authdate"].ToString());
                            lstclsradpostauth.Add(varclsradpostauth);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radpostauth' avec la classe 'clsradpostauth' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradpostauth;
        }

        public List<clsradpostauth> getAllClsradpostauth()
        {
            List<clsradpostauth> lstclsradpostauth = new List<clsradpostauth>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT radpostauth.*,(SELECT COUNT(id) FROM radpostauth) AS nbr_enreg FROM radpostauth ORDER BY id ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradpostauth varclsradpostauth = null;
                        while (dr.Read())
                        {
                            varclsradpostauth = new clsradpostauth();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradpostauth.Id = int.Parse(dr["id"].ToString());
                            varclsradpostauth.Username = dr["username"].ToString();
                            varclsradpostauth.Pass = dr["pass"].ToString();
                            varclsradpostauth.Reply = dr["reply"].ToString();
                            if (!dr["authdate"].ToString().Trim().Equals("")) varclsradpostauth.Authdate = DateTime.Parse(dr["authdate"].ToString());
                            varclsradpostauth.Nombre_enregistrement = long.Parse(dr["nbr_enreg"].ToString());
                            lstclsradpostauth.Add(varclsradpostauth);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradpostauth;
        }

        public int insertClsradpostauth(clsradpostauth varclsradpostauth)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radpostauth ( username,pass,reply,authdate ) VALUES (@username,@pass,@reply,@authdate  )");
                    if (varclsradpostauth.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradpostauth.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradpostauth.Pass != null) cmd.Parameters.Add(getParameter(cmd, "@pass", DbType.String, 64, varclsradpostauth.Pass));
                    else cmd.Parameters.Add(getParameter(cmd, "@pass", DbType.String, 64, DBNull.Value));
                    if (varclsradpostauth.Reply != null) cmd.Parameters.Add(getParameter(cmd, "@reply", DbType.String, 32, varclsradpostauth.Reply));
                    else cmd.Parameters.Add(getParameter(cmd, "@reply", DbType.String, 32, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@authdate", DbType.DateTime, 8, varclsradpostauth.Authdate));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradpostauth(clsradpostauth varclsradpostauth)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radpostauth  SET username=@username,pass=@pass,reply=@reply,authdate=@authdate  WHERE 1=1  AND id=@id ");
                    if (varclsradpostauth.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradpostauth.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradpostauth.Pass != null) cmd.Parameters.Add(getParameter(cmd, "@pass", DbType.String, 64, varclsradpostauth.Pass));
                    else cmd.Parameters.Add(getParameter(cmd, "@pass", DbType.String, 64, DBNull.Value));
                    if (varclsradpostauth.Reply != null) cmd.Parameters.Add(getParameter(cmd, "@reply", DbType.String, 32, varclsradpostauth.Reply));
                    else cmd.Parameters.Add(getParameter(cmd, "@reply", DbType.String, 32, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@authdate", DbType.DateTime, 8, varclsradpostauth.Authdate));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradpostauth.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradpostauth(clsradpostauth varclsradpostauth)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radpostauth  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradpostauth.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradpostauth()
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radpostauth");
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADPOSTAUTH 
        #region  CLSRADREPLY
        public clsradreply getClsradreply(object intid)
        {
            clsradreply varclsradreply = new clsradreply();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radreply WHERE id={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradreply.Id = int.Parse(dr["id"].ToString());
                            varclsradreply.Username = dr["username"].ToString();
                            varclsradreply.Attribute = dr["attribute"].ToString();
                            varclsradreply.Op = dr["op"].ToString();
                            varclsradreply.Value = dr["value"].ToString();
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradreply;
        }

        public List<clsradreply> getAllClsradreply(string criteria)
        {
            List<clsradreply> lstclsradreply = new List<clsradreply>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radreply  WHERE";
                    sql += "  username LIKE '%" + criteria + "%'";
                    sql += "  OR   attribute LIKE '%" + criteria + "%'";
                    sql += "  OR   op LIKE '%" + criteria + "%'";
                    sql += "  OR   value LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradreply varclsradreply = null;
                        while (dr.Read())
                        {
                            varclsradreply = new clsradreply();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradreply.Id = int.Parse(dr["id"].ToString());
                            varclsradreply.Username = dr["username"].ToString();
                            varclsradreply.Attribute = dr["attribute"].ToString();
                            varclsradreply.Op = dr["op"].ToString();
                            varclsradreply.Value = dr["value"].ToString();
                            lstclsradreply.Add(varclsradreply);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radreply' avec la classe 'clsradreply' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradreply;
        }

        public List<clsradreply> getAllClsradreply()
        {
            List<clsradreply> lstclsradreply = new List<clsradreply>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radreply ");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradreply varclsradreply = null;
                        while (dr.Read())
                        {
                            varclsradreply = new clsradreply();
                            if (!dr["id"].ToString().Trim().Equals("")) varclsradreply.Id = int.Parse(dr["id"].ToString());
                            varclsradreply.Username = dr["username"].ToString();
                            varclsradreply.Attribute = dr["attribute"].ToString();
                            varclsradreply.Op = dr["op"].ToString();
                            varclsradreply.Value = dr["value"].ToString();
                            lstclsradreply.Add(varclsradreply);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radreply' avec la classe 'clsradreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradreply;
        }

        public int insertClsradreply(clsradreply varclsradreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radreply ( username,attribute,op,value ) VALUES (@username,@attribute,@op,@value  )");
                    if (varclsradreply.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradreply.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradreply.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradreply.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradreply.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradreply.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradreply.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradreply.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradreply(clsradreply varclsradreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radreply  SET username=@username,attribute=@attribute,op=@op,value=@value  WHERE 1=1  AND id=@id ");
                    if (varclsradreply.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradreply.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradreply.Attribute != null) cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, varclsradreply.Attribute));
                    else cmd.Parameters.Add(getParameter(cmd, "@attribute", DbType.String, 64, DBNull.Value));
                    if (varclsradreply.Op != null) cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, varclsradreply.Op));
                    else cmd.Parameters.Add(getParameter(cmd, "@op", DbType.String, 2, DBNull.Value));
                    if (varclsradreply.Value != null) cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, varclsradreply.Value));
                    else cmd.Parameters.Add(getParameter(cmd, "@value", DbType.String, 253, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradreply.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradreply(clsradreply varclsradreply)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radreply  WHERE  1=1  AND id=@id ");
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, varclsradreply.Id));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADREPLY 
        #region  CLSRADUSERGROUP
        public clsradusergroup getClsradusergroup(object intid)
        {
            clsradusergroup varclsradusergroup = new clsradusergroup();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT *  FROM radusergroup WHERE ={0}", intid);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            varclsradusergroup.Username = dr["username"].ToString();
                            varclsradusergroup.Groupname = dr["groupname"].ToString();
                            if (!dr["priority"].ToString().Trim().Equals("")) varclsradusergroup.Priority = int.Parse(dr["priority"].ToString());
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return varclsradusergroup;
        }

        public List<clsradusergroup> getAllClsradusergroup(string criteria)
        {
            List<clsradusergroup> lstclsradusergroup = new List<clsradusergroup>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    string sql = "SELECT *  FROM radusergroup  WHERE";
                    sql += "  username LIKE '%" + criteria + "%'";
                    sql += "  OR   groupname LIKE '%" + criteria + "%'";
                    cmd.CommandText = string.Format(sql);
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        clsradusergroup varclsradusergroup = null;
                        while (dr.Read())
                        {
                            varclsradusergroup = new clsradusergroup();
                            varclsradusergroup.Username = dr["username"].ToString();
                            varclsradusergroup.Groupname = dr["groupname"].ToString();
                            if (!dr["priority"].ToString().Trim().Equals("")) varclsradusergroup.Priority = int.Parse(dr["priority"].ToString());
                            lstclsradusergroup.Add(varclsradusergroup);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radusergroup' avec la classe 'clsradusergroup' suivant un critère : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradusergroup;
        }

        public List<clsradusergroup> getAllClsradusergroup()
        {
            List<clsradusergroup> lstclsradusergroup = new List<clsradusergroup>();
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT radusergroup.*,(SELECT COUNT(username) FROM radusergroup) AS nbr_enreg FROM radusergroup ORDER BY username ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {

                        clsradusergroup varclsradusergroup = null;
                        while (dr.Read())
                        {
                            varclsradusergroup = new clsradusergroup();
                            varclsradusergroup.Username = dr["username"].ToString();
                            varclsradusergroup.Groupname = dr["groupname"].ToString();
                            if (!dr["priority"].ToString().Trim().Equals("")) varclsradusergroup.Priority = int.Parse(dr["priority"].ToString());
                            varclsradusergroup.Nombre_enregistrement = long.Parse(dr["nbr_enreg"].ToString());
                            lstclsradusergroup.Add(varclsradusergroup);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return lstclsradusergroup;
        }

        public int insertClsradusergroup(clsradusergroup varclsradusergroup)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("INSERT INTO radusergroup ( username,groupname,priority ) VALUES (@username,@groupname,@priority  )");
                    if (varclsradusergroup.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradusergroup.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradusergroup.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradusergroup.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@priority", DbType.Int32, 4, varclsradusergroup.Priority));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int updateClsradusergroup(clsradusergroup varclsradusergroup)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("UPDATE radusergroup  SET username=@username,groupname=@groupname,priority=@priority  WHERE username=@username  ");
                    if (varclsradusergroup.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradusergroup.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    if (varclsradusergroup.Groupname != null) cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, varclsradusergroup.Groupname));
                    else cmd.Parameters.Add(getParameter(cmd, "@groupname", DbType.String, 64, DBNull.Value));
                    cmd.Parameters.Add(getParameter(cmd, "@priority", DbType.Int32, 4, varclsradusergroup.Priority));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        public int deleteClsradusergroup(clsradusergroup varclsradusergroup)
        {
            int i = 0;
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("DELETE FROM radusergroup  WHERE  username=@username  ");
                    if (varclsradusergroup.Username != null) cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, varclsradusergroup.Username));
                    else cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 64, DBNull.Value));
                    i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exc)
            {
                conn.Close();
                string MasterDirectory = ImplementUtilities.Instance.MasterDirectoryConfiguration;
                ImplementLog.Instance.PutLogMessage(MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.Message, DirectoryUtilLog, MasterDirectory + "LogFileRadiusStudent.txt");
                throw new Exception(exc.Message);
            }
            return i;
        }

        #endregion CLSRADUSERGROUP 

        #region GESTION SMS
        /// <summary>
        /// Permet l'ouverture de la connection au port passé en premier paramètre, avec comme vitesse 
        /// du port le second paramètre et comme troisième paramètre le temps de reponse avant ré - connection
        /// </summary>
        /// <param name="port">Numéro de port</param>
        /// <param name="baud">Vitesse de transfère du port</param>
        /// <param name="timeout">Temps de réponse avant réconnection</param>
        public void OpenConnection(int port, int baud, int timeout)
        {
            comm = new GsmCommMain(port, baud, timeout);
            int inc = 0;
            try
            {
                comm.Open();

                while (!comm.IsConnected())
                {
                    if (inc > 0)
                    {
                        comm.Close();
                        return;
                    }
                    inc++;
                    throw new Exception("");
                }

                //comm.Close();
            }
            catch (Exception)
            {
                throw new Exception("Echec de l'ouverture du port choisi");
            }
        }

        /// <summary>
        /// Permet de retourner le statut de la connection au modem
        /// True =>Si elle est ouverte et False dans le cas contraire
        /// </summary>
        /// <returns></returns>
        public bool GetStatusConnectionModem()
        {
            if (comm.IsOpen()) return true;
            else return false;
        }

        public void CloseModemConnection()
        {
            if (comm != null || comm.IsOpen() || comm.IsConnected()) comm.Close();
            else { }
        }

        /// <summary>
        /// Permet l'envoie d'un SMS à un seul destinataire en passant en paramètre  
        /// le message à envoyé ainsi que le N°Tél du destinataire
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="destinataires">N° du destinataire</param>
        public void SendOneSMS(string message, string destinataire)
        {
            SmsSubmitPdu pdu;

            pdu = new SmsSubmitPdu(message, destinataire, "");
            if (comm.IsOpen()) { }
            else comm.Open();
            comm.SendMessage(pdu);
            //comm.Close();
        }

        /// <summary>
        /// Permet l'envoie d'un SMS à plusieurs destinataires en passant en paramètre le message à envoyer  
        /// et toutes les adresses des destinataires (N° de Tél) séparés par des point virgules
        /// </summary>
        /// <param name="message">Message à envoyer</param>
        /// <param name="destinataires">N° du destinataire</param>
        public void SendManySMS(string message, string destinataires)
        {
            SmsSubmitPdu pdu;
            string[] tbDestinataires;

            tbDestinataires = destinataires.Split(';');

            if (comm.IsOpen()) { }
            else comm.Open();

            for (int i = 0; i < tbDestinataires.Length; i++)
            {
                pdu = new SmsSubmitPdu(message, tbDestinataires[i], "");
                comm.SendMessage(pdu);
            }
        }

        /// <summary>
        /// Permet de retourner le numéro de port sous forme quasi entière en ométant la désignation COM
        /// et retourne un entier tout en prennant en paramètre le string contenant les deux valeur concaténées
        /// Ex: COM2 -> 2
        /// </summary>
        /// <param name="valeur"></param>
        /// <returns></returns>
        public int RecupNumeroPort(string valeur)
        {
            try
            {
                int numPort = 0;
                numPort = Convert.ToInt32(valeur.Substring(3, valeur.Length - 3));
                return numPort;
            }
            catch (Exception)
            {
                throw new Exception("Erreur lors de la récupération du numéro de port");
            }
        }

        /// <summary>
        /// Chargement du Baut Rate ou vitesse de transfère du port du modem qui retourne un tableau des entiers
        /// </summary>
        /// <returns>Tableau des entiers</returns>
        public int[] LoadBaudPorts()
        {
            int[] baud = new int[] { 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 460800, 921600 };
            return baud;
        }

        /// <summary>
        /// Chargement des valeurs pour le bit des données du modem et qui retourne un tableau des entiers
        /// </summary>
        /// <returns>Tableau des entiers</returns>
        public int[] LoadDataBit()
        {
            int[] dataBit = new int[] { 4, 5, 6, 7, 8 };
            return dataBit;
        }

        /// <summary>
        /// Chargement des valeurs pour le bit de parité du modem et qui retourne un tableau des strings
        /// </summary>
        /// <returns>Tableau des strings</returns>
        public string[] LoadParityBit()
        {
            string[] parityBit = new string[] { "Aucun", "Pair", "Impair", "Marque", "Espace" };
            return parityBit;
        }

        /// <summary>
        /// Chargement des valeurs pour le delais de connection du Modem et retourne un tableau des entiers
        /// </summary>
        /// <returns>Tableau des entiers</returns>
        public int[] LoadTimeOut()
        {
            int[] timeOut = new int[] { 150, 300, 600, 900, 1200, 1500, 1800, 2000 };
            return timeOut;
        }

        /// <summary>
        /// Permet d'avoir une liste contenant tous les ports series disponibles
        /// </summary>
        /// <returns>Liste des ports</returns>
        public List<string> GetAllports()
        {
            try
            {
                List<string> liste = new List<string>();

                foreach (string portSerie in SerialPort.GetPortNames())
                    liste.Add(portSerie);
                return liste;
            }
            catch (Exception)
            {
                throw new Exception("Impossible de charger tous les ports series");
            }
        }
        #endregion
    } //***fin class 
} //***fin namespace 
