using System;
using UnityEngine;


public class CurrentPositionIdentify : MonoBehaviour
{
    //public ARTrackedImageManager arTrackedImageManager;

    [SerializeField]
    public GameObject Desk;

    [SerializeField]
    public GameObject Kitchen;

    [SerializeField]
    public Transform deskObject;
    [SerializeField]
    public Transform kitchenObject;
    [SerializeField]
    public Transform arCamera;
    [SerializeField]
    public Transform indicator;

   
    private void OnEnable()
    {

            if (Desk.activeSelf)
            {
                PlaceObjectsAtImagePosition(Desk.transform.position);
            }
            else if (Kitchen.activeSelf)
            {
                PlaceObjectsAtImagePosition(Kitchen.transform.position);
            }
            
        //}
    }

    private void PlaceObjectsAtImagePosition(Vector3 imagePosition)
    {
        arCamera.position = imagePosition;
        indicator.position = imagePosition;

        if (imagePosition == deskObject.position)
        {
            deskObject.position = imagePosition;
        }
        else if (imagePosition == kitchenObject.position)
        {
            kitchenObject.position = imagePosition;
        }
    }
}
