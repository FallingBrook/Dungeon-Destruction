using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackBScript : MonoBehaviour
{
    public GameObject weaponGO;

    public bool buttonPressed = false;
    public bool isQuickFire = false;
    //Collider2D[] pickupDetection;
    //public float pickupRadius;

    //private GameObject player;
    //private GameObject closestObject;
    //public LayerMask weaponLayer;
    //private GameObject invManager;
    //private WeaponInfoScript weaponInfo;
    //public Material outline;
    //public Material normalMat;
    //public float cldwn;
    //private float cldwnCounter;
    // Start is called before the first frame update
    void Start()
    {
        //weaponInfo = GameObject.FindWithTag("WeaponInfo").GetComponent<WeaponInfoScript>();
        //player = GameObject.FindWithTag("Player");
        //invManager = GameObject.FindWithTag("InventoryManager");
    }
    private void FixedUpdate()
    {
        //pickupDetection = Physics2D.OverlapCircleAll(player.transform.position, pickupRadius, weaponLayer);
        if (buttonPressed)
        {
            weaponGO.GetComponent<WeaponParent>().Attack();

            transform.parent.GetComponent<FixedJoystick>().handle.anchoredPosition = transform.parent.GetComponent<FixedJoystick>().oldPos;
        }
    }

    //private void Attack()
    //{
    //    weaponGO.GetComponent<WeaponParent>().Attack();
    //}

    public void OnPointerDown()
    {
        buttonPressed = true;
    }
    IEnumerator cldwn()
    {
        float timePassed = 0;
        while (timePassed < 0.15)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= 0.15)
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            yield return null;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        isQuickFire = false;
    }
}
