using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using ManageUtilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;

namespace smartManage.RadiusAdminModel
{
    public class clsMetier1: IDisposable
    {
        //***Les variables globales***
        private static string _ConnectionString, _host, _db, _user, _pwd;
        private static clsMetier1 fact;
        private MySqlConnection conn;
        private static GsmCommMain comm;
        #region prerecquis
        public static clsMetier1 GetInstance()
        {
            if (fact == null)
                fact = new clsMetier1();
            return fact;
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
        public void Initialize(clsConnexion1 con)
        {
            _host = con.Serveur;// host;
            _db = con.DB; 
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3};", _host, _db, _user, _pwd);
            conn = new MySqlConnection(sch);
        }
        public void Initialize(clsConnexion1 con, int type)
        {
            _host = con.Serveur;// host;
            _db = con.DB; ;
            _user = con.User;
            _pwd = con.Pwd;
            string sch = string.Format("Server={0}; Database={1}; Uid={2}; Pwd={3}", _host, _db, _user, _pwd);

            //On garde la chaine de connexion pour utilisation avec les reports
            smartManage.RadiusAdminModel.Properties.Settings.Default.strChaineConnexion = sch;
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
                if (conn.State != ConnectionState.Open) conn.Open();
            }
            catch (Exception exc)
            {
                bl = false;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat de la connexion à la BD sans paramètre : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + "LogFileRadiusStudent.txt");
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return bl;
        }
        public bool isConnect(clsConnexion1 con)
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
            }
            catch (Exception exc)
            {
                sch = string.Format("server={0}; database={1};id user={2}; pwd={3}", _host, _db, _user, _pwd);
                bl = false;
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Etat connexion à la BD avec paramètre : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Récupération de toutes les bases de Données SQLServer : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Echec lors de la modification de l'utilisateur : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
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
                    cmd.CommandText = "SELECT *  FROM nas WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = @"SELECT *  FROM nas  WHERE nasname LIKE @criteria1 OR shortname LIKE @criteria2 OR
                    type LIKE @criteria3 OR secret LIKE @criteria4 OR server LIKE @criteria5 OR community LIKE @criteria6
                    OR description LIKE @criteria7";

                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria5", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria6", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria7", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'nas' avec la classe 'clsnas' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'nas' avec la classe 'clsnas' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'nas' avec la classe 'clsnas' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radacct WHERE radacctid=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = @"SELECT *  FROM radacct  WHERE acctsessionid LIKE @criteria1 OR acctuniqueid LIKE @criteria2 OR username LIKE @criteria3 OR groupname LIKE @criteria4 OR realm LIKE @criteria5 
                    OR nasipaddress LIKE @criteria6 OR nasportid LIKE @criteria7 OR nasporttype LIKE @criteria8 OR acctauthentic LIKE @criteria9 OR connectinfo_start LIKE @criteria10 OR connectinfo_stop LIKE @criteria11 
                    OR calledstationid LIKE @criteria12 OR callingstationid LIKE @criteria13 OR acctterminatecause LIKE @criteria14 OR servicetype LIKE @criteria15 OR framedprotocol LIKE @criteria16 OR 
                    framedipaddress LIKE @criteria17 OR xascendsessionsvrkey LIKE @criteria18";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria5", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria6", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria7", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria8", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria9", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria10", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria11", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria12", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria13", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria14", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria15", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria16", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria17", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria18", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radacct' avec la classe 'clsradacct' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return lstclsradacct;
        }

        public DataTable getAllClsradcheck_dt_file(string filenamePath)
        {
            DataTable lstclsradcheck = new DataTable();

            try
            {
                //Creating Columns
                lstclsradcheck.Columns.Add("id", typeof(int));
                lstclsradcheck.Columns.Add("username", typeof(string));
                lstclsradcheck.Columns.Add("attribute", typeof(string));
                lstclsradcheck.Columns.Add("op", typeof(string));
                lstclsradcheck.Columns.Add("value", typeof(string));
                lstclsradcheck.Columns.Add("priority", typeof(int));
                lstclsradcheck.Columns.Add("nbr_enreg", typeof(string));
                lstclsradcheck.Columns.Add("groupname", typeof(string));

                if (File.Exists(filenamePath))
                {
                    //On recupere toutes les lignes du fichier existant
                    string[] AllLines = File.ReadAllLines(filenamePath);

                    //On teste si le fichier est vide en regardant sa taille
                    if (AllLines.Length != 0)
                    {
                        string content = null;
                        //Chargement des elements du fichier dans le Datatable
                        using (StreamReader sr = new StreamReader(filenamePath))
                        {
                            int countId = 1;
                            int recordCount = AllLines.Length;

                            while (!sr.EndOfStream)
                            {
                                content = sr.ReadLine().ToString().Trim();

                                //On n'ajoute pas les lignes vides
                                if (!string.IsNullOrEmpty(content))
                                {
                                    DataRow row = lstclsradcheck.NewRow();
                                    row["id"] = countId;
                                    row["username"] = content;
                                    row["attribute"] = "Cleartext-Password";
                                    row["op"] = ":=";
                                    row["value"] = null;
                                    row["priority"] = 10;
                                    row["nbr_enreg"] = recordCount;
                                    row["groupname"] = "utilisateur";

                                    lstclsradcheck.Rows.Add(row);
                                    countId++;
                                }
                            }
                        }
                    }
                    else
                        throw new Exception("Le fichier est vide !!!");
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            return lstclsradcheck;
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radacct' avec la classe 'clsradacct' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radcheck WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radcheck  WHERE username LIKE @criteria1 OR attribute LIKE @criteria2 OR op LIKE @criteria3 OR value LIKE @criteria4";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = string.Format(@"SELECT radcheck.id,radcheck.username,radcheck.attribute,radcheck.op,radcheck.value,radusergroup.priority,(SELECT COUNT(radcheck.id) FROM radcheck) AS nbr_enreg,radgroupcheck.id AS id_gp,radusergroup.groupname FROM radcheck
                    INNER JOIN radusergroup ON radcheck.username = radusergroup.username
                    INNER JOIN radgroupcheck ON radusergroup.groupname = radgroupcheck.groupname ORDER BY radcheck.username ASC");

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradcheck.Load(dr);
                    }
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = @"SELECT DISTINCT radcheck.id,radcheck.username,radcheck.attribute,radcheck.op,radcheck.value,radusergroup.priority,(SELECT COUNT(radcheck.id) FROM radcheck) AS nbr_enreg,radgroupcheck.id AS id_gp,radusergroup.groupname FROM radcheck
                    INNER JOIN radusergroup ON radcheck.username = radusergroup.username
                    INNER JOIN radgroupcheck ON radusergroup.groupname = radgroupcheck.groupname WHERE radcheck.username LIKE @criteria1 OR radcheck.attribute LIKE @criteria2 OR radcheck.op LIKE @criteria3
                    OR radcheck.value LIKE @criteria4 OR radusergroup.priority LIKE @criteria5 OR radgroupcheck.groupname LIKE @criteria6 ORDER BY radcheck.username ASC";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria5", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria6", DbType.String, 50, string.Format("%{0}%", criteria)));

                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradcheck.Load(dr);
                    }
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    transaction.Dispose();
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    throw;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return i;
        }

        public string GeneratePassword(string username)
        {
            Random rd = new Random();
            username = username + rd.Next(1000, 9999).ToString();
            return username;
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
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    throw;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (transaction != null)
                    transaction.Dispose();
            }
            return i;
        }

        public int insertClsradcheck_multiple_dtrowv(DataRowView varclsradcheck)
        {
            int i = 0;
            IDbTransaction transaction = null;

            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                transaction = conn.BeginTransaction(IsolationLevel.Serializable);

                //Etant donne que la table radusergroup n'a pas d'index et depend de radcheck, on va drop son contenu 
                //avant de faire des insertions
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM radusergroup";
                    cmd.Transaction = transaction;
                    i = cmd.ExecuteNonQuery();
                }

                foreach (DataRow dtr in varclsradcheck.Row.Table.Rows)
                {
                    //Insert in radcheck
                    using (IDbCommand cmd1 = conn.CreateCommand())
                    {
                        cmd1.CommandText = string.Format(@"INSERT INTO radcheck ( id,username,attribute,op,value ) VALUES ( @id,@username,@attribute,@op,@value) 
                        ON DUPLICATE KEY UPDATE
                        attribute=@attribute, op=@op, value=@value");
                        cmd1.Parameters.Add(getParameter(cmd1, "@id", DbType.Int32, 4, dtr["id"]));
                        cmd1.Parameters.Add(getParameter(cmd1, "@username", DbType.String, 64, dtr["username"]));
                        cmd1.Parameters.Add(getParameter(cmd1, "@attribute", DbType.String, 64, dtr["attribute"]));
                        cmd1.Parameters.Add(getParameter(cmd1, "@op", DbType.String, 2, dtr["op"]));
                        cmd1.Parameters.Add(getParameter(cmd1, "@value", DbType.String, 253, dtr["value"]));
                        cmd1.Transaction = transaction;
                        i = cmd1.ExecuteNonQuery();
                    }

                    //Insert in radusergroup
                    using (IDbCommand cmd2 = conn.CreateCommand())
                    {
                        cmd2.CommandText = "INSERT INTO radusergroup ( username,groupname,priority ) VALUES ( @username,@groupname,@priority)";
                        cmd2.Parameters.Add(getParameter(cmd2, "@username", DbType.String, 64, dtr["username"]));
                        cmd2.Parameters.Add(getParameter(cmd2, "@groupname", DbType.String, 64, dtr["groupname"]));
                        cmd2.Parameters.Add(getParameter(cmd2, "@priority", DbType.Int32, 4, dtr["priority"]));
                        cmd2.Transaction = transaction;
                        i = cmd2.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    throw;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (transaction != null)
                    transaction.Dispose();
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
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    throw;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (transaction != null)
                    transaction.Dispose();
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
                }
            }
            catch (Exception exc)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radcheck' avec la classe 'clsradcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                    throw;
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (transaction != null)
                    transaction.Dispose();
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
                    cmd.CommandText = "SELECT *  FROM radgroupcheck WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = @"SELECT *  FROM radgroupcheck  WHERE groupname LIKE @criteria1 OR attribute LIKE @criteria2
                    OR op LIKE @criteria3 OR value LIKE @criteria4";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = string.Format("SELECT radgroupcheck.id AS id_gp,radgroupcheck.groupname,radgroupcheck.attribute,radgroupcheck.op,radgroupcheck.value,(SELECT COUNT(radgroupcheck.id) FROM radgroupcheck) AS nbr_enreg FROM radgroupcheck ORDER BY groupname ASC");
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        lstclsradgroupcheck.Load(dr);
                    }
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radgroupcheck' avec la classe 'clsradgroupcheck' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radgroupreply WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radgroupreply  WHERE groupname LIKE @criteria1 OR attribute LIKE @criteria2 OR op LIKE @criteria3 OR value LIKE @criteria4";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radgroupreply' avec la classe 'clsradgroupreply' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radgroupreply' avec la classe 'clsradgroupreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radpostauth WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radpostauth  WHERE username LIKE @criteria1 OR pass LIKE @criteria2 OR reply LIKE @criteria3";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", string.Format("%{0}%", criteria))));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radpostauth' avec la classe 'clsradpostauth' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radpostauth' avec la classe 'clsradpostauth' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radreply WHERE id=@id";
                    cmd.Parameters.Add(getParameter(cmd, "@id", DbType.Int32, 4, Convert.ToInt32(intid)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radreply  WHERE username LIKE @criteria1 OR attribute LIKE @criteria2 OR op LIKE @criteria3 OR value LIKE @criteria4";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria3", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria4", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radreply' avec la classe 'clsradreply' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radreply' avec la classe 'clsradreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radreply' avec la classe 'clsradreply' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radusergroup WHERE username=@username";
                    cmd.Parameters.Add(getParameter(cmd, "@username", DbType.String, 50, intid));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection d'un enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                    cmd.CommandText = "SELECT *  FROM radusergroup  WHERE username LIKE @criteria1 OR groupname LIKE @criteria2";
                    cmd.Parameters.Add(getParameter(cmd, "@criteria1", DbType.String, 50, string.Format("%{0}%", criteria)));
                    cmd.Parameters.Add(getParameter(cmd, "@criteria2", DbType.String, 50, string.Format("%{0}%", criteria)));

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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection des tous les enregistrements de la table : 'radusergroup' avec la classe 'clsradusergroup' suivant un critère : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Sélection de tous les enregistrements de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Insertion enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Update enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
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
                }
            }
            catch (Exception exc)
            {
                ImplementLog.Instance.PutLogMessage(Properties.Settings.Default.MasterDirectory, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + " : Suppression enregistrement de la table : 'radusergroup' avec la classe 'clsradusergroup' : " + exc.GetType().ToString() + " : " + exc.Message, Properties.Settings.Default.DirectoryUtilLog, Properties.Settings.Default.MasterDirectory + Properties.Settings.Default.LogFileName);
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
            return i;
        }

        #endregion CLSRADUSERGROUP 

        #region GESTION POUR FICHIER
        public static void GenerateFiles(System.Windows.Forms.BindingSource bdsrc, string chemin_access)
        {
            StreamWriter srw = new StreamWriter(chemin_access + @"\radcheck_insert.sql", false);
            StreamWriter srw1 = new StreamWriter(chemin_access + @"\radcheck_update.sql", false);
            StreamWriter srw2 = new StreamWriter(chemin_access + @"\radusergroup_insert.sql", false);
            StreamWriter srw3 = new StreamWriter(chemin_access + @"\radusergroup_update.sql", false);

            foreach (DataRow dtr in ((DataRowView)bdsrc.Current).Row.Table.Rows)
            {
                srw.WriteLine(string.Format("insert into radcheck(username,attribute,op,value) values('{0}','{1}','{2}','{3}');", dtr["username"], "Cleartext-Password", ":=", dtr["value"]));
                srw1.WriteLine(string.Format("update radcheck set attribute='{0}',op='{1}',value='{2}' where username='{3}';", "Cleartext-Password", ":=", dtr["value"], dtr["username"]));
                srw2.WriteLine(string.Format("insert into radusergroup(username,groupname,priority) values ('{0}','{1}',{2});", dtr["username"], dtr["groupname"], dtr["priority"]));
                srw3.WriteLine(string.Format("update radusergroup set groupname='{0}',priority={1} where username='{2}';", dtr["groupname"], dtr["priority"], dtr["username"]));
            }

            srw.Close();
            srw1.Close();
            srw2.Close();
            srw3.Close();
        }
        #endregion
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (conn != null)
                    conn.Close();
            }
        }
        #endregion
    } //***fin class 
} //***fin namespace 
