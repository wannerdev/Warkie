using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  this script handles the interaction of the base player projectiles
/// </summary>
public class PlayerShot : MonoBehaviour {

    public float lifetime = 5.0f;
    public int damage;
    public GameObject Parent;
    public float Player; 
	// Use this for initialization
	void Start () {
        // set off timer
        Destroy(this.gameObject, lifetime);
        // set collision layer
        if (Player == 1)
        {
            gameObject.layer = 9;

        }
        else
        {
            gameObject.layer = 8;


        }
	}
    /// <summary>
    ///  handle collisions and interactions
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {

     
            TDBaseClass Base = collision.gameObject.GetComponent<TDBaseClass>();
            if (Base)
            {

                Base.take_damage(damage, Parent);
            }



            Destroy(gameObject);
        }
    }



