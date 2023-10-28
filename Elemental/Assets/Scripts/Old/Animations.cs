using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator anim;
    public GameObject AttackCircle;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackAnimation(Attack attackUsed)
    {
        anim.SetInteger("attackBeingUsed",attackUsed.AttackIndex);
        //AttackCircle.transform.localScale = new Vector3(attackUsed.AttackRangeX, attackUsed.AttackRangeY, 1.0f);
        //CHANGE TO HAVE THIS LINE WORK IN STARTHITBOX()
    }

    public void StartHitBox()
    {
        AttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        AttackCircle.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void EndHitBox()
    {
        AttackCircle.GetComponent<SpriteRenderer>().enabled = false;
        AttackCircle.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void StopAttackAnimation()
    {
        anim.SetInteger("attackBeingUsed",100);
    }
}
