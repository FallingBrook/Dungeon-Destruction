using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject weaponGO;

    public bool buttonPressed;

    Collider2D[] pickupDetection;
    public float pickupRadius;

    private GameObject player;
    private GameObject closestObject;
    public LayerMask weaponLayer;
    private GameObject invManager;
    private float coolDwn = 0.5f;
    private float coolDwnCounter;
    private WeaponInfoScript weaponInfo;
    public Material outline;
    public Material normalMat;
    // Start is called before the first frame update
    void Start()
    {
        weaponInfo = GameObject.FindWithTag("WeaponInfo").GetComponent<WeaponInfoScript>();
        player = GameObject.FindWithTag("Player");
        invManager = GameObject.FindWithTag("InventoryManager");
    }


    private void FixedUpdate()
    {
        pickupDetection = Physics2D.OverlapCircleAll(player.transform.position, pickupRadius, weaponLayer);
        if (pickupDetection.Length > 0)
        {
            //transform.GetChild(0).gameObject.SetActive(false);
            //transform.GetChild(1).gameObject.SetActive(true);
            FindClosest();
            if (closestObject.transform.parent == null && !closestObject.CompareTag("Chest") && closestObject.layer != 15)
            {
                weaponInfo.ShowText(closestObject);
                GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().ToIdle();
            }
            else if (!closestObject.CompareTag("Chest"))
            {
                if (closestObject.layer == 15 && closestObject.transform.parent != null)
                {
                    int mana = closestObject.GetComponent<ConsumablePotionsScript>().manaInc;
                    int health = closestObject.GetComponent<ConsumablePotionsScript>().healthInc;
                    GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().ShowShopTextPotion(closestObject, closestObject.transform.parent.GetComponent<ShopSellableItemScript>().price, mana, health);
                }
                else if (closestObject.layer != 15)
                    GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().ShowShopText(closestObject, closestObject.transform.parent.GetComponent<ShopSellableItemScript>().price);
                weaponInfo.ToIdle();
            }
            if (buttonPressed && coolDwnCounter <= 0)
            {
                if (closestObject.layer == 14)
                {
                    closestObject.GetComponent<ChestScript>().Open();
                    SetNormalMat(closestObject);
                    closestObject.layer = 0;
                    closestObject = null;
                    coolDwnCounter = coolDwn;
                }
                else if (closestObject.transform.parent != null && closestObject.transform.parent.CompareTag("SellTable"))
                {
                    if (closestObject.transform.parent.GetComponent<ShopSellableItemScript>().price <= invManager.GetComponent<InventoryManager>().coins)
                    {
                        weaponInfo.HideText();
                        GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().HideText();
                        int price = closestObject.transform.parent.GetComponent<ShopSellableItemScript>().price;
                        if (closestObject.layer == 15)
                        {
                            invManager.GetComponent<InventoryManager>().UpdateCoinsNeg(closestObject.transform.parent.GetComponent<ShopSellableItemScript>().price);
                            closestObject.GetComponent<ConsumablePotionsScript>().UsePotion();
                        }
                        else
                        {
                            closestObject.transform.SetParent(null);
                            invManager.GetComponent<InventoryManager>().PickupWeapon(closestObject, price);
                        }
                        SetNormalMat(closestObject);
                        closestObject = null;
                        coolDwnCounter = coolDwn;
                    }
                }
                else if (closestObject.layer == 15)
                {
                    //weaponInfo.HideText();
                    closestObject.GetComponent<ConsumablePotionsScript>().UsePotion();
                    SetNormalMat(closestObject);
                    closestObject.layer = 0;
                    closestObject = null;
                    coolDwnCounter = coolDwn;
                }
                else
                {
                    invManager.GetComponent<InventoryManager>().PickupWeapon(closestObject, 0);
                    SetNormalMat(closestObject);
                    closestObject = null;
                    coolDwnCounter = coolDwn;
                }
            }
            coolDwnCounter -= Time.deltaTime;
        }
        else
        {
            //transform.GetChild(0).gameObject.SetActive(true);
            //transform.GetChild(1).gameObject.SetActive(false);
            if (closestObject != null)
            {
                SetNormalMat(closestObject);
                if (closestObject.layer != 14)
                {
                    if (closestObject.transform.parent == null)
                        weaponInfo.HideText();
                    else
                        GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().HideText();
                }
                closestObject = null;
            }
        }


    }
    private void FindClosest()
    {
        foreach (Collider2D i in pickupDetection)
        {
            if (closestObject == null || Vector2.Distance(player.transform.position, i.transform.position) < Vector2.Distance(player.transform.position, closestObject.transform.position))
            {
                if (closestObject != null)
                {
                    SetNormalMat(closestObject);
                    if (closestObject.layer != 15 && closestObject.layer != 14)
                    {
                        if (closestObject.transform.parent == null)
                            weaponInfo.HideText();
                        else
                            GameObject.FindWithTag("WeaponInfoShop").GetComponent<WeaponInfoScript>().HideText();
                    }
                }
                closestObject = i.gameObject;
                SetOutlineMat(closestObject);
            }
            else if (i.gameObject != closestObject)
            {
                SetNormalMat(i.gameObject);
                SetOutlineMat(closestObject);
            }
        }

    }

    private void SetOutlineMat(GameObject ob)
    {
        if (ob.CompareTag("MeleeWeapon"))
        {
            if (!ob.GetComponent<SwordWeapon>().GetComponent<SwordWeapon>().regular)
                ob.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().material = outline;
            else
                ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = outline;
        }
        else if (ob.layer == 14)
            ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = outline;
        else
            ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = outline;
    }
    private void SetNormalMat(GameObject ob)
    {
        if (ob.CompareTag("MeleeWeapon"))
        {
            if (!ob.GetComponent<SwordWeapon>().GetComponent<SwordWeapon>().regular)
                ob.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().material = normalMat;
            else
                ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = normalMat;
        }
        else if (ob.layer == 14)
            ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = normalMat;
        else
            ob.transform.GetChild(0).GetComponent<SpriteRenderer>().material = normalMat;
    }

    private void Attack()
    {
        weaponGO.GetComponent<WeaponParent>().Attack();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
