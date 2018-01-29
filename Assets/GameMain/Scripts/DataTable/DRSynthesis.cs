using GameFramework.DataTable;
using System.Collections.Generic;

namespace StarForce
{
    /// <summary>
    /// 生物表。
    /// </summary>
    public class DRSynthesis : IDataRow
    {
        private static int _id = 0;
        public static Dictionary<int, MonIdAndCounts> allDictionary = new Dictionary<int, MonIdAndCounts>(); 
        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }
        public string Id_Id;

        public string cId;

        public void ParseDataRow(string dataRowText)
        {
            //使用\t分割
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            //这里有一个坑  
            Id = _id++;
            Id_Id = text[0];
            string[] ids = Id_Id.Split('_');
            int tid = int.Parse(ids[0]);


            cId = text[1];
            if ( !cId.Equals("200") && !cId.Equals("100"))
            {
                if (allDictionary.ContainsKey(tid))
                {
                    allDictionary[tid].AddSynFun(int.Parse(ids[1]), int.Parse(cId));
                }
                else
                {
                    MonIdAndCounts tmp = new MonIdAndCounts();
                    tmp.AddSynFun(int.Parse(ids[1]), int.Parse(cId));
                    allDictionary.Add(tid,tmp );
                }
            }
           
           
        }

        public bool CanCompose()
        {
            return true;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAircraft>();
        }
    }
}
