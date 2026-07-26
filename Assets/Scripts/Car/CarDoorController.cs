using UnityEngine;
using Rewired;
using Unity.Collections;

public class CarDoorController : MonoBehaviour
{
    private CarController CarControllerObj;

    private GameObject Player;

    private bool CanEnterCar = false;

    private Player RewiredPlayer;

    void Start()
    {
        CarControllerObj = gameObject.transform.parent.gameObject.GetComponent<CarController>();
        RewiredPlayer = ReInput.players.GetPlayer(0);
    }

    void Update()
    {
        if (CanEnterCar && Player.GetComponent<PlayerController>().ReturnMovementEnable())
        {
            if (RewiredPlayer.GetButtonDown("Interact"))
            {
                if(!CarControllerObj.ReturnPlayerIsOnCar())
                {
                    Player.transform.parent = CarControllerObj.transform;
                    Player.GetComponent<PlayerController>().ChangeMovementEnabled(false);
                    Player.SetActive(false);
                    CarControllerObj.SetPlayerIsOnCar(true);
                }
                else
                {
                    CheckIfPlayerCanLeave();
                }
            }
        }
    }

    private void CheckIfPlayerCanLeave()
    {
        var centerOfPlane = gameObject.transform.GetComponent<Renderer>().bounds.center;

        if(RaycastFromObject(centerOfPlane))
        {
            Debug.Log("No can do sir! Trying the other door");

            var parent = gameObject.transform.parent;

            for (int x = 0; x < parent.childCount; x++)
            {
                if(parent.GetChild(x).GetComponent<CarDoorController>() && parent.GetChild(x).name != name)
                {
                    centerOfPlane = parent.GetChild(x).transform.GetComponent<Renderer>().bounds.center;

                    if (RaycastFromObject(centerOfPlane))
                    {
                        Debug.Log("Both doors blocked...");
                    }
                    else
                    {
                        SetPlayerOutOfCar(centerOfPlane);
                    }
                }
            }
        }
        else
        {
            SetPlayerOutOfCar(centerOfPlane);
        }
    }

    private void SetPlayerOutOfCar(Vector3 playerNewPos)
    {
        Player.transform.position = playerNewPos;
        Player.transform.parent = null;
        Player.GetComponent<PlayerController>().ChangeMovementEnabled(true);
        Player.SetActive(true);
        CarControllerObj.SetPlayerIsOnCar(false);
    }

    private bool RaycastFromObject(Vector3 origin)
    {
        Debug.DrawRay(origin, Vector3.up, Color.aquamarine, Mathf.Infinity);

        var RaycasterHit = Physics.Raycast(origin, Vector3.up, out RaycastHit Hit, Mathf.Infinity);

        if(RaycasterHit)
        {
            Debug.Log("Hit this thing: " + Hit.transform.name);
            return true;
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player enterred Door Collider!");
            Player = other.gameObject;
            CanEnterCar = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player left Door Collider!");
            Player = null;
            CanEnterCar = false;
        }
    }
}
