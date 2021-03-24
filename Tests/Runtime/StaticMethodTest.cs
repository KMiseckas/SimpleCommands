using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMethodTest : MonoBehaviour
{
    public static void EmptyMethod()
    {

    }

    public static int ReturnMethod()
    {
        return 10;
    }

    public static void ParamMethod(int a)
    {

    }

    public static void OverloadedMethod(int a)
    {

    }

    public static void OverloadedMethod(int a, int b)
    {

    }

    public static void OutParamMethod(int a, out string b)
    {
        b = "Output String";
    }

    public static void OptionalParamMethod(int a, int b = 0, string c = "")
    {

    }
}
