using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Menu_LevelSelect : MonoBehaviour
{

    Manager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();
    }
    // Start is called before the first frame update
    public void OpenLevel(int level_number)
    {
        SceneManager.LoadScene(level_number, LoadSceneMode.Single);
        manager.level = level_number;
    }

}
