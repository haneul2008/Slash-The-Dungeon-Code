using System;
using System.Collections;
using System.Security.Cryptography;
using HN.Code.Combat;
using HN.Code.Entities;
using HN.Code.EventSystems;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    public event Action OnRollEndEvent;
    public UnityEvent OnRollingFeedback;
    
    [SerializeField] private PlayerAnimatorTrigger animTrigger;
    [SerializeField] private float _dashSpeed = 5f;
    [SerializeField] private StatSO moveSpeedStat, rollCooldownStat;
    [SerializeField] private StatCompo statCompo;
    [SerializeField] private PlayerAttackCompo attackCompo;
    [SerializeField] private GameEventChannelSO playerChannel;
    [SerializeField] private Health health;
    [SerializeField] private float attackForce = 2;
    [SerializeField] private LayerMask whatIsProjectile;

    public bool CanMove { get; set; } = true;
    public bool IsRolling { get; private set; }

    private EntityAnimator _anim;
    private Rigidbody2D _rigid;
    private Vector2 _moveDir;
    private Vector2 _lastDir;
    private Collider2D _collider;
    private float _lastRollTime = -999;
    private float _moveSpeed, _rollCooldown;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<EntityAnimator>();
        _collider = GetComponent<Collider2D>();
        
        animTrigger.OnRollingEndEvent += HandleRollEnd;
        animTrigger.OnAnimationEndEvent += HandleAnimationEnd;
        animTrigger.OnSetAttackMovementEvent += HandleSetAttackMovement;
        attackCompo.OnAttackEvent.AddListener(HandleAttackEvent);
    }

    private void Start()
    {
        StatSO spdStat = statCompo.GetStat(moveSpeedStat);
        StatSO cooldownStat = statCompo.GetStat(rollCooldownStat);

        spdStat.OnValueChanged += HandleMoveSpeedChanged;
        cooldownStat.OnValueChanged += HandleRollCooldownChanged;

        _moveSpeed = spdStat.BaseValue;
        _rollCooldown = cooldownStat.BaseValue;
    }

    private void OnDestroy()
    {
        StatSO spdStat = statCompo.GetStat(moveSpeedStat);
        StatSO cooldownStat = statCompo.GetStat(rollCooldownStat);

        spdStat.OnValueChanged -= HandleMoveSpeedChanged;
        cooldownStat.OnValueChanged -= HandleRollCooldownChanged;

        animTrigger.OnRollingEndEvent -= HandleRollEnd;
        animTrigger.OnAnimationEndEvent -= HandleAnimationEnd;
        animTrigger.OnSetAttackMovementEvent -= HandleSetAttackMovement;
        attackCompo.OnAttackEvent.RemoveListener(HandleAttackEvent);
    }
    
    private void HandleSetAttackMovement()
    {
        _rigid.linearVelocity = Vector2.zero;
    }

    private void HandleAnimationEnd()
    {
        CanMove = true;
    }

    private void HandleAttackEvent()
    {
        CanMove = false;
        StopImmediately();
        Vector2 attackDir = attackCompo.SetDamageCasterPos(_lastDir);

        if (Mathf.Approximately(_moveDir.x, 0) && Mathf.Approximately(_moveDir.y, 0)) return;

        _rigid.AddForce(attackDir * attackForce, ForceMode2D.Impulse);
    }

    private void HandleMoveSpeedChanged(StatSO stat, float prev, float current) => _moveSpeed = current;
    private void HandleRollCooldownChanged(StatSO stat, float prev, float current) => _rollCooldown = current;

    private void FixedUpdate()
    {
        Move();
        CheckRolling();
    }

    private void CheckRolling()
    {
        if (IsRolling && !CanMove)
        {
            IsRolling = false;
            StopImmediately();
        }
    }

    private void Move()
    {
        if (IsRolling || CanMove == false) return;

        _rigid.linearVelocity = _moveDir * _moveSpeed;

        playerChannel.RaiseEvent(PlayerEvents.PlayerMoveEvent.Initializer(transform.position));

        _anim.PlayAnimation("Move", !Mathf.Approximately(_rigid.linearVelocity.magnitude, 0));

        if ((Mathf.Approximately(_moveDir.x, 0) && Mathf.Approximately(_moveDir.y, 0)) || attackCompo.IsAttack) return;

        _lastDir = _moveDir;

        _anim.PlayAnimation("X", Mathf.Approximately(_moveDir.x, 0) ? 0 : Mathf.Sign(_moveDir.x));
        _anim.PlayAnimation("Y", Mathf.Approximately(_moveDir.y, 0) ? 0 : Mathf.Sign(_moveDir.y));
    }

    public void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>().normalized;
    }

    public void OnRoll(InputValue value)
    {
        bool isRollCooldown = _lastRollTime + _rollCooldown > Time.time;

        if (CanMove == false || isRollCooldown) return;
        
        StartRoll();
    }

    private void StartRoll()
    {
        attackCompo.CanAttack = false;
        IsRolling = true;
        health.CanHit = false;
        _collider.excludeLayers = whatIsProjectile.value;
        
        bool isDirZero = Mathf.Approximately(_moveDir.x, 0) && Mathf.Approximately(_moveDir.y, 0);
        Vector2 targetDir = isDirZero ? _lastDir : _moveDir;

        _rigid.linearVelocity = Vector3.zero;
        _rigid.AddForce(targetDir * _dashSpeed, ForceMode2D.Impulse);
        OnRollingFeedback?.Invoke();
        _anim.PlayAnimation("Rolling", true);
    }

    private void HandleRollEnd()
    {
        attackCompo.CanAttack = true;
        IsRolling = false;
        health.CanHit = true;
        _lastRollTime = Time.time;
        _anim.PlayAnimation("Rolling", false);
        _collider.excludeLayers = 0;
        
        OnRollEndEvent?.Invoke();
    }

    public void StopImmediately() => _rigid.linearVelocity = Vector2.zero;

    public void StopDash()
    {
        if (IsRolling == false) return;

        HandleRollEnd();
    }

    public void Knockback(Vector2 dir, float knockbackPower)
    {
        StopImmediately();
        _rigid.AddForce(dir * knockbackPower, ForceMode2D.Impulse);
    }
}