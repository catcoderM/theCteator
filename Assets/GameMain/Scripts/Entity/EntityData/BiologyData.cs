using System;
using UnityEngine;

namespace StarForce
{
    [Serializable]
    public class BiologyData : EntityData
    {
        private string m_Name;
        public BiologyData(int entityId, int typeId ,string name)
            : base(entityId, typeId)
        {
            m_Name = name;
        }



        public string name
        {
            get
            {
                return m_Name;
            }
        }
    }
}
