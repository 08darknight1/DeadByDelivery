using Rewired;
using UnityEngine;

public class DeliveryObjective : MonoBehaviour
{
    public bool TutorialObjective;

    public GameObject Marker;
    private GameController GameController;
    private Player RewiredPlayer;
    private bool PlayerEnteredObjectiveZone, ObjectiveDone;

    void Start()
    {
        RewiredPlayer = ReInput.players.GetPlayer(0);
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerEnteredObjectiveZone)
        {
            if (RewiredPlayer.GetButtonDown("Interact") && !ObjectiveDone)
            {
                ObjectiveDone = true;
                Marker.SetActive(false);

                if (TutorialObjective)
                {
                    GameController.ChangeGameTrigger(5, true);
                }
                else
                {
                    //FAZER ALGO AQUI COM O GAMECONTROLLER PARA OS OBJECTIVES DA GAMEPLAY NORMAR
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerEnteredObjectiveZone = true;
        }
    }

    void OTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerEnteredObjectiveZone = false;
        }
    }
}
