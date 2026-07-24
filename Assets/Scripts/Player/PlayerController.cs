using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed;

    private Rigidbody PlayerRigidbody;

    private Player RewiredPlayer;

    private bool MovementEnabled;

    void Start()
    {
        PlayerRigidbody = gameObject.GetComponent<Rigidbody>();
        RewiredPlayer = ReInput.players.GetPlayer(0);
        MovementEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(MovementEnabled)
        {
            var horizontalMov = RewiredPlayer.GetAxisRaw("MoveX") * PlayerSpeed;
            var verticalMov = RewiredPlayer.GetAxisRaw("MoveY") * PlayerSpeed;
            PlayerRigidbody.AddForce(new Vector3(horizontalMov, 0, verticalMov));
        }
    }

    public void ChangeMovementEnabled(bool newValue)
    {
        MovementEnabled = newValue;
    }
}
