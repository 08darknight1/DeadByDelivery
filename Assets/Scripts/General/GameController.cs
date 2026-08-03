using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FadePanel, DeliveryPackagePrefab;

    private DialogHandler _dialogHandler;

    private PlayerController _playerController;

    private CarController _carController;

    private CutsceneController _cutsceneController;

    private GPSController _gpsController;

    private GameObject _packagesText;

    private int _gameState;

    private GameObject[] _packagesToDeliver;

    private bool _activateFadePanel, _fadeIn, _setupNewWorkDay;

    private List<bool> _gameTriggers = new List<bool>();

    /*
        0 - Intro
        1 - Tutorial
        2 - Gameplay
        3 - Day Intermission
        4 - Dies
        5 - GameOver
    */

    void Start()
    {
        Time.timeScale = 1;

        _dialogHandler = gameObject.GetComponent<DialogHandler>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _carController = GameObject.FindGameObjectWithTag("Car").GetComponent<CarController>();
        _cutsceneController = gameObject.GetComponent<CutsceneController>();
        _gpsController = gameObject.GetComponent<GPSController>();

        _packagesText = GameObject.Find("PackagesText");
        _packagesText.SetActive(false);

        PlayerData.DaysOnTheJob = 0;

        _gameTriggers.Add(false); //GameTrigger 0 - PlayerIsWatchingIntro
        _gameTriggers.Add(false); //GameTrigger 1 - PlayerHasStartedTutorial
        _gameTriggers.Add(false); //GameTrigger 2 - PlayerHasEnteredCar
        _gameTriggers.Add(false); //GameTrigger 3 - PlayerHasReachedEndOfCourse
        _gameTriggers.Add(false); //GameTrigger 4 - PlayerCanDeliverTutorialPackage
        _gameTriggers.Add(false); //GameTrigger 5 - PlayerHasDeliveredTutorialPackage
        _gameTriggers.Add(false); //GameTrigger 6 - PlayerOnlyHasToReturnToCar
        _gameTriggers.Add(false); //GameTrigger 7 - PlayerHasCompletedTutorial
    }

    // Update is called once per frame
    void Update()
    {
        if (_activateFadePanel)
        {
            MakePanelFade();
        }

        switch (_gameState)
        {
            case 0:
                if (!_activateFadePanel && !_gameTriggers[0])
                {
                    _cutsceneController.StartCutscene(1);
                    _gameTriggers[0] = true;
                }

                if (_gameTriggers[0] && !_cutsceneController.ReturnCutsceneActive())
                {
                    _gameState = 1;
                }
            break;
            case 1:
                if(!_gameTriggers[1])
                {
                    if (!_dialogHandler.ReturnConversationStatus())
                    {
                        _dialogHandler.StartNewConversation(2);
                        _gameTriggers[1] = true;
                    }
                }
                else
                {
                    if (!_gameTriggers[2])
                    {
                        if (!_dialogHandler.ReturnConversationStatus())
                        {
                            _playerController.ChangeMovementEnabled(true);

                            if (_carController.ReturnPlayerIsOnCar())
                            {
                                _gameTriggers[2] = true;
                                _cutsceneController.StartCutscene(3);
                                _playerController.ChangeMovementEnabled(false);
                            }
                        }
                    }
                }

                if (!_gameTriggers[3])
                {
                    if(_gameTriggers[2] && !_cutsceneController.ReturnCutsceneActive())
                    {
                        _playerController.ChangeMovementEnabled(true);
                        var tutorialObjective = GameObject.Find("TutorialZone").transform.Find("DeliveryPackage").transform;
                        _gpsController.SetNewObjective(tutorialObjective);
                    }
                }
                else
                {
                    if(!_gameTriggers[4])
                    {
                        _carController.StopMovementCompletely();
                        _playerController.ChangeMovementEnabled(false);
                        _dialogHandler.StartNewConversation(4);
                        _gameTriggers[4] = true;
                        _gpsController.StopTracking();
                    }
                }

                if(!_gameTriggers[6])
                {
                    if(!_gameTriggers[5])
                    {
                        if (_gameTriggers[4])
                        {
                            if (!_dialogHandler.ReturnConversationStatus())
                            {
                                _playerController.ChangeMovementEnabled(true);
                            }
                        }
                    }
                    else
                    {
                        _playerController.ChangeMovementEnabled(false);
                        _dialogHandler.StartNewConversation(5);
                        _gameTriggers[6] = true;
                    }
                }
                else
                {
                    if (!_carController.ReturnPlayerIsOnCar())
                    {
                        if (!_dialogHandler.ReturnConversationStatus())
                        {
                            _playerController.ChangeMovementEnabled(true);
                        }
                    }
                    else
                    {
                        if(!_gameTriggers[7])
                        {
                            _gameTriggers[7] = true;
                            _playerController.ChangeMovementEnabled(false);
                            _cutsceneController.StartCutscene(6);
                        }
                    }
                }

                if(_gameTriggers[7])
                {
                    if (!_cutsceneController.ReturnCutsceneActive())
                    {
                        _gameState = 2;
                    }
                    else
                    {
                        if(UseFadePanel(true))
                        {
                            _carController.transform.position = Vector3.one;
                        }
                    }
                }
            break;
            case 2:
                if(!_setupNewWorkDay)
                {
                    NewWorkDay();
                    _setupNewWorkDay = true;
                }
                else if(UseFadePanel(false))
                {
                    _playerController.ChangeMovementEnabled(true);
                }

                UpdateUIGameplayText();
            break;
            default:
                Debug.Log("Idk, just want to do wjat I can at this point...");
            break;
        }
    }

    public void NewWorkDay()
    {
        _packagesToDeliver = new GameObject[Random.Range(3, 3 + PlayerData.DaysOnTheJob)];

        var fullSpawnSpots = GameObject.FindGameObjectsWithTag("DeliverySpawn");

        var previousCitySectionSelected = "";

        for(int x = 0; x < _packagesToDeliver.Length; x++)
        {
            var setNewPos = false;

            _packagesToDeliver[x] = Instantiate(DeliveryPackagePrefab);

            while (!setNewPos)
            {
                var randomPos = Random.Range(0, fullSpawnSpots.Length - 1);

                var spawnSelected = fullSpawnSpots[randomPos];

                var spawnParentName = spawnSelected.transform.parent.name;

                if(spawnSelected.activeSelf && spawnParentName != previousCitySectionSelected)
                {
                    setNewPos = true;

                    var spawnSelectedPos = spawnSelected.transform.position;
                    spawnSelectedPos = new Vector3(spawnSelectedPos.x, 2.2f, spawnSelectedPos.z);

                    _packagesToDeliver[x].transform.position = spawnSelectedPos;

                    previousCitySectionSelected = spawnParentName;
                    fullSpawnSpots[randomPos].SetActive(false);
                }
            }

            _packagesToDeliver[x].transform.parent = GameObject.Find("City").transform;
        }

        _gpsController.SetNewObjective(_packagesToDeliver[0].transform);
    }

    public void DeliveredNewPackage()
    {
        if(_packagesToDeliver != null)
        {
            var packagesLenghtMinus = _packagesToDeliver.Length - 1;

            if(packagesLenghtMinus > 0)
            {
                var newPackageList = new GameObject[_packagesToDeliver.Length - 1];

                var copyListIndex = 0;

                for(int x = 0; x < _packagesToDeliver.Length; x++)
                {
                    if(x != 0)
                    {
                        newPackageList[copyListIndex] = _packagesToDeliver[x];
                        copyListIndex++;
                    }
                }

                _packagesToDeliver = new GameObject[newPackageList.Length];

                for(int x = 0; x < newPackageList.Length; x++)
                {
                    _packagesToDeliver[x] = newPackageList[x];
                }

                _gpsController.SetNewObjective(_packagesToDeliver[0].transform);
            }
            else
            {
                _packagesToDeliver = null;
                _gpsController.StopTracking();
            }
        }
    }

    public void UpdateUIGameplayText()
    {
        _packagesText.SetActive(true);

        if(_packagesToDeliver != null)
        {
            if(_packagesToDeliver.Length > 0)
            {
                var currentText = "Packages to Deliver: ";
                _packagesText.GetComponent<TextMeshProUGUI>().text = currentText + " " + _packagesToDeliver.Length;
            }
        }
        else
        {
            var newText = "No more packages!";
            _packagesText.GetComponent<TextMeshProUGUI>().text = newText;
        }
    }

    public void ChangeGameTrigger(int index, bool newValue)
    {
        _gameTriggers[index] = newValue;
    }

    public int ReturnGameState()
    {
        return _gameState;
    }

    public void SetNewGameSpeed(float newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }

    private void MakePanelFade()
    {
        if (_fadeIn && FadePanel.GetComponent<Image>().color.a < 1)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a + (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
        }
        else if(!_fadeIn && FadePanel.GetComponent<Image>().color.a > 0)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a - (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
        }
        else
        {
            _activateFadePanel = false;
        }
    }

    public bool UseFadePanel(bool fadeIn)
    {
        _activateFadePanel = true;
        _fadeIn = fadeIn;

        if (_fadeIn && FadePanel.GetComponent<Image>().color.a >= 1)
        {
            return true;
        }
        else if(!_fadeIn && FadePanel.GetComponent<Image>().color.a <= 0)
        {
            return true;
        }

        return false;
    }
}
