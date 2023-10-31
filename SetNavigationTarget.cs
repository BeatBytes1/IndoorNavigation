using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SetNavigationTarget : MonoBehaviour
{


    [SerializeField]  //Inspectorから操作可能にするため
    private TMP_Dropdown navigationTargetDropDown;

    [SerializeField]
    private List<Target> navigationTargetObjects = new List<Target>();

    [SerializeField]
    private Slider navigationYOffset;


    private NavMeshPath path; //current calculated path
    private LineRenderer line; //LineRenderer to dispay path
    private Vector3 targetPosition = Vector3.zero; //current target position

    private int currentFloor = 1;
    private bool lineToggle = false; // lineのオンオフを決める

    // Start is called before the first frame update
    private void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;

    }

    // Update is called once per frame
    private void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)　// lineToggleがオンで、ターゲットポジションが今のターゲットポジションと異なる場合
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;　// cornerはむきを変える距離
            Vector3[] calculatedPathAndOffset = AddLineOffset();
            line.SetPositions(calculatedPathAndOffset);

        }
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {

        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.ToLower().Equals(selectedText.ToLower()));
        if (currentTarget != null)
        {
            if (!line.enabled)
            {
                ToggleVisibility();
            }
            //check if floor is changing
            // if yes, lead to elevator
            // if no, navigate
            targetPosition = currentTarget.PositionObject.transform.position;
        }

　
    }
    public void ToggleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }
    public void ChangeActiveFloor(int floorNumber)
    {
        currentFloor = floorNumber;
        SetNavigationTargetDropDownOptions(currentFloor);
    }
    private Vector3[] AddLineOffset()
    {
        if (navigationYOffset.value == 0)
        {
            return path.corners;
        }
        Vector3[] calculateLine = new Vector3[path.corners.Length];
        for (int i = 0; i < path.corners.Length; i++)
        {
            calculateLine[i] = path.corners[i] + new Vector3(0, navigationYOffset.value, 0);


        }
        return calculateLine;

    }
    private void SetNavigationTargetDropDownOptions(int floorNumber)
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        if (line.enabled)
        {
            ToggleVisibility();
        }
        if (floorNumber == 1)
        {

            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ketichen"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("MainEntrance"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Closet"));
        }
        if(floorNumber == 2)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("SecondfloorMainEntrance"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("ExtraRoom"));
        }

    }
}
