using UnityEngine;

[System.Serializable]
public class DebugOption : MonoBehaviour
{
    protected string Name;

    protected bool Activated;

    public DebugOption(string newName)
    {
        Name = newName;
        Activated = false;
    }

    public virtual void OptionSetup()
    {
        Debug.Log("Starting Option - " + Name);

        Activated = true;
    }

    public virtual void OptionExec()
    {
        Debug.Log("Executing Option - " + Name);
    }

    public virtual void OptionQuit()
    {
        Debug.Log("Closing Option - " + Name);

        Activated = false;
    }

    public virtual bool OptionReturnActivation()
    {
        return Activated;
    }

    public virtual string OptionReturnName()
    {
        return Name;
    }
}
