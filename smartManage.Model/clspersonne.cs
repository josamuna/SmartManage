using System;
using System.Data;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clspersonne
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private int id;
        private string nom;
        private string postnom;
        private string prenom;
        private int id_grade;
        private bool? isenseignant;
        private bool? isagent;
        private bool? isetudiant;
        private string user_created;
        private DateTime? date_created;
        private string user_modified;
        private DateTime? date_modified;
        //***Listes***
        public List<clspersonne> listes()
        {
            return clsMetier.GetInstance().getAllClspersonne();
        }
        public List<clspersonne> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClspersonne(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClspersonne(this);
        }
        public int update(clspersonne varscls)
        {
            return clsMetier.GetInstance().updateClspersonne(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClspersonne(this);
        }
        public int delete(clspersonne varscls)
        {
            return clsMetier.GetInstance().deleteClspersonne(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClspersonne(this);
        }
        //***Le constructeur par defaut***
        public clspersonne()
        {
        }

        //***Accesseur de id***
        public int Id
        {
            get { return id; }
            set { id = value; }
        }  //***Accesseur de nom***
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }  //***Accesseur de postnom***
        public string Postnom
        {
            get { return postnom; }
            set { postnom = value; }
        }  //***Accesseur de prenom***
        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }  //***Accesseur de id_grade***
        public int Id_grade
        {
            get { return id_grade; }
            set { id_grade = value; }
        }  //***Accesseur de isenseignant***
        public bool? Isenseignant
        {
            get { return isenseignant; }
            set { isenseignant = value; }
        }  //***Accesseur de isagent***
        public bool? Isagent
        {
            get { return isagent; }
            set { isagent = value; }
        }  //***Accesseur de isetudiant***
        public bool? Isetudiant
        {
            get { return isetudiant; }
            set { isetudiant = value; }
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
        public string NomComplet
        {
            get
            {
                return string.Format("{0} {1} {2}".Trim(), Nom, Postnom, Prenom);
            }
        }
    } //***fin class
} //***fin namespace