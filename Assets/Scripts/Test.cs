using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;


//the actual script to put in a gameobject
public class Test : MonoBehaviour
{
    TileMapReader reader;
    [SerializeField] private Vector3 location;
    [SerializeField] private Vector2Int size;
    [SerializeField] private Tilemap targetTileMap;
    [SerializeField] private float tileSize;
    [SerializeField] private int patternSize = 1;
    
    private PatternManager patternManager;

    private Grid_Pro<int> DebugPatternGrid;

    // Start is called before the first frame update
    void Start()
    {
        SampleTileData();


        
    }

   
    //this is module one from what we have proposed
    public void SampleTileData() {

        reader = new TileMapReader(location, size, tileSize, targetTileMap, transform, true);
        patternManager = new PatternManager(patternSize, true);
        patternManager.RecognizePattern(reader, true);
        patternManager.ReadPattern();
        

        //debug neighbours at index 0
        for (int i = 0; i < 4; i++)
        {
            HashSet<int> set = patternManager.GetPossibleNeighboursForPatternInDirection(0, (Direction)i);
            int[] array = new int[set.Count];
            set.CopyTo(array);
            Debug.Log($"direction: {i} : {string.Join(" ", array)}");
        }
    

            
        //debug the pattern grid on the screen, not using right now but useful when we want to show debug ui
        //this is very finiky and does not represent the actual grid
        //if you don't want to see the text comment on this code
        PatternDataResults patternGrid = patternManager.patternGrid;
        int patternOffset;
        if (patternSize >= 2)
        {
            patternOffset = patternSize - 1;
        }
        else {
            patternOffset = 1;
        }
        DebugPatternGrid = new Grid_Pro<int>(patternGrid.GetPatternGridSize(), patternGrid.GetPatternGridSize(), tileSize, new Vector3(location.x - (patternOffset* tileSize), location.y - (patternOffset * tileSize), location.z), transform);

        for (int x = 0; x < patternGrid.GetPatternGridSize(); x++)
        {
            for (int y = 0; y < patternGrid.GetPatternGridSize(); y++)
            {
                DebugPatternGrid.SetGridObject(x, y, patternGrid.GetIndexAt(x, y));
            }
        }
    }

}
