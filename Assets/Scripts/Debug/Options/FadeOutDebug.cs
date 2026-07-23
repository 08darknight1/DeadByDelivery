using UnityEngine;
using UnityEngine.UI;

public class FadeOutDebug : DebugOption
{
    private GameObject FadePanel;

    public FadeOutDebug(): base("FadeOutDebug"){}
    
    public override void OptionSetup()
    {
        FadePanel = GameObject.Find("FadePanel");
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(FadePanel != null)
        {
            if(FadePanel.GetComponent<Image>().color.a > 0)
            {
                var newAlpha = FadePanel.GetComponent<Image>().color.a - (0.25f * Time.deltaTime);
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
