using UnityEngine;

public class PlayTestDialogDebug : DebugOption
{
    private DialogHandler DialogHandlerObj;

    public PlayTestDialogDebug(): base("PlayTestDialogueDebug"){}

    private bool DialogActivated;
    
    public override void OptionSetup()
    {
        DialogHandlerObj = GameObject.Find("GameController").GetComponent<DialogHandler>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(DialogHandlerObj != null)
        {
            if(!DialogActivated){
                DialogHandlerObj.StartNewConversation(0);
                DialogActivated = true;
            }
            else if(DialogActivated && !DialogHandlerObj.ReturnConversationStatus())
            {
                OptionQuit();
            }
        }
    }

    public override void OptionQuit()
    {
        DialogActivated = false;
        DialogHandlerObj = null;
        base.OptionQuit();
    }
}
