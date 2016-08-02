using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic; // HashSets

public class GridGenerator : MonoBehaviour {

    // HashSet of all the grid cell Planes created by GridGenerator
    private HashSet<GameObject> cells = new HashSet<GameObject>();

    // Grid cell x radius (half-width)
    private float cellX = 30f;
    // Grid cell z radius (half-height)
    private float cellZ = 30f;

    // Road radius (half-width)
    // (Roads are actually just gaps between grid cells)
    private float roadR = 6f;

    // Grid x radius (half-width)
    private int gridX = 4;
    // Grid z radius (half-height)
    private int gridZ = 4;

    // Grid cells are offset this much off the ground, so they render properly
    private float offsetY = 0.05f;

    // X distance between consecutive grid cells
    private float spacingX;
    // Z distance beteween consecutive grid cells
    private float spacingZ;

    // Random class
    private System.Random rng = new System.Random();

    // The Material to use for each grid cell
    Material cellMaterial;

    // The material to use for buildings
    Material buildMaterial;

	// Use this for initialization
	void Start () {

        // Compute the spacing between grid cells
        spacingX = (cellX + roadR);
        spacingZ = (cellZ + roadR);

        // Load the grid cell material
        cellMaterial = Resources.Load("Cell") as Material;
        if (cellMaterial == null) {
            Debug.Log("Error getting Cell material");
            return;
        }

        // Load the building material
        buildMaterial = Resources.Load("Building") as Material;

        // Create a grid of cells
        for (int x = -gridX; x <= gridX; ++x) {

            // X coordinate for cell (x, z)
            float x0 = x * spacingX;

            for (int z = -gridZ; z <= gridZ; ++z) {

                // Z coordinate for cell (x, z)
                float z0 = z * spacingZ;
                
                // Create the cell center position vector
                var cellCenter = new Vector3(x0, offsetY, z0);

                // Create a grid cell
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // Add the cell to the cell HashSet
                cells.Add(cell);

                // Set the cell's position
                cell.transform.position = cellCenter;

                // Set the cell's size
                cell.transform.localScale = new Vector3(cellX, offsetY, cellZ);

                // Set the cell's Material
                cell.GetComponent<MeshRenderer>().material = cellMaterial;

                // Add buildings to the cell
                GenerateBuildings(cell);
            }
        }

        Debug.Log("Finished Generation");
	}
	


	// Update is called once per frame
	void Update () {
	
	}



    // Generate some simple buildings on a grid cell
    void GenerateBuildings(GameObject cell, float buildSize=5f, float deltaSize=0.8f) {

        // Calculate the number of buildings in the x and z directions
        int xMax = (int)Mathf.Floor( (cellX + 0.5f * buildSize) / (buildSize + deltaSize) );
        int zMax = (int)Mathf.Floor( (cellZ + 0.5f * buildSize) / (buildSize + deltaSize) );
        
        // Calculate the locations of the centers for each building
        for (int x=1; x<=xMax; ++x) {
            // building X location
            float xCenter = x * deltaSize + (2 * x - 1) * 0.5f * buildSize;

            // To figure out X scaling, compute the left and right x bounds and 
            // (possibly) recenter
            float xLeft = Mathf.Max(xCenter - 0.5f * buildSize, deltaSize);
            float xRight = Mathf.Min(xCenter + 0.5f * buildSize, cellX - deltaSize);
            xCenter = 0.5f * (xLeft + xRight);

            // Compute the building X scaling
            float scaleX = xRight - xCenter;

            for (int z=1; z<=zMax; ++z) {

                // building Z location
                float zCenter = z * deltaSize + (2 * z - 1) * 0.5f * buildSize;

                // Compute Z scaling and recentering
                float zDown = Mathf.Max(zCenter - 0.5f * buildSize, deltaSize);
                float zUp = Mathf.Min(zCenter + 0.5f * buildSize, cellZ - deltaSize);
                zCenter = 0.5f * (xLeft + xRight);

                // Compute the building Z scaling
                float scaleZ = zUp - zCenter;

                // Compute the building height (Y scaling)
                float height = 10f + 9f * (float)rng.NextDouble();

                // Create a building
                GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // Make the building a child of the cell
                building.transform.parent = cell.transform;

                // Translate the building
                building.transform.position = new Vector3(xCenter, 0.5f * height, zCenter);

                // Scale the building
                building.transform.localScale = new Vector3(scaleX, 0.5f * height, scaleZ);

                // Add a material to the building
                building.GetComponent<MeshRenderer>().material = buildMaterial;

            }
        }

    }

}
