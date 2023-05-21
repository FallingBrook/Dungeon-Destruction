using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablePotionsScript : MonoBehaviour
{
    public int manaInc;
    public int healthInc;

    public void UsePotion()
    {
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateManaPos(manaInc);
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateHealthPos(healthInc);
        Destroy(gameObject);
    }
}
