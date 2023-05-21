using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModernGunProjectileScript : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public LayerMask layers;

    RaycastHit2D enemyHit;

    [HideInInspector]public float damage;
    [HideInInspector]public float knockback;

    private Vector2 dir;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(transform.position, new Vector3(0.15f, 0.1f, 0), 0, new Vector2(0, 0), 0, groundLayer);
        enemyHit = Physics2D.BoxCast(transform.position, new Vector3(0.15f, 0.1f, 0), 0, new Vector2(0, 0), 0, enemyLayer);

        if (groundHit)
        {
            Destroy();
        }
        else if (enemyHit)
        {
            DealDamage();
        }
    }
    private void FixedUpdate()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.15f, 0.1f, 0));
    }

    public void Move(Vector2 direction)
    {
        dir = direction.normalized;

        //if (dir.x <= 0.05 && dir.y <= 0.05)
        //    direction = new Vector2(dir.x * 15f, dir.y * 15f);
        //else if (dir.x <= 0.1 && dir.y <= 0.1)
        //    direction = new Vector2(dir.x * 7f, dir.y * 7f);
        //else if (dir.x <= 0.25 && dir.y <= 0.25)
        //    direction = new Vector2(dir.x * 3, dir.y * 3);
        //else if (dir.x <= 0.5 && dir.y <= 0.5)
        //    direction = new Vector2(dir.x * 1.5f, dir.y * 1.5f);
        var s = dir.magnitude;
        Vector2 sp = dir / s;
        GetComponent<Rigidbody2D>().velocity = sp * speed;
        float zAxis = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0f, -zAxis + 90);
    }

    private void DealDamage()
    {
        
        //if (enemyHit.collider.tag == "SmallTouchEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallTouchEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SmallProjectileEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallProjectileEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SmallBomberEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SmallBomberEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        //else if (enemyHit.collider.tag == "SwordEnemy")
        //    enemyHit.collider.gameObject.GetComponent<SwordEnemyScript>().ReceiveDamage(damage, dir, knockback, "Rifle");
        enemyHit.collider.transform.parent.gameObject.GetComponent<EnemyParent>().ReceiveDamage(damage, dir, knockback, "Rifle");
        Destroy(gameObject);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
            Destroy();
    }
}
