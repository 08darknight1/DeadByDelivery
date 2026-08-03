using Rewired;
using UnityEngine;

public class DeliveryObjective : MonoBehaviour
{
    public bool TutorialObjective;

    private GameController _gameController;

    private Player _playerInput;

    private bool _playerOnDeliveryZone, _deliveryDone;

    private GameObject _groundMarker;

    void Start()
    {
        _playerInput = ReInput.players.GetPlayer(0);
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _groundMarker = transform.Find("GroundMarker").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerOnDeliveryZone)
        {
            if (_playerInput.GetButtonDown("Interact") && !_deliveryDone)
            {
                _deliveryDone = true;
                _groundMarker.SetActive(false);

                if (TutorialObjective)
                {
                    _gameController.ChangeGameTrigger(5, true);
                }
                else
                {
                    _gameController.DeliveredNewPackage();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerOnDeliveryZone = true;
        }
    }

    void OTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerOnDeliveryZone = false;
        }
    }
}
