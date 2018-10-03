using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsaffectation_materiel
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string code_ac;
        private int id_lieu_affectation;
        private int id_materiel;
        private int? id_salle;
        private DateTime date_affectation;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clsaffectation_materiel> listes()
        {
            return clsMetier.GetInstance().getAllClsaffectation_materiel();
        }
        public List<clsaffectation_materiel> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsaffectation_materiel(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsaffectation_materiel(this);
        }
        public int update(clsaffectation_materiel varscls)
        {
            return clsMetier.GetInstance().updateClsaffectation_materiel(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsaffectation_materiel(this);
        }
        public int delete(clsaffectation_materiel varscls)
        {
            return clsMetier.GetInstance().deleteClsaffectation_materiel(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsaffectation_materiel(this);
        }
        //***Le constructeur par defaut***
        public clsaffectation_materiel()
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
        }  //***Accesseur de id_lieu_affectation***
        public int Id_lieu_affectation
        {
            get { return id_lieu_affectation; }
            set { id_lieu_affectation = value; }
        }  //***Accesseur de id_materiel***
        public int Id_materiel
        {
            get { return id_materiel; }
            set { id_materiel = value; }
        }  //***Accesseur de id_salle***
        public int? Id_salle
        {
            get { return id_salle; }
            set { id_salle = value; }
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