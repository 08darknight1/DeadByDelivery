using UnityEngine;

public class SpawnTestCarDebug : DebugOption
{
    public SpawnTestCarDebug(): base("SpawnTestCarDebug"){}

    private GameObject Player;

    public GameObject CarPrefab;

    private bool SpawnedCar;
    
    public override void OptionSetup()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        base.OptionSetup();
    }

    public override void OptionExec()
    {
        if(Player != null && !SpawnedCar)
        {
            var playerPos = Player.transform.position;

            var playerPosPlus = new Vector3(playerPos.x + 20, playerPos.y, playerPos.z);

            Instantiate(CarPrefab, playerPosPlus, Quaternion.identity);

            SpawnedCar = true;
        }
    }

    public override void OptionQuit()
    {
        SpawnedCar = false;
        Player = null;
        base.OptionQuit();
    }
}
