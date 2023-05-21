using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door


	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;

	public float waitTime = 4f;

	public GameObject horizontalWall;

	public GameObject verticalWall;

	public LayerMask layer;

	void Start()
	{
		Destroy(this.gameObject, waitTime);
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		Invoke("Spawn", 0.3f);
	}

	private void Update()
	{
		//Invoke("CheckSide", 2.5f);
		//CheckSide();
		//if (templates.rooms.Count >= 5)
		//	CheckSide();
	}

	void CheckSide()
	{
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 100f, layer);
		//Debug.Log(hit.gameObject.name);
		if (openingDirection == 1 && hit == null)
		{
			Instantiate(horizontalWall, new Vector3(transform.position.x - 0.125f, transform.position.y - 2.575f, transform.position.z), Quaternion.identity);
			Instantiate(horizontalWall, new Vector3(transform.position.x + 0.375f, transform.position.y - 2.575f, transform.position.z), Quaternion.identity);
		}
		else if (openingDirection == 2 && hit == null)
		{
			Instantiate(horizontalWall, new Vector3(transform.position.x - 0.125f, transform.position.y + 2.425f, transform.position.z), Quaternion.identity);
			Instantiate(horizontalWall, new Vector3(transform.position.x + 0.375f, transform.position.y + 2.425f, transform.position.z), Quaternion.identity);
		}
		if (openingDirection == 3 && hit == null)
			Instantiate(verticalWall, new Vector3(transform.position.x - 2.969f, transform.position.y, transform.position.z), Quaternion.identity);
		else if (openingDirection == 4 && hit == null)
			Instantiate(verticalWall, new Vector3(transform.position.x + 2.969f, transform.position.y, transform.position.z), Quaternion.identity);

	}
	
	void Spawn()
	{
		if (spawned == false && templates.rooms.Count <= templates.maxNumOfRooms)
		{
			if (openingDirection == 1)
			{
				// Need to spawn a room with a BOTTOM door.
				rand = Random.Range(0, templates.bottomRooms.Length);
				Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
			}
			else if (openingDirection == 2)
			{
				// Need to spawn a room with a TOP door.
				rand = Random.Range(0, templates.topRooms.Length);
				Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
			}
			else if (openingDirection == 3)
			{
				// Need to spawn a room with a LEFT door.
				rand = Random.Range(0, templates.leftRooms.Length);
				Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
			}
			else if (openingDirection == 4)
			{
				// Need to spawn a room with a RIGHT door.
				rand = Random.Range(0, templates.rightRooms.Length);
				Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
			}
			spawned = true;
		}
	}

	//void OnTriggerEnter2D(Collider2D other)
	//{
	//	if (other.CompareTag("SpawnPoint"))
	//	{
	//		if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
	//		{
	//			//Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
	//			//Destroy(gameObject);
	//		}
	//		spawned = true;
	//	}
	//}

  //  private void OnDrawGizmos()
  //  {
		//Gizmos.DrawSphere(transform.position, 2);
  //  }
}
