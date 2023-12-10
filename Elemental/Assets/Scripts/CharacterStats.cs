using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public string CharacterName;
    public ElementType ElementalAttribute;
    public Stat MaxHealth;
    public Stat CurrHealth;
    public Stat Attack;
    public Stat ElementAttack;
    public Stat Defense;
    public Stat Evasion;
    public Stat MaxStance;
    public Stat CurrStance;
    public Stat Focus;
    public Stat Speed;
    public Stat MaxStamina;
    public Stat CurrStamina;
    public Stat MaxElement;
    public Stat CurrElement;
    //----------------------------------------------------------------------
    public Stat fireDef;
    public Stat waterDef;
    public Stat iceDef;
    public Stat windDef;
    public Stat groundDef;
    public Stat metalDef;
    public Stat electricDef;
    public Stat lightDef;
    public Stat shadowDef;
    public Stat lifeDef;
    public Stat deathDef;
    public Stat nullDef;
    public Stat lunarDef;
    //----------------------------------------------------------------------
    public Stat fireRes;
    public Stat waterRes;
    public Stat iceRes;
    public Stat windRes;
    public Stat groundRes;
    public Stat metalRes;
    public Stat electricRes;
    public Stat lightRes;
    public Stat shadowRes;
    public Stat lifeRes;
    public Stat deathRes;
    public Stat nullRes;
    public Stat lunarRes;
    //----------------------------------------------------------------------
    public Stat WalkRange;
    //public GameObject gameManager;
    //public GameObject attackManager;
    //public Attack IncomingAttack;
    //private GameObject WalkCircle;
    //public GameObject Opponent;
    public bool HasActed;
    public bool HasWalked;
    //public GameObject
    //public List<StatNames> AllStatChanges = new List<StatChange>();
    //public List<Attack> AttackList = new List<Attack>();
    public GameObject BattleManager;

    public List<StatChange> AllStatChanges = new List<StatChange>();
    public Dictionary<string,int> EnmityList = new Dictionary<string,int>();
    public GameObject PriorityTarget;
    public int HighestEnmity = 0;


    void Start()
    {
        //currHealth = maxHealth;
        //gameManager = GameObject.Find("Gamemanager");
        HasActed = false;
        HasWalked = false;
        //Debug.Log(hasAttacked);

        //RACE CONDITION? <-------------------------------------------------------------------------------------------

        BattleManager = GameObject.Find("DemoBattleManager");
        Debug.Log(CharacterName + ": " + this.transform.parent.gameObject.tag);
        if(this.transform.parent.gameObject.tag == "Player")
        {
            PriorityTarget = BattleManager.GetComponent<TurnOrder>().EnemyList.transform.GetChild(0).GetChild(0).gameObject;
        }
        else if(this.transform.parent.gameObject.tag == "Enemy")
        {
            this.GetComponent<EnemyBehaviour>().BattleManager = BattleManager;
            PriorityTarget = BattleManager.GetComponent<TurnOrder>().PlayerList.transform.GetChild(0).GetChild(1).gameObject;
        }
    }

