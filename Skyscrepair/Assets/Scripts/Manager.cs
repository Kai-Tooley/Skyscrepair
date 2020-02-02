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

     public int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        atmosphear = FMODUnity.RuntimeManager.CreateInstance(atmosphearEvent);
        music = FMODUnity.RuntimeManager.CreateInstance(musicEvent);

        atmosphear.start();
        music.start();
    }

    // Update is called once per frame
    void Update()
    {
        music.setParameterValue("Level", (float)level);
        music.setParameterValue("Tilt", 0.0f);
    }

    private void OnDestroy()
    {
        atmosphear.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        atmosphear.release();
        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        music.release();
    }
}
