using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityEngine.UI;

namespace StarForce{
    
    public class TestPrefab : UGuiForm
    {
        [SerializeField]
        private GameObject m_TestBtn = null;
        [SerializeField]
        private GameObject m_MoveImg = null;


        private bool m_Moving = false; 
        public void OnClickTestBtn()
        {
            m_Moving = !m_Moving;
            Debug.Log("click btn");

            IDataTable<DRTestData> dtScene = GameEntry.DataTable.GetDataTable<DRTestData>();
            DRTestData drScene = dtScene.GetDataRow(1);
            Debug.Log("[testPrefab]测试配置表 " + drScene.Value);

            //切换ui视图
            //坑 配置 PauseCoveredUIForm(下层ui全暂停）UIGroupName（ui层次）
            //参数 透传值
            GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
        }

        protected internal override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected internal override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected internal override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_Moving && m_MoveImg)
            {
                float posy = m_MoveImg.transform.position.y + 10;
                m_MoveImg.transform.SetPositionY(posy);

            }
        }
    }
}

