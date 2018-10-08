using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsfrequence
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
        public List<clsfrequence> listes()
        {
            return clsMetier.GetInstance().getAllClsfrequence();
        }
        public List<clsfrequence> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsfrequence(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsfrequence(this);
        }
        public int update(clsfrequence varscls)
        {
            return clsMetier.GetInstance().updateClsfrequence(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsfrequence(this);
        }
        public int delete(clsfrequence varscls)
        {
            return clsMetier.GetInstance().deleteClsfrequence(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsfrequence(this);
        }
        //***Le constructeur par defaut***
        public clsfrequence()
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