using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float CarAcceleration, CarTurningSpeed;

    private float HorizontalMov, VerticalMov;

    private bool PlayerIsOnCar;

    private Rigidbody CarRigidbody;

    private Player RewiredPlayer;

    private PlayerController PlayerControllerObj;

    private List<WheelCollider> WheelList = new List<WheelCollider>();

    void Start()
    {
        CarRigidbody = gameObject.GetComponent<Rigidbody>();

        RewiredPlayer = ReInput.players.GetPlayer(0);

        PlayerControllerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        var WheelParent = gameObject.transform.Find("Wheels");
        
        for(int x = 0; x < 4; x++)
        {
            WheelList.Add(WheelParent.transform.GetChild(x).GetComponent<WheelCollider>());
        }
    }

    void Update()
    {
        if(PlayerIsOnCar && PlayerControllerObj.ReturnMovementEnable())
        {
            HorizontalMov = RewiredPlayer.GetAxisRaw("MoveX");
            VerticalMov = RewiredPlayer.GetAxisRaw("MoveY");

            for(int x = 0; x < 2; x++)
            {
                WheelList[x].steerAngle = CarTurningSpeed * HorizontalMov;
            }

            MakeWheelModelSpin();
        }
    }

    void FixedUpdate()
    {
        if(PlayerIsOnCar && PlayerControllerObj.ReturnMovementEnable())
        {
            for(int x = 0; x < WheelList.Count; x++)
            {
                WheelList[x].motorTorque = VerticalMov * CarAcceleration;

                if(VerticalMov <= 0)
                {
                    CarRigidbody.linearDamping = 0.5f;
                }
                else
                {
                    CarRigidbody.linearDamping = 0.1f;
                }
            }
        }
    }

    private void MakeWheelModelSpin()
    {
        var wheelModels = gameObject.transform.Find("Model").transform.Find("WheelModels").transform;

        for(int x=0; x < 4; x++)
        {
            var child = wheelModels.GetChild(x).transform;
            var childEulAng = child.localEulerAngles;

            if(HorizontalMov != 0 && x <= 1)
            {
                child.localEulerAngles = new Vector3(childEulAng.x, WheelList[x].steerAngle - childEulAng.z, childEulAng.z);
            }

            //child.Rotate(WheelList[x].rpm / 60 * 360 * Time.deltaTime, 0, 0);
        }
    }

    public void StopMovementCompletely()
    {
        CarRigidbody.linearVelocity = Vector3.zero;
        CarRigidbody.isKinematic = true;
        CarRigidbody.isKinematic = false;
    }

    public void SetPlayerIsOnCar(bool newValue)
    {
        PlayerIsOnCar = newValue;
    }

    public bool ReturnPlayerIsOnCar()
    {
        return PlayerIsOnCar;
    }
}
