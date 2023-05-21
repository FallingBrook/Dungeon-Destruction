using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private EnemyController controller;
    private FloatingJoystick Joystick;

    Rigidbody2D myBody;
    Vector2 move;
    public float moveSpeed;

    public static bool PointerDown = false;

    public float speed;

    Animator anim;
    private string currentState;


    public InputActionReference pointerPosition;

    public float health;

    public float detectionRadius;
    public LayerMask enemyLayer;
    //Collider2D[] hitEnemy;

    public GameObject weapon;

    private int count = 0;

    private GameObject currentEnemy = null;

    [SerializeField, Range(1f, 0.01f)] private float meleeKnockbackResistance;
    [SerializeField, Range(1f, 0.01f)] private float rifleKnockbackResistance;
    public ParticleSystem groundParticles;
    //private Vector2 enemyPos;
    // Start is called before the first frame update
    void Start()
    {
        Joystick = GameObject.FindWithTag("Joystick").GetComponent<FloatingJoystick>();
        myBody = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        if(GameObject.FindWithTag("EnemyController") != null)
            controller = GameObject.FindWithTag("EnemyController").GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Joystick.Horizontal;
        move.y = Joystick.Vertical;
        Collider2D detection = Physics2D.OverlapCircle(transform.position, detectionRadius, enemyLayer);
        if (detection)
        {
            if (currentEnemy == null && controller.enemies.Count > 0)
            {
                currentEnemy = controller.enemies[0];
            }
            else if (count >= controller.enemies.Count)
            {
                count = 0;
            }
            else if (controller.enemies.Count > 0)
            {
                GameObject newEnemy = controller.enemies[count];
                if (newEnemy.CompareTag("BreakableLoot"))
                {
                    count += 1;
                    return;
                }
                if (newEnemy == null)
                {

                }
                if (Vector2.Distance(transform.position, newEnemy.transform.position) < Vector2.Distance(transform.position, currentEnemy.transform.position))
                    currentEnemy = newEnemy;
                count += 1;
            }
            if (controller.enemies.Count > 0)
            {
                //enemyPos = hitEnemy.ClosestPoint(transform.position);
                EnemyDetection();
                //if (currentEnemy.transform.position.x > transform.position.x)
                //    transform.localScale = new Vector3(1, 1, 0);
                //else
                //    transform.localScale = new Vector3(-1, 1, 0);
            }
        }
        else
        {
            PointInDirection();
            //if (move.x > 0)
            //    transform.localScale = new Vector3(1, 1, 0);
            //else
            //    transform.localScale = new Vector3(-1, 1, 0);
        }

    }
    private void FixedUpdate()
    {
        //if (PointerDown)
        //{
        //    myBody.velocity = Vector3.zero;
        //}
        //else
        //{
        //    myBody.MovePosition(myBody.position + move * moveSpeed * Time.fixedDeltaTime);
        //}
        //if(Joystick.input.magnitude > 0.5f)
        //    myBody.MovePosition((myBody.position + move * moveSpeed * Time.deltaTime) * 2);
        //else
        //moveSpeed = moveSpeed / move;
        var s = move.magnitude;
        Vector2 sp = move / s;
        if(move != Vector2.zero)
            myBody.MovePosition(myBody.position + sp * speed * Time.deltaTime);
        else
            myBody.MovePosition(myBody.position);

        Animate();
    }
    private void EnemyDetection()
    {
        if (GameObject.FindWithTag("AimJoystick").transform.GetChild(1).GetComponent<AttackBScript>().isQuickFire)
        {
            Debug.Log("AIGHT");
            weapon.GetComponent<WeaponParent>().PointTowardsEnemy(currentEnemy.transform.position);
        }
        else
            PointInDirection();
    }
    private void PointInDirection()
    {
        if (!GameObject.FindWithTag("AttackB").GetComponent<AttackBScript>().buttonPressed)
        {
            Debug.Log("OK");
            weapon.GetComponent<WeaponParent>().PointInDirection(Joystick);
        }
        else
            weapon.GetComponent<WeaponParent>().PointInDirection(GameObject.FindWithTag("AimJoystick").GetComponent<FixedJoystick>());
    }

    private void Move()
    {
        //myBody.velocity = new Vector2(movement.x * speed, movement.y * speed);
    }

    private void Animate()
    {
        if (Joystick.Direction.x > 0)
        {
            changeAnimationState("PlayerWalk");
            groundParticles.Play();
        }
        else if (Joystick.Direction.x < 0)
        {
            groundParticles.Play();
            changeAnimationState("PlayerWalk");
        }
        else
        {
            groundParticles.Stop();
            changeAnimationState("Idle");
        }
    }
    public void ReceiveDamage(float damage, Vector2 direction, float knockback, string type)
    {
        if (type == "Melee")
            knockback *= meleeKnockbackResistance;
        else
            knockback *= rifleKnockbackResistance;
        StartCoroutine(Knockback(direction, knockback));
        health -= damage;
        GameObject.FindWithTag("InventoryManager").GetComponent<InventoryManager>().UpdateHealth();
    }

    IEnumerator Knockback(Vector2 direction, float knockback)
    {
        float timePassed = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        while (timePassed < 0.15)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            GetComponent<Rigidbody2D>().AddForce(direction * knockback);
            // Code to go left here
            timePassed += Time.deltaTime;
            if(timePassed >= 0.15)
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
            yield return null;
        }
    }
    void changeAnimationState(string newState)
    {
        //Debug.Log(direction.x);
        // stop same anim from playing
        if (currentState == newState) return;
        // play new anim
        anim.Play(newState);
        //GameObject.Find("PlayerPicture").GetComponent<Animator>().Play(newState);
        // reassign the current state
        currentState = newState;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
