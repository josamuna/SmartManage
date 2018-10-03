using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clspage_par_minute
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
        public List<clspage_par_minute> listes()
        {
            return clsMetier.GetInstance().getAllClspage_par_minute();
        }
        public List<clspage_par_minute> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClspage_par_minute(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClspage_par_minute(this);
        }
        public int update(clspage_par_minute varscls)
        {
            return clsMetier.GetInstance().updateClspage_par_minute(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClspage_par_minute(this);
        }
        public int delete(clspage_par_minute varscls)
        {
            return clsMetier.GetInstance().deleteClspage_par_minute(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClspage_par_minute(this);
        }
        //***Le constructeur par defaut***
        public clspage_par_minute()
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