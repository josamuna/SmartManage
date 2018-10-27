using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradpostauth
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string username;
        private string pass;
        private string reply;
        private DateTime authdate;
        //***Listes***
        public List<clsradpostauth> listes()
        {
            return clsMetier1.GetInstance().getAllClsradpostauth();
        }
        public List<clsradpostauth> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradpostauth(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradpostauth(this);
        }
        public int update(clsradpostauth varscls)
        {
            return clsMetier1.GetInstance().updateClsradpostauth(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradpostauth(this);
        }
        public int delete(clsradpostauth varscls)
        {
            return clsMetier1.GetInstance().deleteClsradpostauth(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradpostauth(this);
        }
        //***Le constructeur par defaut***
        public clsradpostauth()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de username***
        public string Username
        {
            get { return username; }
            set { username = value; }
        }  //***Accesseur de pass***
        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }  //***Accesseur de reply***
        public string Reply
        {
            get { return reply; }
            set { reply = value; }
        }  //***Accesseur de authdate***
        public DateTime Authdate
        {
            get { return authdate; }
            set { authdate = value; }
        }
    } //***fin class
} //***fin namespace