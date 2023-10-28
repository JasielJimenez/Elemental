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
    public Attack AttackUsed;
    public Text AttackInfo;
    public Text AttackOneText;
    public Text AttackTwoText;
    public Text AttackThreeText;

    public Button firstAttackButton, secondAttackButton, thirdAttackButton, fourthAttackButton, fifthAttackButton;
    // Start is called before the first frame update
    void Start()
    {
        firstAttackButton.onClick.AddListener(delegate{pressAttackButton(playerAttackCircle.GetComponent<CharacterAttacks>().AttackOne);});
        secondAttackButton.onClick.AddListener(delegate{pressAttackButton(playerAttackCircle.GetComponent<CharacterAttacks>().AttackTwo);});
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
        AttackOneText.text = playerAttackCircle.GetComponent<CharacterAttacks>().AttackOne.AttackName;
        AttackTwoText.text = playerAttackCircle.GetComponent<CharacterAttacks>().AttackTwo.AttackName;
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
        AttackInfo.text = playerAttackCircle.GetComponent<CharacterAttacks>().AttackOne.AttackInfo;
        //playerAttackCircle.transform.localScale = new Vector3(playerAttackCircle.GetComponent<CharacterAttacks>().AttackOne.AttackRangeX,playerAttackCircle.GetComponent<CharacterAttacks>().AttackOne.AttackRangeY,1.0f);
    }

    public void infoAttackTwo()
    {
        playerAttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        AttackInfo.text = playerAttackCircle.GetComponent<CharacterAttacks>().AttackTwo.AttackInfo;
        //playerAttackCircle.transform.localScale = new Vector3(playerAttackCircle.GetComponent<CharacterAttacks>().AttackTwo.AttackRangeX,playerAttackCircle.GetComponent<CharacterAttacks>().AttackTwo.AttackRangeY,1.0f);
    }

    public void pressAttackButton(Attack attack)
    {
        player.transform.GetChild(0).Find("Stats").GetComponent<CharacterStats>().HasActed = true;
        playerModel.GetComponent<Animations>().AttackAnimation(attack);
        //playerAttackCircle.GetComponent<SpriteRenderer>().enabled = true;
        //playerAttackCircle.GetComponent<CapsuleCollider>().enabled = true;  //SET TRUE DURING ATTACK ANIMATION
        //playerAttackCircle.transform.localScale = new Vector3(attack.attackRangeX, attack.attackRangeY, 1.0f);
        AttackUsed = attack;
        StartCoroutine(waitTime(attack.AttackTime));
        gameManager.GetComponent<Gamemanager>().returnFromAttack();
    }
}
