using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clspoids
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
        public List<clspoids> listes()
        {
            return clsMetier.GetInstance().getAllClspoids();
        }
        public List<clspoids> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClspoids(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClspoids(this);
        }
        public int update(clspoids varscls)
        {
            return clsMetier.GetInstance().updateClspoids(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClspoids(this);
        }
        public int delete(clspoids varscls)
        {
            return clsMetier.GetInstance().deleteClspoids(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClspoids(this);
        }
        //***Le constructeur par defaut***
        public clspoids()
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