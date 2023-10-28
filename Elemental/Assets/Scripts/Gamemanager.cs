using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gamemanager : MonoBehaviour {
    public GameObject cam;
    public GameObject attackManager;
    public GameObject chooseMenu;
    public GameObject attackMenu;
    public GameObject moveMenu;
    public GameObject playerStatsMenu;
    public GameObject playerHUD;
    public GameObject enemyStatsMenu;
    public GameObject enemyHUD;
    public GameObject confirmEndUI;
    public GameObject endTurnUI;
    public GameObject turnDecider;
    //public GameObject levelHUD;
    public GameObject playerSelectCircle;
    public GameObject enemySelectCircle;
    public GameObject playerHealthBar;
    public GameObject enemyHealthBar;
    public float playerMaxHealth;
    public float playerCurrHealth;
    public float enemyMaxHealth;
    public float enemyCurrHealth;
    public bool playerSelected;
    public bool enemySelected;
    public bool showPlayerStats;
    public bool showEnemyStats;
    public bool playerTurn;
    public bool isWalking;
    public Text characterName;
    public Text characterHealth;
    public Text characterStats;
    public Text enemyName;
    public Text enemyHealth;
    public Text enemyStats;
    public Text turn;

    //public PlayerMovement playerWalk;
    //[SerializeField]
    public GameObject selectedPlayer;
    public GameObject selectedEnemy;

	// Use this for initialization
	void Start () {
        //chooseMenu = GameObject.Find("Choose Menu");
        //attackMenu = GameObject.Find("Attack Menu");
        //moveMenu = GameObject.Find("Move Menu");
        //statsMenu = GameObject.Find("Stats Menu");
        //playerHUD = GameObject.Find("Player HUD");
        //endTurnUI = GameObject.Find("EndTurnUI");
        //confirmEndUI = GameObject.Find("ConfirmEndTurnUI");
        //turnDecider = GameObject.Find("TurnDecider");

        cam = GameObject.Find("CameraBody");

        chooseMenu.SetActive(false);
        attackMenu.SetActive(false);
        moveMenu.SetActive(false);
        playerStatsMenu.SetActive(false);
        playerHUD.SetActive(false);
        enemyStatsMenu.SetActive(false);
        enemyHUD.SetActive(false);
        confirmEndUI.SetActive(false);
        //chooseMenu.transform.position = new Vector3(chooseMenu.transform.position.x, chooseMenu.transform.position.y - 250.0f, chooseMenu.transform.position.z);
        menuMovement(endTurnUI, -150.0f, 150.0f, 1.0f);
        //menuMovement(turnDecider, true, 150.0f, -150.0f, 1.0f);
        playerSelected = false;
        enemySelected = false;
        showPlayerStats = false;
        showEnemyStats = false;
        isWalking = false;

        playerTurn = true;
	}
	

    /*
    public void debugRay()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if(raycastResults.Count > 0)
            {
                foreach(var go in raycastResults)
                {  
                    Debug.Log(go.gameObject.name,go.gameObject);
                }
            }
    }
    */

    public IEnumerator waitTime(float time)
    {
        yield return new WaitForSeconds(time);
        playerTurn = true;
        menuMovement(endTurnUI, -150.0f, 150.0f, 1.0f);
        cam.GetComponent<CameraMovement>().EnableMove();
    }

    public void menuMovement(GameObject menuMoved, float startFrom, float endAt, float timeTaken)
    {
        //Works with stopped time?
            menuMoved.transform.position = new Vector3(menuMoved.transform.position.x, menuMoved.transform.position.y + startFrom, menuMoved.transform.position.z);
            menuMoved.SetActive(true);
        Vector3 destination = new Vector3(menuMoved.transform.position.x, menuMoved.transform.position.y + endAt, menuMoved.transform.position.z);
        iTween.MoveTo(menuMoved, iTween.Hash("position", destination,"time", timeTaken,"easetype", iTween.EaseType.easeInOutSine));
        //if(toggle == false)
        //{
            //StartCoroutine(waitTime(0.5f, menuMoved, -endAt));
        //    menuMoved.transform.position = new Vector3(menuMoved.transform.position.x, menuMoved.transform.position.y - endAt, menuMoved.transform.position.z);
        //}
    }

	// Update is called once per frame
	void Update () {
        if(playerTurn == true && isWalking == false)
        {
            if(Input.GetMouseButtonDown(0))
            {
                //debugRay();
                if(EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.tag == "Player")
                    {
                        selectedPlayer = hit.transform.gameObject;
                        attackManager.GetComponent<useAttack>().updateCurrentPlayer(); //Takes a moment to write attack names
                        //chooseMenu.SetActive(true);
                        //playerHUD.SetActive(true);
                        menuMovement(chooseMenu, -250.0f, 250.0f, 0.2f);
                        menuMovement(playerHUD, 250.0f, -250.0f, 0.2f);
                        attackMenu.SetActive(false);
                        getCharacterInfo(true);
                        playerSelected = true;
                        playerSelectCircle.SetActive(true);
                        playerSelectCircle.transform.position = new Vector3(selectedPlayer.transform.position.x, 0.1f, selectedPlayer.transform.position.z);
                        playerSelectCircle.transform.SetParent(selectedPlayer.transform);
                        //cam.transform.position = new Vector3(selected.transform.position.x,20,selected.transform.position.z - 10);
                        Vector3 temp = new Vector3(selectedPlayer.transform.position.x, 20, selectedPlayer.transform.position.z - 10);
                        iTween.MoveTo(cam, iTween.Hash("position", temp,"time", 0.25f,"easetype", iTween.EaseType.easeInOutSine));
                    }
                    else if(hit.transform.tag == "Enemy")
                    {
                        selectedEnemy = hit.transform.gameObject;
                        menuMovement(enemyHUD, 250.0f, -250.0f, 0.2f);
                        getCharacterInfo(false);
                        enemySelectCircle.SetActive(true);
                        enemySelectCircle.transform.position = new Vector3(selectedEnemy.transform.position.x, 0.1f, selectedEnemy.transform.position.z);
                        enemySelectCircle.transform.SetParent(selectedEnemy.transform);
                    }
                    else if(hit.transform.tag == "Destructible")
                    {
                        //selected = hit.transform.gameObject;
                    }
                }
            }

            if(Input.GetKeyUp("escape"))
            {
                chooseMenu.SetActive(false);
                playerSelected = false;
                playerSelectCircle.SetActive(false);
            }
        }
	}

    //IMPORTANT: Change so health stat can't drop below zero and can't go higher than max health stat <------------------------------
    public void updateHealthBar(bool isPlayer)
    {
        if(isPlayer == true)
        {
            playerMaxHealth = selectedPlayer.GetComponentInChildren<CharacterStats>().MaxHealth.GetCurrStat();
            playerCurrHealth = selectedPlayer.GetComponentInChildren<CharacterStats>().CurrHealth.GetCurrStat();
            if(playerCurrHealth > -1 && playerCurrHealth <= playerMaxHealth)
            {
                iTween.ScaleTo(playerHealthBar, new Vector3(playerCurrHealth / playerMaxHealth , 1.0f),0.5f);
                characterHealth.text = playerCurrHealth.ToString() + " / " + playerMaxHealth.ToString();
            }
            else if(playerCurrHealth < 0)
            {
                iTween.ScaleTo(playerHealthBar, new Vector3(0.0f , 1.0f),0.5f);
                characterHealth.text = "0 / " + playerMaxHealth.ToString();
            }
            else
            {
                iTween.ScaleTo(playerHealthBar, new Vector3(1.0f , 1.0f),0.5f);
                characterHealth.text = playerMaxHealth.ToString() + " / " + playerMaxHealth.ToString();
            }
        }
        else
        {
            enemyMaxHealth = selectedEnemy.GetComponentInChildren<CharacterStats>().MaxHealth.GetCurrStat();
            enemyCurrHealth = selectedEnemy.GetComponentInChildren<CharacterStats>().CurrHealth.GetCurrStat();
            if(enemyCurrHealth > -1)
            {
                iTween.ScaleTo(enemyHealthBar, new Vector3(enemyCurrHealth / enemyMaxHealth , 1.0f),0.5f);
            }
            else
            {
                iTween.ScaleTo(enemyHealthBar, new Vector3(0.0f , 1.0f),0.5f);
            }
            enemyHealth.text = enemyCurrHealth.ToString() + " / " + enemyMaxHealth.ToString();
        }
    }


    public void getCharacterInfo(bool isPlayer)
    {
        updateHealthBar(isPlayer);
        if(isPlayer == true)
        {
            characterName.text = selectedPlayer.GetComponentInChildren<CharacterStats>().CharacterName;
            characterStats.text = selectedPlayer.GetComponentInChildren<CharacterStats>().Attack.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().ElementAttack.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().Defense.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().Evasion.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().Speed.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().CurrStance.GetCurrStat().ToString() + "\n"
            + selectedPlayer.GetComponentInChildren<CharacterStats>().Focus.GetCurrStat().ToString();
            
        }
        else
        {
            enemyName.text = selectedEnemy.GetComponentInChildren<CharacterStats>().CharacterName;
            enemyStats.text = selectedEnemy.GetComponentInChildren<CharacterStats>().Attack.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().ElementAttack.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().Defense.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().Evasion.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().Speed.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().CurrStance.GetCurrStat().ToString() + "\n"
            + selectedEnemy.GetComponentInChildren<CharacterStats>().Focus.GetCurrStat().ToString();
        }
    }

    public void chooseMove()
    {
        //moveMenu.transform.position = new Vector3(moveMenu.transform.position.x,moveMenu.transform.position.y - 100.0f,moveMenu.transform.position.z);
        chooseMenu.SetActive(false);
        moveMenu.SetActive(true);
        //menuMovement(moveMenu,new Vector3(moveMenu.transform.position.x,moveMenu.transform.position.y + 100.0f,moveMenu.transform.position.z),0.1f);
        selectedPlayer.GetComponent<PlayerMovement>().enableWalk();
        cam.GetComponent<CameraMovement>().DisableMove();
        isWalking = true;
    }

    public void chooseAttack()
    {
        //Debug.Log(selectedPlayer.gameObject.name);
        if(selectedPlayer.transform.GetChild(0).Find("Stats").GetComponent<CharacterStats>().HasActed == false)
        {
            chooseMenu.SetActive(false);
            attackMenu.SetActive(true);
            menuMovement(attackMenu, -250.0f, 250.0f, 0.1f);
        }
    }

    public void chooseItem()
    {
        chooseMenu.SetActive(false);

    }

    public void chooseReturn()
    {
        chooseMenu.SetActive(false);
        playerHUD.SetActive(false);
        playerSelectCircle.SetActive(false);
    }

    public void returnFromAttack()
    {
        attackMenu.SetActive(false);
        menuMovement(chooseMenu, -250.0f, 250.0f, 0.1f);
    }

    public void returnFromWalk()
    {
        moveMenu.SetActive(false);
        menuMovement(chooseMenu, -250.0f, 250.0f, 0.1f);
        selectedPlayer.GetComponent<PlayerMovement>().disableWalk();
        cam.GetComponent<CameraMovement>().EnableMove();
        isWalking = false;
        //Return to original position?
    }

    public void toggleStats()
    {
        if(showPlayerStats == false)
        {
            menuMovement(playerStatsMenu, 210.0f, -210.0f, 0.1f);
            showPlayerStats = true;
        }
        else
        {
            //menuMovement(statsMenu, false, 0.0f, 210.0f, 0.1f);
            playerStatsMenu.SetActive(false);
            //StartCoroutine(waitTime(2.0f, statsMenu));
            showPlayerStats = false;
        }
    }

    public void toggleEnemyStats()
    {
        if(showEnemyStats == false)
        {
            menuMovement(enemyStatsMenu, 210.0f, -210.0f, 0.1f);
            showEnemyStats = true;
        }
        else
        {
            //menuMovement(statsMenu, false, 0.0f, 210.0f, 0.1f);
            enemyStatsMenu.SetActive(false);
            //StartCoroutine(waitTime(2.0f, statsMenu));
            showEnemyStats = false;
        }
    }

    public void confirmEndTurn()
    {
        confirmEndUI.SetActive(true);
        cam.GetComponent<CameraMovement>().DisableMove();
    }

    public void rejectEndTurn()
    {
        confirmEndUI.SetActive(false);
        cam.GetComponent<CameraMovement>().EnableMove();
    }

    public void endTurn()
    {
        playerTurn = false;
        chooseMenu.SetActive(false);
        attackMenu.SetActive(false);
        moveMenu.SetActive(false);
        playerStatsMenu.SetActive(false);
        playerHUD.SetActive(false);
        enemyStatsMenu.SetActive(false);
        enemyHUD.SetActive(false);
        endTurnUI.SetActive(false);
        confirmEndUI.SetActive(false);
        playerSelectCircle.SetActive(false);
        enemySelectCircle.SetActive(false);
        StartCoroutine(waitTime(2.0f));
    }

}
