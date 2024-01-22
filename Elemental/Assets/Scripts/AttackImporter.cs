using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class AttackImporter : MonoBehaviour
{
    public static List<string> TextArray;
    public string FileName;
    public bool DoneReading;
    private TextAsset TextAsset;
    public List<Attack> AttackDatabaseList = new List<Attack>();
    public List<List<int>> DemoCharacterAttacks = new List<List<int>>();

    // Start is called before the first frame update
    void Awake()
    {
        DoneReading = false;
        TextAsset = Resources.Load("TextFiles/" + FileName) as TextAsset;
        ReadTextFile();
        UpdatePlayerMovesets();
        var PlayerList = GameObject.Find("PlayerList");
        var NumPlayers = PlayerList.transform.childCount;
    }

    private void CreateDemoMovesets()
    {
        var characterOneAttacks = new List<int>() {0,1,2};
        var characterTwoAttacks = new List<int>() {0,3,2};
        DemoCharacterAttacks.Add(characterOneAttacks);
        DemoCharacterAttacks.Add(characterTwoAttacks);
    }

    public void UpdatePlayerMovesets()
    {
        CreateDemoMovesets();
        var PlayerList = GameObject.Find("PlayerList");
        var NumPlayers = PlayerList.transform.childCount;
        for(int i = 0; i < NumPlayers; i++)
        {
            var CharacterAttackList = new List<Attack>();
            for(int j = 0; j < DemoCharacterAttacks[i].Count; j++)
            {
                CharacterAttackList.Add(AttackDatabaseList[DemoCharacterAttacks[i][j]]);
            }
            PlayerList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<CharacterAttackList>().AttackList = CharacterAttackList;
        }
    }

    public void ReadTextFile()
    {
        //Debug.Log("TextAsset 1st: " + TextAsset);
        TextArray = TextAsset.text.Split (',').ToList();
        //Debug.Log("TextArray 1st: " + TextArray[0]);
        //Debug.Log(TextArray.Count);
        for(int i = 0; i < TextArray.Count; i = i + 28)     //MAKE SURE IT ITERATES ON THE CORRECT NUMBER
        {
            //Debug.Log("|" + TextArray[i] + "|");
            Attack attackData = new Attack();
            attackData.AttackIndex = int.Parse(TextArray[i]);
            attackData.AttackName = TextArray[i + 1];
            attackData.AttackInfo = TextArray[i + 2];
            attackData.AttackRange = FindGameObjectByName("Prefabs/AttackRanges/" + TextArray[i + 3]);
            attackData.AttackDirection = (AttackDirectionType)Enum.Parse(typeof(AttackDirectionType), TextArray[i + 4]); //AttackDirectionType
            attackData.ParticleEffects = FindGameObjectByName("Prefabs/ParticleEffects/" + TextArray[i + 5]);
            attackData.AddedDamage = int.Parse(TextArray[i + 6]);
            attackData.ElementDamage = int.Parse(TextArray[i + 7]);
            attackData.Accuracy = int.Parse(TextArray[i + 8]);
            attackData.AddedFocus = int.Parse(TextArray[i + 9]);
            attackData.AttackTime = int.Parse(TextArray[i + 10]);
            attackData.StaminaCost = int.Parse(TextArray[i + 11]);
            attackData.ElementCost = int.Parse(TextArray[i + 12]);
            attackData.WillMovePlayer = bool.Parse(TextArray[i + 13]);
            attackData.MovePlayerDistance = int.Parse(TextArray[i + 14]);
            attackData.WillMoveEnemy = bool.Parse(TextArray[i + 15]);
            attackData.MoveEnemyDistance = int.Parse(TextArray[i + 16]);
            attackData.IsCharge = bool.Parse(TextArray[i + 17]);
            attackData.TurnsToCharge = int.Parse(TextArray[i + 18]);
            attackData.IsDebuff = bool.Parse(TextArray[i + 19]);
            attackData.DebuffList = CreateStatChangeList(TextArray[i + 20]);
            attackData.IsBuff = bool.Parse(TextArray[i + 21]);
            attackData.BuffList = CreateStatChangeList(TextArray[i + 22]);
            attackData.IsStatusAilment = bool.Parse(TextArray[i + 23]);
            attackData.isGrazing = bool.Parse(TextArray[i + 24]);
            attackData.isUnavoidable = bool.Parse(TextArray[i + 25]);
            attackData.isPiercing = bool.Parse(TextArray[i + 26]);
            attackData.isUnBlockable = bool.Parse(TextArray[i + 27]);

            
            //Debug.Log("| Adding attack to list |");


            AttackDatabaseList.Add(attackData);
            //Debug.Log(AttackDatabaseList.Count);
        }
        DoneReading = true;
    }

    private List<StatChange> CreateStatChangeList(string value)
    {
        var test = new List<StatChange>();
        if(value == "none")
        {
            //NO STAT CHANGE
            return test;
        }
        var statChangeText = value.Split ('|').ToList();
        //Debug.Log("statChangeText 1st: " + statChangeText[0]);
        //Debug.Log(statChangeText.Count);
        for(int i = 0; i < statChangeText.Count; i = i + 4)     //MAKE SURE IT ITERATES ON THE CORRECT NUMBER
        {
            //Debug.Log("|" + statChangeText[i] + "|");
            StatChange statChangeData = new StatChange();
            statChangeData.SetStatName(statChangeText[i]);
            statChangeData.SetNumChange(int.Parse(statChangeText[i + 1]));
            statChangeData.SetChangeDuration(int.Parse(statChangeText[i + 2]));
            statChangeData.SetIsPercentChange(bool.Parse(statChangeText[i + 3]));
            
            //Debug.Log("| Adding stat change to list |");

            test.Add(statChangeData);
            //Debug.Log(statChangeDatabaseList.Count);
        }
        return test;
    }

    private GameObject FindGameObjectByName(string value)
    {
        var objectToFind = Resources.Load(value) as GameObject;
        return objectToFind;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
