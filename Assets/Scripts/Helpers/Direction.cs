using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//direction helper class
//an enum for 4 fdirections
//also contains a static function that can get the opposite direction given
public enum Direction { Up,Down,Left,Right}

public static class DirectionHelper {

    public static Direction GetOppositeDirection(this Direction direction) {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
               
            case Direction.Down:
                return Direction.Up;
                
            case Direction.Left:
                return Direction.Right;
               
            case Direction.Right:
                return Direction.Left;
               
            default:
                return direction;
                
        }

    }
}
