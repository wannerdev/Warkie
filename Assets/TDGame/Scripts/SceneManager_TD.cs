using System.Collections.Generic;
using UnityEngine;

public class SceneManager_TD : MonoBehaviour {

    public static SceneManager_TD instance;
    public GameObject addButton;

    public string objectToPlace;

    public List<GameObject> objects;

	void Start () {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
	}
	public void placeObject(){
        //instantiate object on placed anchor
        print("Anchor Position: " + DetectSurface.instance.anchorPosition);
        GameObject newObject = Instantiate(Resources.Load(objectToPlace) as GameObject, DetectSurface.instance.anchorPosition, DetectSurface.instance.anchorRotation * Quaternion.Euler(0,180,0));
        //add object to manager list
        objects.Add(newObject);
    }
    public void addObjectToScene(){
        if(Anchor.instance != null){
            Debug.LogWarning("Anchoring already processing, please first finish anchoring before adding a new object!");
            return;
        }
        Instantiate(Resources.Load("Anchor")as GameObject, Vector3.zero, Quaternion.identity);
    }
}
