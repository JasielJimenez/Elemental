using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrder : MonoBehaviour
{
    public enum CurrTurnEnum
    {
        Player,
        Enemy
    }
    public CurrTurnEnum CurrentTurn = CurrTurnEnum.Player;

    public GameObject cam;

    public int NumPlayers;
    public int NumEnemies;

    public int PlayerActionsRemaining;
    public int EnemyActionsRemaining;

    public GameObject PlayerList;
    public GameObject EnemyList;

    public int CurrentEnemyIndex = 0;

    public GameObject BattleMenu;
    public GameObject TurnMenu;
    public GameObject ChangeTurnWindow;
    public Text TurnText;
    public Text CurrentTurnText;

    // Start is called before the first frame update
    void Awake()
    {
        //Gets and sets Current Turn Text
        BattleMenu = GameObject.Find("BattleHud");
        TurnMenu  = BattleMenu.transform.GetChild(1).gameObject;
        ChangeTurnWindow = TurnMenu.transform.GetChild(0).gameObject;
        ChangeTurnWindow.SetActive(false);
        TurnText = ChangeTurnWindow.transform.GetChild(1).GetComponent<Text>();
        CurrentTurnText = TurnMenu.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        CurrentTurnText.text = "PLAYER TURN";
        
        //Gets list of enemies and players
        PlayerList = GameObject.Find("PlayerList");
        NumPlayers = PlayerList.transform.childCount;
        EnemyList = GameObject.Find("EnemyList");
        NumEnemies = EnemyList.transform.childCount;
        PlayerActionsRemaining = NumPlayers;
        EnemyActionsRemaining = 0;
        Debug.Log($"There is {NumEnemies} enemy in this room.");

        cam = GameObject.Find("CameraBody");
    }

    ///Sets 
    public void ChangeTurn()
    {
        this.GetComponent<DemoClick>().PlayerTurn = false;
        switch(CurrentTurn)
        {
            case CurrTurnEnum.Player:
                //Now it is the enemy's turn
                CurrentTurn = CurrTurnEnum.Enemy;
                //NOTE: HANDLE DOWNED ENEMIES
                NumEnemies = EnemyList.transform.childCount;
                EnemyActionsRemaining = NumEnemies;
                TurnText.text = "ENEMY TURN";
                CurrentTurnText.text = "ENEMY TURN";
                cam.GetComponent<CameraMovement>().DisableMove();
                StartCoroutine(DisplayTurnChangeText(CurrentTurn));
                //HandleEnemyTurn();
            break;
            case CurrTurnEnum.Enemy:
                //Now it is the player's turn
                CurrentTurn = CurrTurnEnum.Player;
                //NOTE: HANDLE DOWNED PLAYERS
                NumPlayers = PlayerList.transform.childCount;
                PlayerActionsRemaining = NumPlayers;
                TurnText.text = "PLAYER TURN";
                CurrentTurnText.text = "PLAYER TURN";
                cam.GetComponent<CameraMovement>().EnableMove();
                StartCoroutine(DisplayTurnChangeText(CurrentTurn));
            break;
            default:
                Debug.Log("ERROR: NOBODY'S TURN");
                CurrentTurn = CurrTurnEnum.Player;
                ChangeTurn();
            break;
        }
    }

    ///Changes display text for a few seconds to show whose turn it is
    ///Then starts enemy's turn or player's turn
    IEnumerator DisplayTurnChangeText(CurrTurnEnum test)
    {
        ChangeTurnWindow.SetActive(true);
        yield return new WaitForSeconds(2);
        ChangeTurnWindow.SetActive(false);

        if(test == CurrTurnEnum.Enemy)
        {
            //First enemy's IsSelected circle glows
            EnemyList.transform.GetChild(CurrentEnemyIndex).GetChild(0).GetChild(1).gameObject.SetActive(true);
            //First enemy's turn starts
            EnemyList.transform.GetChild(CurrentEnemyIndex).GetChild(0).GetChild(0).gameObject.GetComponent<EnemyBehaviour>().ChooseBestAttack();
        }
        else
        {
            this.GetComponent<DemoClick>().PlayerTurn = true;
            UpdateCharacters();
        }
    }

    ///After an enemy has their turn, checks to see if there are other enemies who have yet to act
    ///Called from EnemyBehavior script
    public void HandleNextEnemyTurn()
    {
        //Hide current enemy's IsSelected circle
        EnemyList.transform.GetChild(CurrentEnemyIndex).GetChild(0).GetChild(1).gameObject.SetActive(false);

        CurrentEnemyIndex++;
        if(CurrentEnemyIndex < NumEnemies)
        {
            //Next enemy's turn starts
            EnemyList.transform.GetChild(CurrentEnemyIndex).GetChild(0).GetChild(1).gameObject.SetActive(true);
            //Next enemy's turn starts
            EnemyList.transform.GetChild(CurrentEnemyIndex).GetChild(0).GetChild(0).gameObject.GetComponent<EnemyBehaviour>().ChooseBestAttack();
        }
        else
        {
            CurrentEnemyIndex = 0;
            ChangeTurn();
        }
    }

    //IEnumerator DemoEnemyTurn()
    //{
    //    Debug.Log("DemoEnemyTurn start");
    //    yield return new WaitForSeconds(2);
    //    Debug.Log("DemoEnemyTurn end");
    //    EnemyActionsRemaining = 0;
    //    ChangeTurn();
    //}

    ///Sets all players' walk circles to their current location
    ///Sets all players' to be ready for actions
    public void UpdateCharacters()
    {
        for(int i = 0; i < NumPlayers; i++)
        {
            var currCharacter = PlayerList.transform.GetChild(i);
            var walkCircle = currCharacter.GetChild(0);
            var character = currCharacter.GetChild(1);
            walkCircle.transform.position = new Vector3(character.position.x, walkCircle.position.y, character.position.z);

            character.GetChild(0).gameObject.GetComponent<CharacterStats>().HasActed = false;
            character.GetChild(0).gameObject.GetComponent<CharacterStats>().HasWalked = false;
        }
    }

    ///Ends the turn of the currently selected player
    public void EndCurrentPlayerTurn(int actionsEnded)
    {
        PlayerActionsRemaining -= actionsEnded;
        if(PlayerActionsRemaining <= 0)
        {
            ChangeTurn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
