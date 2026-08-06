using UnityEngine;

public class FadeOutDebug : DebugOption
{
    private FadeController _fadeController;

    public FadeOutDebug(): base("FadeOutDebug"){}
    
    public override void OptionSetup()
    {
        _fadeController = GameObject.Find("GameController").GetComponent<FadeController>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(_fadeController != null)
        {
            _fadeController.MakePanelFade(false);

            if(!_fadeController.ReturnFadeHasFinished()[0] && _fadeController.ReturnFadeHasFinished()[1])
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
