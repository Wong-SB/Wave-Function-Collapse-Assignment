using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class Pattern
    {
        //public int[][] patternGrid;
        //public int index;
        //public int HashIndex;

        //public Pattern(int[][] patternGrid, int index, int hashIndex)
        //{
        //    this.patternGrid = patternGrid;
        //    this.index = index;
        //    HashIndex = hashIndex;
        //}

        //public bool CheckValueAtPosition(int x, int y, int value)
        //{
        //    return value.Equals(patternGrid[x][y]);
        //}

        //internal bool ComparePatternToAnotherPattern(Direction dir, Pattern pattern)
        //{
        //    int[][] myGrid = GetGridValuesInDirection(dir);
        //    int[][] otherGrid = pattern.GetGridValuesInDirection(dir.GetOppositeDirectionTo());

        //    for (int row = 0; row < myGrid.Length; row++)
        //    {
        //        for (int col = 0; col < myGrid[0].Length; col++)
        //        {
        //            if (myGrid[row][col] != otherGrid[row][col])
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        //private int[][] GetGridValuesInDirection(Direction dir)
        //{
        //    int[][] gridPartToCompare;
        //    switch (dir)
        //    {


        //        case Direction.Up:
        //            gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length - 1, patternGrid.Length);
        //            CreatePartOfGrid(0, patternGrid.Length, 1, patternGrid.Length, gridPartToCompare);
        //            break;
        //        case Direction.Down:
        //            gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length - 1, patternGrid.Length);
        //            CreatePartOfGrid(0, patternGrid.Length, 0, patternGrid.Length - 1, gridPartToCompare);
        //            break;
        //        case Direction.Left:
        //            gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length, patternGrid.Length - 1);
        //            CreatePartOfGrid(0, patternGrid.Length - 1, 0, patternGrid.Length, gridPartToCompare);
        //            break;
        //        case Direction.Right:
        //            gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length, patternGrid.Length - 1);
        //            CreatePartOfGrid(1, patternGrid.Length, 0, patternGrid.Length, gridPartToCompare);
        //            break;
        //        default:
        //            return patternGrid;
        //    }

        //    return gridPartToCompare;
        //}

        //private void CreatePartOfGrid(int xmin, int xmax, int ymin, int ymax, int[][] gridPartToCompare)
        //{
        //    List<int> tempList = new List<int>();
        //    for (int row = ymin; row < ymax; row++)
        //    {
        //        for (int col = xmin; col < xmax; col++)
        //        {
        //            tempList.Add([row][col]);
        //        }
        //    }

        //    for (int i = 0; i < tempList.Count; i++)
        //    {
        //        int x = i % gridPartToCompare.Length;
        //        int y = i / gridPartToCompare.Length;
        //        gridPartToCompare[x][y] = tempList[i];
        //    }
        //}

    }

}