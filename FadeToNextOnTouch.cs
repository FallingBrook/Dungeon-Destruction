using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToNextOnTouch : MonoBehaviour
{
    private bool fading;
    public GameObject image;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !fading)
        {
            fading = true;
            image.GetComponent<FadeInOut>().FadeOut();
        }
    }
}
