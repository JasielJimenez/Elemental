using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack 
{
    public int AttackIndex;
    public string AttackName;
    public string AttackInfo;
    public GameObject AttackRange;
    public bool HasMultipleAttackRanges;            //Not sure if needed
    public GameObject MultipleAttackRanges;         //AttackRange can probably hold some form of attack range spawner
    public AttackDirectionType AttackDirection;
    public GameObject ParticleEffects;              //Make into a list?
    public int AddedDamage;
    public int ElementDamage;
    public int Accuracy;
    public int AddedFocus;
    public int AttackTime;
    public int StaminaCost;
    public int ElementCost;
    //public Vector3 AttackLocation;
    public bool WillMovePlayer;
    public float MovePlayerDistance;
    public bool WillMoveEnemy;
    public float MoveEnemyDistance;

    public bool IsCharge;
    public int TurnsToCharge;
    public bool IsDebuff;
    public List<StatChange> DebuffList = new List<StatChange>();
    public bool IsBuff;
    public List<StatChange> BuffList = new List<StatChange>();
    public bool IsStatusAilment;
    //public List<StatChange> StatusAilment = new List<StatChange>();

    public bool isGrazing;
    public bool isUnavoidable;
    public bool isPiercing;
    public bool isUnBlockable;
    //public bool isViolent;

    /*
    public bool singleTarget;
    public bool coolDown;
    public bool delay;
    public bool appendElement;
    public int increaseAttack;
    public int increaseElement;
    public int increaseDefense;
    public int increaseEvasion;
    public int increaseStance;
    public int increaseFocus;
    public int increaseSpeed;
    public int increaseElementalDefense;
    public int increaseResilience;
    public int buffDuration;
    public int debuffDuration;
    public int healthHeal;
    */
}
