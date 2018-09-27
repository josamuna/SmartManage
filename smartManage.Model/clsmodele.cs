using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsmodele
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
        public List<clsmodele> listes()
        {
            return clsMetier.GetInstance().getAllClsmodele();
        }
        public List<clsmodele> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsmodele(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsmodele(this);
        }
        public int update(clsmodele varscls)
        {
            return clsMetier.GetInstance().updateClsmodele(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsmodele(this);
        }
        public int delete(clsmodele varscls)
        {
            return clsMetier.GetInstance().deleteClsmodele(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsmodele(this);
        }
        //***Le constructeur par defaut***
        public clsmodele()
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