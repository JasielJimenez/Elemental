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
        textArray = textAsset.text.Split (',').ToList();
        //Debug.Log(textArray.Count);
        for(int i = 0; i < textArray.Count; i = i + 11) 
        {
            //Debug.Log("|" + textArray[i] + "|");
            Attack attackData = new Attack();
            attackData.AttackIndex = int.Parse(textArray[i]);
            attackData.AttackName = textArray[i + 1];
            attackData.AttackInfo = textArray[i + 2];
            attackData.AddedDamage = int.Parse(textArray[i + 3]);
            attackData.ElementDamage = int.Parse(textArray[i + 4]);
            //attackData.elementDamage = (textArray[i + 4] == "true");
            attackData.Accuracy = int.Parse(textArray[i + 5]);
            attackData.AddedFocus = int.Parse(textArray[i + 6]);
            //attackData.attackRangeX = int.Parse(textArray[i + 7]);
            //attackData.attackRangeY = int.Parse(textArray[i + 8]);
            //attackData.attackTime = int.Parse(textArray[i + 9]);
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
