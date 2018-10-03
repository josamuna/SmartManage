using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstype_clavier
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
        public List<clstype_clavier> listes()
        {
            return clsMetier.GetInstance().getAllClstype_clavier();
        }
        public List<clstype_clavier> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstype_clavier(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstype_clavier(this);
        }
        public int update(clstype_clavier varscls)
        {
            return clsMetier.GetInstance().updateClstype_clavier(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstype_clavier(this);
        }
        public int delete(clstype_clavier varscls)
        {
            return clsMetier.GetInstance().deleteClstype_clavier(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstype_clavier(this);
        }
        //***Le constructeur par defaut***
        public clstype_clavier()
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