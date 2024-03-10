using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMenuButtons : MonoBehaviour
{
    public GameObject Cam;
    
    public GameObject BattleMenu;
    public GameObject PlayerMenu;
    public GameObject ActionsMenu;
    public GameObject WalkMenu;
    public GameObject CharacterInfoMenu;
    public GameObject StatsHud;
    public GameObject AttackMenu;
    public GameObject SkillsMenu;
    public GameObject EndTurnMenu;

    public GameObject SelectedObject;
    public GameObject WalkCircle;

    public GameObject BattleLog;
    public TextMeshProUGUI BattleLogText;

    public float RotateSpeed = 50;

    public bool IsRotatingAttack = false;

    private Colors UIColors = new Colors();

    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.Find("CameraBody");
        BattleMenu = this.GetComponent<TurnOrder>().BattleMenu;
        //BattleMenu.SetActive(false);
        PlayerMenu = BattleMenu.transform.GetChild(0).gameObject;
        ToggleMenu(nameof(PlayerMenu),false);
        ActionsMenu = PlayerMenu.transform.GetChild(0).gameObject;
        WalkMenu = PlayerMenu.transform.GetChild(1).gameObject;
        AttackMenu = PlayerMenu.transform.GetChild(2).gameObject;
        SkillsMenu = PlayerMenu.transform.GetChild(3).gameObject;
        EndTurnMenu = PlayerMenu.transform.GetChild(4).gameObject;

        CharacterInfoMenu = BattleMenu.transform.GetChild(2).gameObject;
        StatsHud = CharacterInfoMenu.transform.Find("StatsHud").gameObject;
        ToggleMenu(nameof(StatsHud),false);
        ToggleMenu(nameof(CharacterInfoMenu),false);

        BattleLog = GameObject.Find("BattleLog");
        BattleLogText = BattleLog.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        BattleLogText.text = "BATTLE START";
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
            case "ToggleStatsHud":
            ToggleStatsHud();
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
        this.GetComponent<DemoClick>().ClickDisabled = true;
        //Debug.Log(WalkCircle.name);
        ToggleMenu(nameof(WalkMenu),true);
        ToggleMenu(nameof(ActionsMenu),false);
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
        this.GetComponent<DemoClick>().ClickDisabled = false;
        ToggleMenu(nameof(WalkMenu),false);
        SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().HasWalked = true;
        ToggleMenu(nameof(ActionsMenu),true);
        this.GetComponent<DemoClick>().ToggleNavMesh(false);
        CharacterTurnFinished();
    }

    public void CancelWalk()
    {
        WalkCircle.SetActive(false);
        this.GetComponent<DemoClick>().ClickDisabled = false;
        ToggleMenu(nameof(WalkMenu),false);
        ToggleMenu(nameof(ActionsMenu),true);

        var walkCirclePosition = new Vector3(WalkCircle.transform.position.x, WalkCircle.transform.position.y, WalkCircle.transform.position.z);
        this.GetComponent<DemoClick>().ToggleNavMesh(false);
        SelectedObject.transform.position = walkCirclePosition;
    }

    #endregion

    #region Attack and Skill Menu

    public void EnableBasicAttack()
    {
        //Debug.Log(selectedPlayer.gameObject.name);
        this.GetComponent<CombatManager>().CreateAttackRange(0);
        ToggleMenu(nameof(AttackMenu),true);
        ToggleMenu(nameof(ActionsMenu),false);
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
        ToggleMenu(nameof(AttackMenu),false);
        this.GetComponent<CombatManager>().PlayerAction();
        //Cam.GetComponent<CameraMovement>().EnableMove();    //Call from coroutine in CombatManager
        IsRotatingAttack = false;
        //SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().HasActed = true;

        this.GetComponent<CombatManager>().BuffBeenApplied = false;
        //ActionsMenu.SetActive(true);
        //Debug.Log("From BattleMenu (BuffBeenApplied = " + SelectedObject.transform.GetChild(0).GetComponent<CharacterCombat>().BuffBeenApplied + ")");

        //CancelAttack();
        
    }

    public void ReopenMenuAfterAttack()
    {
        //CALL FROM IENUMERATOR IN COMBATMANAGER
    }

    public void CharacterTurnFinished()
    {
        //Called from CombatManager after player character has completed an action
        //Also called from here after confirming walk action
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
        this.GetComponent<CombatManager>().DeleteAttackRange();
        ToggleMenu(nameof(AttackMenu),false);
        ToggleMenu(nameof(ActionsMenu),true);
        Cam.GetComponent<CameraMovement>().EnableMove();
        IsRotatingAttack = false;
    }

    ///Opens Skill menu and updates buttons with all current attacks
    public void EnableSkills()
    {
        ToggleMenu(nameof(SkillsMenu),true);
        var skillList = SelectedObject.transform.GetChild(0).GetComponent<CharacterAttackList>().AttackList;
        
        //Change this to check better <--------------------------------------------------------------------------------------
        //Only allow 4 skills?
        for(int i = 0,j = 1; j < skillList.Count && j < 4; i++, j++)
        {
            SkillsMenu.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Text>().text = skillList[j].AttackName;
            Debug.Log("1st skill " + skillList[j].AttackName + " stamina cost: " + skillList[j].StaminaCost);
            if((SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().CurrStamina.GetCurrStat() <= 0 && skillList[j].StaminaCost > 0) || (SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>().CurrElement.GetCurrStat() <= 0 &&  skillList[j].ElementCost > 0))
            {
                SkillsMenu.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                SkillsMenu.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
        }
        SkillsMenu.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = skillList[1].AttackName;
        ToggleMenu(nameof(ActionsMenu),false);
    }

    public void ReturnFromSkills()
    {
        ToggleMenu(nameof(SkillsMenu),false);
        ToggleMenu(nameof(ActionsMenu),true);
    }

    public void ReturnFromCurrentSkill()
    {
        ToggleMenu(nameof(SkillsMenu),true);
    }

    public void UseSkill(int skillIndex)
    {
        ToggleMenu(nameof(SkillsMenu),false);
        this.GetComponent<CombatManager>().CreateAttackRange(skillIndex);
        ToggleMenu(nameof(AttackMenu),true);
        ToggleMenu(nameof(ActionsMenu),false);
        HandleAttackDirection(SelectedObject.transform.GetChild(0).GetComponent<CharacterAttackList>().AttackList[skillIndex].AttackDirection);
    }

    public void HandleAttackDirection(AttackDirectionType attackDirection)
    {
        Cam.GetComponent<CameraMovement>().DisableMove();
        //Debug.Log(attackDirection);
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

    #region ToggleButtons

    public void ToggleMenu(string menuName, bool toggle)
    {
        switch(menuName)
        {
            case nameof(BattleMenu):
            BattleMenu.SetActive(toggle);
            break;
            case nameof(PlayerMenu):
            PlayerMenu.SetActive(toggle);
            break;
            case nameof(ActionsMenu):
            ActionsMenu.SetActive(toggle);
            break;
            case nameof(WalkMenu):
            WalkMenu.SetActive(toggle);
            break;
            case nameof(CharacterInfoMenu):
            CharacterInfoMenu.SetActive(toggle);
            if(toggle)
            {
                this.GetComponent<CharacterHud>().UpdateCharacterHudMenu(SelectedObject);
            }
            break;
            case nameof(StatsHud):
            StatsHud.SetActive(toggle);
            break;
            case nameof(AttackMenu):
            AttackMenu.SetActive(toggle);
            break;
            case nameof(SkillsMenu):
            SkillsMenu.SetActive(toggle);
            break;
            case nameof(EndTurnMenu):
            EndTurnMenu.SetActive(toggle);
            break;
            default:
            Debug.Log("Invalid name for menu");
            break;
        }
    }

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
        var character = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
        if(character.HasActed && character.HasWalked)
        {
            this.GetComponent<TurnOrder>().EndCurrentPlayerTurn(1);
            return true;
        }
        return false;
    }

    public void EndTurn()
    {
        var character = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
        character.HasActed = true;
        character.HasWalked = true;
        this.GetComponent<TurnOrder>().EndCurrentPlayerTurn(1);
        DisablePlayerSelection();
    }

    //Calls DeselectCurrentObject() in DemoClick.cs
    public void DisablePlayerSelection()
    {
        //this.GetComponent<DemoClick>().ClickDisabled = false;
        //ToggleMenu(nameof(PlayerMenu),false);
        //ToggleMenu(nameof(CharacterInfoMenu),false);
        //ToggleMenu("CharacterInfoMenu",false);
        //SelectedObject.transform.GetChild(1).gameObject.SetActive(false);
        //if(WalkMenu.activeSelf)
        //{
        //    CancelWalk();
        //}
        //if(AttackMenu.activeSelf)
        //{
        //    CancelAttack();
        //}
        //this.GetComponent<DemoClick>().DeselectCurrentObject();
        //SelectedObject = null;
        this.GetComponent<DemoClick>().DeselectCurrentObject();
    }
    #endregion

    #region StatsHud

    public void ToggleStatsHud()
    {
        if(StatsHud.activeSelf)
        {
            ToggleMenu(nameof(StatsHud),false);
        }
        else
        {
            ToggleMenu(nameof(StatsHud),true);
        }
    }

    #endregion

    #region Damage Log

    public void AddToDamageLog(string newLog)
    {
        BattleLogText.text += "\n * " + newLog;
    }

    #endregion
}
