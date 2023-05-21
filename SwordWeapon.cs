using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : MonoBehaviour
{
    public bool regular;
    private GameObject player;
    SpriteRenderer sr;
    private FixedJoystick Joystick;
    Vector2 move;

    private string currentState;
    Animator anim;
    [HideInInspector]public bool attack;
    public Vector2 pointerPos { get; set; }
    public float coolDwn;
    [HideInInspector]public float coolDwnCounter = 0;


    [HideInInspector]public Vector2 targ;
    private bool isEnemyTarget;

    public Collider2D[] enemyHit;
    public float damage;
    public float knockback;
    private Vector2 dir;

    private List<GameObject> enemiesHit;
    public LayerMask enemyLayer;
    private int currentEnemyNum = 0;
    public bool stillAttacking = false;
    public bool circleColl;
    public float swingRadius;
    public Vector2 boxDimensions;

    private float ang;
    public int numOfAttacks;

    public float attackMomentum;
    [HideInInspector]public Vector2 direction;
    public int manaCost;
    public GameObject noManaText;
    private float textCldwn = 1;
    private float textCldwnCounter;
    private void Start()
    {

        currentState = gameObject.name + "Idle";
        player = GameObject.FindWithTag("Player");
        if (transform.parent == null)
            GetComponent<SwordWeapon>().enabled = false;
        else if (transform.parent.tag == "SellTable")
        {
            GetComponent<SwordWeapon>().enabled = false;
        }
        else
            gameObject.layer = 0;
        Joystick = GameObject.FindWithTag("AimJoystick").GetComponent<FixedJoystick>();
        enemiesHit = new List<GameObject>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        if(regular)
            sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        else
            sr = transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        coolDwnCounter -= Time.deltaTime;
        ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (circleColl)
            enemyHit = Physics2D.OverlapCircleAll(transform.GetChild(1).transform.position, swingRadius, enemyLayer);
        else
            enemyHit = Physics2D.OverlapBoxAll(transform.GetChild(0).GetChild(0).position, boxDimensions, ang - 90, enemyLayer);
        if (transform.rotation.z < 0.5 && transform.rotation.z > -0.5)
        {
            sr.sortingOrder = 0;
        }
        else
        {
            sr.sortingOrder = 2;
        }
        if (currentState != gameObject.name + "Idle" && stillAttacking == true)
            DealDamage();
        if (!isEnemyTarget)
        {
            if (move.x > 0)
                player.transform.localScale = new Vector3(1, 1, 0);
            else if (move.x < 0)
                player.transform.localScale = new Vector3(-1, 1, 0);
        }
        textCldwnCounter -= Time.deltaTime;
    }
    public void PointTowardsEnemy(Vector2 enemyPos)
    {
        direction = targ;
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
        isEnemyTarget = true;
    }

    public void PointInDirection(Joystick js)
    {
        isEnemyTarget = false;
        move.x = js.Horizontal;
        move.y = js.Vertical;
        direction = move;
        float hAxis = move.x;
        float vAxis = move.y;
        float zAxis = Mathf.Atan2(hAxis, vAxis) * Mathf.Rad2Deg;
        if (zAxis != 0)
            transform.eulerAngles = new Vector3(04, 0f, -zAxis);
        //else if (player.transform.localScale.x > 0)
        //{
        //    transform.eulerAngles = new Vector3(04, 0f, -90);
        //    direction = new Vector2(player.transform.localScale.x, 0.002f);
        //}
        //else
        //{
        //    transform.eulerAngles = new Vector3(04, 0f, 90);
        //    direction = new Vector2(player.transform.localScale.x, 0.002f);
        //}
    }
    public void Attack()
    {
        if (GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana < manaCost && textCldwnCounter <= 0)
        {
            Instantiate(noManaText, new Vector3(transform.position.x, transform.position.y + 0.25f, 0), Quaternion.identity, GameObject.FindFirstObjectByType<Canvas>().transform);
            textCldwnCounter = textCldwn;
            return;
        }
        if (coolDwnCounter < 0 && GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().mana >= manaCost)
        {
            GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateMana(manaCost);
            string num = Random.Range(1, numOfAttacks + 1).ToString();
            ChangeAnimationState(gameObject.name + "Swing" + num);
            if (enemyHit.Length > 0)
            {
                currentEnemyNum = 0;
                DealDamage();

            }
            coolDwnCounter = 10;
        }
        //Invoke("StopAttack", 0.1f);
    }
    private void DealDamage()
    {
        
        dir = new Vector2(targ.x, targ.y) * 50;
        if (currentEnemyNum >= enemyHit.Length - 1)
            currentEnemyNum = 0;
        //if (enemyHit[currentEnemyNum] == null)
        //    return;
        if (enemyHit.Length > 0 && stillAttacking)
        {
            Debug.Log(enemyHit[currentEnemyNum]);
            if (!enemiesHit.Contains(enemyHit[currentEnemyNum].gameObject))
            {
                //if (enemyHit[currentEnemyNum].tag == "SmallTouchEnemy")
                //    enemyHit[currentEnemyNum].gameObject.GetComponent<SmallTouchEnemyScript>().ReceiveDamage(damage, dir, knockback, "Melee");
                //else if (enemyHit[currentEnemyNum].tag == "SmallProjectileEnemy")
                //    enemyHit[currentEnemyNum].gameObject.GetComponent<SmallProjectileEnemyScript>().ReceiveDamage(damage, dir, knockback, "Melee");
                //else if (enemyHit[currentEnemyNum].tag == "SmallBomberEnemy")
                //    enemyHit[currentEnemyNum].gameObject.GetComponent<SmallBomberEnemyScript>().ReceiveDamage(damage, dir, knockback, "Melee");
                //else if (enemyHit[currentEnemyNum].tag == "SwordEnemy")
                //    enemyHit[currentEnemyNum].gameObject.GetComponent<SwordEnemyScript>().ReceiveDamage(damage, dir, knockback, "Melee");
                enemyHit[currentEnemyNum].transform.parent.gameObject.GetComponent<EnemyParent>().ReceiveDamage(damage, dir, knockback, "Melee");
                enemiesHit.Add(enemyHit[currentEnemyNum].gameObject);
                currentEnemyNum++;
            }
        }
        currentEnemyNum++;

    }
    //public void StopAttackDamage()
    //{
    //    stillAttacking = false;
    //}

    public void StopAttack()
    {
        ChangeAnimationState(gameObject.name + "Idle");
        enemiesHit.Clear();
        currentEnemyNum = 0;
        coolDwnCounter = coolDwn;
    }
    void ChangeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        anim.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    public void circleCast()
    {

    }
    public void boxCast()
    {

    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.GetChild(1).transform.position, swingRadius);
        //Gizmos.DrawWireCube(transform.GetChild(2).position, new Vector3(boxDimensions.x, boxDimensions.y, 0));
    }

}
