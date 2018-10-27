using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradreply
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string username;
        private string attribute;
        private string op;
        private string _value;
        //***Listes***
        public List<clsradreply> listes()
        {
            return clsMetier1.GetInstance().getAllClsradreply();
        }
        public List<clsradreply> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradreply(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradreply(this);
        }
        public int update(clsradreply varscls)
        {
            return clsMetier1.GetInstance().updateClsradreply(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradreply(this);
        }
        public int delete(clsradreply varscls)
        {
            return clsMetier1.GetInstance().deleteClsradreply(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradreply(this);
        }
        //***Le constructeur par defaut***
        public clsradreply()
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
        }  //***Accesseur de attribute***
        public string Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }  //***Accesseur de op***
        public string Op
        {
            get { return op; }
            set { op = value; }
        }  //***Accesseur de value***
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    } //***fin class
} //***fin namespace