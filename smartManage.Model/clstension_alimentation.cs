using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstension_alimentation
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
        public List<clstension_alimentation> listes()
        {
            return clsMetier.GetInstance().getAllClstension_alimentation();
        }
        public List<clstension_alimentation> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstension_alimentation(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstension_alimentation(this);
        }
        public int update(clstension_alimentation varscls)
        {
            return clsMetier.GetInstance().updateClstension_alimentation(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstension_alimentation(this);
        }
        public int delete(clstension_alimentation varscls)
        {
            return clsMetier.GetInstance().deleteClstension_alimentation(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstension_alimentation(this);
        }
        //***Le constructeur par defaut***
        public clstension_alimentation()
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