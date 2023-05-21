using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (transform.GetChild(0).tag == "MeleeWeapon" && transform.GetChild(0).GetComponent<SwordWeapon>().coolDwnCounter <= 0)
        {
            transform.GetChild(0).GetComponent<SwordWeapon>().Attack();
        }
        else if (transform.GetChild(0).tag == "BowWeapon" && transform.GetChild(0).GetComponent<BowScript>().coolDwnCounter <= 0)
        {
            transform.GetChild(0).GetComponent<BowScript>().Attack();

        }
        else if (transform.GetChild(0).tag == "MilitaryRangedWeapon" && transform.GetChild(0).GetComponent<ModernGunScript>().coolDwnCounter <= 0)
        {
            transform.GetChild(0).GetComponent<ModernGunScript>().Attack();
        }
        else if (transform.GetChild(0).tag == "MilitaryShotgunWeapon" && transform.GetChild(0).GetComponent<ShotgunWeaponScript>().coolDwnCounter <= 0)
        {
            transform.GetChild(0).GetComponent<ShotgunWeaponScript>().Attack();
        }
    }

    public void PointInDirection(Joystick js)
    {
        if(transform.childCount > 0)
        {
            if (transform.GetChild(0).tag == "MeleeWeapon")
                transform.GetChild(0).GetComponent<SwordWeapon>().PointInDirection(js);
            else if (transform.GetChild(0).tag == "BowWeapon")
                transform.GetChild(0).GetComponent<BowScript>().PointInDirection(js);
            else if (transform.GetChild(0).tag == "MilitaryRangedWeapon")
                transform.GetChild(0).GetComponent<ModernGunScript>().PointInDirection(js);
            else if (transform.GetChild(0).tag == "MilitaryShotgunWeapon")
                transform.GetChild(0).GetComponent<ShotgunWeaponScript>().PointInDirection(js);
        }

    }

    public void PointTowardsEnemy(Vector2 enemyPos)
    {
        if (transform.GetChild(0).tag != "BreakableLoot")
        {
            if (transform.GetChild(0).tag == "MeleeWeapon")
                transform.GetChild(0).GetComponent<SwordWeapon>().PointTowardsEnemy(enemyPos);
            else if (transform.GetChild(0).tag == "BowWeapon")
                transform.GetChild(0).GetComponent<BowScript>().PointTowardsEnemy(enemyPos);
            else if (transform.GetChild(0).tag == "MilitaryRangedWeapon")
                transform.GetChild(0).GetComponent<ModernGunScript>().PointTowardsEnemy(enemyPos);
            else if (transform.GetChild(0).tag == "MilitaryShotgunWeapon")
                transform.GetChild(0).GetComponent<ShotgunWeaponScript>().PointTowardsEnemy(enemyPos);
        }
        //else
        //    PointInDirection();
    }

    //public void Enabled(bool i)
    //{
    //    if (transform.GetChild(0).tag == "MeleeWeapon")
    //        transform.GetChild(0).GetComponent<SwordWeapon>().enabled = false;
    //    else if (transform.GetChild(0).tag == "BowWeapon")
    //        transform.GetChild(0).GetComponent<BowScript>().PointTowardsEnemy(enemyPos);
    //    else if (transform.GetChild(0).tag == "MilitaryRangedWeapon")
    //        transform.GetChild(0).GetComponent<ModernGunScript>().PointTowardsEnemy(enemyPos);
    //    else if (transform.GetChild(0).tag == "MilitaryShotgunWeapon")
    //        transform.GetChild(0).GetComponent<ShotgunWeaponScript>().PointTowardsEnemy(enemyPos);
    //}
}
