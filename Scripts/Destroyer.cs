using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Room") && other.gameObject.name != "StartRoom" && other.gameObject.name != "Room Templates")
        {
            GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>().rooms.Remove(other.gameObject);
            Destroy(other.gameObject);
        }

    }

    private void Update()
    {
        Invoke("Die", 1);
    }

	void Die()
    {
        Destroy(gameObject);
    }
}
