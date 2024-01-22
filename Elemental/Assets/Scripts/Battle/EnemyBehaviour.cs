using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject BattleManager;
    public List<Attack> AttackList = new List<Attack>();
    public GameObject FocusedTarget;
    public bool CanMove;
    public bool IsChargingAttack;
    public int TurnsLeftOfCharge;

    // Start is called before the first frame update
    void Start()
    {
        BattleManager = GameObject.Find("DemoBattleManager");
        AttackList = this.GetComponent<CharacterAttackList>().AttackList;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseBestAttack()
    {
        //For now, just use basic attack
        BattleManager.transform.GetComponent<CombatManager>().EnemyTurn(0);
        //if(IsChargingAttack)
        //{
        //    --TurnsLeftOfCharge;
			//Debug.Log("Turns to charge remaining: " + TurnsLeftOfCharge);
        //}
		//StartCoroutine(DemoEnemyTurn(0));

        //Look through list of attacks
        //see which attack is ready and does the most damage to one of the player party members
        //depending on attack range, see if attack will reach
        //if not, repeat process
        //var playerList = BattleManager.GetComponent<TurnOrder>().PlayerList;
    }
/*
    IEnumerator DemoEnemyTurn(int attackIndex)
    {
        var currAttack = AttackList[attackIndex];
        if(!IsChargingAttack)
        {
            //Debug.Log("Attack Ready");
            this.GetComponent<CharacterCombat>().CreateEnemyAttackRange(currAttack);
            if(currAttack.IsCharge)
        	{
				yield return new WaitForSeconds(2);
				//Debug.Log("Preparing Charge Attack");
				IsChargingAttack = true;
            	TurnsLeftOfCharge = currAttack.TurnsToCharge;
    			BattleManager.GetComponent<TurnOrder>().HandleNextEnemyTurn();
				yield break;
        	}
        }
		else if(IsChargingAttack && TurnsLeftOfCharge > 0)
		{
			//Debug.Log("Still Charging");
			yield return new WaitForSeconds(1);
			BattleManager.GetComponent<TurnOrder>().HandleNextEnemyTurn();
			yield break;
		}
		yield return new WaitForSeconds(2);
        this.GetComponent<CharacterCombat>().CurrAttack = currAttack;
		this.GetComponent<CharacterCombat>().DamageStep();
        //Debug.Log("Attack Finished");
       	yield return new WaitForSeconds(1);
		IsChargingAttack = false;
        BattleManager.GetComponent<TurnOrder>().HandleNextEnemyTurn();
    }
*/
}
