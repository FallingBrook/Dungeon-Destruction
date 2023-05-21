using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{

	public GameObject[] bottomRooms;
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;

	public GameObject closedRoom;

	public List<GameObject> rooms;

	public float waitTime;
	private bool spawnedBoss;
	public GameObject boss;

	public GameObject[] doors;

	public LayerMask doorLayer;

	public GameObject enemyController;
	public int maxNumOfRooms;
	[HideInInspector] public bool shopSpawned = false;
	private void Start()
    {
		//anim = transform.GetChild(0).GetComponent<Animator>();
	}
	void Update()
	{

		if (waitTime <= 0 && spawnedBoss == false)
		{
			for (int i = 0; i < rooms.Count; i++)
			{
				if (i == rooms.Count - 1)
				{
					Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
					spawnedBoss = true;
				}
			}
		}
		else
		{
			waitTime -= Time.deltaTime;
		}
		if(rooms.Count > maxNumOfRooms)
        {
			GameObject ob = rooms[maxNumOfRooms].gameObject;
			rooms.Remove(rooms[maxNumOfRooms].gameObject);
			Destroy(ob);
		}
	}

}
