using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChild : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        transform.parent.GetComponent<ChestScript>().Spawn();
    }

    public void Enable()
    {
        transform.parent.gameObject.layer = 14;
        transform.parent.GetComponent<BoxCollider2D>().enabled = true;
        transform.parent.GetComponent<ChestScript>().ChangeAnimationState("ChestIdle");
    }
}
