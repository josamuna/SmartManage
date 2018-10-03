using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstype_switch
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
        public List<clstype_switch> listes()
        {
            return clsMetier.GetInstance().getAllClstype_switch();
        }
        public List<clstype_switch> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstype_switch(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstype_switch(this);
        }
        public int update(clstype_switch varscls)
        {
            return clsMetier.GetInstance().updateClstype_switch(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstype_switch(this);
        }
        public int delete(clstype_switch varscls)
        {
            return clsMetier.GetInstance().deleteClstype_switch(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstype_switch(this);
        }
        //***Le constructeur par defaut***
        public clstype_switch()
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