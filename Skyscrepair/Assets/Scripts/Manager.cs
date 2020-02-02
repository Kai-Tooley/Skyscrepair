using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string atmosphearEvent = "";
    FMOD.Studio.EventInstance atmosphear;

    [FMODUnity.EventRef]
    public string musicEvent = "";
    FMOD.Studio.EventInstance music;

    // Start is called before the first frame update
    void Start()
    {
        atmosphear = FMODUnity.RuntimeManager.CreateInstance(atmosphearEvent);
        music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
