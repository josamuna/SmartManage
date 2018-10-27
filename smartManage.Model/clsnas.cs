using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsnas
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string nasname;
        private string shortname;
        private string type;
        private int? ports;
        private string secret;
        private string server;
        private string community;
        private string description;
        //***Listes***
        public List<clsnas> listes()
        {
            return clsMetier1.GetInstance().getAllClsnas();
        }
        public List<clsnas> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsnas(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsnas(this);
        }
        public int update(clsnas varscls)
        {
            return clsMetier1.GetInstance().updateClsnas(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsnas(this);
        }
        public int delete(clsnas varscls)
        {
            return clsMetier1.GetInstance().deleteClsnas(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsnas(this);
        }
        //***Le constructeur par defaut***
        public clsnas()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de nasname***
        public string Nasname
        {
            get { return nasname; }
            set { nasname = value; }
        }  //***Accesseur de shortname***
        public string Shortname
        {
            get { return shortname; }
            set { shortname = value; }
        }  //***Accesseur de type***
        public string Type
        {
            get { return type; }
            set { type = value; }
        }  //***Accesseur de ports***
        public int? Ports
        {
            get { return ports; }
            set { ports = value; }
        }  //***Accesseur de secret***
        public string Secret
        {
            get { return secret; }
            set { secret = value; }
        }  //***Accesseur de server***
        public string Server
        {
            get { return server; }
            set { server = value; }
        }  //***Accesseur de community***
        public string Community
        {
            get { return community; }
            set { community = value; }
        }  //***Accesseur de description***
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    } //***fin class
} //***fin namespace