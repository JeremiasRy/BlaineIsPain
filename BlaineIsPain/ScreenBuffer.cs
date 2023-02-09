using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaineIsPain;

static class ScreenBuffer
{
    static char[][] _screenArr = new char[Console.WindowHeight][];
    static readonly char _whitespace = ' ';

    public static void Initialize()
    {
        for (int y = 0; y < _screenArr.Length; y++)
        {
            _screenArr[y] = new char[Console.WindowWidth];
            for (int x = 0; x < _screenArr[y].Length; x++)
            {
                _screenArr[y][x] = _whitespace;
            }
        }
    }
    public static void Draw(char ch, int y, int x) => _screenArr[y][x] = ch;
    public static void DrawScreen()
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < _screenArr.Length; y++)
        {
            Console.WriteLine(string.Join("",_screenArr[y]));
        }
        Clear();
    }
    public static void Clear() 
    {
        for (int y = 0; y < _screenArr.Length; y++)
        {
            for (int x = 0; x < _screenArr[y].Length; x++)
            {
                _screenArr[y][x] = _whitespace;
            }
        }
    }
}
