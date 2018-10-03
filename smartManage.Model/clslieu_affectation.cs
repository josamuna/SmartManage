using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clslieu_affectation
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string code_ac;
        private int id_type_lieu_affectation;
        private int? id_personne;
        private int? id_fonction;
        private string designation;
        private DateTime date_affectation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clslieu_affectation> listes()
        {
            return clsMetier.GetInstance().getAllClslieu_affectation();
        }
        public List<clslieu_affectation> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClslieu_affectation(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClslieu_affectation(this);
        }
        public int update(clslieu_affectation varscls)
        {
            return clsMetier.GetInstance().updateClslieu_affectation(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClslieu_affectation(this);
        }
        public int delete(clslieu_affectation varscls)
        {
            return clsMetier.GetInstance().deleteClslieu_affectation(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClslieu_affectation(this);
        }
        //***Le constructeur par defaut***
        public clslieu_affectation()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de code_ac***
        public string Code_ac
        {
            get { return code_ac; }
            set { code_ac = value; }
        }  //***Accesseur de id_type_lieu_affectation***
        public int Id_type_lieu_affectation
        {
            get { return id_type_lieu_affectation; }
            set { id_type_lieu_affectation = value; }
        }  //***Accesseur de id_personne***
        public int? Id_personne
        {
            get { return id_personne; }
            set { id_personne = value; }
        }  //***Accesseur de id_fonction***
        public int? Id_fonction
        {
            get { return id_fonction; }
            set { id_fonction = value; }
        }  //***Accesseur de designation***
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
        }  //***Accesseur de date_affectation***
        public DateTime Date_affectation
        {
            get { return date_affectation; }
            set { date_affectation = value; }
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