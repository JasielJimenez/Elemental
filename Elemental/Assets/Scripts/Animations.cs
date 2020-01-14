using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator anim;
    public GameObject attackCircle;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackAnimation(Attack attackUsed)
    {
        anim.SetInteger("attackBeingUsed",attackUsed.attackIndex);
        attackCircle.transform.localScale = new Vector3(attackUsed.attackRangeX, attackUsed.attackRangeY, 1.0f);
        //CHANGE TO HAVE THIS LINE WORK IN STARTHITBOX()
    }

    public void startHitBox()
    {
        attackCircle.GetComponent<SpriteRenderer>().enabled = true;
        attackCircle.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void endHitBox()
    {
        attackCircle.GetComponent<SpriteRenderer>().enabled = false;
        attackCircle.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void stopAttackAnimation()
    {
        anim.SetInteger("attackBeingUsed",100);
    }
}
