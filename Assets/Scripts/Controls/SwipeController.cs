using Assets.Scripts.Events;
using UnityEngine;
using EventHandler = Assets.Scripts.Events.EventHandler;

namespace Assets.Scripts.Controls
{
    public class SwipeController : MonoBehaviour
    {

        // Min swipe distance downwards.
        private float minSwipeDistance = 50f;
        // The difference the fingers can go to the left or right.
        private float swipeVariance = 30f;
        public bool IsSwipeHappening { get; set; }
        private Vector2 SwipingVector;

        private void Awake()
        {
            IsSwipeHappening = false;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                UpdateSwipeActions(Input.touchCount);
            }
        }


        private void UpdateSwipeActions(int fingerCount)
        {
            for (int i = 0; i < fingerCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began && !IsSwipeHappening)
                {
                    SwipingVector = touch.position;
                    IsSwipeHappening = true;
                }
                else if (touch.phase == TouchPhase.Moved && IsSwipeHappening)
                {
                    Vector2 delta = touch.position - SwipingVector;
                    Debug.Log("----------");
                    Debug.Log("Swipe Delta: " + delta);
                    Debug.Log("Swipe Deltamag: " + delta.magnitude);
                    Debug.Log("----------");
                    if (delta.magnitude > minSwipeDistance && Mathf.Abs(delta.x) < swipeVariance)
                    {
                        if (delta.y < 0)
                        {
                            // down swipe
                            IsSwipeHappening = false;
                            OnSwipeAction swipeDown = new OnSwipeAction(fingerCount, false, false, false, true);
                            EventHandler.dispatch<OnSwipeAction>(swipeDown);
                        }
                        else if (delta.y > 0)
                        {
                            // Up swipe
                            IsSwipeHappening = false;
                            OnSwipeAction swipeUp = new OnSwipeAction(fingerCount, false, false, true, false);
                            EventHandler.dispatch<OnSwipeAction>(swipeUp);
                        }
                    }
                        // Left and right swipe not working yet.
                    else if (delta.magnitude > minSwipeDistance && Mathf.Abs(delta.y) < swipeVariance)
                    {
                        if (delta.x < 0)
                        {
                            // Swipe left
                            //IsSwipeHappening = false;
                            //OnSwipeAction swipe = new OnSwipeAction(fingerCount, true, false, false, false);
                            //EventHandler.dispatch<OnSwipeAction>(swipe);
                        }
                        else if (delta.x > 0)
                        {
                            // Swipe right
                            //IsSwipeHappening = false;
                            //OnSwipeAction swipe = new OnSwipeAction(fingerCount, false, true, false, false);
                            //EventHandler.dispatch<OnSwipeAction>(swipe);
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended && IsSwipeHappening)
                {
                    SwipingVector = Vector2.zero;
                    IsSwipeHappening = false;
                }
            }
        }
    }
}