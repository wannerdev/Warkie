using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public static Interaction instance;

    void Start()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    RaycastHit FindHit()
    {
        RaycastHit hit;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

        }
        else
        {
            hit = new RaycastHit();
        }
        return hit;
    }
    void Update()
    {
        //if anchoring is processing
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //if anchor exists place object
            if (Anchor.instance != null)
            {
                //check if button is hit, if yes cancel
                RaycastHit hitResult = FindHit();

                if (hitResult.transform)
                {
                    if (hitResult.transform.GetComponent<Button>())
                    {
                        return;
                    }
                }
                return;
            }
        }
    }
}