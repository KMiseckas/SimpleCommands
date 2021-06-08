using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParserMethods
{
    public static Vector2 ParseVector2(string s)
    {
        string[] values = s.Split(',');

        return new Vector2(float.Parse(values[0]), float.Parse(values[1]));
    }

    public static Vector3 ParseVector3(string s)
    {
        string[] values = s.Split(',');

        return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
    }

    public static Vector4 ParseVector4(string s)
    {
        string[] values = s.Split(',');

        return new Vector4(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
    }

    public static Rect ParseRect(string s)
    {
        string[] values = s.Split(',');

        return new Rect(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
    }

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

    public static GameObject ParseGOByName(string s)
    {
        return GameObject.Find(s);
    }
}
