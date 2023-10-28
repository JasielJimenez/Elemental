using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public CharacterStats MyStats;
    public Attack CurrAttack;
    public GameObject CurrAttackRange;
    public List<GameObject> TargetsInRange = new List<GameObject>();
    public List<Attack> AttackList = new List<Attack>();
    public List<int> DamageList = new List<int>();
    public bool BuffBeenApplied;
    // Start is called before the first frame update
    void Start()
    {
        MyStats = this.GetComponent<CharacterStats>();
        AttackList = this.GetComponent<CharacterAttackList>().AttackList;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Get and Damage Targets in Range

    //Gets all targets in range of current attack and adds them to TargetList
    public void AddToTargetList(GameObject target)
    {
        if(target.name != this.transform.parent.gameObject.name)
        {
            //Set TargetCircle to active
            target.transform.GetChild(2).gameObject.SetActive(true);
            //Show health bar of target
            target.transform.GetChild(3).gameObject.SetActive(true);
            TargetsInRange.Add(target);

            var attackStrength = MyStats.Attack.GetCurrStat() + CurrAttack.AddedDamage;
            var opponentStats = target.transform.GetChild(0).gameObject.GetComponent<CharacterStats>();
            var actualDamage = attackStrength - opponentStats.Defense.GetCurrStat();

            DamageList.Add(actualDamage);
            float potentialDamage = 0f;
            if(opponentStats.CurrHealth.GetCurrStat() > 0)
            {
                if(actualDamage > opponentStats.CurrHealth.GetCurrStat())
                {
                    potentialDamage = 1;
                }
                else
                {
                    //Debug.Log("ActualDamage: " + actualDamage + "/ CurrHealth: " + (float)opponentStats.CurrHealth.GetCurrStat());
                    potentialDamage = actualDamage / (float)opponentStats.CurrHealth.GetCurrStat();
                }
            }
            //show potential damage bar of target
            target.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(true);
            target.transform.GetChild(3).gameObject.GetComponent<Healthbar>().DisplayPotentialDamage(potentialDamage);
        }
    }

    public void RemoveFromTargetList(GameObject target)
    {
        //Set TargetCircle to inactive
        target.transform.GetChild(2).gameObject.SetActive(false);
        //Hide health bar of target
        target.transform.GetChild(3).gameObject.SetActive(false);
        //NOTE: Maybe have one list with each target having a damage value? Then only one index would need to be deleted.
        var index = TargetsInRange.IndexOf(target);
        TargetsInRange.Remove(target);
        DamageList.RemoveAt(index);
    }

    public void DamageStep()
    {
        Debug.Log("(BuffBeenApplied = " + BuffBeenApplied + ")");
        if(TargetsInRange.Count == DamageList.Count)
        {
            for(int i = 0; i < TargetsInRange.Count; i++)
            {
                var target = TargetsInRange[i];
                var opponentStats = target.transform.GetChild(0).gameObject.GetComponent<CharacterStats>();
                float healthRemaining = 0f;

                //Check to see if target evades attack
                if(CurrAttack.Accuracy < opponentStats.Evasion.GetCurrStat())
                {
                    Debug.Log("MISSED! Attack Acc: " + CurrAttack.Accuracy + " < Target Evasion: " + opponentStats.Evasion.GetCurrStat());
                    continue;
                }

                opponentStats.ChangeStat(-DamageList[i], "CurrentHealth");
                opponentStats.UpdateEnmityList(MyStats.CharacterName, DamageList[i]); //Maybe update what determines enmity value?

                CheckForStatChanges();

                //Handle blocking attacks
                if(DamageList[i] == 0)
                {
                    Debug.Log("BLOCKED!");
                    continue;
                }

                if(opponentStats.CurrHealth.GetCurrStat() > 0)
                {
                    healthRemaining = (float)opponentStats.CurrHealth.GetCurrStat() / (float)opponentStats.MaxHealth.GetCurrStat();
                }
                target.transform.GetChild(3).gameObject.GetComponent<Healthbar>().UpdateHealthBar(healthRemaining);
                //hide potential damage bar
                target.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(false);
                //hide target circle
                target.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("ERROR: There were " +  TargetsInRange.Count + " targets, while there were " + DamageList.Count + " damages calculated.");
        }
        DeleteAttackRange();
    }

    public void CheckForStatChanges()
    {
        if(CurrAttack.IsBuff && !BuffBeenApplied)
        {
            //REMOVE WHEN NOT TESTING
            CreateDemoBuff();

            //Debug.Log("BUFF APPLIED (BuffBeenApplied = " + BuffBeenApplied + ")");
            Debug.Log("BuffList size = " + CurrAttack.BuffList.Count);
            for(int i = 0; i < CurrAttack.BuffList.Count; i++)
            {
                //SEEMS TO BE ADDING AN EXTRA 10. CHECK TO SEE WHY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                MyStats.HandleStatChanges(CurrAttack.BuffList[i]);
            }
            BuffBeenApplied = true;
            //Debug.Log("Set to true! BUFF APPLIED (BuffBeenApplied = " + BuffBeenApplied + ")");
            CurrAttack.BuffList.Clear();
            Debug.Log("BuffList size after clearing = " + CurrAttack.BuffList.Count);
        }
        if(CurrAttack.IsDebuff)
        {
            for(int i = 0; i < CurrAttack.DebuffList.Count; i++)
            {
                
            }
        }
    }

    private void CreateDemoBuff()
    {
        //TEST---------------------------------------------------------------------------
        Debug.Log("BuffList size before creating test buff = " + CurrAttack.BuffList.Count);
        StatChange test = new StatChange();
        test.SetStatName("Attack");
        test.SetNumChange(10);
        test.SetChangeDuration(3);
        test.SetIsPercentChange(false);
        CurrAttack.BuffList.Add(test);
        Debug.Log("BuffList size after adding test buff to list = " + CurrAttack.BuffList.Count);
        //TEST---------------------------------------------------------------------------
    }

    #endregion

    #region Attack Range Creation/Deletion

    public void CreateAttackRange(int attackNumber)
    {
        CurrAttack = this.GetComponent<CharacterAttackList>().AttackList[attackNumber];
        var basicAttackRange = CurrAttack.AttackRange;
        SpawnAttackRange(basicAttackRange);
    }

    public void SpawnAttackRange(GameObject range)
    {
        CurrAttackRange = Instantiate(range, new Vector3(this.transform.position.x, 0, this.transform.position.z), this.transform.rotation);
        CurrAttackRange.transform.SetParent(this.transform, true);
    }

    public void DeleteAttackRange()
    {
        if(CurrAttackRange != null)
        {
            foreach(GameObject target in TargetsInRange)
            {
                //Set TargetCircle to inactive
                target.transform.GetChild(2).gameObject.SetActive(false);
                //Hide health bar of target
                target.transform.GetChild(3).gameObject.SetActive(false);
            }
            TargetsInRange.Clear();
            DamageList.Clear();
            DestroyImmediate(CurrAttackRange,true);
            CurrAttackRange = null;
        }
    }

    #endregion
}
