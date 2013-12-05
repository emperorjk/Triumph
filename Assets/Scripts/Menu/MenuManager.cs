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
        menuPositions = new Vector3[3];
        Vector3 startScreen = GameObject.FindGameObjectWithTag("BackgroundStart").renderer.bounds.center;
        Vector3 levelScreen = GameObject.FindGameObjectWithTag("BackgroundLevel").renderer.bounds.center;
        Vector3 guideScreen = GameObject.FindGameObjectWithTag("BackgroundGuide").renderer.bounds.center;

        // set all z values to zero otherwise the camera has the same z value as the background image
        levelScreen.z = 0;
        guideScreen.z = 0;
        startScreen.z = 0;

        menuPositions[0] = startScreen;
        menuPositions[1] = levelScreen;
        menuPositions[2] = guideScreen;
    }
}
