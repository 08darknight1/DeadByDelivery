using UnityEngine;
using UnityEngine.UI;

public class FadeOutDebug : DebugOption
{
    private GameController GameControllerObj;

    public FadeOutDebug(): base("FadeOutDebug"){}
    
    public override void OptionSetup()
    {
        GameControllerObj = GameObject.Find("GameController").GetComponent<GameController>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(GameControllerObj != null)
        {
            GameControllerObj.UseFadePanel(false);

            if(GameControllerObj.UseFadePanel(false))
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
