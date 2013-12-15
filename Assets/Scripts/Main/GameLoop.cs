using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour {

    public GameObject doneButton;

    private GameManager _manager;
    private float _tapTimer = 0.5f;
    private float _resetTimer = 0.5f;
    private float _tapCount = 0;
    private RaycastHit _touchBox;
    private GameObject _doneButtonGameObject;

	void Start () 
    {
        _manager = GameManager.Instance;
	}
	
	void Update ()
    {
        if (!_manager.isDoneButtonActive)
        {
            // start timer when user has clicked once
            if (_tapCount == 1)
            {
                _tapTimer -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_tapTimer >= 0)
                {
                    _tapCount++;

                    if (_tapCount >= 2)
                    {
                        doneButton.SetActive(true);
                        _manager.isDoneButtonActive = true;
                        _tapCount = 0;
                        _tapTimer = _resetTimer;
                    }
                }
                else
                {
                    _tapTimer = _resetTimer;
                    _tapCount = 0;
                }
            }
        }
        else if (_manager.isDoneButtonActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
               Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

               if (Physics.Raycast(ray, out _touchBox))
               {
                   if (_touchBox.collider == doneButton.collider)
                   {
                       doneButton.SetActive(false);
                       _manager.isDoneButtonActive = false;
                       _manager.NextPlayer();
                   }
                   else
                   {
                       doneButton.SetActive(false);
                       _manager.isDoneButtonActive = false;
                   }
               }
            }
        }
	}
}