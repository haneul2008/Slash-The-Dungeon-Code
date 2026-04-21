using System;
using UnityEngine;

public class PlayerAnimatorTrigger : MonoBehaviour
{
    public event Action OnAnimationEndEvent;
    public event Action OnRollingEndEvent;
    public event Action OnAttackEvent;
    public event Action OnSetAttackMovementEvent;

    public void OnAnimationEnd()
    {
        OnAnimationEndEvent?.Invoke();
    }

    public void OnRollingEnd()
    {
        OnRollingEndEvent?.Invoke();
    }

    public void OnAttackTrigger()
    {
        OnAttackEvent?.Invoke();
    }

    public void SetAttackMovement()
    {
        OnSetAttackMovementEvent?.Invoke();
    }
}
