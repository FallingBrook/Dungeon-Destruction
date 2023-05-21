using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask groundLayer;   

    RaycastHit2D enemyHit;

    [HideInInspector] public float damage;
    [HideInInspector] public float knockback;

    public float coolDwn;
    private float coolDwnCounter = 0;


    private Vector2 dir;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, new Vector3(0.3f, 0.1f, 0), 0, new Vector2(0, 0), 0, groundLayer);
        enemyHit = Physics2D.BoxCast(transform.position, new Vector3(0.3f, 0.1f, 0), 0, new Vector2(0, 0), 0, enemyLayer);

        if (groundHit)
        {
            if (GetComponent<Rigidbody2D>().constraints != RigidbodyConstraints2D.FreezeAll)
                coolDwnCounter = coolDwn;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (enemyHit)
        {
            DealDamage();
        }


        if(GetComponent<Rigidbody2D>().constraints == RigidbodyConstraints2D.FreezeAll && coolDwnCounter <= 0)
        {
            Destroy();
        }
        coolDwnCounter -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.3f, 0.1f, 0));
    }

    public void Move(Vector2 direction)
    {
        dir = direction;
        var s = dir.magnitude;
        Vector2 sp = dir / s;
        GetComponent<Rigidbody2D>().velocity = sp * 10;
        float zAxis = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0f, -zAxis);
    }

    private void DealDamage()
    {
        enemyHit.collider.transform.parent.gameObject.GetComponent<EnemyParent>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //if (enemyHit.collider.tag == "SmallTouchEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallTouchEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SmallProjectileEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallProjectileEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SmallBomberEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallBomberEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SwordEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SwordEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "BowEnemy")
        //    enemyHit.collider.gameObject.GetComponent<BowEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");


        Destroy(gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
