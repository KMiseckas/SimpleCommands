using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMethodTest
{
    public void EmptyMethod()
    {

    }

    public int ReturnMethod()
    {
        return 10;
    }

    public void ParamMethod(int a)
    {

    }

    public void OverloadedMethod(int a)
    {

    }

    public void OverloadedMethod(int a, int b)
    {

    }

    public void OutParamMethod(int a, out string b)
    {
        b = "Output String";
    }

    public void OptionalParamMethod(int a, int b = 0, string c = "")
    {

    }
}
