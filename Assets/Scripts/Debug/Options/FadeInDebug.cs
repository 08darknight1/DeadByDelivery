using UnityEngine;
using UnityEngine.UI;

public class FadeInDebug : DebugOption
{
    private GameController GameControllerObj;

    public FadeInDebug(): base("FadeInDebug"){}

    public override void OptionSetup()
    {
        GameControllerObj = GameObject.Find("GameController").GetComponent<GameController>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(GameControllerObj != null)
        {
            GameControllerObj.UseFadePanel(true);

            if(GameControllerObj.UseFadePanel(true))
            {
                OptionQuit();
            }
        }
    }

    public override void OptionQuit()
    {
        GameControllerObj = null;
        base.OptionQuit();
    }
}
