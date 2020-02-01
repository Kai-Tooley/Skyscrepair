using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu_LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenLevel(int level_number)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + level_number, LoadSceneMode.Single);
    }

}
