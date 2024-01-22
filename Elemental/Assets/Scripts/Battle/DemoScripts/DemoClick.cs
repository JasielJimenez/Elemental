using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class DemoClick : MonoBehaviour
{
    public GameObject SelectedObject;
    public bool PlayerTurn;
    public bool ClickDisabled;
    public bool IsCurrentlyMoving;
    public enum SelectObjectEnum
    {
        None,
        Player,
        Enemy,
        Object
    }
    public SelectObjectEnum CurrentSelection = SelectObjectEnum.None;
    public LayerMask Ground;
    private NavMeshAgent MyAgent;

    public bool test = false;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerTurn = true;
        ClickDisabled = false;
        IsCurrentlyMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerTurn == true && ClickDisabled == false)
        {
            if(SelectedObject != null)
            {
                if(SelectedObject.transform.position.x > 0)
                {
                    test = true;
                }
                else
                {
                    test = false;
                }
            }
            //On player turn, allow player to click on objects
            if(Input.GetMouseButtonDown(0))
            {
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
                        //Select player, open player menu, and show player stats (DOESN'T SHOW PLAYER STATS YET)
                        DeselectCurrentObject();
                        SelectedObject = hit.transform.gameObject;
                        Debug.Log("Player selected");
                        CurrentSelection = SelectObjectEnum.Player;
                        //Show player menu
                        var battleMenu = this.GetComponent<BattleMenuButtons>();
                        battleMenu.SelectedObject = SelectedObject;
                        CheckAvailableActions();
                        battleMenu.ToggleMenu(nameof(battleMenu.PlayerMenu),true);
                        battleMenu.ToggleMenu(nameof(battleMenu.ActionsMenu),true);
                        battleMenu.ToggleMenu(nameof(battleMenu.WalkMenu),false);
                        battleMenu.ToggleMenu(nameof(battleMenu.SkillsMenu),false);
                        battleMenu.ToggleMenu(nameof(battleMenu.CharacterInfoMenu),true);
                        this.GetComponent<CombatManager>().SetAttacker(SelectedObject);
                        //Show selection circle
                        SelectedObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if(hit.transform.tag == "Enemy")
                    {
                        //Select enemy and display enemy stats, abilities, etc (DOESN'T SHOW ENEMY STATS YET)
                        DeselectCurrentObject();
                        SelectedObject = hit.transform.gameObject;
                        Debug.Log("Enemy selected");
                        CurrentSelection = SelectObjectEnum.Enemy;
                        //Hide player menu
                        var battleMenu = this.GetComponent<BattleMenuButtons>();
                        battleMenu.SelectedObject = SelectedObject;
                        battleMenu.ToggleMenu("PlayerMenu",false);
                        battleMenu.ToggleMenu(nameof(battleMenu.CharacterInfoMenu),true);
                        this.GetComponent<CombatManager>().SetAttacker(SelectedObject);
                        //Show selection circle
                        SelectedObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if(hit.transform.tag == "EnvironmentObject")
                    {
                        //Select object and display object stats (DOESN'T SHOW OBJECT STATS YET)
                        DeselectCurrentObject();
                        SelectedObject = hit.transform.gameObject;
                        Debug.Log("Object selected");
                        CurrentSelection = SelectObjectEnum.Object;
                        //Hide player menu
                        var battleMenu = this.GetComponent<BattleMenuButtons>();
                        battleMenu.SelectedObject = SelectedObject;
                        battleMenu.ToggleMenu("PlayerMenu",false);
                        battleMenu.ToggleMenu(nameof(battleMenu.CharacterInfoMenu),true);
                        this.GetComponent<CombatManager>().SetAttacker(SelectedObject);
                        //Show selection circle
                        SelectedObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        //NEEDS SOME FINE TUNING
                        //Deselect current character when not clicking on a character
                        DeselectCurrentObject();
                    }
                }
            }
        }
        else if(ClickDisabled == true && CurrentSelection == SelectObjectEnum.Player)
        {
            //This is for handling point + click for moving a character
            if(!IsCurrentlyMoving)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 100, Ground))
                    {
                      //Only register clicks inside of the character's walk circle
                        if(hit.transform.tag != "WalkCircle")
                        {
                            return;
                        }
                        if(SelectedObject != null)
                        {
                            //Move character to selected point within walk circle
                            MyAgent = SelectedObject.GetComponent<NavMeshAgent>();
                            MyAgent.isStopped = false;
                            SelectedObject.transform.LookAt(new Vector3(hit.point.x, SelectedObject.transform.position.y, hit.point.z));
                            MyAgent.SetDestination(hit.point);
                            IsCurrentlyMoving = true;
                            this.GetComponent<BattleMenuButtons>().ToggleConfirmCancelWalkButton(false, false);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log(MyAgent.remainingDistance);
                if(MyAgent.remainingDistance <= 0.1)
                {
                    print("Destination reached");
                    IsCurrentlyMoving = false;
                    MyAgent.isStopped = true;
                    this.GetComponent<BattleMenuButtons>().ToggleConfirmCancelWalkButton(true, true);
                }
            }
            
        }
    }

    //PERHAPS MOVE THIS TO TURNORDER?
    public void CheckAvailableActions()
    {
        if(SelectedObject != null)
        {
            var character = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
            if((character.HasWalked && character.HasActed) || character.isDowned)
            {
                //DEACTIVATE ALL BUTTONS
                this.GetComponent<BattleMenuButtons>().ToggleWalkButton(false);
                this.GetComponent<BattleMenuButtons>().ToggleActionButtons(false);
                this.GetComponent<BattleMenuButtons>().ToggleEndTurnButton(false);
                //this.GetComponent<BattleMenuButtons>().ActionsMenu.SetActive(false);
            }
            else if(character.HasWalked)
            {
                //DEACTIVATE WALK BUTTON
                this.GetComponent<BattleMenuButtons>().ToggleWalkButton(false);
                this.GetComponent<BattleMenuButtons>().ToggleActionButtons(true);
                this.GetComponent<BattleMenuButtons>().ToggleMenu("ActionsMenu",true);
            }
            else if(character.HasActed)
            {
                //DEACTIVATE ALL BUTTONS BUT WALK
                this.GetComponent<BattleMenuButtons>().ToggleWalkButton(true);
                this.GetComponent<BattleMenuButtons>().ToggleActionButtons(false);
                this.GetComponent<BattleMenuButtons>().ToggleMenu("ActionsMenu",true);
            }
            else
            {
                //ACTIVATE ALL BUTTONS
                this.GetComponent<BattleMenuButtons>().ToggleWalkButton(true);
                this.GetComponent<BattleMenuButtons>().ToggleActionButtons(true);
                this.GetComponent<BattleMenuButtons>().ToggleEndTurnButton(true);
            }
        }
    }

    public void ToggleNavMesh(bool toggle)
    {
        if(SelectedObject != null)
        {
            MyAgent = SelectedObject.GetComponent<NavMeshAgent>();
            if(toggle)
            {
                MyAgent.enabled = true;
            }
            else
            {
                MyAgent.isStopped = true;
                MyAgent.enabled = false;
            }
        }
    }

    // Sets SelectedObject to null
    public void DeselectCurrentObject()
    {
        if(SelectedObject != null)
        {
            var battleMenuButtons = this.GetComponent<BattleMenuButtons>();
            ClickDisabled = false;
            battleMenuButtons.ToggleMenu("PlayerMenu",false);
            battleMenuButtons.ToggleMenu("CharacterInfoMenu",false);
            //ToggleMenu("CharacterInfoMenu",false);

            if(battleMenuButtons.WalkMenu.activeSelf)
            {
                battleMenuButtons.CancelWalk();
            }
            if(battleMenuButtons.AttackMenu.activeSelf)
            {
                battleMenuButtons.CancelAttack();
            }

            SelectedObject.transform.GetChild(1).gameObject.SetActive(false);
            SelectedObject = null;
            battleMenuButtons.SelectedObject = null;
            this.GetComponent<CombatManager>().SetAttacker(null);
            //this.GetComponent<BattleMenuButtons>().ToggleMenu("CharacterInfoMenu",false);
        }
    }
}
