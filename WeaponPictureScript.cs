using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPictureScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void ChangePicture(GameObject wp)
    {
        transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
        if (wp.transform.GetChild(0).CompareTag("MeleeWeapon") && !wp.transform.GetChild(0).GetComponent<SwordWeapon>().regular)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = wp.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite;
        }
        else
            transform.GetChild(0).GetComponent<Image>().sprite = wp.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
        if (wp.transform.GetChild(0).CompareTag("MeleeWeapon"))
            transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -45);
        else
            transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 30);
        if (transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x > 68 || transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y > 64)
        {
            transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(0.95f, 0.95f, 0);
            if (wp.transform.GetChild(0).CompareTag("MeleeWeapon") && !wp.transform.GetChild(0).GetComponent<SwordWeapon>().regular)
            {
                transform.GetChild(0).GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -135);
                transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(-0.95f, -0.95f, 0);
            }
        }
        transform.GetChild(0).GetComponent<Image>().SetNativeSize();
    }
}
