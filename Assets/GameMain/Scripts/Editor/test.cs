using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEditor;

public class test : MonoBehaviour
{


    [MenuItem("Tools/CsvToLua")]
    public static void csvToLua()
    {
        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + "/Csv/");
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
      
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(fs, Encoding.UTF8);

        //存储文件 存储到CsvToLua中
        string outurl = Application.dataPath + "/" + "Synthesis" + ".txt";
        Debug.Log(outurl);
        FileStream csvToLua = new FileStream(outurl, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(csvToLua);

        //以文件作为表名
        //sw.WriteLine(Path.GetFileNameWithoutExtension(info.Name) + " {");
        sw.WriteLine("#id\tendId");

        string[] title;

        List<string> lineArray = new List<string>();

        while (sr.Peek() != -1)
        {
            lineArray.Add(sr.ReadLine());
        }
        Debug.Log(" cuowu " + lineArray.Count);
        //读取第一行属性
        string[] datas = lineArray[0].ToString().Split(',');
        title = new string[datas.Length];

        for (int k = 0; k < datas.Length; k++)
        {
            title[k] = datas[k];
        }

        //约定 1_1 ->2
        //所有到信息
        //List<string,string> allMon = new List<string,string>();
        //行数
        int vNum = title.Length - 1;
        string id_id;
        for (int i = 1; i < lineArray.Count; i++)
        {
            datas = lineArray[i].ToString().Split(',');
            string h_id = datas[0];
            if (h_id == "")
            {
                continue;
            }
            //从第二个开始
            for (int j = 1; j < vNum; j++)
            {
                id_id = h_id + "_" + title[j];
                string tid = datas[j];
                if (tid == "")
                {
                    continue;
                }
               
                sw.WriteLine(id_id + "\t" + tid );
            }
        }

        sw.Close();


        Debug.Log("tran end");
    }
    [MenuItem("Tools/changeFormat")]
    public static void changeFormat()
    {
        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + "/Csv/");
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        StreamReader sr = new StreamReader(fs, Encoding.UTF8);

        //存储文件 存储到CsvToLua中
        string outurl = Application.dataPath + "/" + "Monster" + ".txt";
        Debug.Log(outurl);
        FileStream csvToLua = new FileStream(outurl, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw = new StreamWriter(csvToLua);


        List<string> lineArray = new List<string>();

        while (sr.Peek() != -1)
        {
            lineArray.Add(sr.ReadLine());
        }
        for (int i = 0; i < lineArray.Count; i++)
        {
            sw.WriteLine(lineArray[i]);

        }
        sw.Close();
        Debug.Log("tran end");
    }
}
