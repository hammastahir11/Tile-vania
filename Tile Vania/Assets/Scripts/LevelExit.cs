using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay=2f;
    void OnTriggerEnter2D(Collider2D col)
    {
        var desobj = FindObjectOfType<ScenePersistent>();
        if (desobj!=null)
        {
            Destroy(desobj);
        }
        
        StartCoroutine(LoadNextLevel());   
    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1); 
    }


}
