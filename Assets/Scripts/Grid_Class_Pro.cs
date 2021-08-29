using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

//things to do (for the authur only):
//check if this class is very reusable
//implement a proper way to destroy grids
//add more debug features and controls
//test this in 3D (using x and z axis)
//remove all mentions of  real names in this class and replace them with screen names

//this is Wong Sui Bin's grid class, I advise not to touch this if you are not the authur of this class
//this is a class that greats a 2D grid in the game world that maps with real coordinates
//requires text mesh pro to work

//so far used for
//level editors
//tile based building system
//grid reresentation of positions of game objects

public class Grid_Pro<TGridObject>
{
    public event EventHandler<OnGridObjectChangedArgs> OnGridObjectChanged;

    public class OnGridObjectChangedArgs: EventArgs {
        public int x, y;
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMeshPro[,] gridText;

    private Color textColour;
    private bool showText;
    private int textSize;
    private bool spawnText;
    private bool debugLine;

    //arrangement of y is up and x is right
    //grid constructor for custom data types
    public Grid_Pro(int width, int height, float cellSize, Vector3 originPosition,Transform parent, Func<Grid_Pro<TGridObject>, int , int, TGridObject> createGridObject, bool spawnText = true, bool debugLine = true)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridArray = new TGridObject[width, height];
        this.gridText = new TextMeshPro[width, height];
        this.originPosition = originPosition;
        this.textColour = Color.white;
        this.showText = true;
        this.textSize = 5;
        this.spawnText = spawnText;
        this.debugLine = debugLine;

        //construct grid object
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }

        //set debug visuals
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (debugLine) {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }
               

                if (spawnText)
                gridText[x, y] = CreateGridText(gridArray[x, y]?.ToString(), parent,x,y, GetWorldPositionCenter(x, y), textSize, textColour, TMPro.TextAlignmentOptions.CenterGeoAligned);

                //disable text if configured to not show
                gridText[x, y]?.gameObject.SetActive(showText);
            }


        }
        if (debugLine)
        {
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }

        //assign object changed even for debug visuals
        OnGridObjectChanged += (object sender, OnGridObjectChangedArgs eventArgs) =>
        {
            gridText[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        };
    }

    //grid constructor for non custom data type
    public Grid_Pro(int width, int height, float cellSize, Vector3 originPosition, Transform parent, bool spawnText = true, bool debugLine = true)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridArray = new TGridObject[width, height];
        this.gridText = new TextMeshPro[width, height];
        this.originPosition = originPosition;
        this.textColour = Color.white;
        this.showText = true;
        this.textSize = 5;
        this.spawnText = spawnText;
        this.debugLine = debugLine;

        //set debug visuals
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (debugLine)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

                }

                if (spawnText)
                    gridText[x, y] = CreateGridText(gridArray[x, y]?.ToString(), parent,x,y, GetWorldPositionCenter(x, y), textSize, textColour, TMPro.TextAlignmentOptions.CenterGeoAligned);

                //disable text if configured to not show
                gridText[x, y]?.gameObject.SetActive(showText);
            }


        }
        if (debugLine)
        {
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
        //assign object changed even for debug visuals
        OnGridObjectChanged += (object sender, OnGridObjectChangedArgs eventArgs) =>
        {
            gridText[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
        };
    }

    //the hash index for the grid, start from 0 in the bottom left cell and counting right and upwards
    //hash the ridArray position into hash index
    public int positionHash(int x, int y) {
        return y * width + x;
    }

    //hash the index back to gridArray position
    public void positionHash(int index, out int x,out int y)
    {
        y = index / width;
        x = index % width;
    }

    //trigger the update debug text event using gridArray Position
    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null && spawnText) OnGridObjectChanged(this, new OnGridObjectChangedArgs { x = x, y = y });
    }

    //trigger the update debug text event using world Position
    public void TriggerGridObjectChanged(Vector3 position)
    {
        int x, y;
        GetXY(position, out x, out y);
        if (OnGridObjectChanged != null && spawnText) OnGridObjectChanged(this, new OnGridObjectChangedArgs { x = x, y = y });
    }

    //configure debug text option
    //so far no use for this
    public void configureDebugText(bool show, Color textColour, int size)
    {
       
        this.showText = show;
        this.textColour = textColour;
        this.textSize = size;

        if (!spawnText) return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                //disable text if configured to not show
                gridText[x, y]?.gameObject.SetActive(showText);
                if (gridText[x, y] != null) {
                    gridText[x, y].color = this.textColour;
                    gridText[x, y].fontSize = this.textSize;
                }
                

            }
        }
    }

    //get if the text is shown
    public bool getShowText()
    {
        return showText;
    }

    //get world posisiton of a corner of a cell
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //get world positon of the center of a cell
    public Vector3 GetWorldPositionCenter(int x, int y)
    {
        return GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
    }

    //get x and y of grid from world posistion
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //set value from x and y of the array
    public void SetGridObject(int x, int y, TGridObject gridObject)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = gridObject;
            TriggerGridObjectChanged(x,y);
        }
    }

    //set value from world position
    public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, gridObject);
    }

    //get value from x and y of the array
    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    //get value from world position
    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);

    }

    //convert a vector 3 position to the center of a grid
    public Vector3 GetSnapGridCenter(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetWorldPositionCenter(x, y);

    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public TGridObject[,] OutputArray() {

        return gridArray;
    }

    public void DestroyGrid() {
        if (!spawnText) return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                
                if (gridText[x, y] != null)
                {
                    MonoBehaviour.Destroy(gridText[x, y].gameObject);
                    
                }


            }
        }
    }


    // Create Text in the Grid
    private TextMeshPro CreateGridText(string text, Transform parent = null, int x= 0, int y = 0, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Left, int sortingOrder = 5000)
    {
        if (color == null) color = Color.white;
        return CreateGridText(parent, text, $"Grid_Text_[{x}, {y}]", localPosition, fontSize, (Color)color, textAlignment, sortingOrder);
    }

    // Create Text in the Grid
    private TextMeshPro CreateGridText(Transform parent, string text, string objectName, Vector3 localPosition, int fontSize, Color color, TextAlignmentOptions textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject(objectName, typeof(TextMeshPro));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.alignment = textAlignment;
        //textMesh.anchor = AnchorPositions.Center;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

}
