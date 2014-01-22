using UnityEngine;
using System.Collections;

public class GameBar : MonoBehaviour {

    // These values are set via unity editor.
    public GUISkin customSkin;
    public Texture2D gamebarTexture;
    public Texture2D settingsTexture;
    public Texture2D speakerOnTexture;
    public Texture2D speakerOffTexture;

    private Rect gamebarSize;
    public float verticalGameBarSize = 50f;
    public float minSizeBar = 175f;
    public float maxSizeBar = 240;

    // Update function for the GUI
    void OnGUI()
    {
        gamebarSize = new Rect(0, 0, Screen.width, verticalGameBarSize);
        if(GUI.changed || !GUI.changed)
        {
            // If you want to change the style of the buttons, labels, etc. Look for GameBarGUISkin in Unity and edit the style in there.
            GUI.skin = customSkin;

            // The commented code should do the same as the line below, but the second rect needs some values i cant figure out. UV coordinates or some stuff.
            //GUI.DrawTextureWithTexCoords(gamebarSize, gamebarTexture, new Rect(0f, 0f, 1f, 1f));

            TileTexture(gamebarTexture, new Rect(0, 0, gamebarTexture.width, gamebarTexture.height), gamebarSize, ScaleMode.StretchToFill);
            
            GUI.BeginGroup(gamebarSize);

            float margin = 100;
            float cellSize = Mathf.Clamp(gamebarSize.width / 5, minSizeBar, maxSizeBar);

            // TO-DO stop using .top and instead use xMin etc. And general improvements / cleanups to the code.
            Rect player = new Rect(margin, gamebarSize.height / 2 - 20, cellSize, gamebarSize.height);
            Rect gold = new Rect(player.width + margin, player.top, player.width + cellSize, player.height);
            Rect turn = new Rect(gold.width + margin, gold.top, gold.width + cellSize, gold.height);
            
            Rect settings = new Rect(turn.width + margin, 0, settingsTexture.width, settingsTexture.height - 12);
            Rect speaker = new Rect(turn.width + cellSize, 0, speakerOnTexture.width, speakerOnTexture.height - 12);

            GUI.Label(player, "Player: " + GameManager.Instance.CurrentPlayer.name);
            GUI.Label(gold, "Current gold: " + GameManager.Instance.CurrentPlayer.gold);
            GUI.Label(turn, "Turn: " + GameManager.Instance.currentTurn);

            if(GUI.Button(settings, settingsTexture))
            {
                // TO-DO create quit or no quit asking stuff thingie.
            }

            // Did not use .pause. Because if it is disabled and you click on a unit then enable it will play the sound. E.g. it stacks the sounds.
            if(GameManager.Instance.IsAudioOn)
            {
                if (GUI.Button(speaker, speakerOnTexture))
                {
                    GameManager.Instance.IsAudioOn = false;
                    AudioListener.volume = 0f;
                }
            }
            else
            {
                if (GUI.Button(speaker, speakerOffTexture))
                {
                    GameManager.Instance.IsAudioOn = true;
                    AudioListener.volume = 1f;
                }
            }

            GUI.EndGroup();
        }
    }

    private void TileTexture(Texture texture, Rect tile, Rect areaToFill, ScaleMode scaleMode)
    { 
        for (float y = areaToFill.y; y < areaToFill.y + areaToFill.height; y = y + tile.height)
        {
            for (float x = areaToFill.x; x < areaToFill.x + areaToFill.width; x = x + tile.width)
            {
                tile.x = x; tile.y = y;
                GUI.DrawTexture(tile, texture, scaleMode);
            }
        }
    }
}
