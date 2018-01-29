using GameFramework;
using UnityEngine;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine.UI;
using GameFramework.Resource;
using DG.Tweening;

namespace StarForce
{
    public class UFOForm : UGuiForm
    {




        public GameObject endingSprImg;
        private Vector3 firTrans;
        private Vector3 secTrans;


        public GameObject monsSpr;
        public float rotateSpeed = 0.06f;
        public float rotateTime = 0.7f;

        private bool m_start = false;

        public Text tipTxt;

        private float speed;
        [SerializeField]
        private GameObject eacrhImg = null;

        private bool pausebg = false;
        private DRMonster first;
        private DRMonster second;
        private float m_ElapseSeconds = 0f;

        private float m_monElapseSd = 0f;

        //操作间隔
        public float opt_monWait = 2f;

        //操作间隔
        public float optWaitTime = 1f;
        [SerializeField]
        private GameObject ufoMpoveImg = null;

        [SerializeField]
        private GameObject effectImg = null;

        [SerializeField]
        private GameObject firstGO = null;
        [SerializeField]
        private GameObject secondGO = null;
        [SerializeField]
        private GameObject finGO = null;


        private int currIndex;

        public GameObject[] goMons = null;
        //private 

        //当前位置
        private Vector3 curPos;
        //是否向上
        private bool upFlag;

        private float lastTime;

        private float tickTime;


        private int maxNum = 9;

        //当前到怪物表
        protected List<DRMonster> initMonstors = null;
        //当前到怪物表
        protected List<DRMonster> currMonstors = null;

        //已经全部合成的表
        protected Dictionary<int, bool> allGenWithDelet = new Dictionary<int, bool>();
        //每个怪物已经完成的表
        Dictionary<int, ComposedMon> myDictionary = new Dictionary<int, ComposedMon>();

        protected List<MonstorSpr> genMonstors = null;



        private MonstorSpr[] monPool;
        private TransformAndPos[] transAndPoss;

        public GameObject Buttons;

        private GameObject startBtn;
        private GameObject pauseBtn;
        private GameObject quitBtn;

        public TableView tableView;

        private DRMonster defaultMon;


        [SerializeField]
        private GameObject lImg;
        [SerializeField]
        private GameObject rImg;
        [SerializeField]
        private GameObject eimg;

        //
        [SerializeField]
        private int maxHp;
        [SerializeField]
        private int failSub;

        [SerializeField]
        private int sucAdd;
        [SerializeField]
        private int sucAddOnce;
        [SerializeField]
        private Slider sld;

        private int curHp;

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            InitConfig();
            upFlag = true;
            lastTime = 0;
            currIndex = 0;
            tickTime = 0;
            curPos = ufoMpoveImg.transform.localPosition;

            int ll = goMons.Length;
            monPool = new MonstorSpr[ll];
            transAndPoss = new TransformAndPos[ll];
            for (int i = 0; i < ll; i++)
            {
                GameObject go = goMons[i];
                monPool[i] = new MonstorSpr(go);
                transAndPoss[i] = new TransformAndPos(go.transform.localPosition, go.transform.localScale);
            }

            tableView.InitTableView(null, 0, updateItem);

            startBtn = Buttons.transform.Find("Start").gameObject;
            pauseBtn = Buttons.transform.Find("pause").gameObject;
            quitBtn = Buttons.transform.Find("quit").gameObject;

            //monsSpr = this.transform.Find("monsSpr").gameObject;
            defaultMon = new DRMonster();
            defaultMon.asset = "none";
            defaultMon.BName = "未知";

            sld.maxValue = maxHp;
            sld.value = maxHp;
            lastHp = curHp;

            firTrans = firstGO.transform.localPosition;
            secTrans = secondGO.transform.localPosition;
            this.showStartButton();
        }


