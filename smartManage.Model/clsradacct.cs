using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradacct
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private long radacctid;
        private string acctsessionid;
        private string acctuniqueid;
        private string username;
        private string groupname;
        private string realm;
        private string nasipaddress;
        private string nasportid;
        private string nasporttype;
        private DateTime? acctstarttime;
        private DateTime? acctstoptime;
        private int? acctsessiontime;
        private string acctauthentic;
        private string connectinfo_start;
        private string connectinfo_stop;
        private long? acctinputoctets;
        private long? acctoutputoctets;
        private string calledstationid;
        private string callingstationid;
        private string acctterminatecause;
        private string servicetype;
        private string framedprotocol;
        private string framedipaddress;
        private int? acctstartdelay;
        private int? acctstopdelay;
        private string xascendsessionsvrkey;
        private long nombre_enregistrement;
        //***Listes***
        public List<clsradacct> listes()
        {
            return clsMetier1.GetInstance().getAllClsradacct();
        }
        public List<clsradacct> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradacct(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradacct(this);
        }
        public int update(clsradacct varscls)
        {
            return clsMetier1.GetInstance().updateClsradacct(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradacct(this);
        }
        public int delete(clsradacct varscls)
        {
            return clsMetier1.GetInstance().deleteClsradacct(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradacct(this);
        }
        public int delete_all()
        {
            return clsMetier1.GetInstance().deleteClsradacct();
        }
        //***Le constructeur par defaut***
        public clsradacct()
        {
        }

        //***Accesseur de radacctid***
        public long Radacctid
        {
            get { return radacctid; }
            set { radacctid = value; }
        }  //***Accesseur de acctsessionid***
        public string Acctsessionid
        {
            get { return acctsessionid; }
            set { acctsessionid = value; }
        }  //***Accesseur de acctuniqueid***
        public string Acctuniqueid
        {
            get { return acctuniqueid; }
            set { acctuniqueid = value; }
        }  //***Accesseur de username***
        public string Username
        {
            get { return username; }
            set { username = value; }
        }  //***Accesseur de groupname***
        public string Groupname
        {
            get { return groupname; }
            set { groupname = value; }
        }  //***Accesseur de realm***
        public string Realm
        {
            get { return realm; }
            set { realm = value; }
        }  //***Accesseur de nasipaddress***
        public string Nasipaddress
        {
            get { return nasipaddress; }
            set { nasipaddress = value; }
        }  //***Accesseur de nasportid***
        public string Nasportid
        {
            get { return nasportid; }
            set { nasportid = value; }
        }  //***Accesseur de nasporttype***
        public string Nasporttype
        {
            get { return nasporttype; }
            set { nasporttype = value; }
        }  //***Accesseur de acctstarttime***
        public DateTime? Acctstarttime
        {
            get { return acctstarttime; }
            set { acctstarttime = value; }
        }  //***Accesseur de acctstoptime***
        public DateTime? Acctstoptime
        {
            get { return acctstoptime; }
            set { acctstoptime = value; }
        }  //***Accesseur de acctsessiontime***
        public int? Acctsessiontime
        {
            get { return acctsessiontime; }
            set { acctsessiontime = value; }
        }  //***Accesseur de acctauthentic***
        public string Acctauthentic
        {
            get { return acctauthentic; }
            set { acctauthentic = value; }
        }  //***Accesseur de connectinfo_start***
        public string Connectinfo_start
        {
            get { return connectinfo_start; }
            set { connectinfo_start = value; }
        }  //***Accesseur de connectinfo_stop***
        public string Connectinfo_stop
        {
            get { return connectinfo_stop; }
            set { connectinfo_stop = value; }
        }  //***Accesseur de acctinputoctets***
        public long? Acctinputoctets
        {
            get { return acctinputoctets; }
            set { acctinputoctets = value; }
        }  //***Accesseur de acctoutputoctets***
        public long? Acctoutputoctets
        {
            get { return acctoutputoctets; }
            set { acctoutputoctets = value; }
        }  //***Accesseur de calledstationid***
        public string Calledstationid
        {
            get { return calledstationid; }
            set { calledstationid = value; }
        }  //***Accesseur de callingstationid***
        public string Callingstationid
        {
            get { return callingstationid; }
            set { callingstationid = value; }
        }  //***Accesseur de acctterminatecause***
        public string Acctterminatecause
        {
            get { return acctterminatecause; }
            set { acctterminatecause = value; }
        }  //***Accesseur de servicetype***
        public string Servicetype
        {
            get { return servicetype; }
            set { servicetype = value; }
        }  //***Accesseur de framedprotocol***
        public string Framedprotocol
        {
            get { return framedprotocol; }
            set { framedprotocol = value; }
        }  //***Accesseur de framedipaddress***
        public string Framedipaddress
        {
            get { return framedipaddress; }
            set { framedipaddress = value; }
        }  //***Accesseur de acctstartdelay***
        public int? Acctstartdelay
        {
            get { return acctstartdelay; }
            set { acctstartdelay = value; }
        }  //***Accesseur de acctstopdelay***
        public int? Acctstopdelay
        {
            get { return acctstopdelay; }
            set { acctstopdelay = value; }
        }  //***Accesseur de xascendsessionsvrunique***
        public string Xascendsessionsvrkey
        {
            get { return xascendsessionsvrkey; }
            set { xascendsessionsvrkey = value; }
        }
        public long Nombre_enregistrement
        {
            get { return nombre_enregistrement; }
            set { nombre_enregistrement = value; }
        } //***Accesseur de nombre_enregistrement***
    } //***fin class
} //***fin namespace