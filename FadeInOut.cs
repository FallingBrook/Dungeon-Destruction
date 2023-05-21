using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public string newScene;
    public bool fadeIn;
    // Start is called before the first frame update
    void Start()
    {
        if (fadeIn)
        {
            FadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("FadeOut"))
            SwitchScene();
    }

    public void FadeIn()
    {
        GetComponent<Animator>().Play("FadeIn");
    }

    public void FadeOut()
    {
        GetComponent<Animator>().Play("FadeOut");
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(newScene);
    }
}
