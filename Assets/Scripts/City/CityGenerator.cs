using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public int CityBlocksSize;

    private List<CityBlock> CityBlocksList = new List<CityBlock>();

    public GameObject[] RoadTilesPrefabs;

    public GameObject[] BuildingPrefabs;

    private GameObject City;

    void Start()
    {
        for(int x = 0; x < CityBlocksSize; x++)
        {
            var newBlock = new CityBlock();

            newBlock.SetCityBlock(CrossCityBlock.CrossCityBlockString, CrossCityBlock.CrossCityBlockWalkable);

            CityBlocksList.Add(newBlock);
        }

        Debug.Log("Created Exactly " + CityBlocksList.Count + " City Block(s)!");

        SpawnCurrentCity();
    }

    private void SpawnCurrentCity()
    {
        City = new GameObject();

        City.transform.position = new Vector3(0, 0, 0);

        City.name = "City";

        var numberForObj = 0;
        var numberForBlock = 0;

        var positionIncrement = 40;

        var posXMultiplier = 0;
        var posXMultiplierOG = 0;

        var posYMultiplier = 0;
        var posYMultiplierOG = 0;

        var columnsCreated = 0;

        var createNewLine = false;

        if(CityBlocksSize >= 4 && CityBlocksSize%2 == 0)
        {
            createNewLine = true;
        }

        for(int x = 0; x < CityBlocksList.Count; x++)
        {
            var newBlock = new GameObject();

            newBlock.transform.parent = City.transform;

            newBlock.name = "Block" + numberForBlock.ToString();

            var tilesToCreate = CityBlocksList[x].ReturnCityBlockTiles();

            for(int y = 0; y < tilesToCreate.Length; y++) //SEMPRE VAI SER 9
            {

            Debug.Log("POSXMULT: " + posXMultiplier + " | OGVALUE: " + posXMultiplierOG +
                 " ||| POSYMULT: " + posYMultiplier + " | OGVALUE: " + posYMultiplierOG);

                var type = "road";

                if (!tilesToCreate[y].ReturnWalkable())
                {
                    type = "building";
                }

                switch (type)
                {
                    case "road":
                        for(int z = 0; z < RoadTilesPrefabs.Length; z++)
                        {
                            var prefabLastChars = RoadTilesPrefabs[z].name.Remove(0, (RoadTilesPrefabs[z].name.Length - 2));
                            if(tilesToCreate[y].ReturnDefiningChar() == prefabLastChars)
                            {
                                //Debug.Log("Creating New Road Tile!");
                                var newTile = Instantiate(RoadTilesPrefabs[z]);
                                var posXValue = positionIncrement * posXMultiplier;
                                var posYValue = positionIncrement * posYMultiplier;
                                newTile.transform.position = new Vector3(posXValue, 0, posYValue * -1);
                                newTile.transform.parent = newBlock.transform;
                                newTile.name += numberForObj.ToString();
                                numberForObj++;
                            }
                        }
                    break;
                    case "building":
                        for(int z = 0; z < BuildingPrefabs.Length; z++)
                        {
                            var prefabLastChars = BuildingPrefabs[z].name.Remove(0, (BuildingPrefabs[z].name.Length - 2));
                            if(tilesToCreate[y].ReturnDefiningChar() == prefabLastChars)
                            {
                                //Debug.Log("Creating New Building Tile!");
                                var newTile = Instantiate(BuildingPrefabs[z]);
                                var posXValue = positionIncrement * posXMultiplier;
                                var posYValue = positionIncrement * posYMultiplier;
                                newTile.transform.position = new Vector3(posXValue, 0, posYValue * -1);
                                newTile.transform.parent = newBlock.transform;
                                newTile.name += numberForObj.ToString();
                                numberForObj++;
                            }
                        }
                    break;
                    default:
                        Debug.Log("Something def went wrong here...");
                    break;
                }

                posXMultiplier++;

                var thingy = (tilesToCreate.Length / 2) - 1;
                var thingy2 = columnsCreated + 1;
                var thingy3 = thingy * thingy2;

                Debug.Log("Tiles Length: " + thingy + " | Columns + 1: " + thingy2 + " | Result Multi: " + thingy3);

                if(posXMultiplier >= ((tilesToCreate.Length / 2) - 1) * (columnsCreated + 1))
                {
                    posXMultiplier = posXMultiplierOG;
                    posYMultiplier++;
                }
            }

            posXMultiplierOG += (tilesToCreate.Length / 2) - 1;
            posXMultiplier = posXMultiplierOG;
            posYMultiplier = posYMultiplierOG;
            columnsCreated++;

           // if (createNewLine)
           // {
                if(columnsCreated >= CityBlocksList.Count / 2)
                {
                    columnsCreated = 0;

                    posXMultiplierOG = 0;
                    posXMultiplier = posXMultiplierOG;

                    posYMultiplierOG += (tilesToCreate.Length / 2) - 1;
                    posYMultiplier = posYMultiplierOG;
                }
           // }
        }
    }

    public void DestroyAndRecreate()
    {
        Destroy(City);

        CityBlocksList.Clear();

        for(int x = 0; x < CityBlocksSize; x++)
        {
            var newBlock = new CityBlock();

            newBlock.SetCityBlock(CrossCityBlock.CrossCityBlockString, CrossCityBlock.CrossCityBlockWalkable);

            CityBlocksList.Add(newBlock);
        }

        SpawnCurrentCity();
    }
}