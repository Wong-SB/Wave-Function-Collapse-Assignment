using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace WaveFunctionCollapse
{

    //data set of a pattern containing it's strucutre, frequency
    //this contains a struct called Pattern that was orinigally a class.

    public class PatternData
    {
        public Pattern pattern;
        private int frequency;
        public float frequencyRelative;
        public float frequencyRelativeLog2;

        public PatternData(int[][] patternGrid, string hashIndex, int index)
        {
            pattern = new Pattern(patternGrid, hashIndex, index);
            frequencyRelative = 0;
            frequencyRelative = 0;
        }

        public void AddToFrequency()
        {
            frequency++;
        }

        public void CalculateRelativeFrequency(int total)
        {
            frequencyRelative = (float)frequency / total;
            frequencyRelativeLog2 = Mathf.Log(frequencyRelative, 2);
        }

        public bool CompareGrid(Direction dir, PatternData data)
        {
            return pattern.ComparePatternToAnotherPattern(dir, data.pattern);
        }

        //pattern struct
        //contails the grid of tiles represent a pattern and it's hashed int
        public struct Pattern
        {
            public int[][] patternGrid;
            public int index;
            public string HashIndex;

            public Pattern(int[][] patternGrid, string hashIndex, int index)
            {
                this.patternGrid = patternGrid;
                this.index = index;
                HashIndex = hashIndex;
            }

            public bool CheckValueAtPosition(int x, int y, int value)
            {
                return value.Equals(patternGrid[x][y]);
            }

            internal bool ComparePatternToAnotherPattern(Direction dir, Pattern pattern)
            {
                int[][] myGrid = GetGridValuesInDirection(dir);
                int[][] otherGrid = pattern.GetGridValuesInDirection(DirectionHelper.GetOppositeDirection(dir));

                for (int row = 0; row < myGrid.Length; row++)
                {
                    for (int col = 0; col < myGrid[0].Length; col++)
                    {
                        if (myGrid[row][col] != otherGrid[row][col])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            private int[][] GetGridValuesInDirection(Direction dir)
            {
                int[][] gridPartToCompare;
                switch (dir)
                {


                    case Direction.Up:
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length - 1, patternGrid.Length);
                        CreatePartOfGrid(0, patternGrid.Length, 1, patternGrid.Length, gridPartToCompare);
                        break;
                    case Direction.Down:
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length - 1, patternGrid.Length);
                        CreatePartOfGrid(0, patternGrid.Length, 0, patternGrid.Length - 1, gridPartToCompare);
                        break;
                    case Direction.Left:
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length, patternGrid.Length - 1);
                        CreatePartOfGrid(0, patternGrid.Length - 1, 0, patternGrid.Length, gridPartToCompare);
                        break;
                    case Direction.Right:
                        gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGrid.Length, patternGrid.Length - 1);
                        CreatePartOfGrid(1, patternGrid.Length, 0, patternGrid.Length, gridPartToCompare);
                        break;
                    default:
                        return patternGrid;
                }

                return gridPartToCompare;
            }

            private void CreatePartOfGrid(int xmin, int xmax, int ymin, int ymax, int[][] gridPartToCompare)
            {
                List<int> tempList = new List<int>();
                for (int row = ymin; row < ymax; row++)
                {
                    for (int col = xmin; col < xmax; col++)
                    {
                        tempList.Add(patternGrid[row][col]);
                    }
                }

                for (int i = 0; i < tempList.Count; i++)
                {
                    int x = i % gridPartToCompare.Length;
                    int y = i / gridPartToCompare.Length;
                    gridPartToCompare[x][y] = tempList[i];
                }
            }
        }
    }
}