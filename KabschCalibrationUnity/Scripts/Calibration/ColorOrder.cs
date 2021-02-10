using UnityEngine;

public static class ColorOrder
{
    public static Color GetColor(int number)
    {
        switch (number % 7)
        {
            case 0:
            {
                return Color.blue;
            }
            case 1:
            {
                return Color.red;
            }
            case 2:
            {
                return Color.yellow;
            }
            case 3:
            {
                return Color.green;
            }
            case 4:
            {
                return Color.magenta;
            }
            case 5:
            {
                return Color.cyan;
            }
            case 6:
            {
                return Color.white;
            }
            default:
            {
                return Color.black;
            }
        }
    }
}
