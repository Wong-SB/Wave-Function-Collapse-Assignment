using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System;

namespace WaveFunctionCollapse
{

    //reads the tilemap and convert each tile to it's unique int
    //stores the grip of int that represents the tile map
    //has an offset functionality for finding a set of tiles according to it's offset 
    //this uses my grid function to grab tiles from the tilemap easier
    //we are using int instead of IValue
    public class TileMapReader
    {
        //private Grid_Pro<TileData> tileGrid;
        private Grid_Pro<int> tileHashGrid;
        private Vector3 location;
        private Vector2Int gridSize;
        private float tileSize;
        private Tilemap tilemapObject;
        private Dictionary<int, TileBase> HashDictionary = new Dictionary<int, TileBase>();
        private Transform transformOfGrid;

        //doe snot init unless told to
        public TileMapReader(Vector3 location, Vector2Int size, float tileSize, Tilemap testTilemap, Transform transformOfGrid, bool immediatlyInit = false)
        {

            this.location = location;
            this.gridSize = size;
            this.tileSize = tileSize;
            this.tilemapObject = testTilemap;
            this.transformOfGrid = transformOfGrid;
            transformOfGrid.position = Vector3.zero;
            CreateGrid();
            if (immediatlyInit) InitGrid();

        }

        public void CreateGrid()
        {
            tileHashGrid = new Grid_Pro<int>(gridSize.x, gridSize.y, tileSize, location, transformOfGrid, (Grid_Pro<int> g, int x, int y) => 0, false, false);
        }


        void InitGrid()
        {
            if (tileHashGrid == null) return;

            HashDictionary = new Dictionary<int, TileBase>();

            for (int y = 0; y < tileHashGrid.GetHeight(); y++)
            {
                for (int x = 0; x < tileHashGrid.GetWidth(); x++)
                {
                    tileHashGrid.SetGridObject(x, y, RecognizeTile(tilemapObject.GetTile(tilemapObject.WorldToCell(tileHashGrid.GetWorldPositionCenter(x, y)))));

                }
            }
        }

        public Vector2 GetGridSize()
        {
            return new Vector2(tileHashGrid.GetWidth(), tileHashGrid.GetHeight());
        }

        private int keyIndex = 0;
        int RecognizeTile(TileBase tile)
        {
            if (tileHashGrid == null || HashDictionary == null) return -1;

            int myKey;
            if (HashDictionary.ContainsValue(tile))
            {
                myKey = HashDictionary.FirstOrDefault(x => x.Value == tile).Key;
            }
            else
            {
                HashDictionary.Add(keyIndex, tile);
                myKey = keyIndex;
                keyIndex++;
            }
            return myKey;
        }

        //not using for now, might delete later
        void ReadPatternAll()
        {
            if (tileHashGrid == null || HashDictionary == null) return;

            for (int y = 0; y < tileHashGrid.GetHeight(); y++)
            {
                for (int x = 0; x < tileHashGrid.GetWidth(); x++)
                {
                    int[][] pattern = GetPatternValuesAt(x, y, 3);
                    string text = "";

                    for (int row = 0; row < pattern.Length; row++)
                    {
                        for (int col = 0; col < pattern[0].Length; col++)
                        {
                            text += "" + pattern[row][col];
                        }
                    }


                    Debug.Log(text);

                }
            }

        }

        public int[][] GetPatternValuesAt(int x, int y, int patternSize)
        {

            //create jaggeded array
            int[][] pattern = new int[patternSize][];
            for (int i = 0; i < patternSize; i++)
            {
                pattern[i] = new int[patternSize];
            }

            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    pattern[row][col] = GetGridValueIncludingOffset(x + col, y + row, tileHashGrid);

                }
            }
            return pattern;
        }

        int GetGridValueIncludingOffset(int x, int y, Grid_Pro<int> grid)
        {
            if (x < 0) x = grid.GetWidth() + x;
            else if (x >= grid.GetWidth()) x = x - grid.GetWidth();

            if (y < 0) y = grid.GetHeight() + y;
            else if (y >= grid.GetHeight()) y = y - grid.GetHeight();

            return grid.GetGridObject(x, y);

        }

        //not using now, might delete later
        //just ignore this, there is nothing fun heres
        //struct TileData
        //{
        //    public TileBase tile;
        //    public int key;
        //    public Vector2Int positionInGrid;

        //    public TileData(TileBase tile, Vector2Int positionInGrid) : this()
        //    {
        //        this.tile = tile;
        //        this.positionInGrid = positionInGrid;
        //    }

        //    public TileData(TileBase tile, int key, Vector2Int positionInGrid)
        //    {
        //        this.tile = tile;
        //        this.key = key;
        //        this.positionInGrid = positionInGrid;
        //    }

        //    public override bool Equals(object obj)
        //    {
        //        // If this and obj do not refer to the same type, then they are not equal.
        //        if (obj.GetType() != this.GetType()) return false;

        //        // Return true if  x and y fields match.
        //        var other = (TileData)obj;
        //        return this.key.Equals(other.key);
        //    }

        //    public override string ToString()
        //    {
        //        return key.ToString();
        //    }
        //}

        
    }
}