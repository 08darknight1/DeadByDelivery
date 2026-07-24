using Rewired;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float CarAcceleration;

    private float CurrentCarSpeed;

    private bool PlayerIsOnCar;

    private Rigidbody CarRigidbody;

    private Player RewiredPlayer;

    void Start()
    {
        CarRigidbody = gameObject.GetComponent<Rigidbody>();
        RewiredPlayer = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerIsOnCar)
        {
            var horizontalMov = RewiredPlayer.GetAxisRaw("MoveX") * CarAcceleration;
            var verticalMov = RewiredPlayer.GetAxisRaw("MoveY") * CarAcceleration;
            CarRigidbody.AddForce(new Vector3(horizontalMov, 0, verticalMov));
        }
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
