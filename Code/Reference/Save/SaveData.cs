using System;
using System.Collections.Generic;

namespace HN.Code.Reference.Save
{
    [Serializable]
    public class SaveData
    {
        public int saveId;
        public string data;
    }
    
    [Serializable]
    public struct DataCollection
    {
        public List<SaveData> dataList;
    }
}