/*
    //HANDLES HITTING/ HIT BY ENEMY
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            //MAKE THIS ONLY APPLY TO THE PLAYER
            CurrHealth.UpdateCurrStat(-5.0f);
            //gameManager.GetComponent<Gamemanager>().updateHealthBar(true);
        }
        else if(other.gameObject.tag == "PlayerAttack" && gameObject.tag == "Enemy")
        {
            Debug.Log("HIT");
            Opponent = other.transform.parent.Find("Stats").gameObject;
            //incomingAttack = attackManager.GetComponent<useAttack>().attackUsed;
            float enemyAttackPower = IncomingAttack.addedDamage + Opponent.GetComponent<CharacterStats>().Attack.GetCurrStat();
            MoveEffects(IncomingAttack, enemyAttackPower, Opponent);
            float damage = enemyAttackPower - Defense.GetCurrStat();
            if((damage > 0) && (IncomingAttack.accuracy > Evasion.GetCurrStat()) && (CurrHealth.GetCurrStat() > 0))
            {
                CurrHealth.UpdateCurrStat(-damage);
                //gameManager.GetComponent<Gamemanager>().updateHealthBar(false); 
            }
        }
    }

    //HANDLES BASIC ATTACK
    public void MoveEffects(Attack attackUsed, float enemyPower, GameObject attacker)
    {
        if(attackUsed.attackIndex == 0)
        {
            float speedDiff = attacker.GetComponent<CharacterStats>().Speed.GetCurrStat() - Speed.GetCurrStat();
            if(speedDiff > 0)
            {
                float numHits = speedDiff / 5;
                //FIGURE OUT HOW TO CHANGE ANIMATOR VARIABLES FROM HERE
                Debug.Log("Hit " + numHits + " time(s)!");
            }
            return;
        }
    }
*/

    public void HandleStatChanges(StatChange incomingStatChange)
    {
        //StatChange test = new StatChange();
        //test.SetNumChange(change);
        //test.SetChangeDuration(duration);
        //test.SetIsPercentChange(isPercentage);
        //ChangeStat(test.GetNumChange(), statName);
        AllStatChanges.Add(incomingStatChange);

        //MAKE STAT CHANGE TEMPORARY *****
        //Handle in TurnOrder.cs?

        Debug.Log("All Stat Changes size: " + AllStatChanges.Count);

        ChangeStat(incomingStatChange.GetNumChange(), incomingStatChange.GetStatName());
    }

    public void UpdateEnmityList(GameObject opponent, int enmityAdded)
    {
        int value = 0;
        if(EnmityList.TryGetValue(opponent.name, out value))
        {
            Debug.Log("ENMITY VALUE: " + value);
            value += enmityAdded;
            EnmityList[opponent.name] = value;
        }
        else
        {
            EnmityList.Add(opponent.name, enmityAdded);
            value = enmityAdded;
        }

        if(value >= HighestEnmity)
        {
            HighestEnmity = value;
            PriorityTarget = opponent;
        }
        else
        {
            Debug.Log("VALUE: " + value + ", HIGHESTENMITY: " + HighestEnmity + ", OPPONENTNAME: " + opponent.name + ", PriorityTargetName: " + PriorityTarget.name);
        }
    }

    public void ChangeStat(int statChange, string statName)
    {
        switch(statName)
        {
            case "MaxHealth":
                MaxHealth.UpdateCurrStat(statChange);
            break;
            case "CurrentHealth":
                CurrHealth.UpdateCurrStat(statChange);
            break;
            case "Attack":
            Debug.Log("Changing Attack Stat");
                Attack.UpdateCurrStat(statChange);
            break;
            case "ElementAttack":
                ElementAttack.UpdateCurrStat(statChange);
            break;
            case "Defense":
                Defense.UpdateCurrStat(statChange);
            break;
            case "Evasion":
                Evasion.UpdateCurrStat(statChange);
            break;
            case "MaxStance":
                MaxStance.UpdateCurrStat(statChange);
            break;
            case "CurrStance":
                CurrStance.UpdateCurrStat(statChange);
            break;
            case "Focus":
                Focus.UpdateCurrStat(statChange);
            break;
            case "Speed":
                Speed.UpdateCurrStat(statChange);
            break;
            case "MaxStamina":
                MaxStamina.UpdateCurrStat(statChange);
            break;
            case "CurrentStamina":
                CurrStamina.UpdateCurrStat(statChange);
            break;
            case "MaxElement":
                MaxElement.UpdateCurrStat(statChange);
            break;
            case "CurrentElement":
                CurrElement.UpdateCurrStat(statChange);
            break;
            default:
                Debug.Log("Invalid stat name given to be changed");
            break;
        }
    }

}
