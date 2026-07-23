using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FadePanel;
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

    public void MakeFade(int value)
    {
        FadePanel.GetComponent<Image>().color = new Color(255, 255, 255, value);
    }
}
