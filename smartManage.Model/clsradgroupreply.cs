using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsradgroupreply
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string groupname;
        private string attribute;
        private string op;
        private string _value;
        //***Listes***
        public List<clsradgroupreply> listes()
        {
            return clsMetier1.GetInstance().getAllClsradgroupreply();
        }
        public List<clsradgroupreply> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradgroupreply(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradgroupreply(this);
        }
        public int update(clsradgroupreply varscls)
        {
            return clsMetier1.GetInstance().updateClsradgroupreply(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradgroupreply(this);
        }
        public int delete(clsradgroupreply varscls)
        {
            return clsMetier1.GetInstance().deleteClsradgroupreply(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradgroupreply(this);
        }
        //***Le constructeur par defaut***
        public clsradgroupreply()
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
    } //***fin class
} //***fin namespace