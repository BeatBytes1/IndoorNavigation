using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : DefaultObserverEventHandler
{
    public GameObject TargetPositionObject;
    public GameObject positionObject;
    public GameObject indicator;
    public GameObject ThisImageTarget;
    private bool firstTimeTrackingFound = true;
    public Vector3 initialTargetPosition;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        if (firstTimeTrackingFound)
        {
            // Store the initial target position
            initialTargetPosition = TargetPositionObject.transform.position;
            firstTimeTrackingFound = false;
        }

        // Set the positions to the initial target position
        indicator.transform.position = initialTargetPosition;
        positionObject.transform.position = initialTargetPosition;

       /*Debug.Log("x = " + mainCamera.transform.position.x);
        Debug.Log("y = " + mainCamera.transform.position.y);
        Debug.Log("z = " + mainCamera.transform.position.z);*/
        ThisImageTarget.SetActive(false);


    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        // Reset the flag when tracking is lost
        firstTimeTrackingFound = true;
        // Deactivate the ImageTarget
        ThisImageTarget.SetActive(false);
    }
}