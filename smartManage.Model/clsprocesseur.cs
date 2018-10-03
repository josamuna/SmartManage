using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsprocesseur
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
        public List<clsprocesseur> listes()
        {
            return clsMetier.GetInstance().getAllClsprocesseur();
        }
        public List<clsprocesseur> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsprocesseur(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsprocesseur(this);
        }
        public int update(clsprocesseur varscls)
        {
            return clsMetier.GetInstance().updateClsprocesseur(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsprocesseur(this);
        }
        public int delete(clsprocesseur varscls)
        {
            return clsMetier.GetInstance().deleteClsprocesseur(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsprocesseur(this);
        }
        //***Le constructeur par defaut***
        public clsprocesseur()
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