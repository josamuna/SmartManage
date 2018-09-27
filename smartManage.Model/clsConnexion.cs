using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace smartManage.Model
{
    public class clsConnexion : INotifyPropertyChanged, IDataErrorInfo
    {
        private string server = @"localhost", db, user = "sa", pwd = "sa";
        private List<clsConnexion> lstbd = new List<clsConnexion>();
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, string> _errorInfos;
        public void changeConnect()
        {
            clsMetier.GetInstance().setDB(db);
        }
        protected void RaisePropertyChanged(string propertyName, object objet)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            ValidateProperty(propertyName, objet);
        }
        public List<string> Lstbd
        {
            get
            {
                clsMetier.GetInstance().Initialize(new clsConnexion(server, "Master", user, pwd), 1);
                return clsMetier.GetInstance().getAllDB();
                //return lstbd;
            }
        }
        public string Pwd
        {
            get { return pwd; }
            set
            {
                if (pwd != value)
                {
                    pwd = value;
                    RaisePropertyChanged("Pwd", value);
                }
            }
        }
        public string User
        {
            get { return user; }
            set
            {
                if (user != value)
                {
                    user = value;
                    RaisePropertyChanged("User", value);
                }
            }
        }
        public string DB
        {
            get { return db; }
            set
            {
                if (db != value)
                {
                    db = value;
                    RaisePropertyChanged("DB", value);
                }
            }
        }
        public string Serveur
        {
            get { return server; }
            set
            {
                if (server != value)
                {
                    server = value;
                    RaisePropertyChanged("Serveur", value);
                }
            }
        }
        public clsConnexion()
        {
            _errorInfos = new Dictionary<string, string>();
        }
        public bool isConnexion()
        {
            clsMetier.GetInstance().Initialize(this, 1);
            return clsMetier.GetInstance().isConnect();
        }
        public override string ToString()
        {
            return string.Format("{0}", this.server);
        }
        public clsConnexion(string server, string db, string user, string pwd)
        {
            _errorInfos = new Dictionary<string, string>();
            this.server = server;
            this.db = db; this.user = user; this.pwd = pwd; this.db = db;
        }
        #region IDataErrorInfo
        [Browsable(false)]
        public string Error
        {
            get
            {
                if (_errorInfos.Count > 0)
                {
                    return " Champs vides ";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string this[string columnName]
        {
            get
            {
                if (_errorInfos.ContainsKey(columnName))
                    return _errorInfos[columnName];
                else
                    return string.Empty;
            }
        }
        #endregion
        private void ValidateProperty(string propertyName, object value)
        {
            var name = value as string;
            if ((name.Equals("")))
            {
                SetError(propertyName, "Ce champs ne peut pas être vide.");
            }
            else SetError(propertyName, string.Empty);
        }
        private void SetError(string propertyName, string error)
        {
            _errorInfos.Remove(propertyName);
            if (!(error.Equals("")))
            {
                _errorInfos.Add(propertyName, error);
            }
        }
    }
} //***fin namespace
