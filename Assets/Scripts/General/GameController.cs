using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FadePanel;

    private DialogHandler DialogHandlerObj;

    private PlayerController PlayerControllerObj;

    private CarController CarControllerObj;

    private int GameState;

    private bool ActivateFadePanel, FadeIn;

    private List<bool> GameTriggers = new List<bool>();

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
        DialogHandlerObj = gameObject.GetComponent<DialogHandler>();
        ActivateFadePanel = true;
        PlayerControllerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CarControllerObj = GameObject.FindGameObjectWithTag("Car").GetComponent<CarController>();

        GameTriggers.Add(false); //GameTrigger 0 - PlayerHasWatchedIntro
        GameTriggers.Add(false); //GameTrigger 1 - PlayerHasStartedTutorial
        GameTriggers.Add(false); //GameTrigger 2 - PlayerHasEnteredCar
        GameTriggers.Add(false); //GameTrigger 3 - PlayerHasReachedEndOfCourse
        GameTriggers.Add(false); //GameTrigger 4 - PlayerCanDeliverTutorialPackage
        GameTriggers.Add(false); //GameTrigger 5 - PlayerHasDeliveredTutorialPackage
        GameTriggers.Add(false); //GameTrigger 6 - PlayerOnlyHasToReturnToCar
        GameTriggers.Add(false); //GameTrigger 7 - PlayerHasCompletedTutorial
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivateFadePanel)
        {
            MakePanelFade();
        }

        switch (GameState)
        {
            case 0:
                if (!ActivateFadePanel && !GameTriggers[0])
                {
                    DialogHandlerObj.StartNewConversation(1);
                    GameTriggers[0] = true;
                }

                if (GameTriggers[0] && !DialogHandlerObj.ReturnConversationStatus())
                {
                    GameState = 1;
                }
            break;
            case 1:
                if(!GameTriggers[1])
                {
                    if (!DialogHandlerObj.ReturnConversationStatus())
                    {
                        DialogHandlerObj.StartNewConversation(2);
                        GameTriggers[1] = true;
                    }
                }
                else
                {
                    if (!GameTriggers[2])
                    {
                        if (!DialogHandlerObj.ReturnConversationStatus())
                        {
                            PlayerControllerObj.ChangeMovementEnabled(true);

                            if (CarControllerObj.ReturnPlayerIsOnCar())
                            {
                                GameTriggers[2] = true;
                                DialogHandlerObj.StartNewConversation(3);
                                PlayerControllerObj.ChangeMovementEnabled(false);
                            }
                        }
                    }
                }

                if (!GameTriggers[3])
                {
                    if(GameTriggers[2] && !DialogHandlerObj.ReturnConversationStatus())
                    {
                        PlayerControllerObj.ChangeMovementEnabled(true);
                    }
                }
                else
                {
                    if(!GameTriggers[4])
                    {
                        CarControllerObj.StopMovementCompletely();
                        PlayerControllerObj.ChangeMovementEnabled(false);
                        DialogHandlerObj.StartNewConversation(4);
                        GameTriggers[4] = true;
                    }
                }

                if(!GameTriggers[6])
                {
                    if(!GameTriggers[5])
                    {
                        if (GameTriggers[4])
                        {
                            if (!DialogHandlerObj.ReturnConversationStatus())
                            {
                                PlayerControllerObj.ChangeMovementEnabled(true);
                            }
                        }
                    }
                    else
                    {
                        PlayerControllerObj.ChangeMovementEnabled(false);
                        DialogHandlerObj.StartNewConversation(5);
                        GameTriggers[6] = true;
                    }
                }
                else
                {
                    if (!CarControllerObj.ReturnPlayerIsOnCar())
                    {
                        if (!DialogHandlerObj.ReturnConversationStatus())
                        {
                            PlayerControllerObj.ChangeMovementEnabled(true);
                        }
                    }
                    else
                    {
                        if(!GameTriggers[7])
                        {
                            GameTriggers[7] = true;
                            PlayerControllerObj.ChangeMovementEnabled(false);
                            DialogHandlerObj.StartNewConversation(6);
                        }
                    }
                }

                if(GameTriggers[7] && !DialogHandlerObj.ReturnConversationStatus())
                {
                    GameState = 2;
                }
            break;
            case 2:
                CarControllerObj.transform.position = Vector3.zero;
            break;
            default:
                Debug.Log("Idk, just want to do wjat I can at this point...");
            break;
        }
    }

    public void ChangeGameTrigger(int index, bool newValue)
    {
        GameTriggers[index] = newValue;
    }

    public int ReturnGameState()
    {
        return GameState;
    }

    public void SetNewGameSpeed(float newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }

    private void MakePanelFade()
    {
        if (FadeIn && FadePanel.GetComponent<Image>().color.a < 1)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a + (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
        }
        else if(!FadeIn && FadePanel.GetComponent<Image>().color.a > 0)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a - (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
        }
        else
        {
            ActivateFadePanel = false;
        }
    }

    public bool UseFadePanel(bool fadeIn)
    {
        ActivateFadePanel = true;
        FadeIn = fadeIn;

        if (FadeIn && FadePanel.GetComponent<Image>().color.a >= 1)
        {
            return true;
        }
        else if(!FadeIn && FadePanel.GetComponent<Image>().color.a <= 0)
        {
            return true;
        }

        return false;
    }
}
