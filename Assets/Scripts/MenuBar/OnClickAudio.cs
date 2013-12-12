using UnityEngine;
using System.Collections;

public class OnClickAudio : MonoBehaviour 
{

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (GameManager.Instance.IsAudioOn())
            {
                this.gameObject.guiTexture.texture = (Texture)Resources.Load("Textures/IngameMenu/speaker_off");

                AudioListener.pause = true;
                GameManager.Instance.ChangeAudio(false);
            }
            else if (!GameManager.Instance.IsAudioOn())
            {
                this.gameObject.guiTexture.texture = (Texture)Resources.Load("Textures/IngameMenu/speaker_on");

                AudioListener.pause = false;
                GameManager.Instance.ChangeAudio(true);
            }
        }
    }
}
