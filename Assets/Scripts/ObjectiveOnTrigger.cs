using UnityEngine;

public class ObjectiveOnTrigger : MonoBehaviour
{
    private GameController GameControllerObj;

    void Start()
    {
        GameControllerObj = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Car")
        {
            GameControllerObj.ChangeGameTrigger(3, true);
        }
    }
}
