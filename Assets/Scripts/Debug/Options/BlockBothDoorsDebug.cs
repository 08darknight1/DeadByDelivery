using UnityEngine;

public class BlockBothDoorsDebug : DebugOption
{
    public BlockBothDoorsDebug(): base("BlockBothDoorsDebug"){}

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
                if (Car.transform.GetChild(x).GetComponent<CarDoorController>())
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
