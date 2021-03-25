using SimpleCommands.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMethodTest : MonoBehaviour
{
    [SCCommand("InstanceTest1")]
    public void EmptyMethod()
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand("InstanceTest2")]
    public int ReturnMethod()
    {
        Debug.LogWarning("COMMAND WORKS");
        return 10;
    }

    [SCCommand("InstanceTest3")]
    public void ParamMethod(int a)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand("InstanceTest4")]
    public void OverloadedMethod(int a)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand("InstanceTest5")]
    public void OverloadedMethod(int a, int b)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    public void OutParamMethod(int a, out string b)
    {
        Debug.LogWarning("COMMAND WORKS");
        b = "Output String";
    }

    public void OptionalParamMethod(int a, int b = 0, string c = "")
    {
        Debug.LogWarning("COMMAND WORKS");
    }
}
