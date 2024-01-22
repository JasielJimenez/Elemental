using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public GameObject BattleManager;
    public List<GameObject> ColliderList = new List<GameObject>();
    public string AttackOwnerTag;

    //public string OwnerTag = "";

    // Start is called before the first frame update
    void Start()
    {
        BattleManager = GameObject.Find("DemoBattleManager");
        AttackOwnerTag = this.transform.parent.parent.gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        //PREVENT FRIENDLY FIRE! Maybe check to see who it is coming from and if it is a damaging attack
        var targetTag = collider.gameObject.tag;
        if(!ColliderList.Contains(collider.gameObject) && targetTag != AttackOwnerTag)
        {
            if (targetTag == "Enemy" || targetTag == "EnvironmentObject" || targetTag == "Player")
            {
                ColliderList.Add(collider.gameObject);
                BattleManager.transform.GetComponent<CombatManager>().AddToTargetList(collider.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        var targetTag = collider.gameObject.tag;
        if (ColliderList.Contains(collider.gameObject) && (targetTag == "Enemy" || targetTag == "EnvironmentObject" || targetTag == "Player"))
        {
            ColliderList.Remove(collider.gameObject);
            BattleManager.transform.GetComponent<CombatManager>().RemoveFromTargetList(collider.gameObject);
        }
    }
}
