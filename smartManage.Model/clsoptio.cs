using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsoptio
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string designation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsoptio> listes()
        {
            return clsMetier.GetInstance().getAllClsoptio();
        }
        public List<clsoptio> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsoptio(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsoptio(this);
        }
        public int update(clsoptio varscls)
        {
            return clsMetier.GetInstance().updateClsoptio(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsoptio(this);
        }
        public int delete(clsoptio varscls)
        {
            return clsMetier.GetInstance().deleteClsoptio(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsoptio(this);
        }
        //***Le constructeur par defaut***
        public clsoptio()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de designation***
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
        }  //***Accesseur de user_created***
        public string User_created
        {
            get { return user_created; }
            set { user_created = value; }
        }  //***Accesseur de date_created***
        public DateTime? Date_created
        {
            get { return date_created; }
            set { date_created = value; }
        }  //***Accesseur de user_modified***
        public string User_modified
        {
            get { return user_modified; }
            set { user_modified = value; }
        }  //***Accesseur de date_modified***
        public DateTime? Date_modified
        {
            get { return date_modified; }
            set { date_modified = value; }
        }
    } //***fin class
} //***fin namespace