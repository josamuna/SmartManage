using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsram
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
        public List<clsram> listes()
        {
            return clsMetier.GetInstance().getAllClsram();
        }
        public List<clsram> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsram(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsram(this);
        }
        public int update(clsram varscls)
        {
            return clsMetier.GetInstance().updateClsram(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsram(this);
        }
        public int delete(clsram varscls)
        {
            return clsMetier.GetInstance().deleteClsram(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsram(this);
        }
        //***Le constructeur par defaut***
        public clsram()
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
}//***fin namespace