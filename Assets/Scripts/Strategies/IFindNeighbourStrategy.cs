using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    //interface ot any strategy class
    //basically telling the alglirithm how to read a patterns neighbour
    //can use this to expand our own
    public interface IFindNeighbourStrategy
    {

        Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult);
    }

}

