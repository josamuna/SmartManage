using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clstelephone
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private int id_personne;
        private string code;
        private string numero;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clstelephone> listes()
        {
            return clsMetier.GetInstance().getAllClstelephone();
        }
        public List<clstelephone> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClstelephone(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClstelephone(this);
        }
        public int update(clstelephone varscls)
        {
            return clsMetier.GetInstance().updateClstelephone(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClstelephone(this);
        }
        public int delete(clstelephone varscls)
        {
            return clsMetier.GetInstance().deleteClstelephone(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClstelephone(this);
        }
        //***Le constructeur par defaut***
        public clstelephone()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de id_personne***
        public int Id_personne
        {
            get { return id_personne; }
            set { id_personne = value; }
        }  //***Accesseur de code***
        public string Code
        {
            get { return code; }
            set { code = value; }
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
        public string NumeroComplet
        {
            get { return string.Format("{0}{1}", code, numero); }
            set { string.Format("{0}{1}", code, numero); }
        }
    } //***fin class
} //***fin namespace