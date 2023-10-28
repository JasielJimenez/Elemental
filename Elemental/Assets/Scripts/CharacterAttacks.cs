using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttacks : MonoBehaviour
{
    public GameObject attackDatabase;
    public int attackOnePick;
    public int attackTwoPick;
    public int attackThreePick;
    public int attackFourPick;
    public int attackFivePick;
    public float attackUsed;
    public Attack AttackOne;
    public Attack AttackTwo;
    // Start is called before the first frame update
    void Start()
    {
        /*
        attackOne.attackIndex = 1.0f;
        attackOne.attackName = "Strike";
        attackOne.attackInfo = "Basic attack. Can hit multiple times if speed is higher than opponent";
        attackOne.addedDamage = 0;
        attackOne.elementDamage = false;
        attackOne.accuracy = 100.0f;
        attackOne.addedFocus = 0.0f;
        attackOne.attackRangeX = 30.0f;
        attackOne.attackRangeY = 30.0f;
        attackOne.attackTime = 1.0f;
        */
        attackOnePick = 0;
        attackTwoPick = 1;
        //Debug.Log("Database Count: " + attackDatabase.GetComponent<AttackImporter>().attackDatabase.Count);
        //attackOne = attackDatabase.GetComponent<AttackImporter>().attackDatabase[attackOnePick];
        //attackTwo = attackDatabase.GetComponent<AttackImporter>().attackDatabase[attackTwoPick];
        Invoke("buildMovePool",0.5f);
    }

    public void buildMovePool()
    {
        AttackOne = attackDatabase.GetComponent<AttackImporter>().attackDatabase[attackOnePick];
        AttackTwo = attackDatabase.GetComponent<AttackImporter>().attackDatabase[attackTwoPick];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