        private void updateItem(GameObject go, int index)
        {
            SimpleName spd = null;
            //需要优化
            if (myDictionary.ContainsKey(first.Id) && myDictionary[first.Id].currMonstors.Count > index)
            {
                spd = myDictionary[first.Id].currMonstors[index];
            }
            //Transform ts = go.transform.Find("Text");
            Debug.Log("显示合成" + index);
            //if(ts != null){
            //    string info = first.BName + " + " + spd.comMon.BName + " = " + spd.GetFinalName();
            //    Debug.Log(info);
            //    ts.gameObject.GetComponent<Text>().text = info;
            //}
            setMonInfo(go.transform.Find("first").gameObject, first);

            if (spd != null)
            {
                setMonInfo(go.transform.Find("second").gameObject, spd.comMon);
                setMonInfo(go.transform.Find("result").gameObject, spd.toMon);
            }
            else
            {

                setMonInfo(go.transform.Find("second").gameObject, defaultMon);
                setMonInfo(go.transform.Find("result").gameObject, defaultMon);
            }

        }
        private void setMonInfo(GameObject go, DRMonster data)
        {
            GameObject ts = go.transform.Find("Text").gameObject;
            GameObject img = go.transform.Find("Image").gameObject;
            string asset = "wrong";
            if (data != null)
            {
                string info = data.BName;
                ts.GetComponent<Text>().text = info;
                asset = data.asset;

            }
            else
            {
                ts.GetComponent<Text>().text = "失败";

            }
            LoadAndSetImage(asset, img.GetComponent<Image>());

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {

            base.OnClose(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            this.roateEarch();
            //sld.value = Mathf.Lerp(curHp, lastHp, Time.time);
            if (m_start == false)
            {
                return;
            }
            lastTime += realElapseSeconds;
            if (lastTime > 1)
            {
                upFlag = !upFlag;
                lastTime = 0;
            }
            if (upFlag)
            {
                curPos.y += 1;
            }
            else
            {
                curPos.y -= 1;
            }
            ufoMpoveImg.transform.localPosition = curPos;

            //base.OnUpdate(elapseSecond, realElapseSeconds);
            tickTime += realElapseSeconds;





            //玩家操作
            m_ElapseSeconds += realElapseSeconds;

            if (pausebg)
            {
                //m_monElapseSd += realElapseSeconds/2;

            }
            else
            {
                //m_monElapseSd += realElapseSeconds;

                if (m_ElapseSeconds >= optWaitTime)
                {
                    //Log.Debug("进入操作判断" + m_ElapseSeconds);
                    m_ElapseSeconds = 0f;

                    //主要逻辑
                    //1.玩家操作
                    playerTouchOpt();
                }

            }

            this.dealWithMons(realElapseSeconds);


            //移动到上面
            if (currClone != null)
            {
                Vector3 scale = currClone.transform.localScale;
                float tmp = scale.x - 0.01f;
                tmp = tmp > 0.3f ? tmp : 0.3f;
                scale.x = scale.y = tmp;
                currClone.transform.localScale = scale;
                Vector3 pos = currClone.transform.localPosition;
                pos.y += 400 * elapseSeconds;

                if (pos.x > 0)
                {
                    pos.x -= 10;
                    pos.x = pos.x < 0 ? 0 : pos.x;
                }
                else if (pos.x < 0)
                {
                    pos.x += 10;
                    pos.x = pos.x > 0 ? 0 : pos.x;
                }
                if (pos.y > 800)
                {

                    Debug.Log("删除clone");
                    Image img;
                    if (currIndex == 0)
                    {
                        currIndex = 1;
                        img = firstGO.GetComponent<Image>();
                        firstGO.transform.Find("Text").gameObject.GetComponent<Text>().text = first.BName;
                        lImg.SetActive(true);
                        lImg.transform.Find("infoTxt").gameObject.GetComponent<Text>().text = first.intro;
                        pausebg = false;
                    }
                    else
                    {
                        currIndex = 0;
                        img = secondGO.GetComponent<Image>();
                        secondGO.transform.Find("Text").gameObject.GetComponent<Text>().text = second.BName;
                        rImg.SetActive(true);
                        rImg.transform.Find("infoTxt").gameObject.GetComponent<Text>().text = second.intro;
                        doMoveToCompose();
                    }

                    img.gameObject.SetActive(true);
                    img.sprite = currClone.GetComponent<Image>().sprite;
                    Destroy(currClone);
                    currClone = null;


                }
                else
                {
                    //Debug.Log("移动clone");
                    currClone.transform.localPosition = pos;
                }
            }
        }

        private void doMoveToCompose()
        {
            //a + b = c效果
            Invoke("doComposeSetp1", 0.1f);


        }
        private void doComposeSetp1()
        {
            GameEntry.Sound.PlaySound(7);

            //Invoke("doCompose", 0.2f);
            //doCompose();
            Vector3 tarpos = finGO.transform.localPosition;
            tarpos.x = tarpos.x - 50;
            firstGO.transform.DOLocalMove(tarpos, 0.2f, false).onComplete = delegate
            {
                firstGO.transform.localPosition = firTrans;
            };
            tarpos.x = tarpos.x + 100;
            secondGO.transform.DOLocalMove(tarpos, 0.2f, false).onComplete = delegate
            {
                secondGO.transform.localPosition = secTrans;
                doCompose();
            };
        }
        private void doCompose()
        {

            pausebg = false;
            if (first == null || second == null)
            {
                Debug.LogError(string.Format("存在空 {0}  {1}", first == null, second == null));
                return;
            }
            Debug.Log("开始合成 .. " + first.Id + "   " + second.Id);

            string cid = first.Id + "_" + second.Id;
            IDataTable<DRSynthesis> dtScene = GameEntry.DataTable.GetDataTable<DRSynthesis>();
            DRSynthesis[] list = dtScene.GetAllDataRows();
            DRSynthesis drScene = null;
            int lent = list.Length;
            for (int i = 0; i < lent; i++)
            {
                DRSynthesis dr = list[i];
                if (dr.Id_Id.Equals(cid))
                {
                    drScene = dr;
                    break;
                }
            }


            if (drScene != null)
            {
                drScene.CanCompose();
                string tid = drScene.cId;
                Debug.Log("合成id" + drScene.cId);
                if (tid != null && tid != "")
                {
                    //查找表
                    IDataTable<DRMonster> dtmon = GameEntry.DataTable.GetDataTable<DRMonster>();
                    DRMonster drmon = dtmon.GetDataRow(int.Parse(tid));
                    if (drmon != null)
                    {
                        DRMonster dmon = currMonstors.Find((DRMonster obj) => drmon.Id == obj.Id);
                        if (dmon == null)
                        {
                            Debug.Log("添加新物种" + drmon.Id + "  " + drmon.asset);

                            addToGenList(drmon, first, second);
                            if (drmon.Id != 200 && drmon.Id != 100)
                            {
                                setHp(sucAddOnce);
                            }
                            else if (drmon.Id == 200)
                            {
                                //setHp(failSub);
                            }

                        }
                        else
                        {
                            Debug.Log("已有物种" + drmon.Id + "  " + drmon.asset);
                            if (drmon.Id != 200 && drmon.Id != 100)
                            {
                                setHp(sucAdd);
                            }
                            else if (drmon.Id == 200)
                            {
                                //setHp(failSub);
                            }
                        }

                        //显示合成
                        //音效
                        GameEntry.Sound.PlaySound(drmon.soundId);

                        showCompose(drmon, first, second);
                        //显示提示
                        tipTxt.text = drmon.intro;
                        eimg.SetActive(true);
                        eimg.transform.Find("infoTxt").gameObject.GetComponent<Text>().text = drmon.intro;
                        Invoke("hideTxt", 0.8f);
                        //drmon.endValue = 1000;
                        if (drmon.endValue >= 1000)
                        {
                            //结束
                            m_start = false;
                            doEndStory(drmon);
                        }
                    }
                    else
                    {
                        //todo  配置
                        //是不是平局
                        //if (tid.Equals("100"))
                        //{
                        //    //音效
                        //    GameEntry.Sound.PlaySound(5);
                        //    clearShow(false,"ashes","灰烬");
                        //}else if (tid.Equals("200"))
                        //{
                        //    clearShow(true,"wrong","残渣");
                        //}else
                        //{
                        clearShow(true);
                        //}


                    }
                }
                else
                {
                    clearShow(true);
                }
            }
            else
            {
                clearShow(true);

            }
        }
        private void hideTxt()
        {
            eimg.SetActive(false);
            tipTxt.text = "";
        }

        private void clearShow(bool des = false)
        {
            if (des)
            {
                //音效
                GameEntry.Sound.PlaySound(5);
                showCompose(null, first, second);
                setHp(failSub);
            }
            first = null;
            second = null;
            firstGO.gameObject.SetActive(false);
            secondGO.gameObject.SetActive(false);
            lImg.SetActive(false);
            rImg.SetActive(false);
            //finGO.gameObject.SetActive(false);
            tableView.gameObject.SetActive(false);
        }


        //添加到怪物->生成 表
        private void addToGenList(DRMonster data, DRMonster fir, DRMonster sec)
        {
            if (data.Id != 100 && data.Id != 200)
            {
                currMonstors.Add(data);
                if (myDictionary.ContainsKey(fir.Id))
                {
                    myDictionary[fir.Id].AddCompose(new SimpleName(sec, data));
                }
                else
                {
                    ComposedMon cmon = new ComposedMon();
                    myDictionary.Add(fir.Id, cmon);
                    cmon.AddCompose(new SimpleName(sec, data));


                }
                if (myDictionary.ContainsKey(sec.Id))
                {
                    myDictionary[fir.Id].AddCompose(new SimpleName(sec, data));
                }
                else
                {
                    ComposedMon cmon = new ComposedMon();
                    myDictionary.Add(sec.Id, cmon);
                    cmon.AddCompose(new SimpleName(fir, data));


                }

                //完全合成之后不再显示这怪物
                if ((myDictionary[fir.Id].currMonstors.Count >= DRSynthesis.allDictionary[fir.Id].Count()))
                {
                    delMonWithId(fir);
                }
                if (fir.Id != sec.Id)
                {
                    if ((myDictionary[sec.Id].currMonstors.Count >= DRSynthesis.allDictionary[sec.Id].Count()))
                    {
                        delMonWithId(sec);
                    }
                }

            }

        }
        private void delMonWithId(DRMonster data)
        {
            allGenWithDelet.Add(data.Id, true);
            Debug.Log("删除" + data.Id);
            currMonstors.Remove(data);
        }

        private void showCompose(DRMonster data, DRMonster fir, DRMonster sec)
        {

            string asset = "wrong";
            string BName = "失败";
            if (data != null)
            {
                asset = data.asset;
                BName = data.BName;


            }

            string imgUrl = AssetUtility.GetUIImageAssets(asset);
            GameEntry.Resource.LoadAsset(imgUrl, new LoadAssetCallbacks((assetName, assete, duration, userData) =>
            {
                //Log.Info("Load font '{0}' OK.{1}", imgUrl,asset);
                Image img = finGO.GetComponent<Image>();
                //创建Sprite
                Texture2D texture = (Texture2D)assete;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;

                finGO.transform.Find("Text").gameObject.GetComponent<Text>().text = BName;

                finGO.SetActive(true);
                Invoke("hideFinGo", 1.2f);


                clearShow();

            },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", asset, assetName, errorMessage);
                }));
        }


        private void hideFinGo()
        {
            finGO.SetActive(false);
        }



        private void playerTouchOpt()
        {
        }
        //private int clickCount = 0;
        public void onMouseClick()
        {
            if (pausebg || m_start == false)
            {
                return;
            }
            for (int i = 0; i < genMonstors.Count; i++)
            {
                MonstorSpr m = genMonstors[i];
                if (m.IsCurrent())
                {
                    m.moveFlag = true;
                    doClone(m);

                    break;
                }

            }
        }


        private GameObject currClone;
        private void doClone(MonstorSpr m)
        {

            pausebg = true;
            if (currClone != null)
            {
                Destroy(currClone);
                Debug.Log("销毁生成的");
            }
            Debug.Log("生成clone  " + Time.time);
            GameObject go = m.go;
            if (currIndex == 0)
            {
                first = m.data;
                //GameEntry.Event.Fire(this, new ChooseMonstorEvent(first));
                this.showFirstComList();
            }
            else
            {
                second = m.data;
            }
            //音效
            GameEntry.Sound.PlaySound(1);
            //特效
            doEffect();
            currClone = Instantiate(go) as GameObject;
            currClone.transform.parent = monsSpr.transform;
            currClone.transform.localPosition = go.transform.localPosition;
            m.SetActive(false);
        }
        private void doEffect()
        {
            effectImg.SetActive(true);
            //entityComponent.ShowEntity(typeof(MyAircraft), "Aircraft", data);

            Invoke("hideEffect", 0.6f);
        }
        private void hideEffect()
        {
            effectImg.SetActive(false);
        }





        //上次生成的id
        private int lastGenId = 0;
        private Dictionary<int, int> weightList;
        private MonstorSpr generateSingle()
        {
            int num = currMonstors.Count;
            DRMonster mon = null;
            int tryCount = 0;
            //是否有选中
            if (first != null && myDictionary.ContainsKey(first.Id))
            {
                //增加可合成几率
                //可合成的材料
                int sNum = myDictionary[first.Id].currMonstors.Count;
                int allNum = sNum + num;
                int t = Mathf.FloorToInt(Random.Range(0, allNum));
                int r = (int)t;
                //默认逻辑
                while (tryCount < 9)
                {

                    if (r >= num)
                    {
                        mon = myDictionary[first.Id].currMonstors[r - num].comMon;
                    }
                    else
                    {
                        mon = currMonstors[r];
                        //Debug.Log("使用monstor" + (mon ==null));
                    }
                    Debug.Log(num + " 随机数生成 " + r);
                    if (mon != null && mon.Id != lastGenId)
                    {
                        break;
                    }
                    //伪随机
                    r++;
                    r = r > allNum - 1 ? 0 : r;
                    tryCount++;
                }
            }
            else
            {

                int t = Mathf.FloorToInt(Random.Range(0, num));
                int r = (int)t;
                //默认逻辑
                while (tryCount < 9)
                {

                    Debug.Log(num + " 随机数 " + r);
                    mon = currMonstors[r];
                    //Debug.Log("使用monstor" + (mon == null));
                    if (mon != null && mon.Id != lastGenId)
                    {
                        break;
                    }
                    //伪随机
                    r++;
                    r = r > num - 1 ? 0 : r;
                    tryCount++;
                }

            }
            lastGenId = mon.Id;


            for (int i = 0; i < goMons.Length; i++)
            {
                if (monPool[i].used == false)
                {
                    monPool[i].SetData(mon);
                    return monPool[i];
                }
            }
            return null;
        }


        private void desMon(MonstorSpr tmp)
        {
            tmp.destory();

        }



        public void InitConfig()
        {
            //获得第一类怪物
            IDataTable<DRMonster> dtMonstor = GameEntry.DataTable.GetDataTable<DRMonster>();
            DRMonster[] list = dtMonstor.GetAllDataRows();
            currMonstors = new List<DRMonster>();
            genMonstors = new List<MonstorSpr>();
            int lent = list.Length;
            for (int i = 0; i < lent; i++)
            {
                DRMonster dr = list[i];
                if (dr.IsFirst())
                {
                    currMonstors.Add(dr);
                }
            }
            initMonstors = currMonstors;


            Debug.Log("初始话怪物表结束");
        }


        private void showFirstComList()
        {
            if (first != null)
            {
                if (DRSynthesis.allDictionary.ContainsKey(first.Id))
                //if (myDictionary.ContainsKey(first.Id))
                {
                    tableView.gameObject.SetActive(true);

                    tableView.ReLoad(DRSynthesis.allDictionary[first.Id].Count());//myDictionary[first.Id].currMonstors.Count);
                }
                else
                {
                    tableView.gameObject.SetActive(false);
                }
            }
            else
            {
                tableView.gameObject.SetActive(false);
            }

        }


        //-----------
        Vector3 rotSpeed = new Vector3(0, 0, 90);
        //float timer = 0;
        //float earthStopTime = 1;
        //int step = 60;
        //int cur_step = 30;
        //float minEarthRotationTime = 0.4f;

        private void roateEarch()
        {
            Vector3 rot = rotSpeed * Time.deltaTime;
            rot.z = (int)rot.z;
            this.eacrhImg.gameObject.transform.Rotate(rot / 5);

        }


        //生成怪物
        public void GenerateMonstor()
        {
            int l = genMonstors.Count;
            MonstorSpr monData = generateSingle();
            //monData.go.gameObject.transform.localPosition = new Vector3(337, -228, 0);//new Vector3(230,-93,9);
            if (l >= maxNum)
            {
                //删除第一个 
                desMon(genMonstors[0]);
                genMonstors.RemoveAt(0);
            }

            genMonstors.Add(monData);
        }

        //物体移动
        //生成
        //指定
        private void dealWithMons(float realElapseSeconds)
        {

            tickTime += realElapseSeconds;
            m_monElapseSd += realElapseSeconds;
            if (m_monElapseSd >= opt_monWait)
            {
                //m_monElapseSd应该移动的时间
                m_monElapseSd = 0f;
                Debug.Log("移动一步");
                GenerateMonstor();
                //每个生物tick
                for (int i = 0; i < genMonstors.Count; i++)
                {
                    if (genMonstors[i] != null)
                    {
                        genMonstors[i].Tick();
                    }

                }
                //需要走一步
                tickTime = 0;

                //扣血
                setHp(failSub);



            }
            if (tickTime < rotateTime)
            {
                speed = this.rotateSpeed;
            }
            else
            {
                speed = 0f;
            }
            moveMons(realElapseSeconds);



        }


        public float rotateR = 300; //半径  
        private void moveMons(float elapseSeconds)
        {
            if (genMonstors != null)
            {
                for (int i = 0; i < genMonstors.Count; i++)
                {
                    MonstorSpr go = genMonstors[i];
                    //if (go != null)
                    //{
                    //go.SetTransInfo(transAndPoss[go.leftTime]);
                    //}

                    //-80
                    //
                    Log.Debug(Time.fixedTime);
                    float t = (float)elapseSeconds / 0.03f;
                    Vector3 s = new Vector3(0, 0, speed * t);
                    Vector3 rot = s * Time.deltaTime;
                    go.monBase.transform.Rotate(rot);

                    //Debug.Log("旋转修改");
                    //s.z += speed;
                    //go.monBase.transform.Rotate(s);

                    //GameObject go = genMonstors[i].monBase;


                    //Vector3 pos = go.transform.localPosition;
                    //if (pausebg)
                    //{ 
                    //genMonstors[i].w += (float)0.7 * Time.fixedDeltaTime/2 * speed; //   
                    //pos.x = pos.x - 35 * Time.deltaTime;
                    //}else{
                    //pos.x = pos.x - 70 * Time.deltaTime;
                    //genMonstors[i].w += speed ;//Time.deltaTime * speed; //
                    //}
                    //float f = (-2.0f * i / cnt - 0.5f) * Mathf.PI + 2 * Mathf.PI * selectIndex / cnt;

                    //float x = Mathf.Cos(genMonstors[i].w) * rotateR + 0;
                    //float y = Mathf.Sin(genMonstors[i].w) * rotateR - 289;
                    //Debug.Log("显示 "+ x +"  "+y + " " + genMonstors[i].w + " " + r);
                    //go.transform.localPosition = new Vector3(x, y, 0);
                    //go.transform.localPosition = pos;
                    //if (pos.x < 0)
                    //{
                    //生成新物体
                    //pos
                    //}
                }
            }
        }

        //游戏重新进行 清除上次信息
        private void clearCacheData()
        {
            currMonstors = initMonstors;
            while (genMonstors.Count > 0)
            {
                desMon(genMonstors[0]);
                genMonstors.RemoveAt(0);
            }
            allGenWithDelet = new Dictionary<int, bool>();
            myDictionary = new Dictionary<int, ComposedMon>();

            upFlag = true;
            lastTime = 0;
            currIndex = 0;
            tickTime = 0;

            curHp = maxHp;
            m_start = true;
            sld.maxValue = maxHp;
            sld.value = maxHp;
            lastHp = curHp;
            GameObject endGo = this.gameObject.transform.Find("endTxt").gameObject;
            endGo.GetComponent<Text>().text = "";
            endingSprImg.SetActive(false);
        }

        private void showStartButton()
        {
            startBtn.SetActive(true);
            pauseBtn.SetActive(false);
        }

        public void onClickStart()
        {
            m_start = true;
            startBtn.SetActive(false);
            pauseBtn.SetActive(true);
            clearCacheData();
        }

        public void onClickAbout()
        {
            GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
        }

        public static void LoadAndSetImage(string assetN, Image img)
        {
            string imgUrl = AssetUtility.GetUIImageAssets(assetN);
            GameEntry.Resource.LoadAsset(imgUrl, new LoadAssetCallbacks((assetName, asset, duration, userData) =>
            {
                //Log.Info("Load font '{0}' OK.{1}", imgUrl,asset);
                //创建Sprite
                Texture2D texture = (Texture2D)asset;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;


            },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", assetN, assetName, errorMessage);
                }));

        }


