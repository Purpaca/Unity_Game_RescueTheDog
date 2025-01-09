using UnityEngine;

/// <summary>
/// 能够杀死狗头的物体
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class DogKillingCapable : MonoBehaviour
{
    private bool _hasTriggeredThisFrame = false;

    /// <summary>
    /// 这个能够杀死狗头的物体是否为蜂类？
    /// </summary>
    protected abstract bool IsBeeKilling { get; }

    /// <summary>
    /// 当杀死狗头时触发的回调方法
    /// </summary>
    protected virtual void OnKillingDog() { }

    #region Unity 消息
    protected virtual void LateUpdate()
    {
        _hasTriggeredThisFrame = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasTriggeredThisFrame) 
        {
            DogController dog = collision.gameObject.GetComponent<DogController>();
            if (dog != null && dog.IsAlive)
            {
                dog.Kill(IsBeeKilling);
                OnKillingDog();
                _hasTriggeredThisFrame = true;
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (!_hasTriggeredThisFrame)
        {
            DogController dog = other.GetComponent<DogController>();
            if (dog != null && dog.IsAlive)
            {
                dog.Kill(IsBeeKilling);
                OnKillingDog();
                _hasTriggeredThisFrame = true;
            }
        }
    }
    #endregion
}