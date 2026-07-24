using UnityEngine;

public class BlockRightDoorDebug : DebugOption
{
    public BlockRightDoorDebug(): base("BlockRightDoorDebug"){}

    private GameObject Car;
    
    public override void OptionSetup()
    {
        Car = GameObject.FindGameObjectWithTag("Car");
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(Car != null)
        {
            for(int x = 0; x < Car.transform.childCount; x++)
            {
                if (Car.transform.GetChild(x).GetComponent<CarDoorController>() && Car.transform.GetChild(x).name.Contains("Right"))
                {
                    var centerPos = Car.transform.GetChild(x).transform.GetComponent<Renderer>().bounds.center;
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = centerPos;
                }
            }

            OptionQuit();
        }
    }

    public override void OptionQuit()
    {
        base.OptionQuit();
    }
}