        //结果判断
        private void doEndStory(DRMonster dMon)
        {
            Debug.Log("游戏结束 完成结局");
            GameEntry.Sound.PlaySound(8);
            GameObject endGo = this.gameObject.transform.Find("endTxt").gameObject;
            endGo.GetComponent<Text>().text = getEndStory(dMon.endValue.ToString());

            endingSprImg.SetActive(true);

            showStartButton();
            m_start = false;
        }



        private void failGame()
        {

            Debug.Log("游戏结束");
            GameEntry.Sound.PlaySound(3);
            showStartButton();
            m_start = false;
            GameObject endGo = this.gameObject.transform.Find("endTxt").gameObject;
            endGo.GetComponent<Text>().text = "失败";
        }

        public void onClickPause()
        {
            //this.showStartButton();
            m_start = !m_start;
            if (m_start)
            {
                Time.timeScale = 1;
                GameEntry.Sound.gameObject.SetActive(true);
                GameEntry.Sound.PlayMusic(1);
            }
            else
            {
                Time.timeScale = 0;
                GameEntry.Sound.gameObject.SetActive(false);
            }
            pauseBtn.transform.Find("Border").gameObject.SetActive(m_start);
            pauseBtn.transform.Find("Border2").gameObject.SetActive(!m_start);
        }

