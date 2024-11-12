using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// 可以存在一个实例对象，但只有一个实例对象能处于激活状态的MonoBehaviour基类
    /// </summary>
    public abstract class SingleActiveMonoBehaviour<T> : MonoBehaviour where T : SingleActiveMonoBehaviour<T>
    {
        private static T m_activated;

        #region Protected Methods
        /// <summary>
        /// 当前处于激活状态的实例对象
        /// </summary>
        protected static T Activated
        {
            get => m_activated;
        }

        /// <summary>
        /// 激活指定的实例对象，先前激活的对象实例会被失活
        /// </summary>
        /// <param name="instance">实例对象</param>
        protected static void Activate(T instance)
        {
            if (object.ReferenceEquals(m_activated, instance))
            {
                return;
            }

            m_activated.enabled = false;
            m_activated = instance;
            instance.enabled = true;
        }
        #endregion

        #region Unity Messages
        protected virtual void OnEnable()
        {
            if (m_activated != null && !object.ReferenceEquals(m_activated, this))
            {
                this.enabled = false;
            }
            else
            {
                m_activated = (T)this;
            }
        }

        protected virtual void OnDisable()
        {
            if (object.ReferenceEquals(m_activated, this))
            {
                m_activated = null;
            }
        }
        #endregion
    }
}