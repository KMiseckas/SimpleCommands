using SimpleCommands.Attributes;
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

    [SCCommand("test1", CommandTargetType = CommandTargetType.Static)]
    public static void Test1()
    {
        Debug.LogWarning("TEST WORKS");
    }

    [SCCommand("test2")]
    public static void Test2(int a)
    {
        Debug.LogWarning("TEST WORKS");
    }

    [SCCommand("test3")]
    public static void Test3(int a, int b)
    {
        Debug.LogWarning("TEST WORKS");
    }

    [SCCommand("test4")]
    public static void Test4(int a, int b, bool c)
    {
        Debug.LogWarning("TEST WORKS");
    }

    [SCCommand("test5")]
    public static void Test5(string a, int b)
    {
        Debug.LogWarning("TEST WORKS");
    }

    [SCCommand("test6")]
    public static void Test6(string a, int b, int d = 2)
    {
        Debug.LogWarning("TEST WORKS");
    }
}
