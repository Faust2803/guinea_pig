using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberFormatter
{
    public static string FormatValue(int num)
    {
        if (num >= 100000)
            return FormatValue(num / 1000) + "K";

        //if (num >= 1000)
        //    return (num / 1000D).ToString("0.#") + "K";

        return num.ToString("#,0");
    }
}
