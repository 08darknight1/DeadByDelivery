using Rewired;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed;

    private Rigidbody _playerRigidbody;

    private Player _playerInput;

    private bool _movementEnabled;

    void Start()
    {
        _playerRigidbody = gameObject.GetComponent<Rigidbody>();
        _playerInput = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(_movementEnabled)
        {
            var horizontalMov = _playerInput.GetAxisRaw("MoveX") * PlayerSpeed;
            var verticalMov = _playerInput.GetAxisRaw("MoveY") * PlayerSpeed;
            _playerRigidbody.AddForce(new Vector3(horizontalMov, 0, verticalMov));
        }
    }

    public void ChangeMovementEnabled(bool newValue)
    {
        _movementEnabled = newValue;
    }

    public bool ReturnMovementEnable()
    {
        return _movementEnabled;
    }
}
