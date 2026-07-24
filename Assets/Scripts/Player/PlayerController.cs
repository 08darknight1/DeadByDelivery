using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed;
    private Rigidbody PlayerRigidbody;

    private Player RewiredPlayer;

    void Start()
    {
        PlayerRigidbody = gameObject.GetComponent<Rigidbody>();
        RewiredPlayer = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalMov = RewiredPlayer.GetAxisRaw("MoveX") * PlayerSpeed;
        var verticalMov = RewiredPlayer.GetAxisRaw("MoveY") * PlayerSpeed;

        PlayerRigidbody.linearVelocity = new Vector2(horizontalMov, verticalMov);
    }
}
