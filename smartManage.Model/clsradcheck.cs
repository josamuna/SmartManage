using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradcheck
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string username;
        private string attribute;
        private string op;
        private string _value;
        //***Listes***
        public List<clsradcheck> listes()
        {
            return clsMetier1.GetInstance().getAllClsradcheck();
        }
        public List<clsradcheck> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradcheck(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradcheck(this);
        }
        public int update(clsradcheck varscls)
        {
            return clsMetier1.GetInstance().updateClsradcheck(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradcheck(this);
        }
        public int delete(clsradcheck varscls)
        {
            return clsMetier1.GetInstance().deleteClsradcheck(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradcheck(this);
        }
        //***Le constructeur par defaut***
        public clsradcheck()
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