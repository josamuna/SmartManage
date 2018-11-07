using System;
using System.Collections.Generic;
using System.Data;

namespace smartManage.RadiusAdminModel
{
    public class clsradcheck:clsradusergroup
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string username;
        private string attribute;
        private string op;
        private string _value;
        private long nombre_enregistrement;
        //***Listes***
        public new List<clsradcheck> listes()
        {
            return clsMetier1.GetInstance().getAllClsradcheck();
        }
        public new List<clsradcheck> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradcheck(criteria);
        }
        public new int inserts()
        {
            return clsMetier1.GetInstance().insertClsradcheck(this);
        }
        public int inserts(DataRowView varscls)
        {
            return clsMetier1.GetInstance().insertClsradcheck_dtrowv(varscls);
        }
        public int update(DataRowView varscls)
        {
            return clsMetier1.GetInstance().updateClsradcheck_dtrowv(varscls);
        }
        public new int update()
        {
            return clsMetier1.GetInstance().updateClsradcheck(this);
        }
        public int delete(DataRowView varscls)
        {
            return clsMetier1.GetInstance().deleteClsradcheck_dtrowv(varscls);
        }
        public new int delete()
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
        public new string Username
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
        public new long Nombre_enregistrement
        {
            get { return nombre_enregistrement; }
            set { nombre_enregistrement = value; }
        } //***Accesseur de nombre_enregistrement***
    } //***fin class
} //***fin namespace