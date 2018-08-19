using UnityEngine;
using System.Collections;
using UnityEngine.XR.iOS;
using System.Collections.Generic;

public class DetectSurface : MonoBehaviour
{
    public static DetectSurface instance;
    public bool searching;

    public Vector3 anchorPosition;
    public Quaternion anchorRotation;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    public IEnumerator searchSurface()
    {
        while (searching)
        {
            //center of screen with depth of max search plane depth
            Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0.5f);

            Vector2 screenPosition = Camera.main.ScreenToViewportPoint(center);

            ARPoint point = new ARPoint
            {
                x = screenPosition.x,
                y = screenPosition.y
            };

            //result of ray from middle of screen touching a surface in reality
            List<ARHitTestResult> respoints = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeFeaturePoint);
            foreach (ARHitTestResult respoint in respoints)
            {
                //make sure point is not too close to camera
                if(UnityARMatrixOps.GetPosition(respoint.worldTransform).y <= 0 && UnityARMatrixOps.GetPosition(respoint.worldTransform).z >= 0){
                    
                    if (Anchor.instance != null)
                    {
                        //setting rotation and position of square
                        Anchor.instance.foundSquare.transform.position = UnityARMatrixOps.GetPosition(respoint.worldTransform);
                        anchorPosition = UnityARMatrixOps.GetPosition(respoint.worldTransform);
                        Anchor.instance.foundSquare.transform.rotation = UnityARMatrixOps.GetRotation(respoint.worldTransform);
                    }
                }
            }
            //Is the camera facing downwards?
            if (Vector3.Dot(Camera.main.transform.forward, Vector3.down) > 0)
            {
                Vector3 vecToCamera;
                //vector from camera to focussquare
                if (Anchor.instance != null)
                {
                    vecToCamera = Anchor.instance.foundSquare.transform.position - Camera.main.transform.position;
                }
                else
                {
                    vecToCamera = anchorPosition - Camera.main.transform.position;
                }
                //find vector that is orthogonal to camera vector and up vector
                Vector3 vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                //find vector orthogonal to both above and up vector to find the forward vector in basis function
                Vector3 vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);

                //setting rotation of foundSquare along to calculated vector, so that the square is parallel to the camera
                if (Anchor.instance != null)
                {
                    Anchor.instance.foundSquare.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);
                    anchorRotation = Anchor.instance.foundSquare.transform.rotation;
                }
                else
                {
                    anchorRotation = Quaternion.LookRotation(vecForward, Vector3.up);
                }
            }
            yield return null;
        }
    }
}
