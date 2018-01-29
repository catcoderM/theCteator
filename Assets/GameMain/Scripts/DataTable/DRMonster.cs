using GameFramework.DataTable;
using System.Collections.Generic;

namespace StarForce
{
    /// <summary>
    /// 生物表。
    /// </summary>
    public class DRMonster : IDataRow
    {


        /// <summary>
        /// 编号。
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 编号。
        /// </summary>
        public string BName
        {
            get;
            set;
        }
        public string asset;
        public string intro;
        public int endValue;

        public int soundId;
        public void ParseDataRow(string dataRowText)
        {
            //使用\t分割
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            //这里有一个坑  
            Id = int.Parse(text[0]);
            BName = text[1];
            intro = text[2];
            asset = text[3];
            endValue = int.Parse( text[4]);
            string tmp = text[5];
            if (tmp == "")
            {
                tmp = "6";
            }
            soundId = int.Parse(tmp);
        }

        public bool IsFirst()
        {
            return Id < 5;
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRAircraft>();
        }
    }
}
