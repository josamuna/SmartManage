using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsportee
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
        public List<clsportee> listes()
        {
            return clsMetier.GetInstance().getAllClsportee();
        }
        public List<clsportee> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsportee(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsportee(this);
        }
        public int update(clsportee varscls)
        {
            return clsMetier.GetInstance().updateClsportee(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsportee(this);
        }
        public int delete(clsportee varscls)
        {
            return clsMetier.GetInstance().deleteClsportee(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsportee(this);
        }
        //***Le constructeur par defaut***
        public clsportee()
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