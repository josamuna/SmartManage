using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsemail
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_personne;
        private string designation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsemail> listes()
        {
            return clsMetier.GetInstance().getAllClsemail();
        }
        public List<clsemail> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsemail(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsemail(this);
        }
        public int update(clsemail varscls)
        {
            return clsMetier.GetInstance().updateClsemail(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsemail(this);
        }
        public int delete(clsemail varscls)
        {
            return clsMetier.GetInstance().deleteClsemail(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsemail(this);
        }
        //***Le constructeur par defaut***
        public clsemail()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de id_personne***
        public int Id_personne
        {
            get { return id_personne; }
            set { id_personne = value; }
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