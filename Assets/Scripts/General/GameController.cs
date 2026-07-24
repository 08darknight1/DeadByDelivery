using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FadePanel;

    private DialogHandler DialogHandlerObj;

    void Start()
    {
        Time.timeScale = 1;
        DialogHandlerObj = gameObject.GetComponent<DialogHandler>();
        //DialogHandlerObj.StartNewConversation(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNewGameSpeed(float newGameSpeed)
    {
        Time.timeScale = newGameSpeed;
    }

    public bool UseFadePanel(bool fadeIn)
    {
        if (fadeIn && FadePanel.GetComponent<Image>().color.a < 1)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a + (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
            return false;
        }
        else if(!fadeIn && FadePanel.GetComponent<Image>().color.a > 0)
        {
            var newAlpha = FadePanel.GetComponent<Image>().color.a - (0.35f * Time.deltaTime);
            FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
            return false;
        }

        return true;
    }
}
