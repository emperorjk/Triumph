using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

    public Dictionary<MenuStates, MenuStateBase> states;
    public List<GameObject> images;

    private MenuStateBase _currentState;    
    public Vector3[] menuPositions;
    public GameObject mainCamera;

    void Start () 
    {
        states = new Dictionary<MenuStates, MenuStateBase>();
        menuPositions = new Vector3[5];
        MenuPositions();

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;

        states.Add(MenuStates.StartState, new StartState(this));
        states.Add(MenuStates.GuideState, new GuideState(this));
        states.Add(MenuStates.LevelState, new LevelState(this));

        ChangeState(MenuStates.StartState);
	}
	
	void Update () 
    {
        if (_currentState != null)
        {
            _currentState.OnUpdate();
        }
	}

    public void ChangeState(MenuStates _state)
    {
        if (_currentState != null)
        {
            _currentState.OnInActive();
        }

        _currentState = states[_state];
        _currentState.OnActive();
    }

    /// <summary>
    /// [0] = startScreen
    /// [1] = levelScreen
    /// [2] = guideScreen
    /// </summary>
    private void MenuPositions()
    {
        menuPositions[0] = new Vector3(0, 0, -1);
        menuPositions[1] =  new Vector3(0, -12, -1);
        menuPositions[2] = new Vector3(20, 0, -1);
    }
}
