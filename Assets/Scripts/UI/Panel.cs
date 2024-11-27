using UnityEngine;

namespace UIManagement
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Panel : MonoBehaviour
    {
        Canvas _canvas;

        #region  Ù–‘
        public Canvas canvas 
        {
            get 
            {
                if(_canvas == null)
                {
                    _canvas = GetComponent<Canvas>();
                }

                return _canvas;
            }
        }
        #endregion

        protected virtual void Awake()
        {
        }
    }
}