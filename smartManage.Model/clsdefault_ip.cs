using System;
using System.Collections.Generic;

namespace smartManage.Model
{
    public class clsdefault_ip
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
        public List<clsdefault_ip> listes()
        {
            return clsMetier.GetInstance().getAllClsdefault_ip();
        }
        public List<clsdefault_ip> listes(string criteria)
        {
            return clsMetier.GetInstance().getAllClsdefault_ip(criteria);
        }
        public int inserts()
        {
            return clsMetier.GetInstance().insertClsdefault_ip(this);
        }
        public int update(clsdefault_ip varscls)
        {
            return clsMetier.GetInstance().updateClsdefault_ip(varscls);
        }
        public int update()
        {
            return clsMetier.GetInstance().updateClsdefault_ip(this);
        }
        public int delete(clsdefault_ip varscls)
        {
            return clsMetier.GetInstance().deleteClsdefault_ip(varscls);
        }
        public int delete()
        {
            return clsMetier.GetInstance().deleteClsdefault_ip(this);
        }
        //***Le constructeur par defaut***
        public clsdefault_ip()
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