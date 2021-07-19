using System;
using UnityEngine;

public class HashFunction{
    public static int CombineHashCodes(params int[] hashCodes)
    {
        if (hashCodes == null)
        {
            throw new ArgumentNullException("hashCodes");
        }

        if (hashCodes.Length == 0)
        {
            throw new IndexOutOfRangeException();
        }

        if (hashCodes.Length == 1)
        {
            return hashCodes[0];
        }

        var result = hashCodes[0];

        for (var i = 1; i < hashCodes.Length; i++)
        {
            result = CombineHashCodes(result, hashCodes[i]);
        }

        return result;
    }
    public static int CoodinateHash(Vector2 target){
        return CombineHashCodes(new int[]{(int)target.x,(int)target.y});
    }
}