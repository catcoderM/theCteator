using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StarForce
{
    public class ComposedMon
    {
        //Dictionary<int, ComposedMon> myDictionary = new Dictionary<int, ComposedMon>(); 

        public List<SimpleName> currMonstors = null;
        //已经合成到数据
        public ComposedMon()
        {
            currMonstors = new List<SimpleName>();
        }

        public void AddCompose(SimpleName data)
        {
            for (int i = 0; i < currMonstors.Count; i++)
            {
                if (currMonstors[i].hasAdd(data.comMon))
                {
                    return;
                }
            }
            currMonstors.Add(data);
        }


//        f(myDictionary.ContainsKey(1))
//{
//Console.WriteLine("Key:{0},Value:{1}","1", myDictionary[1]);
 //}
    }

}
