using UnityEngine;

public class ButtonClickHandler : MonoBehaviour
{
    public Transform mainCamera;
    public Transform indicator;
    public Transform positionObject;

    public void LateUpdate()
    {
            // Set the positions of Main Camera and Indicator to match PositionObject
            mainCamera.transform.position = positionObject.transform.position;
            indicator.transform.position = positionObject.transform.position;

           Debug.Log("x = " + mainCamera.transform.position.x);
        Debug.Log("y = " + mainCamera.transform.position.y);
        Debug.Log("z = " + mainCamera.transform.position.z);
    
    }
}
