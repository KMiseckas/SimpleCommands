using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parser methods for several different types of instances.
/// </summary>
public static class ParserMethods
{
    /// <summary>
    /// Parse string to return a Vector2.
    /// </summary>
    public static Vector2 ParseVector2(string s)
    {
        string[] values = s.Split(',');

        return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
    }

    /// <summary>
    /// Parse string to return a Vector3.
    /// </summary>
    public static Vector3 ParseVector3(string s)
    {
        string[] values = s.Split(',');

        return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
    }

    /// <summary>
    /// Parse string to return a Vector4.
    /// </summary>
    public static Vector4 ParseVector4(string s)
    {
        string[] values = s.Split(',');

        return new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
    }

    /// <summary>
    /// Parse string to return a Rect.
    /// </summary>
    public static Rect ParseRect(string s)
    {
        string[] values = s.Split(',');

        return new Rect(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
    }

    /// <summary>
    /// Parse string where the string represents the runtime ID of the gameobject to find and return.
    /// </summary>
    public static GameObject ParseGORuntimeID(string s)
    {
        int intID = int.Parse(s);
        object targetObject = null;

        GameObject[] gameObjectsByID = GameObject.FindObjectsOfType<GameObject>();

        for (int i = 0; i < gameObjectsByID.Length; i++)
        {
            if (gameObjectsByID[i].GetInstanceID() == intID)
            {
                return gameObjectsByID[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Parse string where the string represents the name of a GameObject to find and return.
    /// </summary>
    public static GameObject ParseGOByName(string s)
    {
        return GameObject.Find(s);
    }
}
