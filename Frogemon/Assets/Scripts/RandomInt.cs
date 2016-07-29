using System;

[Serializable]
public class RandomInt
{
    public int minValue; // minimum value selected
    public int maxValue; // maximum value selected

    // construct a random int generator
    public RandomInt(int a_min, int a_max)
    {
        // set the range
        minValue = a_min;
        maxValue = a_max;
    }

    // generate a random int
    public int Value
    {
        get { return UnityEngine.Random.Range(minValue, maxValue + 1); }
    }
}
