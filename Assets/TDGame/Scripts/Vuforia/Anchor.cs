using UnityEngine;

public class Anchor : MonoBehaviour
{
    public static Anchor instance;

    public GameObject foundSquare;

    void Start()
    {
        //Destroy previous instance of Anchor
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;

        DetectSurface.instance.searching = true;
        StartCoroutine(DetectSurface.instance.searchSurface());
    }
    void OnDestroy()
    {
        DetectSurface.instance.searching = false;
    }
}
