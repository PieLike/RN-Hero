using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    private Animator animFade;
    public int sceneToLoad;
    public Slider slider;
    public GameObject loadingScreen;

    private void Start() 
    {
        animFade = GetComponent<Animator>();
    }

    public void FadeToScene()
    {
        animFade.SetTrigger("fade");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    /*IEnumerator LoadingScreenOnFade()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
        
    }*/
}
