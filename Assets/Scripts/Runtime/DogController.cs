using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 狗头行为控制器
/// </summary>
[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class DogController : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;

    private UnityAction m_onDogeInjured;
    private bool _isDead = false;
    private string _previousAnimationState = Enum.GetName(typeof(DogAnimationState),DogAnimationState.Idle);

    public bool IsAlive { get => !_isDead; }

    #region Public 方法
    /// <summary>
    /// 添加当狗狗受伤时的回调方法
    /// </summary>
    public void AddOnDogeInjuredListener(UnityAction callback)
    {
        m_onDogeInjured += callback;
    }

    /// <summary>
    /// 移除当狗狗受伤时的回调方法
    /// </summary>
    public void RemoveOnDogeInjuredListener(UnityAction callback)
    {
        m_onDogeInjured -= callback;
    }

    /// <summary>
    /// 清除当狗狗受伤时的回调方法
    /// </summary>
    public void ClearOnDogeInjuredListener()
    {
        m_onDogeInjured = null;
    }

    /// <summary>
    /// 设置是否开启狗狗的物理模拟
    /// </summary>
    /// <param name="enabled">是否开启？</param>
    public void SetDogeSimulated(bool enabled)
    {
        if (m_rigidbody2D == null)
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        m_rigidbody2D.simulated = enabled;
    }

    /// <summary>
    /// 播放狗头指定状态下的动画
    /// </summary>
    ///<param name="state">狗头要播放的动画状态</param>
    public void PlayAnimation(DogAnimationState state)
    {
        if (m_animator == null || !Enum.IsDefined(typeof(DogAnimationState), state))
        {
            return;
        }

        // 确保即使AnyState到动画状态的过渡被设置为不可过渡到自身，上一次的动画播放Trigger也能复位
        m_animator.ResetTrigger(_previousAnimationState);
        string newState = Enum.GetName(typeof(DogAnimationState), state);
        m_animator.SetTrigger(newState);
        _previousAnimationState = newState;
    }

    /// <summary>
    /// 杀死狗头
    /// </summary>
    /// <param name="isKilledByBee">是否为蜜蜂击杀？</param>
    public void Kill(bool isKilledByBee) 
    {
        if (_isDead) 
        {
            return;
        }

        _isDead = true;
        DogAnimationState state = isKilledByBee ? DogAnimationState.DamagedCrying : DogAnimationState.Crying;
        PlayAnimation(state);
        m_onDogeInjured?.Invoke();
    }

    /// <summary>
    /// 复活狗头
    /// </summary>
    public void Revive() 
    {
        _isDead = false;
        PlayAnimation(DogAnimationState.Idle);
    }
    #endregion

    #region Unity 消息
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();

        _isDead = false;
        PlayAnimation(DogAnimationState.Idle);
    }
    #endregion
}