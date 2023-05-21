using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChild : MonoBehaviour
{
    private GameObject player;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopAttack()
    {
        //GetComponentInChildren<TrailRenderer>().enabled = false;
        if(transform.parent.tag == "MeleeWeapon")
            transform.parent.GetComponent<SwordWeapon>().StopAttack();
        else if(transform.parent.tag == "MilitaryRangedWeapon")
            transform.parent.GetComponent<ModernGunScript>().StopAttack();
        else if(transform.parent.tag == "EnemyMeleeWeapon")
            transform.parent.GetComponent<EnemySwordScript>().StopAttack();
        else if (transform.parent.tag == "MilitaryShotgunWeapon")
            transform.parent.GetComponent<ShotgunWeaponScript>().StopAttack();

    }

    public void StartAttackDamage()
    {

        if (transform.parent.CompareTag("MeleeWeapon"))
        {
            transform.parent.GetComponent<SwordWeapon>().stillAttacking = true;
        }
        else if (transform.parent.tag == "EnemyMeleeWeapon")
            transform.parent.GetComponent<EnemySwordScript>().stillAttacking = true;
    }

    public void StopAttackDamage()
    {
        if (transform.parent.tag == "MeleeWeapon")
            transform.parent.GetComponent<SwordWeapon>().stillAttacking = false;
        else if (transform.parent.tag == "EnemyMeleeWeapon")
            transform.parent.GetComponent<EnemySwordScript>().stillAttacking = false;
    }

    public void StopAttackAnim()
    {
        GetComponent<Animator>().Play("Idle" + gameObject.name);

    }

    public void StartAttackAnim()
    {
        GetComponent<Animator>().Play("Fire" + gameObject.name);

    }
    public void FlipWeapon()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>().flipY)
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = false;
        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipY = true;

    }
    public void MoveForward()
    {
        if(isPlayer)
        {
            Vector2 dir = transform.parent.GetComponent<SwordWeapon>().direction;
            float force = transform.parent.GetComponent<SwordWeapon>().attackMomentum * 50;
            player.GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
        else
        {
            Vector2 dir;
            if (transform.parent.CompareTag("EnemyMeleeWeapon"))
            {
                dir = transform.parent.GetComponent<EnemySwordScript>().direction;
                float force = transform.parent.GetComponent<EnemySwordScript>().attackMomentum * 50;
                transform.parent.GetComponent<EnemySwordScript>().parentEnemy.transform.parent.GetComponent<Rigidbody2D>().AddForce(dir * force);
            }
        }
    }
    public void MoveBackward()
    {

    }
}
