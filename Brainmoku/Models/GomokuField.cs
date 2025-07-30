/*
 * File:        GomokuField.cs
 * Author:      Felix Frybort
 * Project:     Brainmoku
 * Created:     2024-12-15
 * Description: This class holds gomoku logic.
 */

namespace Brainmoku.Models;
public class GomokuField
{
    private const int ArraySideLength = 16;
    
    private static int[,] _gomokuArray = new int[ArraySideLength, ArraySideLength];

    private struct Coordinate
    {
        public int X;
        public int Y;
    }
    public enum Value
    {
        Empty,
        PlayerX,
        PlayerO,
        Black
    }

    public void Clear()
    {
        for (int i = 0; i < ArraySideLength; i++)
        {
            for (int j = 0; j < ArraySideLength; j++)
            {
                _gomokuArray[i, j] = (byte)Value.Empty;
            }
        }
    }
    private int ToIndex(Coordinate c)
    {
        return c.Y*ArraySideLength + c.X;
    }
    private int ToIndex(int x, int y)
    {
        return y*ArraySideLength + x;
    }
    private Coordinate ToCoordinate(int index)
    {
        return new Coordinate { X = index % ArraySideLength, Y = index / ArraySideLength };
    }
    public int GetValue(int index)
    {
        return GetValue(ToCoordinate(index));
    }

    public int[] GetField()
    {
        int[] result = new int[ArraySideLength*ArraySideLength];
        for (int i = 0; i < ArraySideLength; i++)
        {
            for (int j = 0; j < ArraySideLength; j++)
            {
                result[i*ArraySideLength + j] = _gomokuArray[j, i];
            }
        }
        return result;
    }
    // returns list of indexes (if the player won) or an empty list
    public List<int> SetAndCheck(int index, Value value) 
    {
        Coordinate c = ToCoordinate(index);
        SetValue(c, (byte)value);
        return CheckWin(c);
    }
    
    public void SetBlack(int index)
    {
        SetValue(ToCoordinate(index), (byte)Value.Black);
    }
    private void SetValue(Coordinate c, byte value)
    {
        _gomokuArray[c.X, c.Y] = value;
    }
    private int GetValue(Coordinate c)
    {
        return _gomokuArray[c.X, c.Y];
    }
    private int GetValue(int x, int y)
    {
        return _gomokuArray[x, y];
    }
    public bool IsEmpty(int index)
    {
        return GetValue(ToCoordinate(index)) == (byte)Value.Empty;
    }
    public bool IsBlack(int index)
    {
        return GetValue(ToCoordinate(index)) == (byte)Value.Black;
    }
    private static Coordinate ToCoordinate(int x, int y)
    {
        Coordinate c;
        c.X = x;
        c.Y = y;
        return c;
    }
    private List<int>? CheckWin(Coordinate c)
    {
        List<int> list = new List<int>();
        int tempX;
        int tempY;
        int value = GetValue(c);
        list.Add(ToIndex(c));

        // horizontal
        for (tempX = c.X + 1; IsIndexInRange(tempX) && GetValue(tempX, c.Y) == value; tempX++)
        {
            list.Add(ToIndex(tempX, c.Y));
        }

        for (tempX = c.X - 1; IsIndexInRange(tempX) && GetValue(tempX, c.Y) == value; tempX--)
        {
            list.Add(ToIndex(tempX, c.Y));
        }
        if (list.Count() >= 5) { return list;}
        list.RemoveRange(1, list.Count - 1);

        //vertical
        for (tempY = c.Y + 1; IsIndexInRange(tempY) && GetValue(c.X, tempY) == value; tempY++)
        {
            list.Add(ToIndex(c.X, tempY));
        }
        for (tempY = c.Y - 1; IsIndexInRange(tempY) && GetValue(c.X, tempY) == value; tempY--)
        {

            list.Add(ToIndex(c.X, tempY));
        }
        if (list.Count() >= 5) { return list; }
        list.RemoveRange(1, list.Count - 1);

        // diagonal \
        for (tempX = c.X + 1, tempY = c.Y + 1;
             IsIndexInRange(tempX) && IsIndexInRange(tempY) && GetValue(tempX, tempY) == value;
             tempX++, tempY++)
        {
            list.Add(ToIndex(tempX, tempY));
        }
        for (tempX = c.X - 1, tempY = c.Y - 1;
             IsIndexInRange(tempX) && IsIndexInRange(tempY) && GetValue(tempX, tempY) == value;
             tempX--, tempY--)
        {
            list.Add(ToIndex(tempX, tempY));
        }
        if (list.Count() >= 5) { return list; }
        list.RemoveRange(1, list.Count - 1);

        // diagonal / 
        for (tempX = c.X + 1, tempY = c.Y - 1;
             IsIndexInRange(tempX) && IsIndexInRange(tempY) && GetValue(tempX, tempY) == value;
              tempX++, tempY++)
        {
            list.Add(ToIndex(tempX, tempY));
        }
        for (tempX = c.X - 1, tempY = c.Y + 1;
             IsIndexInRange(tempX) && IsIndexInRange(tempY) && GetValue(tempX, tempY) == value;
             tempX--, tempY++)
        {
            list.Add(ToIndex(tempX, tempY));
        }
        if (list.Count() >= 5) { return list; }

        return null;
    }
    private bool IsIndexInRange(int i)
    {
        return i >= 0 && i < ArraySideLength;
    }
    public void PrintArray() // for debugging
    {
        Console.Write("   ");
        for (int i = 0; IsIndexInRange(i); i++)
        {
            if (i < 10) { Console.Write(" " + i + " "); }
            else { Console.Write(" " + i); }
        }
        Console.WriteLine("");
        for (int y = 0; IsIndexInRange(y); y++)
        {
            if (y < 10) { Console.Write(" " + y + " "); }
            else { Console.Write(" " + y); }
            for (int x = 0; IsIndexInRange(x); x++)
            {
                switch (GetValue(x, y))
                {
                    case (byte)Value.PlayerX:
                        Console.Write(" x ");
                        break;
                    case (byte)Value.PlayerO:
                        Console.Write(" o ");
                        break;
                    case (byte)Value.Black:
                        Console.Write(" # ");
                        break;
                    default:
                        Console.Write(" . ");
                        break;
                }
            }
            Console.WriteLine("");
        } 
    }
}

// end of GomokuField.cs