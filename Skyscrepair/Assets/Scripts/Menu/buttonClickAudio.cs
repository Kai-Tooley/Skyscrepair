using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonClickAudio : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string buttonClickEvent = "";

    public void EmittButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonClickEvent);
    }

}
