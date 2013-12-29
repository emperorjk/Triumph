using UnityEngine;
using System.Collections;

public class OnClickAudio : MonoBehaviour 
{

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (GameManager.Instance.IsAudioOn)
            {
                this.gameObject.renderer.material = (Material)Resources.Load("Textures/IngameMenu/Materials/speaker_off");

                AudioListener.pause = true;
                GameManager.Instance.IsAudioOn = false;
            }
            else if (!GameManager.Instance.IsAudioOn)
            {
                this.gameObject.renderer.material  = (Material)Resources.Load("Textures/IngameMenu/Materials/speaker_on");

                AudioListener.pause = false;
                GameManager.Instance.IsAudioOn = true;
            }
        }
    }
}
