using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clscategorie_materiel
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string designation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clscategorie_materiel> listes()
        {
            return clsMetier.GetInstance().getAllClscategorie_materiel();
        }
        public List<clscategorie_materiel> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClscategorie_materiel(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClscategorie_materiel(this);
        }
        public int update(clscategorie_materiel varscls)
        {
            return clsMetier.GetInstance().updateClscategorie_materiel(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClscategorie_materiel(this);
        }
        public int delete(clscategorie_materiel varscls)
        {
            return clsMetier.GetInstance().deleteClscategorie_materiel(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClscategorie_materiel(this);
        }
        //***Le constructeur par defaut***
        public clscategorie_materiel()
        {
        }

        //***Accesseur de id***
        public int Id
        {

            get { return id; }
            set { id = value; }
        }  //***Accesseur de designation***
        public string Designation
        {

            get { return designation; }
            set { designation = value; }
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