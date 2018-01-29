using UnityEngine;
using System.Collections;
using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine.UI;
namespace StarForce
{
    public class MonstorSpr
    {

        public GameObject monBase;
        private Image img;
        public GameObject go;
        public DRMonster data;
        public bool used = false;
        public float w;

        public int leftTime = 9;



        private Color grayCol = new Color(1, 1, 1, 0.6f);
        private Color comCol = Color.white;
        public MonstorSpr(GameObject g)
        {
            this.monBase = g;
            this.go = g.transform.Find("item").gameObject;
            img = go.GetComponent<Image>();
        }

        public void SetData(DRMonster d){
            this.data = d;
            this.used = true;
            leftTime = 0;
            monBase.SetActive(true);

            monBase.transform.localEulerAngles = new Vector3(0, 0, -90);
            w = -0.4f;
            moveFlag = false;
            //Debug.Log("显示");
            go.transform.Find("Text").gameObject.GetComponent<Text>().text = data.BName;
            string imgUrl = AssetUtility.GetUIImageAssets(data.asset);
            GameEntry.Resource.LoadAsset(imgUrl, new LoadAssetCallbacks((assetName, asset, duration, userData) =>
            {
                //Log.Info("Load font '{0}' OK.{1}", imgUrl,asset);
                //Image img = go.GetComponent<Image>();
                //创建Sprite
                Texture2D texture = (Texture2D)asset;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                img.sprite = sprite;
              
            },

                (assetName, status, errorMessage, userData) =>
                {
                Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", data.asset, assetName, errorMessage);
                }));
        }

        public void destory()
        {
            this.used = false;
            monBase.SetActive(false);
           
        }

        public void SetActive(bool value)
        {
            monBase.SetActive(value);
        }

        public void Tick()
        {
            leftTime = leftTime + 1;
            if (IsCurrent())
            {
                img.color = comCol;
            }else{
                img.color = grayCol;
            }
        }
        public bool IsCurrent()
        {
            return leftTime == 5 && moveFlag == false;
        }

        public bool moveFlag;

        public void SetTransInfo(TransformAndPos t)
        {
            this.monBase.transform.localScale = t.scale;
            this.monBase.transform.localPosition = t.position;
        }

    }
}
    

