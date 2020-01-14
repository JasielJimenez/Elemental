using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class AttackImporter : MonoBehaviour
{
    public static List<string> textArray;
    public string fileName;
    public bool doneReading;
    private TextAsset textAsset;
    public List<Attack> attackDatabase = new List<Attack>();

    // Start is called before the first frame update
    void Awake()
    {
        doneReading = false;
        textAsset = Resources.Load(fileName) as TextAsset;
        readTextFile();
    }

    public void readTextFile()
    {
        textArray = textAsset.text.Split ('\n').ToList();
        //Debug.Log(textArray.Count);
        for(int i = 0; i < textArray.Count; i = i + 11) 
        {
            //Debug.Log("|" + textArray[i] + "|");
            Attack attackData = new Attack();
            attackData.attackIndex = int.Parse(textArray[i]);
            attackData.attackName = textArray[i + 1];
            attackData.attackInfo = textArray[i + 2];
            attackData.addedDamage = int.Parse(textArray[i + 3]);
            attackData.elementDamage = (textArray[i + 4] == "true");
            attackData.accuracy = int.Parse(textArray[i + 5]);
            attackData.addedFocus = int.Parse(textArray[i + 6]);
            attackData.attackRangeX = int.Parse(textArray[i + 7]);
            attackData.attackRangeY = int.Parse(textArray[i + 8]);
            attackData.attackTime = int.Parse(textArray[i + 9]);
            attackDatabase.Add(attackData);
            //Debug.Log(attackDatabase.Count);
        }
        doneReading = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
