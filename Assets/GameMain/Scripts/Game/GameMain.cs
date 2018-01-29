using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using System;
using GameFramework.DataTable;

namespace StarForce
{
    public class GameMain
    {

        private int maxNum = 5;
        //当前到怪物表
        protected List<DRMonster> currMonstors = null;

        protected List<DRMonster> genMonstors = null;

        public void InitConfig()
        {
            //获得第一类怪物
            IDataTable<DRMonster> dtMonstor = GameEntry.DataTable.GetDataTable<DRMonster>();
            DRMonster[] list = dtMonstor.GetAllDataRows();
            currMonstors = new List<DRMonster>();
            genMonstors = new List<DRMonster>();
            int lent = list.Length;
            for (int i = 0; i < lent; i++)
            {
                DRMonster dr = list[i];
                if (dr.IsFirst())
                {
                    currMonstors.Add(dr);
                }
            }
        }

        private static GameMain _instance;
        public static GameMain GetInstance()
        {
            if (null == _instance)
            {
                _instance = new GameMain();
            }
            return _instance;
        }
        //生成怪物
        public void GenerateMonstor()
        {
            int l = genMonstors.Count;
            DRMonster monData = generateSingle();

            if (l < maxNum)
            {

            }
            else
            {
                //删除第一个 
                genMonstors.RemoveAt(0);
            }
            genMonstors.Add(monData);

            //显示
            GameEntry.Event.Fire(this, new MonstorCountChange(genMonstors));
        }

        private DRMonster generateSingle()
        {
            int num = currMonstors.Count;
            int r = (int)Mathf.Round(num);
            return currMonstors[r];
        }

    }

}
