using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System.Text;
using System;

namespace WaveFunctionCollapse
{
    //manage the grid of pattern and it's neighbours
    //contails a dictionary of available patterns from the given tilemap and a dictionary of each patterns neighbours
    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary; 
        Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
        IFindNeighbourStrategy strategy;
        int patternSize = -1;
        private bool debugGrid = false;

        public PatternDataResults patternGrid;

        public PatternManager(int patternSize, bool debugGird = false)
        {
            this.patternSize = patternSize;
            this.debugGrid = debugGird;
        }

        //do RecognizePattern() and ReadPattern() at the same time
        //RecognizePattern() and ReadPattern() combined are basically CreatePatterns<T>
        public void ProcessGrid(TileMapReader reader, bool equalWeights)
        {
            strategy = GetSuiatbleStrategy();
            RecognizePattern(reader, equalWeights);
            ReadPattern();

        }

        public IFindNeighbourStrategy GetSuiatbleStrategy() {
            if (patternSize <= 1) return new NeighboursStrategySize1Default();
            else return new NeighboursStrategySize2andMore();

        }

        //planned in pseudo code
        //to recognize the patterns of tiles based on the pattern size
        public void RecognizePattern(TileMapReader reader, bool equalWeights) {
            strategy = GetSuiatbleStrategy();
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(reader, patternSize, equalWeights);
            patternGrid = patternFinderResult;

            //debug the pattern values in each grid
            if (debugGrid)
            {
                Debug.Log("pattern generated");
                StringBuilder builder = null;
                List<string> list = new List<string>();
                for (int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
                {
                    builder = new StringBuilder();
                    for (int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                    {
                        builder.Append(patternFinderResult.GetIndexAt(col, row) + " ");
                    }
                    list.Add(builder.ToString());
                }
                list.Reverse();
                foreach (var item in list)
                {
                    Debug.Log(item);
                }
            }
            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;

        }

        //planned in pseudo code
        //to recognize the patterns of tiles based on the pattern size
        //should be renamed to ReadPatternNeighbouts but too late for that
        public void ReadPattern() {
            GetPatternNeighbours(patternGrid, strategy);
        }

        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy strategy)
        {
            patternPossibleNeighboursDictionary = PatternFinder.FindPossibleNeighbursForAllPatterns(strategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighboursForPatternInDirection(int patternIndex, Direction dir)
        {
            return patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).frequencyRelative;
        }

        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).frequencyRelativeLog2;
        }

        public int GetNuberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }

      
    }

}