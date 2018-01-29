using UnityEngine;
using System.Collections;

namespace StarForce
{
    public class SimpleName
    {
        public DRMonster comMon;
        public DRMonster toMon;
        public SimpleName(DRMonster c , DRMonster t)
        {
            this.comMon = c;
            this.toMon = t;
        }


        public bool hasAdd(DRMonster d){
            return d.Id == comMon.Id;
        }

        public string GetFinalName()
        {
            if (toMon != null)
            {
                return toMon.BName;
            }

            return "失败";
        }
    }

}
