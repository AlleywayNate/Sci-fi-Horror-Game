using UnityEngine;

public class CityTerrain : MonoBehaviour
{
    public int gridSize = 200;
    public float cubeSize = 0.1f;
    public GameObject buildingPrefab;
    public GameObject roadPrefab;
    public int chunkSize = 10;
    public float buildingSpacing = 2.0f;
    public float roadWidth = 1.0f;

    private float[,] heightMap;

    void Start()
    {
        // Generate height map
        heightMap = GenerateHeightMap(gridSize, gridSize);

        // Create chunks
        for (int x = 0; x < gridSize; x += chunkSize)
        {
            for (int z = 0; z < gridSize; z += chunkSize)
            {
                CreateChunk(x, z);
            }
        }
    }

    void CreateChunk(int x, int z)
    {
        // Create a new chunk game object
        GameObject chunk = new GameObject("Chunk");
        chunk.transform.parent = transform;

        // Generate buildings for this chunk
        for (int i = 0; i < chunkSize; i++)
        {
            for (int j = 0; j < chunkSize; j++)
            {
                // Check if this is a building spot
                if (IsBuildingSpot(x + i, z + j))
                {
                    // Create a new building
                    GameObject building = Instantiate(buildingPrefab, new Vector3(x + i, heightMap[x + i, z + j], z + j), Quaternion.identity);
                    building.transform.parent = chunk.transform;

                    // Add some variation to the building height
                    building.transform.localScale = new Vector3(1, Random.Range(1.0f, 3.0f), 1);
                }
            }
        }

        // Generate roads for this chunk
        for (int i = 0; i < chunkSize; i++)
        {
            // Check if this is a road spot
            if (IsRoadSpot(x + i, z))
            {
                // Create a new road
                GameObject road = Instantiate(roadPrefab, new Vector3(x + i, heightMap[x + i, z], z), Quaternion.identity);
                road.transform.parent = chunk.transform;

                // Add some variation to the road length
                road.transform.localScale = new Vector3(1, 1, Random.Range(1.0f, 3.0f));
            }
        }
    }

    bool IsBuildingSpot(int x, int z)
    {
        // Check if this spot is not on a road
        if (IsRoadSpot(x, z)) return false;

        // Check if this spot is not too close to another building
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (IsBuildingSpot(x + i, z + j) && Vector3.Distance(new Vector3(x, 0, z), new Vector3(x + i, 0, z + j)) < buildingSpacing)
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool IsRoadSpot(int x, int z)
    {
        // Check if this spot is on a grid line
        if (x % 5 == 0 || z % 5 == 0) return true;

        return false;
    }

    float[,] GenerateHeightMap(int width, int height)
    {
        // Create a new height map
        float[,] heightMap = new float[width, height];

        // Use a noise function to generate the height map
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                heightMap[x, z] = Mathf.PerlinNoise(x * 0.1f, z * 0.1f);
            }
        }

        return heightMap;
    }
}