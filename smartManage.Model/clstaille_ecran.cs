using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstaille_ecran
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
        public List<clstaille_ecran> listes()
        {
            return clsMetier.GetInstance().getAllClstaille_ecran();
        }
        public List<clstaille_ecran> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstaille_ecran(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstaille_ecran(this);
        }
        public int update(clstaille_ecran varscls)
        {
            return clsMetier.GetInstance().updateClstaille_ecran(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstaille_ecran(this);
        }
        public int delete(clstaille_ecran varscls)
        {
            return clsMetier.GetInstance().deleteClstaille_ecran(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstaille_ecran(this);
        }
        //***Le constructeur par defaut***
        public clstaille_ecran()
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