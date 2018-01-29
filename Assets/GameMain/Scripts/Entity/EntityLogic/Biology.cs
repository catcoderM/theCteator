using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    /// <summary>
    /// 生物类
    /// </summary>
    public abstract class Biology : Entity
    {
       
        [SerializeField]
        private BiologyData m_BioloyData = null;




#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);

            m_BioloyData = userData as BiologyData;
            if (m_BioloyData == null)
            {
                Log.Error("m_BioloyData data is invalid.");
                return;
            }



            //创建图片

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnHide(object userData)
#else
        protected internal override void OnHide(object userData)
#endif
        {
            base.OnHide(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttached(childEntity, parentTransform, userData);

          
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDetached(EntityLogic childEntity, object userData)
#else
        protected internal override void OnDetached(EntityLogic childEntity, object userData)
#endif
        {
            base.OnDetached(childEntity, userData);

           
        }
      

    }
}
