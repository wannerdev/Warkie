using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public Vector3 dir;
    public float speed;
    public GameObject Parent;
    public int damage = 1;
    public float lifetime = 5.0f;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.Translate(dir * speed);

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != Parent.tag)
        {
            TDBaseClass Base = collision.gameObject.GetComponent<TDBaseClass>();
            if (Base)
            {

                Base.take_damage(damage,Parent);
            }
       


            Destroy(gameObject);
        }
    }
}