        public void onClickQuit()
        {
            Application.Quit();
        }
        private int lastHp = 0;
        private void setHp(int value)
        {
            lastHp = curHp;
            curHp += value;
            Debug.Log("hp 变换" + lastHp + " , " + curHp + " , " + maxHp);
            if (curHp <= 0)
            {
                //
                curHp = 0;
                failGame();

            }
            else
            {
                curHp = curHp > maxHp ? maxHp : curHp;

            }
            sld.value = curHp;//Mathf.Lerp(curHp, lastHp, Time.time);
        }


        private string getEndStory(string id)
        {
            if (id.Equals("1000"))
            {
                return "<color=#ff0000ff><b>文明毁灭</b></color>\n重大新闻：上纪元的女性遗址中发掘出了两本书《爱需要放肆》《爱需要节制》";
            }
            else if (id.Equals("1001"))
            {
                return "<color=#ff0000ff><b>娜迦文明</b></color>\n娜迦族的历史从没像今天一样繁荣过，上次的繁荣要追述到燃烧军团远征前100年了。";
            }
            else if (id.Equals("1002"))
            {
                return "<color=#ff0000ff><b>文明毁灭</b></color>\n如果人可以像锤头鲨一样雌雄同体就不会面临这么纠结的选择了。";
            }
            else if (id.Equals("1003"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n终于，智慧生物的火种在这颗星球上延续了下来，然而，其它生物都被广东人和福建人吃光了。";
            }
            else if (id.Equals("1004"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n墓志铭：我曾经也是一个风流倜傥的流浪诗人，看过天下所有最美的女人的眼睛，直到那天看到了美杜莎⋯⋯";
            }
            else if (id.Equals("1005"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n终于，如果遇到真爱，你就去亲吻他，即使他看起来是一只青蛙——鲁迅";
            }
            else if (id.Equals("1006"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n这个世界的猎人都是弱鸡，哇哈哈哈哈哈！";
            }
            else if (id.Equals("1007"))
            {
                return "<color=#ff0000ff><b>黑暗文明诞生</b></color>\n你接过了他们手里的剑，但身为余灰的你连作为柴薪燃烧的资格都没有。";
            }
            else if (id.Equals("1008"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n与其说为了追求更高的智商而砸破自己的脑袋，还不如做个愚蠢的笨蛋——鱼人";
            }
            else if (id.Equals("1009"))
            {
                return "<color=#ff0000ff><b>文明诞生</b></color>\n⋯⋯汪汪⋯⋯";
            }
            return "";
        }
    }
}
