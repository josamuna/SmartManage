using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstension_adaptateur
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private double valeur;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clstension_adaptateur> listes()
        {
            return clsMetier.GetInstance().getAllClstension_adaptateur();
        }
        public List<clstension_adaptateur> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstension_adaptateur(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstension_adaptateur(this);
        }
        public int update(clstension_adaptateur varscls)
        {
            return clsMetier.GetInstance().updateClstension_adaptateur(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstension_adaptateur(this);
        }
        public int delete(clstension_adaptateur varscls)
        {
            return clsMetier.GetInstance().deleteClstension_adaptateur(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstension_adaptateur(this);
        }
        //***Le constructeur par defaut***
        public clstension_adaptateur()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de valeur***
        public double Valeur
        {
            get { return valeur; }
            set { valeur = value; }
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