using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clscompte
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string numero;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clscompte> listes()
        {
            return clsMetier.GetInstance().getAllClscompte();
        }
        public List<clscompte> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClscompte(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClscompte(this);
        }
        public int update(clscompte varscls)
        {
            return clsMetier.GetInstance().updateClscompte(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClscompte(this);
        }
        public int delete(clscompte varscls)
        {
            return clsMetier.GetInstance().deleteClscompte(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClscompte(this);
        }
        //***Le constructeur par defaut***
        public clscompte()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de numero***
        public string Numero
        {
            get { return numero; }
            set { numero = value; }
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