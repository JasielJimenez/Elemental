using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class useAttack : MonoBehaviour
{
    public Animator anim;
    public GameObject gameManager;
    public GameObject playerAttackCircle;
    public GameObject player;
    public GameObject playerModel;
    public GameObject playerAttacks;
    public Attack attackUsed;
    public Text attackInfo;
    public Text attackOneText;
    public Text attackTwoText;
    public Text attackThreeText;

    public Button firstAttackButton, secondAttackButton, thirdAttackButton, fourthAttackButton, fifthAttackButton;
    // Start is called before the first frame update
    void Start()
    {
        firstAttackButton.onClick.AddListener(delegate{pressAttackButton(playerAttackCircle.GetComponent<CharacterAttacks>().attackOne);});
        secondAttackButton.onClick.AddListener(delegate{pressAttackButton(playerAttackCircle.GetComponent<CharacterAttacks>().attackTwo);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCurrentPlayer()
    {
        player = gameManager.GetComponent<Gamemanager>().selectedPlayer;
        //Debug.Log(player.name);
        playerModel = player.transform.GetChild(0).GetChild(0).gameObject;
        //anim = playerModel.GetComponent<Animator>();
        playerAttackCircle = player.transform.GetChild(0).Find("AttackCircle").gameObject;
        //playerAttacks = player.transform.GetChild(0).Find("Attacks").gameObject;
        attackOneText.text = playerAttackCircle.GetComponent<CharacterAttacks>().attackOne.attackName;
        attackTwoText.text = playerAttackCircle.GetComponent<CharacterAttacks>().attackTwo.attackName;
        //Debug.Log(player.transform.GetChild(0).Find("AttackCircle").gameObject.name);
    }

    public IEnumerator waitTime(float time)
    {
        Debug.Log("waiting");
        yield return new WaitForSeconds(time);
        playerAttackCircle.GetComponent<CapsuleCollider>().enabled = false;
        playerAttackCircle.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("done");
    }

    public void infoAttackOne()
    {
        playerAttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        attackInfo.text = playerAttackCircle.GetComponent<CharacterAttacks>().attackOne.attackInfo;
        playerAttackCircle.transform.localScale = new Vector3(playerAttackCircle.GetComponent<CharacterAttacks>().attackOne.attackRangeX,playerAttackCircle.GetComponent<CharacterAttacks>().attackOne.attackRangeY,1.0f);
    }

    public void infoAttackTwo()
    {
        playerAttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        attackInfo.text = playerAttackCircle.GetComponent<CharacterAttacks>().attackTwo.attackInfo;
        playerAttackCircle.transform.localScale = new Vector3(playerAttackCircle.GetComponent<CharacterAttacks>().attackTwo.attackRangeX,playerAttackCircle.GetComponent<CharacterAttacks>().attackTwo.attackRangeY,1.0f);
    }

    public void pressAttackButton(Attack attack)
    {
        player.transform.GetChild(0).Find("Stats").GetComponent<CharacterStats>().hasAttacked = true;
        playerModel.GetComponent<Animations>().attackAnimation(attack);
        //playerAttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        //playerAttackCircle.GetComponent<CapsuleCollider>().enabled = true;  //SET TRUE DURING ATTACK ANIMATION
        //playerAttackCircle.transform.localScale = new Vector3(attack.attackRangeX, attack.attackRangeY, 1.0f);
        attackUsed = attack;
        StartCoroutine(waitTime(attack.attackTime));
        gameManager.GetComponent<Gamemanager>().returnFromAttack();
    }
}
