using System.Collections.Generic;
using System.Linq;
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
            var tilesToCreate = CityBlocksList[x].ReturnCityBlockTiles();

            for(int y = 0; y < tilesToCreate.Length; y++) //SEMPRE VAI SER 9
            {
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
                                Debug.Log("Creating New Road Tile!");
                                var newTile = Instantiate(RoadTilesPrefabs[z]);
                                var posXValue = positionIncrement * posXMultiplier;
                                var posYValue = positionIncrement * posYMultiplier;
                                newTile.transform.position = new Vector3(posXValue, 0, posYValue * -1);
                                newTile.transform.parent = City.transform;
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
                                Debug.Log("Creating New Building Tile!");
                                var newTile = Instantiate(BuildingPrefabs[z]);
                                var posXValue = positionIncrement * posXMultiplier;
                                var posYValue = positionIncrement * posYMultiplier;
                                newTile.transform.position = new Vector3(posXValue, 0, posYValue * -1);
                                newTile.transform.parent = City.transform;
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

                if(posXMultiplier >= ((tilesToCreate.Length / 2) - 1) * (columnsCreated + 1))
                {
                    posXMultiplier = posXMultiplierOG;
                    posYMultiplier++;
                }

                if(posYMultiplier >= ((tilesToCreate.Length / 2) - 1) * (columnsCreated + 1))
                {
                    posYMultiplier = posYMultiplierOG;
                }
            }

            if (!createNewLine)
            {
                posXMultiplierOG += (tilesToCreate.Length / 2) - 1;
                posXMultiplier = posXMultiplierOG;
                posYMultiplier = posYMultiplierOG;
                columnsCreated++;
            }
            else
            {
                posXMultiplierOG = 0;
                posXMultiplier = posXMultiplierOG;
                posYMultiplierOG += (tilesToCreate.Length / 2) - 1;
                posYMultiplier = posYMultiplierOG;
                columnsCreated = 0;
            }
        }
    }

    public void DestroyAndRecreate()
    {
        Destroy(City);
        SpawnCurrentCity();
    }
}