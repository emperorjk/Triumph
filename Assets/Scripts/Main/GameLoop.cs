using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Main gameloop
public class GameLoop : MonoBehaviour 
{
    public GameObject doneButton;

    private GameManager _manager;
    private RaycastHit _touchBox;
    private Highlight _highlight;

	void Start () 
    {
        _manager = GameManager.Instance;
		_manager.SetupLevel();

        _highlight = new Highlight();
	}

    void Update()
    {
        GameManager.Instance.productionOverlayMain.OnUpdate();
        ActivateDoneButton();

        _highlight.HandleHighlightInput();
    }

    void ActivateDoneButton()
    {
        // for development KeyCode.Y testing DoneButton
        if (Input.touchCount == 2 || Input.GetKeyDown(KeyCode.Y))
        {
            if (!_manager.IsDoneButtonActive)
            {
                _manager.IsDoneButtonActive = true;
                doneButton.SetActive(true);
            }
            else
            {
                _manager.IsDoneButtonActive = false;
                doneButton.SetActive(false);
            }
        }

        // for development KeyCode.T next player
        if (Input.GetKeyDown(KeyCode.T))
        {
            _manager.NextPlayer();
        }
    }
}