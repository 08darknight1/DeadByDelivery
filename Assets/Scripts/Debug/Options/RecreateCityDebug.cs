using UnityEngine;

public class RecreateCityDebug : DebugOption
{
    private CityGenerator CityGeneratorObj;

    public RecreateCityDebug() : base("RecreateCityDebug"){}

    public override void OptionSetup()
    {
        CityGeneratorObj = GameObject.Find("GameController").GetComponent<CityGenerator>();
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(CityGeneratorObj != null)
        {
            CityGeneratorObj.DestroyAndRecreate();
            
            OptionQuit();
        }
    }

    public override void OptionQuit()
    {
        CityGeneratorObj = null;
        base.OptionQuit();
    }
}
