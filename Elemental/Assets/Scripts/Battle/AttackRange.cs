using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public List<GameObject> colliderList = new List<GameObject>();
    public string OwnerTag = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (!colliderList.Contains(collider.gameObject) && (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnvironmentObject" || collider.gameObject.tag == "Player"))
        {
            colliderList.Add(collider.gameObject);
            this.transform.parent.parent.gameObject.GetComponent<CharacterCombat>().AddToTargetList(collider.gameObject);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (colliderList.Contains(collider.gameObject) && (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnvironmentObject" || collider.gameObject.tag == "Player"))
        {
            colliderList.Remove(collider.gameObject);
            this.transform.parent.parent.gameObject.GetComponent<CharacterCombat>().RemoveFromTargetList(collider.gameObject);
        }
    }
}
