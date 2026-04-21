using System;
using System.Collections.Generic;
using DG.Tweening;
using HN.Code.Combat;
using HN.Code.Entities;
using HN.Code.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerAttackCompo : MonoBehaviour
{
    public UnityEvent OnAttackFailEvent;
    public UnityEvent OnAttackEvent;
    
    [SerializeField] private List<AttackDataSO> attackDatas;
    [SerializeField] private StatSO damageStat, attackCooldownStat, attackSpeedStat;
    [SerializeField] private StatCompo statCompo;
    [SerializeField] private PlayerAnimatorTrigger animTrigger;
    [SerializeField] private EntityAnimator _anim;
    [SerializeField] private DamageCaster damageCaster;
    [SerializeField] private float comboDelay;
    [SerializeField] private float comboStartDelay;

    public bool CanAttack { get; set; } = true;
    public bool IsAttack { get; private set; }
    public bool IsCombo { get; private set; }

    private Dictionary<AttackDirection, AttackDataSO> _dataPairs = new Dictionary<AttackDirection, AttackDataSO>();
    private int _damage;
    private float _attackCooldown;
    private float _attackSpeed;
    private float _lastAttackTime = -999f;
    private Vector2 _attackDir;
    private AttackDataSO _currentData;
    
    private void Awake()
    {
        foreach (AttackDataSO attackData in attackDatas)
        {
            _dataPairs.Add(attackData.direction, attackData);
        }

        animTrigger.OnAnimationEndEvent += TurnOffAttacking;
        animTrigger.OnAttackEvent += HandleAttackTrigger;

        /*for(int i = 0; i <  attackDatas.Count; i++)
        {
            _dataPairs.Add(attackDatas[i].direction, attackDatas[i]);
        }*/
    }

    private void Start()
    {
        StatSO dmgStat = statCompo.GetStat(damageStat);
        StatSO atkCooldownStat = statCompo.GetStat(attackCooldownStat);
        StatSO atkSpdStat = statCompo.GetStat(attackSpeedStat);

        _damage = (int)dmgStat.BaseValue;
        _attackCooldown = atkCooldownStat.BaseValue;
        _attackSpeed = atkSpdStat.BaseValue;
        _anim.PlayAnimation("AttackSpeed", atkSpdStat.BaseValue);

        dmgStat.OnValueChanged += HandleDamageChanged;
        atkSpdStat.OnValueChanged += HandleAttackSpeedChanged;
        atkCooldownStat.OnValueChanged += HandleAttackCooldownChanged;
    }

    private void OnDestroy()
    {
        StatSO dmgStat = statCompo.GetStat(damageStat);
        StatSO atkSpdStat = statCompo.GetStat(attackSpeedStat);
        StatSO atkCooldownStat = statCompo.GetStat(attackCooldownStat);

        dmgStat.OnValueChanged -= HandleDamageChanged;
        atkSpdStat.OnValueChanged -= HandleAttackSpeedChanged;
        atkCooldownStat.OnValueChanged -= HandleAttackCooldownChanged;

        animTrigger.OnAnimationEndEvent -= TurnOffAttacking;
        animTrigger.OnAttackEvent -= HandleAttackTrigger;
    }

    private void HandleAttackCooldownChanged(StatSO stat, float prev, float current) => _attackCooldown = current;
    private void HandleDamageChanged(StatSO stat, float prev, float current) => _damage = (int)current;

    private void HandleAttackSpeedChanged(StatSO stat, float prev, float current)
    {
        _attackSpeed = current;
        _anim.PlayAnimation("AttackSpeed", current);
    }

    public void OnAttack(InputValue value)
    {
        if (CanAttack == false) return;
        
        float speed = 1 / _attackSpeed;
        bool isCombo = _lastAttackTime + comboStartDelay * speed  < Time.time && _lastAttackTime + comboDelay > Time.time;
        if (value.isPressed && isCombo && !IsCombo)
        {
            _anim.PlayAnimation("ComboCounter", 1);
            IsCombo = true;
            Attack();
        }
        else if (value.isPressed && IsAttack == false && _lastAttackTime + _attackCooldown < Time.time)
        {
            IsCombo = false;
            Attack();
        }
    }

    private void Attack()
    {
        IsAttack = true;
        _anim.PlayAnimation("Attack", true);
        _lastAttackTime = Time.time;
    }

    public void TurnOffAttacking()
    {
        _anim.PlayAnimation("Attack", false);
        _anim.PlayAnimation("ComboCounter", 0);
        IsAttack = false;
    }

    private void HandleAttackTrigger()
    {
        if(IsAttack == false) return;
        
        int comboCounter = IsCombo ? 1 : 0;
        OnAttackEvent?.Invoke();
        if(_currentData != null && _currentData.attackInfos[comboCounter].useCircle)
        {
            if(damageCaster.CastDamageOverlapCircle(_damage) == false)
                OnAttackFailEvent?.Invoke();
        }
        else
        {
            if(damageCaster.CastDamageOverlapBox(_damage))
                OnAttackFailEvent?.Invoke();
        }
    }

    public Vector2 SetDamageCasterPos(Vector2 moveDir)
    {
        if (Mathf.Approximately(moveDir.x, 0) == false && Mathf.Approximately(moveDir.y, 0) == false)
        {
            moveDir = new Vector2(0, Mathf.Sign(moveDir.y));
        }
        
        SetAttackData(moveDir);

        return moveDir;
    }

    private void SetAttackData(Vector2 moveDir)
    {
        if (EqualVector(moveDir, Vector2.up))
            SetData(GetAttackData(AttackDirection.Up));
        else if (EqualVector(moveDir, Vector2.down))
            SetData(GetAttackData(AttackDirection.Down));
        else if (Equals(moveDir, Vector2.left))
            SetData(GetAttackData(AttackDirection.Left));
        else if (Equals(moveDir, Vector2.right))
            SetData(GetAttackData(AttackDirection.Right));
    }

    private void SetData(AttackDataSO data)
    {
        int comboCounter = IsCombo ? 1 : 0;
        _currentData = data;
        damageCaster.transform.localPosition = data.attackInfos[comboCounter].pos;
        if (data.attackInfos[comboCounter].useCircle)
            damageCaster.radius = data.attackInfos[comboCounter].radius;
        else
            damageCaster.boxSize = data.attackInfos[comboCounter].boxSize;
    }

    private bool EqualVector(Vector2 a, Vector2 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    }

    private AttackDataSO GetAttackData(AttackDirection direction)
    {
        return _dataPairs.GetValueOrDefault(direction);
    }
}