
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VuforiaTrigger : MonoBehaviour
{
    public GameObject Initializer;
    static Initializer init;

    private void Start()
    {
        init = Initializer.GetComponent<Initializer>();
    }


    private PositionalDeviceTracker deviceTracker;
    private GameObject previousAnchor;

    public void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    public void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    private void OnVuforiaStarted()
    {
        deviceTracker = TrackerManager.Instance.GetTracker<PositionalDeviceTracker>();
    }

    public void SpawnContent()
    {
        init.enabled = true;
    }
}