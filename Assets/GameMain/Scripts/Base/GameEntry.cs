using UnityEngine;
using System;

namespace StarForce
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            setResolution();
            InitBuiltinComponents();
            InitCustomComponents();
        }

        // 设计分辨率
        private const float MAX_WIDTH = 1366.0f;
        private const float MAX_HEIGHT = 768.0f;
        private void setResolution()
        {
            Resolution[] resolution = Screen.resolutions;
            float standard_width, standard_height;
            if (resolution.Length > 0)
            {
                standard_width = resolution[resolution.Length - 1].width;
                standard_height = resolution[resolution.Length - 1].height;
            }
            else
            {
                standard_width = Screen.width;
                standard_height = Screen.height;
            }
            Debug.LogFormat("origin resolution {0} x {1}", standard_width, standard_height);
            //if (standard_width > MAX_WIDTH)
            //{
            //    float scale = MAX_WIDTH / standard_width;
            //    int height = (int)Math.Ceiling(standard_height * scale);
            //    int width = (int)Math.Ceiling(MAX_WIDTH);
            //    Screen.SetResolution(width, height, false);

            //    Debug.LogFormat("set resolution {0} x {1}  scale {2}", width, height, scale);

            if (standard_width > MAX_WIDTH)
            {
                float scale = MAX_WIDTH / standard_width;
                int height = (int)Math.Ceiling(standard_height * scale);
                int width = (int)Math.Ceiling(MAX_WIDTH);
                Screen.SetResolution(width, height, false);

                Debug.LogFormat("set resolution {0} x {1}  scale {2}", width, height, scale);
            }
            //}

            Resolution currReslution = Screen.currentResolution;
            Debug.LogFormat("curr resolution {0} x {1}  {2} x {3}", Screen.width, Screen.height, currReslution.width, currReslution.height);

        }
    }
}
