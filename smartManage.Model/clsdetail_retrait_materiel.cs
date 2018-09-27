using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsdetail_retrait_materiel
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_retrait_materiel;
        private int id_materiel;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsdetail_retrait_materiel> listes()
        {
            return clsMetier.GetInstance().getAllClsdetail_retrait_materiel();
        }
        public List<clsdetail_retrait_materiel> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsdetail_retrait_materiel(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsdetail_retrait_materiel(this);
        }
        public int update(clsdetail_retrait_materiel varscls)
        {
            return clsMetier.GetInstance().updateClsdetail_retrait_materiel(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsdetail_retrait_materiel(this);
        }
        public int delete(clsdetail_retrait_materiel varscls)
        {
            return clsMetier.GetInstance().deleteClsdetail_retrait_materiel(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsdetail_retrait_materiel(this);
        }
        //***Le constructeur par defaut***
        public clsdetail_retrait_materiel()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de id_retrait_materiel***
        public int Id_retrait_materiel
        {
            get { return id_retrait_materiel; }
            set { id_retrait_materiel = value; }
        }  //***Accesseur de id_materiel***
        public int Id_materiel
        {
            get { return id_materiel; }
            set { id_materiel = value; }
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