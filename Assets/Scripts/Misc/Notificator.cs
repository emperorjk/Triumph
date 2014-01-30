using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class Notification
{
    public string text;
    public float time;

    public Notification(string _text, float _time)
    {
        this.text = _text;
        this.time = _time;
    }
}

public class Notificator : MonoBehaviour {

    private static LinkedList<Notification> _notifications;
    private static Notification _current;
    private TextMesh _notificationText;

    void Start () {
        _notifications = new LinkedList<Notification>();
        // Get the text objects and position it exactly at the middle of the screen.
        GameObject nottext = GameObject.Find("NotificationText");
        nottext.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 7));
        _notificationText = nottext.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
        if(_notifications.Count > 0)
        {
            if(_current == null)
            {
                _current = _notifications.First.Value;
                _notificationText.text = _current.text;
            }
            _current.time -= Time.deltaTime;
            if (_current.time < 0)
            {
                _notifications.RemoveFirst();
                _notificationText.text = "";
                _current = null;
            }
        }
	}

    /// <summary>
    /// Add an notify to the last position on the list. So the text that was entered first will display first.
    /// </summary>
    /// <param name="textToDisplay"></param>
    /// <param name="timeToDisplay"></param>
    public static void Notify(string textToDisplay, float timeToDisplay)
    {
        // Make sure we only add the new notification to the list when it is not an empty list and the time to display is greater than 0.
        if(_notifications.Where(x => x.text == textToDisplay).Count() <= 0 && !textToDisplay.Equals("") && timeToDisplay > 0)
        {
            Notification notification = new Notification(textToDisplay, timeToDisplay);
            _notifications.AddLast(notification);
        }
    }
}
