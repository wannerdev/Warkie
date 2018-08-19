using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PlaceBoard : MonoBehaviour {

    public GameObject planeFinder;
    public GameObject button;

    public void stopPlacing(){
        Destroy(planeFinder);
        Destroy(button);
    }
}
