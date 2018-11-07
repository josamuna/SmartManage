using System;
using System.Collections.Generic;

namespace smartManage.RadiusAdminModel
{
    public class clsradusergroup
    {
        //***Les variables globales***
        //****private string schaine_conn*****
        private string username;
        private string groupname;
        private int priority;
        private long nombre_enregistrement;
        //***Listes***
        public List<clsradusergroup> listes()
        {
            return clsMetier1.GetInstance().getAllClsradusergroup();
        }
        public List<clsradusergroup> listes(string criteria)
        {
            return clsMetier1.GetInstance().getAllClsradusergroup(criteria);
        }
        public int inserts()
        {
            return clsMetier1.GetInstance().insertClsradusergroup(this);
        }
        public int update(clsradusergroup varscls)
        {
            return clsMetier1.GetInstance().updateClsradusergroup(varscls);
        }
        public int update()
        {
            return clsMetier1.GetInstance().updateClsradusergroup(this);
        }
        public int delete(clsradusergroup varscls)
        {
            return clsMetier1.GetInstance().deleteClsradusergroup(varscls);
        }
        public int delete()
        {
            return clsMetier1.GetInstance().deleteClsradusergroup(this);
        }
        //***Le constructeur par defaut***
        public clsradusergroup()
        {
        }

        //***Accesseur de username***
        public string Username
        {
            get { return username; }
            set { username = value; }
        }  //***Accesseur de groupname***
        public string Groupname
        {
            get { return groupname; }
            set { groupname = value; }
        }  //***Accesseur de priority***
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        public long Nombre_enregistrement
        {
            get { return nombre_enregistrement; }
            set { nombre_enregistrement = value; }
        } //***Accesseur de nombre_enregistrement***
    } //***fin class
} //***fin namespace