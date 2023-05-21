    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform checkLeft;
    public Transform checkRight;
    public Transform checkTop;
    public Transform checkBottom;
    public LayerMask roomLayer;
    private GameObject doorController;
    public GameObject horizontalWall;
    public GameObject verticalWall;
    // Start is called before the first frame update
    void Start()
    {
        doorController = GameObject.FindWithTag("DoorController");
        //Invoke("CheckForDoor", 2);
        if (checkLeft != null)
            Invoke("CheckSidesLR", 4);
        else if (checkTop != null)
            Invoke("CheckSidesTB", 4);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckForDoor()
    {
        Collider2D check = Physics2D.OverlapCircle(transform.position, 2, 7);
        if (check)
        {
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    private void CheckSidesLR()
    {
        RaycastHit2D left = Physics2D.CircleCast(new Vector2(checkLeft.position.x, checkLeft.position.y), 0.5f, new Vector2(0, 0));
        RaycastHit2D right = Physics2D.CircleCast(new Vector2(checkRight.position.x, checkRight.position.y), 0.5f, new Vector2(0, 0));
        if (left.collider == null)
        {
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            doorController.GetComponent<DoorController>().count = 0;
            Instantiate(verticalWall, new Vector3(transform.position.x, transform.position.y - 0.04599977f, 0), Quaternion.identity);
            Instantiate(verticalWall, new Vector3(transform.position.x, transform.position.y + 0.5470003f, 0), Quaternion.identity);
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            Destroy(gameObject);
        }
        else if (right.collider == null)
        {
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            doorController.GetComponent<DoorController>().count = 0;
            Instantiate(verticalWall, new Vector3(transform.position.x, transform.position.y - 0.04599977f, 0), Quaternion.identity);
            Instantiate(verticalWall, new Vector3(transform.position.x, transform.position.y + 0.5470003f, 0), Quaternion.identity);
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    private void CheckSidesTB()
    {
        RaycastHit2D top = Physics2D.CircleCast(new Vector2(checkTop.position.x, checkTop.position.y), 0.5f, new Vector2(0, 0));
        RaycastHit2D bottom = Physics2D.CircleCast(new Vector2(checkBottom.position.x, checkBottom.position.y), 0.5f, new Vector2(0, 0));
        if (top.collider == null)
        {
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            doorController.GetComponent<DoorController>().count = 0;
            Instantiate(horizontalWall, new Vector3(transform.position.x - 0.421f, transform.position.y, 0), Quaternion.identity);
            Instantiate(horizontalWall, new Vector3(transform.position.x + 0.079f, transform.position.y, 0), Quaternion.identity);
            Instantiate(horizontalWall, new Vector3(transform.position.x + 0.579f, transform.position.y, 0), Quaternion.identity);
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            Destroy(gameObject);
        }
        else if (bottom.collider == null)
        {
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            doorController.GetComponent<DoorController>().count = 0;
            Instantiate(horizontalWall, new Vector3(transform.position.x - 0.421f, transform.position.y, 0), Quaternion.identity);
            Instantiate(horizontalWall, new Vector3(transform.position.x + 0.079f, transform.position.y, 0), Quaternion.identity);
            Instantiate(horizontalWall, new Vector3(transform.position.x + 0.579f, transform.position.y, 0), Quaternion.identity);
            doorController.GetComponent<DoorController>().doors.Remove(gameObject);
            Destroy(gameObject);
        }
    }
        private void OnDrawGizmos()
    {
        if(checkLeft != null)
            Gizmos.DrawWireSphere(new Vector3(transform.position.x - 3.671f, transform.position.y, 0), 0.25f);
    }
    public void CloseDoors()
    {
        transform.GetChild(0).GetComponent<Animator>().Play("DoorCloseAnim");
    }
    public void OpenDoors()
    {
        transform.GetChild(0).GetComponent<Animator>().Play("DoorOpenAnim");
    }
}
