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
    public AttackType AttackTargetType;
    public int AddedDamage;
    public int ElementDamage;
    public int Accuracy;
    public int AddedFocus;
    public int AttackTime;
    public int StaminaCost;
    public int ElementCost;

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
