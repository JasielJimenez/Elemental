using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuButtons : MonoBehaviour
{
    public GameObject Cam;
    
    public GameObject BattleMenu;
    public GameObject PlayerMenu;
    public GameObject ActionsMenu;
    public GameObject WalkMenu;
    public GameObject AttackMenu;
    public GameObject SkillsMenu;
    public GameObject EndTurnMenu;

    public GameObject SelectedObject;
    public GameObject WalkCircle;

    public float RotateSpeed = 50;

    public bool IsRotatingAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.Find("CameraBody");
        BattleMenu = this.GetComponent<TurnOrder>().BattleMenu;
        //BattleMenu.SetActive(false);
        PlayerMenu = BattleMenu.transform.GetChild(0).gameObject;
        PlayerMenu.SetActive(false);
        ActionsMenu = PlayerMenu.transform.GetChild(0).gameObject;
        WalkMenu = PlayerMenu.transform.GetChild(1).gameObject;
        AttackMenu = PlayerMenu.transform.GetChild(2).gameObject;
        SkillsMenu = PlayerMenu.transform.GetChild(3).gameObject;
        EndTurnMenu = PlayerMenu.transform.GetChild(4).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsRotatingAttack)
        {
            float rotation = Input.GetAxis("Horizontal") * RotateSpeed;
            rotation *= Time.deltaTime;
            SelectedObject.transform.Rotate(0, rotation, 0);
        }
    }

    public void HandleMenuButton(string menuAction)
    {
        switch(menuAction)
        {
            case "EnableWalkMenu":
            EnableWalk();
            break;
            case "ConfirmWalk":
            ConfirmWalk();
            break;
            case "CancelWalk":
            CancelWalk();
            break;
            case "EnableBasicAttackMenu":
            EnableBasicAttack();
            break;
            case "ConfirmAttack":
            ConfirmAttack();
            break;
            case "CancelAttack":
            CancelAttack();
            break;
            case "EnableSkillsMenu":
            EnableSkills();
            break;
            case "CancelSkillsMenu":
            ReturnFromSkills();
            break;
            case "CancelCurrentSkill":
            ReturnFromCurrentSkill();
            break;
            case "EndTurn":
            EndTurn();
            break;
            case "UseSkillOne":
            UseSkill(1);
            break;
            case "UseSkillTwo":
            UseSkill(2);
            break;
            case "UseSkillThree":
            UseSkill(3);
            break;
            case "UseSkillFour":
            UseSkill(4);
            break;
            case "UseSkillFive":
            UseSkill(5);
            break;
            default:
            Debug.Log("Invalid name for button");
            break;
        }
    }

    #region Walk Menu

    private void EnableWalk()
    {
        //PlayerMenu.transform.position = new Vector3(PlayerMenu.transform.position.x,PlayerMenu.transform.position.y - 100.0f,PlayerMenu.transform.position.z);
        //BattleMenu.SetActive(false);
        WalkCircle = SelectedObject.transform.parent.GetChild(0).gameObject;
        WalkCircle.SetActive(true);
        this.GetComponent<DemoClick>().IsWalkingPhase = true;
        Debug.Log(WalkCircle.name);
        WalkMenu.SetActive(true);
        ActionsMenu.SetActive(false);
        this.GetComponent<DemoClick>().ToggleNavMesh(true);
        //PlayerMenu.SetActive(true);
        //menuMovement(PlayerMenu,new Vector3(PlayerMenu.transform.position.x,PlayerMenu.transform.position.y + 100.0f,PlayerMenu.transform.position.z),0.1f);
        //SelectedPlayer.GetComponent<PlayerMovement>().enableWalk();
        //Cam.GetComponent<CameraMovement>().disableMove();
        //isWalking = true;
    }

    public void ConfirmWalk()
    {
        WalkCircle.SetActive(false);
        this.GetComponent<DemoClick>().IsWalkingPhase = false;
        WalkMenu.SetActive(false);
        SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().HasWalked = true;
        ActionsMenu.SetActive(true);
        this.GetComponent<DemoClick>().ToggleNavMesh(false);
        if(!CheckForEndTurn())
        {
            this.GetComponent<DemoClick>().CheckAvailableActions();
        }
        else
        {
            DisablePlayerSelection();
        }
    }

    public void CancelWalk()
    {
        WalkCircle.SetActive(false);
        this.GetComponent<DemoClick>().IsWalkingPhase = false;
        WalkMenu.SetActive(false);
        ActionsMenu.SetActive(true);

        //NEEDS SOME TWEAKING
        var test =  new Vector3(WalkCircle.transform.position.x, WalkCircle.transform.position.y, this.transform.position.z);
        this.GetComponent<DemoClick>().ToggleNavMesh(false);
        SelectedObject.transform.position = test;
    }

    #endregion

    #region Attack and Skill Menu

    public void EnableBasicAttack()
    {
        //Debug.Log(selectedPlayer.gameObject.name);
        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().CreateAttackRange(0);
        AttackMenu.SetActive(true);
        ActionsMenu.SetActive(false);
        HandleAttackDirection(SelectedObject.transform.GetChild(0).GetComponent<CharacterAttackList>().AttackList[0].AttackDirection);
        /*
        if(selectedPlayer.transform.GetChild(0).Find("Stats").GetComponent<CharacterStats>().hasAttacked == false)
        {
            chooseMenu.SetActive(false);
            attackMenu.SetActive(true);
            menuMovement(attackMenu, -250.0f, 250.0f, 0.1f);
        }
        */
    }

    public void ConfirmAttack()
    {
        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().DamageStep();
        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().DeleteAttackRange();
        AttackMenu.SetActive(false);
        Cam.GetComponent<CameraMovement>().EnableMove();
        IsRotatingAttack = false;
        SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().HasActed = true;

        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().BuffBeenApplied = false;
        Debug.Log("From BattleMenu (BuffBeenApplied = " + SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().BuffBeenApplied + ")");

        CancelAttack();
        if(!CheckForEndTurn())
        {
            this.GetComponent<DemoClick>().CheckAvailableActions();
        }
        else
        {
            
            DisablePlayerSelection();
        }
    }

    public void CancelAttack()
    {
        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().DeleteAttackRange();
        AttackMenu.SetActive(false);
        ActionsMenu.SetActive(true);
        Cam.GetComponent<CameraMovement>().EnableMove();
        IsRotatingAttack = false;
    }

    ///Opens Skill menu and updates buttons with all current attacks
    public void EnableSkills()
    {
        SkillsMenu.SetActive(true);
        var skillList = SelectedObject.transform.GetChild(0).GetComponent<CharacterAttackList>().AttackList;
        
        //Change this to check better <--------------------------------------------------------------------------------------
        //Only allow 4 skills?
        for(int i = 0,j = 1; j < skillList.Count && j < 4; i++, j++)
        {
            SkillsMenu.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = skillList[j].AttackName;
        }
        SkillsMenu.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = skillList[1].AttackName;
        ActionsMenu.SetActive(false);
    }

    public void ReturnFromSkills()
    {
        SkillsMenu.SetActive(false);
        ActionsMenu.SetActive(true);
    }

    public void ReturnFromCurrentSkill()
    {
        SkillsMenu.SetActive(true);
    }

    public void UseSkill(int skillIndex)
    {
        SkillsMenu.SetActive(false);
        SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().CreateAttackRange(skillIndex);
        AttackMenu.SetActive(true);
        ActionsMenu.SetActive(false);
        HandleAttackDirection(SelectedObject.transform.GetChild(0).GetComponent<CharacterAttackList>().AttackList[skillIndex].AttackDirection);
    }

    public void HandleAttackDirection(AttackDirectionType attackDirection)
    {
        Cam.GetComponent<CameraMovement>().DisableMove();
        Debug.Log(attackDirection);
        switch(attackDirection)
        {
            case AttackDirectionType.Center:
            break;
            case AttackDirectionType.Directional:
                Debug.Log("Directional Attack");
                IsRotatingAttack = true;
            break;
            case AttackDirectionType.Specify:
            break;
        }
    }

    #endregion

    /*
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
        cam.GetComponent<CameraMovement>().disableMove();
    }

    public void rejectEndTurn()
    {
        confirmEndUI.SetActive(false);
        cam.GetComponent<CameraMovement>().enableMove();
    }

    */

    #region ToggleButtons

    public void ToggleWalkButton(bool buttonAvailable)
    {
        ActionsMenu.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = buttonAvailable;
    }

    public void ToggleActionButtons(bool buttonAvailable)
    {
        ActionsMenu.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = buttonAvailable;
        ActionsMenu.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = buttonAvailable;
        ActionsMenu.transform.GetChild(3).gameObject.GetComponent<Button>().interactable = buttonAvailable;
    }

    public void ToggleConfirmCancelWalkButton(bool confirmButtonAvailable, bool cancelButtonAvailable)
    {
        WalkMenu.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = confirmButtonAvailable;
        WalkMenu.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = cancelButtonAvailable;
    }

    public void ToggleEndTurnButton(bool buttonAvailable)
    {
        EndTurnMenu.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = buttonAvailable;
    }

    #endregion

    #region EndTurn

    public bool CheckForEndTurn()
    {
        var test = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
        if(test.HasActed && test.HasWalked)
        {
            this.GetComponent<TurnOrder>().EndCurrentPlayerTurn(1);
            return true;
        }
        return false;
    }

    public void EndTurn()
    {
        var test = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
        test.HasActed = true;
        test.HasWalked = true;
        this.GetComponent<TurnOrder>().EndCurrentPlayerTurn(1);
        DisablePlayerSelection();
    }

    public void DisablePlayerSelection()
    {
        this.GetComponent<DemoClick>().IsWalkingPhase = false;
        PlayerMenu.SetActive(false);
        SelectedObject.transform.GetChild(1).gameObject.SetActive(false);
        if(WalkMenu.activeSelf)
        {
            CancelWalk();
        }
        if(AttackMenu.activeSelf)
        {
            CancelAttack();
        }
        SelectedObject = null;
    }
    #endregion
}
