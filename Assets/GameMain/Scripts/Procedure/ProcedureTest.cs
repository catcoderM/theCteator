using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    
    public class ProcedureTest : ProcedureBase
    {
        private TestPrefab m_TestPrefab = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

            GameEntry.DataTable.LoadDataTable("TestData", this);

            GameEntry.Event.Subscribe(OpenUIFormUpdateEventArgs.EventId,OnOpenUISuccess);

      
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            GameEntry.UI.OpenUIForm(UIFormId.TestPrefab, this);
            Log.Info("Load data table '{0}' OK.", ne.DataTableName);
        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {
            LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);
        }



        private void OnOpenUISuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            m_TestPrefab = (TestPrefab)ne.UIForm.Logic;
        }


        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormUpdateEventArgs.EventId,OnOpenUISuccess);
            if (m_TestPrefab != null)
            {
                //坑 切换场景是否需要关闭当前ui
                m_TestPrefab.Close(isShutdown);
                m_TestPrefab = null;
            }
        }
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

                
            //if (m_StartGame)
            //{
            //    procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, (int)SceneId.Main);
            //    procedureOwner.SetData<VarInt>(Constant.ProcedureData.GameMode, (int)GameMode.Survival);
            //    ChangeState<ProcedureChangeScene>(procedureOwner);
            //}
        }


    }
}
