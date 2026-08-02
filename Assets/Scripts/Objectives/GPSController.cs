using UnityEngine;

public class GPSController : MonoBehaviour
{
    public GameObject ArrowImage;

    private bool _objectiveSelected;

    private Transform _transformToTrack, _playerTransform;
    
    private GameObject _arrowPointer;

    private CarController _carController;

    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _carController = GameObject.FindGameObjectWithTag("Car").GetComponent<CarController>();
    }

    void Update()
    {
        if (_carController.ReturnPlayerIsOnCar())
        {
            _playerTransform = _carController.transform;
        }
        else
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if(_playerTransform != null)
        {
            if (_objectiveSelected)
            {
                if(_arrowPointer == null)
                {
                    ChangeArrowActive(true);
                    _arrowPointer = new GameObject();
                    _arrowPointer.name = "Arrow Pointer";
                    _arrowPointer.transform.SetParent(GameObject.Find("Canvas").transform.Find("GameplayPanel").transform);
                }

                _arrowPointer.transform.position = new Vector3(_transformToTrack.position.x - _playerTransform.position.x
                                                            , _transformToTrack.position.z - _playerTransform.position.z, 0);
                
                ArrowImage.transform.rotation = Quaternion.LookRotation(Vector3.forward, _arrowPointer.transform.position);
            }
            else
            {
                ChangeArrowActive(false);
                _arrowPointer = null;
            }
        }
    }

    public void SetNewObjective(Transform toTrack)
    {
        _objectiveSelected = true;
        _transformToTrack = toTrack;
    }

    public void StopTracking()
    {
        _objectiveSelected = false;
        _transformToTrack = null;
    }

    public void ChangeArrowActive(bool active)
    {
        ArrowImage.SetActive(active);
    }
}
