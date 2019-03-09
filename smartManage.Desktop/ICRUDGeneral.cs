using System;

namespace smartManage.Desktop
{
    interface ICRUDGeneral
    {
        void New();
        void Search(string criteria);
        void Save();
        void UpdateRec();
        void Delete();
        void RefreshRec();
        void Preview();
    }
}
