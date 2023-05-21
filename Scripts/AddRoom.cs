using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{

	private RoomTemplates templates;

    public GameObject enemyController;

    public LayerMask playerLayer;
	void Start()
	{

		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		templates.rooms.Add(this.gameObject);
	}

    private void Update()
    {
    }
}
