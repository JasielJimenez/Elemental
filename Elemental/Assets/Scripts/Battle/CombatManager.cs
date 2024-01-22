using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject Attacker;
    public CharacterStats AttackerStats;
    public Attack CurrAttack;
    public GameObject CurrAttackRange;
    public List<GameObject> TargetsInRange = new List<GameObject>();
    public List<Attack> AttackList = new List<Attack>();
    public List<int> DamageList = new List<int>();
    public bool BuffBeenApplied;
    public bool DebuffBeenApplied;
    public bool IsChargingAttack;
    public int TurnsLeftOfCharge;

    //public Text BattleLogText;

    // Start is called before the first frame update
    void Start()
    {
        //AttackerStats = this.GetComponent<CharacterStats>();
        //AttackList = this.GetComponent<CharacterAttackList>().AttackList;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAttacker(GameObject attacker)
    {
        Attacker = attacker;
        AttackerStats = attacker?.transform.GetChild(0)?.GetComponent<CharacterStats>();
        AttackList = attacker?.transform.GetChild(0)?.GetComponent<CharacterAttackList>()?.AttackList;
    }

    #region Get and Damage Targets in Range

    //Gets all targets in range of current attack and adds them to TargetList
    public void AddToTargetList(GameObject target)
    {
        if(target.name != Attacker.name)
        {
            //Set TargetCircle to active
            target.transform.GetChild(2).gameObject.SetActive(true);
            //Show health bar of target
            target.transform.GetChild(3).gameObject.SetActive(true);
            TargetsInRange.Add(target);

            var attackStrength = AttackerStats.Attack.GetCurrStat() + CurrAttack.AddedDamage;
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
        if(target.transform.GetChild(0).GetComponent<CharacterStats>().CurrHealth.GetCurrStat() == target.transform.GetChild(0).GetComponent<CharacterStats>().MaxHealth.GetCurrStat())
        {
            target.transform.GetChild(3).gameObject.SetActive(false);
        }
        //NOTE: Maybe have one list with each target having a damage value? Then only one index would need to be deleted.
        var index = TargetsInRange.IndexOf(target);
        TargetsInRange.Remove(target);
        DamageList.RemoveAt(index);
    }

    public void DamageStep()
    {
        //Debug.Log("(BuffBeenApplied = " + BuffBeenApplied + ")");
        this.GetComponent<BattleMenuButtons>().AddToDamageLog(AttackerStats.CharacterName + " used " + CurrAttack.AttackName + "!");
        AttackerStats.ChangeStat(-CurrAttack.StaminaCost,"CurrStamina", false);
        AttackerStats.ChangeStat(-CurrAttack.ElementCost,"CurrElement", false);
        if(TargetsInRange.Count == DamageList.Count)
        {
            if(TargetsInRange.Count == 0)
            {
                this.GetComponent<BattleMenuButtons>().AddToDamageLog("but there were no targets...");
            }

            //Debug.Log("TargetsInRange: " + TargetsInRange.Count);
            for(int i = 0; i < TargetsInRange.Count; i++)
            {
                var target = TargetsInRange[i];
                var opponentStats = target.transform.GetChild(0).gameObject.GetComponent<CharacterStats>();
                float healthRemaining = 0f;

                #region Handle Evasion
                //Check to see if target evades attack
                if(CurrAttack.Accuracy < opponentStats.Evasion.GetCurrStat() || CurrAttack.Accuracy == 0)
                {
                    this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " dodged the attack!");
                    Debug.Log("MISSED! Attack Acc: " + CurrAttack.Accuracy + " < Target Evasion: " + opponentStats.Evasion.GetCurrStat());
                    continue;
                }
                #endregion

                if(CurrAttack.ParticleEffects != null)
                {
                    TestParticleEffects(CurrAttack.ParticleEffects);
                }

                #region Handle Blocking
                //Handle blocking attacks
                if(DamageList[i] == 0)
                {
                    this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " blocked the attack!");
                    Debug.Log("BLOCKED!");
                    //hide potential damage bar
                    target.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(false);
                    //hide target circle
                    target.transform.GetChild(2).gameObject.SetActive(false);
                    continue;
                }
                #endregion

                #region Handle Damage and Enmity Updates
                this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " took " + DamageList[i] + " damage!");
                opponentStats.ChangeStat(-DamageList[i], "CurrHealth", true);
                opponentStats.UpdateEnmityList(Attacker, DamageList[i]); //Maybe update what determines enmity value?
                
                opponentStats.ChangeStat(AttackerStats.Focus.GetCurrStat() + CurrAttack.AddedFocus ,"CurrStance", true);
                if(opponentStats.CurrStance.GetCurrStat() >= opponentStats.MaxStance.GetCurrStat())
                {
                    //HANDLE DOWNED OPPONENTS
                    opponentStats.isDowned = true;
                }
                #endregion

                CheckForStatChanges();

                #region Handle Health Change
                if(opponentStats.CurrHealth.GetCurrStat() > 0)
                {
                    healthRemaining = (float)opponentStats.CurrHealth.GetCurrStat() / (float)opponentStats.MaxHealth.GetCurrStat();
                    target.transform.GetChild(3).gameObject.GetComponent<Healthbar>().UpdateHealthBar(healthRemaining);
                    //PLAY DAMAGED ANIMATION
                }
                else
                {
                    target.transform.GetChild(3).gameObject.GetComponent<Healthbar>().UpdateHealthBar(healthRemaining);
                    if(target.tag == "Enemy")
                    {
                        this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " was defeated!");
                        //PLAY DEAD ANIMATION
                        Destroy(target.transform.parent.gameObject);
                        this.GetComponent<TurnOrder>().NumEnemies--;
                        this.GetComponent<TurnOrder>().CheckEndOfBattle();
                    }
                    else if(target.tag == "Player")
                    {
                        this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " was defeated!");
                        //PLAY DEAD ANIMATION
                        Destroy(target);
                        this.GetComponent<TurnOrder>().CheckEndOfBattle();
                    }
                    else if(target.tag == "Object")
                    {
                        this.GetComponent<BattleMenuButtons>().AddToDamageLog(opponentStats.CharacterName + " was destroyed!");
                        //PLAY DEAD ANIMATION
                        Destroy(target);
                    }
                }
                #endregion
                
                if(target != null)
                {
                    //hide potential damage bar
                    target.transform.GetChild(3).GetChild(0).GetChild(1).gameObject.SetActive(false);
                    //hide target circle
                    target.transform.GetChild(2).gameObject.SetActive(false);

                    HandleTargetCombatMovement();
                }
            }
        }
        else
        {
            Debug.Log("ERROR: There were " +  TargetsInRange.Count + " targets, while there were " + DamageList.Count + " damages calculated.");
        }
        HandleCharacterCombatMovement();
        DeleteAttackRange();
    }

    #endregion

    #region Handle Timing

    public void PlayerAction()
    {
        StartCoroutine(DemoPlayerTurn(0));
        Debug.Log("Check enumerator");
    }

    IEnumerator DemoPlayerTurn(int attackNum)
    {
        //PARTICLE BEFORE DAMAGE
        //USER/TARGET ANIMATION BEFORE DAMAGE
        this.GetComponent<DemoClick>().ClickDisabled = true;
        this.GetComponent<BattleMenuButtons>().AddToDamageLog(AttackerStats.CharacterName + " used " + CurrAttack.AttackName + "!");
        yield return new WaitForSeconds(0);
        //PARTICLE DURING DAMAGE
        //TARGET ANIMATION DURING DAMAGE
        DamageStep();
        this.GetComponent<BattleMenuButtons>().UpdateCharacterHudMenu();
        DeleteAttackRange();
        Debug.Log("Enumerator");
        yield return new WaitForSeconds(1);
        Debug.Log("Enumerator after one second");
        //PARTICLE AFTER DAMAGE
        ////USER/TARGET ANIMATION AFTER DAMAGE
        this.GetComponent<BattleMenuButtons>().Cam.GetComponent<CameraMovement>().EnableMove();
        this.GetComponent<DemoClick>().ClickDisabled = false;
        Attacker.transform.GetChild(0).GetComponent<CharacterStats>().HasActed = true;
        this.GetComponent<BattleMenuButtons>().ToggleMenu("ActionsMenu",true);
        this.GetComponent<BattleMenuButtons>().CharacterTurnFinished();
        BuffBeenApplied = false;
        yield return new WaitForSeconds(0);
    }

    public void EnemyTurn(int attackNum)
    {
        if(IsChargingAttack)
        {
            --TurnsLeftOfCharge;
			//Debug.Log("Turns to charge remaining: " + TurnsLeftOfCharge);
        }
        StartCoroutine(DemoEnemyTurn(attackNum));
    }

    IEnumerator DemoEnemyTurn(int attackIndex)
    {
        var currAttack = AttackList[attackIndex];
        if(!IsChargingAttack)
        {
            CreateEnemyAttackRange(currAttack);
            if(currAttack.IsCharge)
        	{
				yield return new WaitForSeconds(2);
                this.GetComponent<BattleMenuButtons>().AddToDamageLog(AttackerStats.CharacterName + " is charging...");
				IsChargingAttack = true;
            	TurnsLeftOfCharge = currAttack.TurnsToCharge;
    			this.transform.GetComponent<TurnOrder>().HandleNextEnemyTurn();
				yield break;
        	}
            else
            {
                this.GetComponent<BattleMenuButtons>().AddToDamageLog(AttackerStats.CharacterName + " used " + CurrAttack.AttackName + "!");
            }
        }
		else if(IsChargingAttack && TurnsLeftOfCharge > 0)
		{
            this.GetComponent<BattleMenuButtons>().AddToDamageLog(AttackerStats.CharacterName + " is still charging...");
			yield return new WaitForSeconds(1);
			this.transform.GetComponent<TurnOrder>().HandleNextEnemyTurn();
			yield break;
		}
		yield return new WaitForSeconds(2);
        CurrAttack = currAttack;
		DamageStep();
        //Debug.Log("Attack Finished");
       	yield return new WaitForSeconds(1);
		IsChargingAttack = false;
        this.transform.GetComponent<TurnOrder>().HandleNextEnemyTurn();
    }

    #endregion

    #region Handle Particle Effects

    public void TestParticleEffects(GameObject particles)
    {
        Debug.Log("CREATE PARTICLES");
        Vector3 test = new Vector3(CurrAttackRange.transform.position.x, CurrAttackRange.transform.position.y + 1, CurrAttackRange.transform.position.z);
        var particle = Instantiate(particles, CurrAttackRange.transform.position, CurrAttackRange.transform.rotation);
        //StartCoroutine(DespawnParticle(2, particle));
    }

    IEnumerator DespawnParticle(int timeToDestroy, GameObject particle)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(particle);
    }

    #endregion

    #region Handle Buffs/Debuffs

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
                var buff = CurrAttack.BuffList[i];

                //SEEMS TO BE ADDING AN EXTRA 10. CHECK TO SEE WHY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                AttackerStats.HandleStatChanges(buff);
            }
            BuffBeenApplied = true;
            //Debug.Log("Set to true! BUFF APPLIED (BuffBeenApplied = " + BuffBeenApplied + ")");
            CurrAttack.BuffList.Clear();
            Debug.Log("BuffList size after clearing = " + CurrAttack.BuffList.Count);
        }
        if(CurrAttack.IsDebuff  && !BuffBeenApplied)
        {
            for(int i = 0; i < CurrAttack.DebuffList.Count; i++)
            {
                AttackerStats.HandleStatChanges(CurrAttack.DebuffList[i]);
            }
            DebuffBeenApplied = true;
            //Debug.Log("Set to true! BUFF APPLIED (BuffBeenApplied = " + BuffBeenApplied + ")");
            CurrAttack.DebuffList.Clear();
            Debug.Log("DebuffList size after clearing = " + CurrAttack.DebuffList.Count);
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

    #region Handle Movement Attacks

    public void HandleTargetCombatMovement()
    {
        if(CurrAttack.WillMoveEnemy)
        {
            //TODO
        }
    }

    public void HandleCharacterCombatMovement()
    {
        if(CurrAttack.WillMovePlayer)
        {
            Debug.Log(Attacker.transform.forward);
            var xDirection = Attacker.transform.forward.x * CurrAttack.MovePlayerDistance;
            Debug.Log("X: " + xDirection);
            var zDirection = Attacker.transform.forward.z * CurrAttack.MovePlayerDistance;
            Debug.Log("Z: " + zDirection);
            Attacker.transform.position += new Vector3(xDirection, 0, zDirection);

            //Moves walk circle to new character location
            Attacker.transform.parent.GetChild(0).position += new Vector3(xDirection,0,zDirection);
        }
    }
    #endregion

    #region Attack Range Creation/Deletion

    public void CreateAttackRange(int attackNumber)
    {
        CurrAttack = AttackList[attackNumber];
        var basicAttackRange = CurrAttack.AttackRange;
        SpawnAttackRange(basicAttackRange, new Vector3(Attacker.transform.position.x, 0, Attacker.transform.position.z), true);
    }

    public void CreateEnemyAttackRange(Attack CurrAttack)
    {
        //CurrAttack = this.GetComponent<CharacterAttackList>().AttackList[attackNumber];
        var basicAttackRange = CurrAttack.AttackRange;
        if(CurrAttack.AttackDirection == AttackDirectionType.Specify)
        {
            var target = AttackerStats.PriorityTarget.transform;
            SpawnAttackRange(basicAttackRange, new Vector3(target.position.x, 0, target.position.z), true);
        }
        else
        {
            SpawnAttackRange(basicAttackRange, new Vector3(Attacker.transform.position.x, 0, Attacker.transform.position.z), true);
        }
    }

    public void SpawnAttackRange(GameObject range, Vector3 rangePosition, bool isVisible)
    {
        CurrAttackRange = Instantiate(range, rangePosition, Attacker.transform.rotation);
        CurrAttackRange.transform.SetParent(Attacker.transform, true);
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
                if(target.transform.GetChild(0).GetComponent<CharacterStats>().CurrHealth.GetCurrStat() == target.transform.GetChild(0).GetComponent<CharacterStats>().MaxHealth.GetCurrStat())
                {
                    target.transform.GetChild(3).gameObject.SetActive(false);
                }
            }
            TargetsInRange.Clear();
            DamageList.Clear();
            DestroyImmediate(CurrAttackRange,true);
            CurrAttackRange = null;
        }
    }

    #endregion
}
