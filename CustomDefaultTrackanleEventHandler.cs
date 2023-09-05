using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class CustomDefaultTrackableEventHandler : DefaultObserverEventHandler
{

    [SerializeField]
    public GameObject AppearObject;
    /*
    [SerializeField]
    public GameObject Kitchen;
    */

    [SerializeField]
    public Transform TargetObject;
 
    [SerializeField]
    public Transform arCamera;
    [SerializeField]
    public Transform indicator;


    public UnityEvent OnTrackingAction;
    public UnityEvent OffTrackingAction;

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        OnTrackingAction.Invoke();
        //if (AppearObject.activeSelf)
        //{
            PlaceObjectsAtImagePosition(TargetObject.transform.position);
        //}
    }



    private void PlaceObjectsAtImagePosition(Vector3 imagePosition)
    {
        arCamera.position = imagePosition;
        indicator.position = imagePosition;

        if (imagePosition != TargetObject.position)
        {
            TargetObject.position = imagePosition;
        }
       
    }

}
