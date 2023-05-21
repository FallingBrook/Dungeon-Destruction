using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    RaycastHit2D groundHit;
    RaycastHit2D playerHit;

    public LayerMask playerLayer;
    public LayerMask groundLayer;

    [HideInInspector] public float damage;
    [HideInInspector] public float knockback;

    private Vector2 dir;

    public float speed;

    private float timeUntilDeath = 10;
    // Update is called once per frame
    void Update()
    {
        playerHit = Physics2D.BoxCast(transform.position, new Vector3(0.15f, 0.15f, 0), 0, new Vector2(0, 0), 0, playerLayer);
        groundHit = Physics2D.BoxCast(transform.position, new Vector3(0.15f, 0.15f, 0), 0, new Vector2(0, 0), 0, groundLayer);
        if (playerHit)
        {
            DealDamage();
        }

        timeUntilDeath -= Time.deltaTime;
        if (timeUntilDeath <= 0 || groundHit)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.15f, 0.15f, 0));
    }
    public void Move(Vector2 direction)
    {
        dir = direction;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        float zAxis = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0f, -zAxis + 90);
    }

    private void DealDamage()
    {
        playerHit.collider.gameObject.GetComponent<PlayerScript>().health -= damage;
        playerHit.collider.gameObject.GetComponent<PlayerScript>().ReceiveDamage(1, dir, knockback, "Rifle");
        Destroy(gameObject);
    }
}
