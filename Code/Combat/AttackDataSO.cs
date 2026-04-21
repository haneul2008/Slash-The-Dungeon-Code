using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirection
{
    Up,
    Down,
    Left,
    Right
}

[Serializable]
public class AttackInfo
{
    public Vector2 pos;
    public bool useCircle;
    public float radius = 1;
    public Vector2 boxSize;
}

[CreateAssetMenu(menuName = "SO/AttackData")]
public class AttackDataSO : ScriptableObject
{
    public AttackDirection direction;
    public List<AttackInfo> attackInfos;
}
