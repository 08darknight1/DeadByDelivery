using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewGameSpeed(float newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }
}
