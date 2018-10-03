using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsmicrophone
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
        public List<clsmicrophone> listes()
        {
            return clsMetier.GetInstance().getAllClsmicrophone();
        }
        public List<clsmicrophone> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsmicrophone(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsmicrophone(this);
        }
        public int update(clsmicrophone varscls)
        {
            return clsMetier.GetInstance().updateClsmicrophone(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsmicrophone(this);
        }
        public int delete(clsmicrophone varscls)
        {
            return clsMetier.GetInstance().deleteClsmicrophone(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsmicrophone(this);
        }
        //***Le constructeur par defaut***
        public clsmicrophone()
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