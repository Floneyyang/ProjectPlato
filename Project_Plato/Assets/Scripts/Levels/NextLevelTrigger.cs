using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public string sceneName;
    public bool open = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && open)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
    }
}
