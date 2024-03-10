using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterHud : MonoBehaviour
{   
    private Colors UIColors = new Colors();

    public GameObject CharacterInfoMenu;

    // Start is called before the first frame update
    void Start()
    {
        CharacterInfoMenu = this.GetComponent<TurnOrder>().BattleMenu.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCharacterHudMenu(GameObject SelectedObject)
    {
        var characterHud = CharacterInfoMenu.transform.Find("CharacterHud").gameObject;
        var statsHud = CharacterInfoMenu.transform.Find("StatsHud").gameObject;
        var characterStats = SelectedObject.transform.GetChild(0).GetComponent<CharacterStats>();
        
        //Name
        var name = characterHud.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        name.text = characterStats.CharacterName;
        
        //Health
        var healthBar = characterHud.transform.GetChild(2).Find("HealthBarScale");
        var healthText = characterHud.transform.GetChild(2).Find("HealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = characterStats.CurrHealth.GetCurrStat() + "/" + characterStats.MaxHealth.GetCurrStat();
        float healthRemaining = (float)characterStats.CurrHealth.GetCurrStat() / (float)characterStats.MaxHealth.GetCurrStat();
        healthBar.transform.localScale = new Vector3(healthRemaining, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

        //Stamina
        var staminaBar = characterHud.transform.GetChild(3).Find("StaminaBarScale");
        var staminaText = characterHud.transform.GetChild(3).Find("StaminaText").GetComponent<TextMeshProUGUI>();
        staminaText.text = characterStats.CurrStamina.GetCurrStat() + "/" + characterStats.MaxStamina.GetCurrStat();
        float staminaRemaining = 0;
        if(characterStats.MaxStamina.GetCurrStat() != 0 && characterStats.CurrStamina.GetCurrStat() < 0 )
        {
            //SHOW NEGATIVE VALUE (MAYBE MAKE GUAGE RED AND INVERT DIRECTION?)
            staminaBar.transform.GetChild(0).GetComponent<Image>().color = UIColors.NegativeStaminaColor;
            staminaRemaining = -(float)characterStats.CurrStamina.GetCurrStat() / (float)characterStats.MaxStamina.GetCurrStat();
        }
        else if(characterStats.MaxStamina.GetCurrStat() != 0)
        {
            staminaBar.transform.GetChild(0).GetComponent<Image>().color = UIColors.PositiveStaminaColor;
            staminaRemaining = (float)characterStats.CurrStamina.GetCurrStat() / (float)characterStats.MaxStamina.GetCurrStat();
        }
        staminaBar.transform.localScale = new Vector3(staminaRemaining, staminaBar.transform.localScale.y, staminaBar.transform.localScale.z);
        
        //Element
        var elementBar = characterHud.transform.GetChild(4).Find("ElementBarScale");
        var elementText = characterHud.transform.GetChild(4).Find("ElementText").GetComponent<TextMeshProUGUI>();
        elementText.text = characterStats.CurrElement.GetCurrStat() + "/" + characterStats.MaxElement.GetCurrStat();
        float elementRemaining = 0;
        if(characterStats.MaxElement.GetCurrStat() != 0 && characterStats.CurrElement.GetCurrStat() < 0 )
        {
            //SHOW NEGATIVE VALUE (MAYBE MAKE GUAGE RED AND INVERT DIRECTION?)
            elementBar.transform.GetChild(0).GetComponent<Image>().color = UIColors.NegativeElementColor;
            elementRemaining = -(float)characterStats.CurrElement.GetCurrStat() / (float)characterStats.MaxElement.GetCurrStat();
        }
        else if(characterStats.MaxElement.GetCurrStat() != 0)
        {
            elementBar.transform.GetChild(0).GetComponent<Image>().color = UIColors.PositiveElementColor;
            elementRemaining = (float)characterStats.CurrElement.GetCurrStat() / (float)characterStats.MaxElement.GetCurrStat();
        }
        elementBar.transform.localScale = new Vector3(elementRemaining, elementBar.transform.localScale.y, elementBar.transform.localScale.z);

        //Stance
        var stanceBar = characterHud.transform.GetChild(5).Find("StanceBarScale");
        float stanceRemaining = 0;
        if(characterStats.MaxStance.GetCurrStat() != 0)
        {
            stanceRemaining = (float)characterStats.CurrStance.GetCurrStat() / (float)characterStats.MaxStance.GetCurrStat();
        }
        stanceBar.transform.localScale = new Vector3(stanceBar.transform.localScale.x, stanceRemaining, stanceBar.transform.localScale.z);

        //Update portrait
        

        //Update Stats
        var attackStat = statsHud.transform.Find("AttackStatText").GetComponent<TextMeshProUGUI>();
        attackStat.text = characterStats.Attack.GetCurrStat().ToString();

        var defenseStat = statsHud.transform.Find("DefenseStatText").GetComponent<TextMeshProUGUI>();
        defenseStat.text = characterStats.Defense.GetCurrStat().ToString();

        var elementAttackStat = statsHud.transform.Find("ElementAttackStatText").GetComponent<TextMeshProUGUI>();
        elementAttackStat.text = characterStats.ElementAttack.GetCurrStat().ToString();

        var evasionStat = statsHud.transform.Find("EvasionStatText").GetComponent<TextMeshProUGUI>();
        evasionStat.text = characterStats.Evasion.GetCurrStat().ToString();

        var focusStat = statsHud.transform.Find("FocusStatText").GetComponent<TextMeshProUGUI>();
        focusStat.text = characterStats.Focus.GetCurrStat().ToString();
    }
}
