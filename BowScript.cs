using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowScript : MonoBehaviour
{
    private FixedJoystick Joystick;
    Vector2 move;
    [HideInInspector] public bool attack;
    public float coolDwn;
    [HideInInspector]public float coolDwnCounter = 0;

    public GameObject projectile;
    private GameObject player;

    public float damage;
    public float knockback;

    private Vector2 targ;
    private bool isEnemyTarget;
    public int manaCost;
    public GameObject noManaText;
    private float textCldwn = 1;
    private float textCldwnCounter;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (transform.parent == null)
            GetComponent<BowScript>().enabled = false;
        else if (transform.parent.tag == "SellTable")
        {
            GetComponent<BowScript>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
            gameObject.layer = 0;
        Joystick = GameObject.FindWithTag("AimJoystick").GetComponent<FixedJoystick>();
        //sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //anim = transform.GetChild(0).GetComponent<Animator>();
    }
    private void Update()
    {

        coolDwnCounter -= Time.deltaTime;
        if (coolDwnCounter <= 0)
            StopAttack();
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
        if (GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana < manaCost && textCldwnCounter <= 0)
        {
            Instantiate(noManaText, new Vector3(transform.position.x, transform.position.y + 0.25f, 0), Quaternion.identity, GameObject.FindFirstObjectByType<Canvas>().transform);
            textCldwnCounter = textCldwn;
            return;
        }
        if (coolDwnCounter <= 0 && GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana >= manaCost)
        {
            GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateMana(manaCost);
            GameObject bullet = Instantiate(projectile, transform.GetChild(1).position, Quaternion.identity);
            //bullet.GetComponent<ArrowProjectile>().Move(targ);
            if (isEnemyTarget)
                bullet.GetComponent<ArrowProjectile>().Move(targ);
            else if (move.x == 0 && move.y == 0)
                bullet.GetComponent<ArrowProjectile>().Move(new Vector2(player.transform.localScale.x, 0.002f));
            else
                bullet.GetComponent<ArrowProjectile>().Move(new Vector2(move.x, move.y));
            bullet.GetComponent<ArrowProjectile>().damage = damage;
            bullet.GetComponent<ArrowProjectile>().knockback = knockback * 50;
            transform.GetChild(1).gameObject.SetActive(false);
            coolDwnCounter = coolDwn;
        }

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
    }

    public void PointInDirection(Joystick js)
    {
        isEnemyTarget = false;
        move.x = js.Horizontal;
        move.y = js.Vertical;

        float hAxis = move.x;
        float vAxis = move.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (zAxis != 0 && !GameObject.FindWithTag("AimJoystick").transform.GetChild(1).GetComponent<AttackBScript>().isQuickFire)
            transform.eulerAngles = new Vector3(04, 0f, -zAxis);
        //else if (player.transform.localScale.x > 0)
        //    transform.eulerAngles = new Vector3(04, 0f, -90);
        //else
        //    transform.eulerAngles = new Vector3(04, 0f, 90);
    }

    public void StopAttack()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
