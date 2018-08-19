using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootAnimTrigger : MonoBehaviour {

    // Use this for initialization
    Infantry Parent;
	void Start () {
        Parent = GetComponentInParent<Infantry>();

    }
	
	public void triggerShot()
    {
        Parent.SpawnProjectile(Parent.projectileOrigin.position,Parent.Target.position);


    }
}
