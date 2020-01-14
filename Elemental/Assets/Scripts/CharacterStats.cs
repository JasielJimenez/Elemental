using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public string elementalAttribute;
    public Stat maxHealth;
    public Stat currHealth;
    public Stat attack;
    public Stat element;
    public Stat defense;
    public Stat evasion;
    public Stat stance;
    public Stat focus;
    public Stat speed;
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
    public Stat walkRange;
    public GameObject gameManager;
    public GameObject attackManager;
    public Attack incomingAttack;
    private GameObject walkCircle;
    public GameObject opponent;
    public float damage;
    public bool hasAttacked;
    //public GameObject

    void Awake()
    {
        //currHealth = maxHealth;
        gameManager = GameObject.Find("Gamemanager");
        hasAttacked = false;
        //Debug.Log(hasAttacked);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            //MAKE THIS ONLY APPLY TO THE PLAYER
            currHealth.updateStat(-5.0f);
            gameManager.GetComponent<Gamemanager>().updateHealthBar(true);
        }
        else if(other.gameObject.tag == "PlayerAttack" && gameObject.tag == "Enemy")
        {
            Debug.Log("HIT");
            opponent = other.transform.parent.Find("Stats").gameObject;
            incomingAttack = attackManager.GetComponent<useAttack>().attackUsed;
            float enemyAttackPower = incomingAttack.addedDamage + opponent.GetComponent<CharacterStats>().attack.GetStat();
            moveEffects(incomingAttack, enemyAttackPower, opponent);
            float damage = enemyAttackPower - defense.GetStat();
            if((damage > 0) && (incomingAttack.accuracy > evasion.GetStat()) && (currHealth.GetStat() > 0))
            {
                currHealth.updateStat(-damage);
                gameManager.GetComponent<Gamemanager>().updateHealthBar(false); 
            }
        }
    }

    public void moveEffects(Attack attackUsed, float enemyPower, GameObject attacker)
    {
        if(attackUsed.attackIndex == 0)
        {
            float speedDiff = attacker.GetComponent<CharacterStats>().speed.GetStat() - speed.GetStat();
            if(speedDiff > 0)
            {
                float numHits = speedDiff / 5;
                //FIGURE OUT HOW TO CHANGE ANIMATOR VARIABLES FROM HERE
                Debug.Log("Hit " + numHits + " time(s)!");
            }
            return;
        }
        if(attackUsed.elementDamage == true)
        {

        }
    }

}
