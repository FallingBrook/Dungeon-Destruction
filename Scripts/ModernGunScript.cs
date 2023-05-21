using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModernGunScript : MonoBehaviour
{
    private FixedJoystick Joystick;
    Vector2 move;

    private string currentState;
    Animator anim;
    [HideInInspector] public bool attack;
    public float coolDwn;
    [HideInInspector]public float coolDwnCounter = 0;

    public GameObject projectile;
    private GameObject player;

    public float damage;
    public float knockback;

    private bool isEnemyTarget;

    private Vector2 targ;

    [HideInInspector]public Vector2 direction;

    public float aimConeOffset;
    public int manaCost;
    public GameObject noManaText;
    private float textCldwn = 1;
    private float textCldwnCounter;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (transform.parent == null)
            GetComponent<ModernGunScript>().enabled = false;
        else if(transform.parent.tag == "SellTable")
        {
            GetComponent<ModernGunScript>().enabled = false;
        }
        else
            gameObject.layer = 0;
        anim = transform.GetChild(0).GetComponent<Animator>();
        Joystick = GameObject.FindWithTag("AimJoystick").GetComponent<FixedJoystick>();
    }
    private void Update()
    {
        coolDwnCounter -= Time.deltaTime;
        //Debug.Log(move.x);
        //Debug.Log(move.y);
        if (!isEnemyTarget)
        {
            if (move.x > 0)
                player.transform.localScale = new Vector3(1, 1, 0);
            else if(move.x < 0)
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

        if (coolDwnCounter <= 0 && currentState != gameObject.name + "Shoot" && GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana >= manaCost)
        {
            GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateMana(manaCost);
            transform.GetChild(0).GetChild(0).GetComponent<WeaponChild>().StartAttackAnim();
            changeAnimationState(gameObject.name + "Shoot");
            GameObject bullet = Instantiate(projectile, transform.GetChild(1).position, Quaternion.identity);
            float numX = Random.Range(-aimConeOffset, aimConeOffset);
            float numY = Random.Range(-aimConeOffset, aimConeOffset);
            if(isEnemyTarget)
                bullet.GetComponent<ModernGunProjectileScript>().Move(new Vector2(targ.x + numX, targ.y + numY));
            else if(move.x == 0 && move.y == 0)
                bullet.GetComponent<ModernGunProjectileScript>().Move(new Vector2(player.transform.localScale.x + numX, 0.002f + numY));
            else
                bullet.GetComponent<ModernGunProjectileScript>().Move(new Vector2(move.x + numX, move.y + numY));
            bullet.GetComponent<ModernGunProjectileScript>().damage = damage;
            bullet.GetComponent<ModernGunProjectileScript>().knockback = knockback * 50;
            //transform.GetChild(1).gameObject.SetActive(false);
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

        float hAxis = move.x;
        float vAxis = move.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (player != null && GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>().enemies.Count == 0)
            transform.eulerAngles = new Vector3(04, 0f, -zAxis);
        //if (player.transform.localScale.x > 0)
        //    transform.eulerAngles = new Vector3(04, 0f, -90);
        //else
        //    transform.eulerAngles = new Vector3(04, 0f, 90);
    }

    void changeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }
}
