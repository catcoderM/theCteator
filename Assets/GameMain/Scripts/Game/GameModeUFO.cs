using GameFramework.DataTable;
using UnityEngine;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework;

namespace StarForce
{
    public class GameModeUFO : GameBase
    {
        public int maxNum = 7;
        private float m_ElapseSeconds = 0f;

        //操作间隔
        public float optWaitTime = 1f;



        public GameModeUFO()
        {

          

        }

      

        public override GameMode GameMode
        {
            get
            {
                return GameMode.UFO;
            }
        }

       

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
            if (m_ElapseSeconds >= optWaitTime)
            {
                m_ElapseSeconds = 0f;

                //主要逻辑
                GameMain.GetInstance().GenerateMonstor();
            }
        }

       
     
    }
}
