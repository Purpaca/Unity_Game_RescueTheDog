using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// 只能存在一个实例对象的MonoBehaviour基类
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T m_instance;

        protected static T instance
        {
            get => m_instance;
        }

        #region Unity Messages
        protected virtual void Awake()
        {
            if (m_instance is not null)
            {
                Destroy(this);
                return;
            }

            m_instance = (T)this;
        }
        #endregion
    }
}