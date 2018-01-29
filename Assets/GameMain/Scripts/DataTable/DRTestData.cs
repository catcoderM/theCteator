using GameFramework.DataTable;
using System.Collections.Generic;

namespace StarForce
{
    /// <summary>
    /// 测试数据表
    /// </summary>
    public class DRTestData : IDataRow{
        //
        // Properties
        //
        public int Id
        {
            get;
            private set;
        }
        public string Key
        {
            get;
            private set;
        }
        public string Value
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            //使用\t分割
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            //这里有一个坑  
            Id = int.Parse(text[1]);
            Key = text[2];
            Value = text[3];
        }

        //// 避免 iOS 系统产生 JIT 异常

        private void AvoidJIT()
        {
            new Dictionary<int, DRThruster>();
        }
    }
}
