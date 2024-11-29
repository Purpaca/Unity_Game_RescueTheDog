using System.Collections.Generic;
using UnityEngine;

namespace UIManagement
{
    /// <summary>
    /// 受屏幕安全区影响的UI面板
    /// </summary>
    [RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
    public class SafeAreaAdaptor : MonoBehaviour
    {
        [SerializeField]
        private List<RectTransform> m_adaptiveRects;
        private Canvas canvas;

        #region Public 方法
        /// <summary>
        /// 使目标矩形适配安全区
        /// </summary>
        public void Adapt()
        {
            Vector2 offsetMin = GetOffsetMin();
            Vector2 offsetMax = GetOffsetMax();

            foreach (var rect in m_adaptiveRects)
            {
                rect.offsetMin = offsetMin;
                rect.offsetMax = offsetMax;
            }
        }

        /// <summary>
        /// 将一个矩形托管以使其适配安全区
        /// </summary>
        public void AddRectToAdapt(RectTransform rect) 
        {
            if (!m_adaptiveRects.Contains(rect)) 
            {
                return;
            }

            InitalizeRect(rect);
            rect.offsetMin = GetOffsetMin();
            rect.offsetMax = GetOffsetMax();

            m_adaptiveRects.Add(rect);
        }

        /// <summary>
        /// 将一个矩形取消托管以使其不再适配安全区
        /// </summary>
        public void RemoveRectFromAdapt(RectTransform rect) 
        {
            if (!m_adaptiveRects.Contains(rect))
            {
                return;
            }

            m_adaptiveRects.Remove(rect);
        }
        #endregion

        #region Private 方法
        /// <summary>
        /// 获得适配至安全区的最小偏移值
        /// </summary>
        private Vector2 GetOffsetMin() 
        {
            Vector2 offsetMin = new Vector2();
            offsetMin.x = -canvas.GetComponent<RectTransform>().rect.width / 2 * UserSettings.Instance.SafeAreaOffsetLeft;
            offsetMin.y = -canvas.GetComponent<RectTransform>().rect.height / 2 * UserSettings.Instance.SafeAreaOffsetDown;

            return offsetMin;
        }

        /// <summary>
        /// 获得适配至安全区的最大偏移值
        /// </summary>
        private Vector2 GetOffsetMax()
        {
            Vector2 offsetMax = new Vector2();
            offsetMax.x = canvas.GetComponent<RectTransform>().rect.width / 2 * UserSettings.Instance.SafeAreaOffsetRight;
            offsetMax.y = canvas.GetComponent<RectTransform>().rect.height / 2 * UserSettings.Instance.SafeAreaOffsetUp;

            return offsetMax;
        }

        /// <summary>
        /// 初始化矩形的锚点、轴心以及位置信息
        /// </summary>
        private void InitalizeRect(RectTransform rect)
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.localPosition = Vector3.zero;
        }
        #endregion

        #region Unity 消息
        private void Awake()
        {
            canvas = GetComponent<Canvas>();

            foreach(var rect in m_adaptiveRects) 
            {
                InitalizeRect(rect);
            }
        }

        protected virtual void OnEnable()
        {
            Adapt();
        }
        #endregion
    }
}