using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to either instantiate and set the globalscript or destroy it. So there are never more than 1 instance of the globalscript prefab.
/// </summary>
public class GlobalScript : MonoBehaviour {

    void Awake()
    {
        if (GameManager.Instance.GlobalScripts)
        {
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.GlobalScripts = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }

}
