using UnityEngine;
using UnityEngine.UI;

public class FadeInDebug : DebugOption
{
    private FadeController _fadeController;

    public FadeInDebug(): base("FadeInDebug"){}

    public override void OptionSetup()
    {
        _fadeController = GameObject.Find("GameController").GetComponent<FadeController>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(_fadeController != null)
        {
            _fadeController.MakePanelFade(true);

            if(_fadeController.ReturnFadeHasFinished()[0] && _fadeController.ReturnFadeHasFinished()[1])
            {
                OptionQuit();
            }
        }
    }

    public override void OptionQuit()
    {
        _fadeController = null;
        base.OptionQuit();
    }
}
