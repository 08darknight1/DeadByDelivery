using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FadePanel;

    private DialogHandler _dialogHandler;

    private PlayerController _playerController;

    private CarController _carController;

    private CutsceneController _cutsceneController;

    private int _gameState;

    private bool _activateFadePanel, _fadeIn;

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
        _activateFadePanel = true;
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _carController = GameObject.FindGameObjectWithTag("Car").GetComponent<CarController>();
        _cutsceneController = gameObject.GetComponent<CutsceneController>();

        _gameTriggers.Add(false); //GameTrigger 0 - PlayerHasWatchedIntro
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
                    _dialogHandler.StartNewConversation(1);
                    _gameTriggers[0] = true;
                }

                if (_gameTriggers[0] && !_dialogHandler.ReturnConversationStatus())
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
                                _dialogHandler.StartNewConversation(3);
                                _playerController.ChangeMovementEnabled(false);
                            }
                        }
                    }
                }

                if (!_gameTriggers[3])
                {
                    if(_gameTriggers[2] && !_dialogHandler.ReturnConversationStatus())
                    {
                        _playerController.ChangeMovementEnabled(true);
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
                            _dialogHandler.StartNewConversation(6);
                        }
                    }
                }

                if(_gameTriggers[7] && !_dialogHandler.ReturnConversationStatus())
                {
                    _gameState = 2;
                }
            break;
            case 2:
                _carController.transform.position = Vector3.zero;
            break;
            default:
                Debug.Log("Idk, just want to do wjat I can at this point...");
            break;
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
