using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsnetette
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
        public List<clsnetette> listes()
        {
            return clsMetier.GetInstance().getAllClsnetette();
        }
        public List<clsnetette> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsnetette(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsnetette(this);
        }
        public int update(clsnetette varscls)
        {
            return clsMetier.GetInstance().updateClsnetette(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsnetette(this);
        }
        public int delete(clsnetette varscls)
        {
            return clsMetier.GetInstance().deleteClsnetette(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsnetette(this);
        }
        //***Le constructeur par defaut***
        public clsnetette()
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