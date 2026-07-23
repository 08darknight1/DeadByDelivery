using UnityEngine;
using UnityEngine.UI;

public class FadeInDebug : DebugOption
{
    private GameObject FadePanel;

    public FadeInDebug(): base("FadeInDebug"){}

    public override void OptionSetup()
    {
        Activated = true;
        FadePanel = GameObject.Find("FadePanel");
    }

    public override void OptionExec()
    {
        if(FadePanel != null)
        {
            //Debug.Log("During FadeIn, Alpha Image value: " + FadePanel.GetComponent<Image>().color.a);
            if(FadePanel.GetComponent<Image>().color.a < 1)
            {
                var newAlpha = FadePanel.GetComponent<Image>().color.a + (0.25f * Time.deltaTime);
                FadePanel.GetComponent<Image>().color = new Color(0, 0, 0, newAlpha);
            }
            else
            {
                OptionQuit();
            }
        }
    }

    public override void OptionQuit()
    {
        FadePanel = null;
        base.OptionQuit();
    }
}
