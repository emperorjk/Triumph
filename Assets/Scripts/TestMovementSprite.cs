using UnityEngine;
using System.Collections;

public class TestMovementSprite : MonoBehaviour {

    private GameObject unitA;
    private Vector2 unitAVector2;
    private Vector2 destHor;
    private Vector2 destVer;
    private Vector2 destSide;


    private float duration = 2f;
    private float startTime;
    private float input = 0;

	void Start () 
    {
        unitA = this.gameObject;
        unitAVector2 = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
        destHor = new Vector2(unitAVector2.x + 3f, unitAVector2.y);
        destVer = new Vector2(unitAVector2.x, unitAVector2.y + 3f);
        destSide = new Vector2(unitAVector2.x + 2f, unitAVector2.y + 3f);

        startTime = Time.time;
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            input++;
            startTime = Time.time;
        }
        if (input == 1)
        {
            unitA.transform.position = Vector2.Lerp(unitAVector2, destHor, (Time.time - startTime) / duration);
        }
        else if (input == 2)
        {
            unitA.transform.position = Vector2.Lerp(destHor, unitAVector2, (Time.time - startTime) / duration);
        }
        else if (input == 3)
        {
            unitA.transform.position = Vector2.Lerp(unitAVector2, destVer, (Time.time - startTime) / duration);
        }
        else if (input == 4)
        {
            unitA.transform.position = Vector2.Lerp(destVer, unitAVector2, (Time.time - startTime) / duration);
        }
        else if (input == 5)
        {
            unitA.transform.position = Vector2.Lerp(unitAVector2, destSide, (Time.time - startTime) / duration);

        }
        else if (input == 6)
        {
            unitA.transform.position = Vector2.Lerp(destSide, unitAVector2, (Time.time - startTime) / duration);
            input = 0;
        }
        Debug.Log(unitA.transform.position);
	}
}
