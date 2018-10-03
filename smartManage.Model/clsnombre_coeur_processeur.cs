using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsnombre_coeur_processeur
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int valeur;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsnombre_coeur_processeur> listes()
        {
            return clsMetier.GetInstance().getAllClsnombre_coeur_processeur();
        }
        public List<clsnombre_coeur_processeur> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsnombre_coeur_processeur(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsnombre_coeur_processeur(this);
        }
        public int update(clsnombre_coeur_processeur varscls)
        {
            return clsMetier.GetInstance().updateClsnombre_coeur_processeur(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsnombre_coeur_processeur(this);
        }
        public int delete(clsnombre_coeur_processeur varscls)
        {
            return clsMetier.GetInstance().deleteClsnombre_coeur_processeur(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsnombre_coeur_processeur(this);
        }
        //***Le constructeur par defaut***
        public clsnombre_coeur_processeur()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de valeur***
        public int Valeur
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