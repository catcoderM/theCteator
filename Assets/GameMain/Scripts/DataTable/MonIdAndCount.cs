using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonIdAndCounts
{
    public Dictionary<int, int> synGens;
    public MonIdAndCounts()
    {
        synGens = new Dictionary<int, int>();
    }

    public void AddSynFun(int s, int f)
    {
        synGens.Add(s,f);
    }

    public int Count()
    {
        return synGens.Count;
    }
}
