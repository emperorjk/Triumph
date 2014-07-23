using Assets.Scripts.Events;
using Assets.Scripts.Main;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.MenuBar
{
    public class DoneButton : MonoBehaviour
    {
        private bool _needsFading;
        private bool _fadeIn = true;
        private GameLoop gameLoop;

        private void Start()
        {
            Vector3 centerWorldSpace = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 5));
            transform.position = new Vector3(centerWorldSpace.x, centerWorldSpace.y + 1, centerWorldSpace.z);
            EventHandler.register<OnSwipeAction>(SwipeDoneButton);
            Color col = renderer.material.color;
            col.a = 0f;
            renderer.material.color = col;

            gameLoop = GameObject.Find("_Scripts").GetComponent<GameLoop>();
        }

        private void OnDestroy()
        {
            EventHandler.unregister<OnSwipeAction>(SwipeDoneButton);
        }

        private void SwipeDoneButton(OnSwipeAction evt)
        {
            if (evt.SwipeUp && evt.fingerCount == 2)
            {
                gameLoop.IsDoneButtonActive = !gameLoop.IsDoneButtonActive;
                renderer.enabled = true;
                collider.enabled = true;
                _needsFading = true;
            }
        }

        private void Fade()
        {
            Color col = renderer.material.color;
            col.a = Mathf.Lerp(renderer.material.color.a, (_fadeIn ? 1 : 0), Time.deltaTime*4.5f);

            if (!_fadeIn && col.a < 0.10f)
            {
                col.a = 0f;
                _needsFading = false;
                _fadeIn = true;
                renderer.enabled = gameLoop.IsDoneButtonActive;
                collider.enabled = gameLoop.IsDoneButtonActive;
            }
            else if (_fadeIn && col.a > 0.95f)
            {
                _fadeIn = false;
                _needsFading = false;
                col.a = 1f;
            }
            renderer.material.color = col;
        }

        private void Update()
        {
            if (_needsFading)
            {
                Fade();
            }

            if (Input.GetKeyDown(KeyCode.T) && !gameLoop.IsDoneButtonActive && !_needsFading)
            {
                SwipeDoneButton(new OnSwipeAction(2, false, false, true, false));
            }

            if (Input.GetMouseButtonDown(0) && !_needsFading)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit _touchBox;
                if (Physics.Raycast(ray, out _touchBox))
                {
                    if (_touchBox.collider == collider)
                    {
                        GameObject.Find("_Scripts").GetComponent<GameLoop>().EndTurn();
                        SwipeDoneButton(new OnSwipeAction(2, false, false, true, false));
                    }
                }
            }
        }
    }
}