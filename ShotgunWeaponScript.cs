using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeaponScript : MonoBehaviour
{
    public int numOfBullets;
    private FixedJoystick Joystick;
    private FloatingJoystick otherJoystick;
    Vector2 move;

    private string currentState;
    Animator anim;
    [HideInInspector] public bool attack;
    public float coolDwn;
    [HideInInspector] public float coolDwnCounter = 0;

    public GameObject projectile;
    private GameObject player;

    public float damage;
    public float knockback;

    private bool isEnemyTarget;

    private Vector2 targ;

    [HideInInspector] public Vector2 direction;

    public float aimConeOffset;
    public int manaCost;
    public GameObject noManaText;
    private float textCldwn = 1;
    private float textCldwnCounter;
    private void Start()
    {
        Joystick = GameObject.FindWithTag("AimJoystick").GetComponent<FixedJoystick>();
        otherJoystick = GameObject.FindWithTag("Joystick").GetComponent<FloatingJoystick>();
        player = GameObject.FindWithTag("Player");
        if (transform.parent == null)
            GetComponent<ShotgunWeaponScript>().enabled = false;
        else if (transform.parent.tag == "SellTable")
        {
            GetComponent<ShotgunWeaponScript>().enabled = false;
        }
        else
            gameObject.layer = 0;
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Update()
    {
        coolDwnCounter -= Time.deltaTime;
        if (!isEnemyTarget)
        {
            if (move.x > 0)
                player.transform.localScale = new Vector3(1, 1, 0);
            else if (move.x < 0)
                player.transform.localScale = new Vector3(-1, 1, 0);
        }
        textCldwnCounter -= Time.deltaTime;
    }

    public void Attack()
    {
        if(GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana < manaCost && textCldwnCounter <= 0)
        {
            Instantiate(noManaText, new Vector3(transform.position.x, transform.position.y + 0.25f, 0), Quaternion.identity, GameObject.FindFirstObjectByType<Canvas>().transform);
            textCldwnCounter = textCldwn;
            return;
        }

        if (coolDwnCounter <= 0 && currentState != gameObject.name + "Shoot" && GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana >= manaCost)
        {
            GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateMana(manaCost);
            changeAnimationState(gameObject.name + "Shoot");
            transform.GetChild(0).GetChild(0).GetComponent<WeaponChild>().StartAttackAnim();
            float num = -(aimConeOffset / numOfBullets);
            for (int i = 0; i < numOfBullets; i++)
            {
                float addedOffset = num;
                // Then add "addedOffset" to whatever rotation axis the player must rotate on
                Quaternion newRot = Quaternion.Euler(transform.GetChild(1).localEulerAngles.x,
                transform.GetChild(1).transform.localEulerAngles.y,
                transform.GetChild(1).transform.localEulerAngles.z + addedOffset);

                GameObject bullet = Instantiate(projectile, transform.GetChild(1).position, newRot);
                bullet.GetComponent<ModernGunProjectileScript>().damage = damage / numOfBullets;
                bullet.GetComponent<ModernGunProjectileScript>().knockback = knockback * 50;
                Vector2 dir = bullet.transform.rotation * targ;
                if (isEnemyTarget)
                    bullet.GetComponent<ModernGunProjectileScript>().Move(dir);
                else if (move.x == 0 && move.y == 0)
                {
                    Vector2 dir2;
                    if (player.transform.localScale.x > 0)
                    {
                        dir2 = bullet.transform.rotation * Vector2.right;
                        bullet.GetComponent<ModernGunProjectileScript>().Move(dir2);
                    }
                    else
                    {
                        dir2 = bullet.transform.rotation * Vector2.left;
                        bullet.GetComponent<ModernGunProjectileScript>().Move(dir2);
                    }
                }
                else
                {
                    Vector2 dir2 = bullet.transform.rotation * move;
                    bullet.GetComponent<ModernGunProjectileScript>().Move(dir2);
                }
                num += aimConeOffset / numOfBullets;
            }

        }
    }
    public void StopAttack()
    {
        changeAnimationState(gameObject.name + "Idle");
        coolDwnCounter = coolDwn;
    }

    public void PointTowardsEnemy(Vector2 enemyPos)
    {
        isEnemyTarget = true;
        targ = enemyPos;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        targ = targ.normalized;
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        if (targ.x > 0)
            player.transform.localScale = new Vector3(1, 1, 0);
        else if (targ.x < 0)
            player.transform.localScale = new Vector3(-1, 1, 0);
        //isEnemyTarget = true;
    }

    public void PointInDirection(Joystick js)
    {
        isEnemyTarget = false;
        move.x = js.Horizontal;
        move.y = js.Vertical;
        float hAxis;
        float vAxis;
        //if (player != null && GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>().enemies.Count == 0)
        //{
        hAxis = move.x;
        vAxis = move.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (zAxis != 0)
            transform.eulerAngles = new Vector3(04, 0f, -zAxis);
        //if (player.transform.localScale.x > 0)
        //    transform.eulerAngles = new Vector3(04, 0f, -90);
        //else
        //    transform.eulerAngles = new Vector3(04, 0f, 90);
        //}
    }

    void changeAnimationState(string newState)
    {
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }
}
