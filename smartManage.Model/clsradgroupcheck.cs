using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradgroupcheck
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string groupname;
        private string attribute;
        private string op;
        private string _value;
        private int nombre_enregistrement;
        //***Listes***
        public List<clsradgroupcheck> listes()
        {
            return clsMetier1.GetInstance().getAllClsradgroupcheck();
        }
        public List<clsradgroupcheck> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradgroupcheck(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradgroupcheck(this);
        }
        public int update(clsradgroupcheck varscls)
        {
            return clsMetier1.GetInstance().updateClsradgroupcheck(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradgroupcheck(this);
        }
        public int delete(clsradgroupcheck varscls)
        {
            return clsMetier1.GetInstance().deleteClsradgroupcheck(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradgroupcheck(this);
        }
        //***Le constructeur par defaut***
        public clsradgroupcheck()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de groupname***
        public string Groupname
        {
            get { return groupname; }
            set { groupname = value; }
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
        public int Nombre_enregistrement
        {
            get { return nombre_enregistrement; }
            set { nombre_enregistrement = value; }
        } //***Accesseur de nombre_enregistrement***
    } //***fin class
} //***fin namespace