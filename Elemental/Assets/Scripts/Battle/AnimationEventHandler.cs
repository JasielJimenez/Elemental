using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public GameObject Character;
    // Start is called before the first frame update
    void Start()
    {
        Character = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackParticleHandler(string test)
    {
        Debug.Log("Event reached: " + test);
        //Get particle effect and then get AttackCollision
        Debug.Log("Child: " + this.transform.parent.GetChild(6));

        for(int i = 0; i < Character.transform.childCount; i++)
        {
            if(Character.transform.GetChild(i).tag == "Attack")
            {
                var attackRange = Character.transform.GetChild(i).Find("Hitboxes");
                Debug.Log(attackRange);
                attackRange.GetComponent<AttackParticles>().SpawnParticle(test);
                break;
            }
        }

        //Character.GetChild(6).GetChild(1).GetComponent<AttackParticles>().SpawnParticle(test);
    }

    public void AttackDealDamage()
    {
        var battleManager = Character.transform.Find("CharacterInfo")?.GetComponent<CharacterStats>().BattleManager;
        battleManager.GetComponent<CombatManager>().DamageStep();
    }

    public void AttackAnimationEnd()
    {
        var battleManager = Character.transform.Find("CharacterInfo")?.GetComponent<CharacterStats>().BattleManager;
        battleManager.GetComponent<CombatManager>().EndOfAttack();
    }

}
