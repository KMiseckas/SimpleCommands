// MIT License 
//
// Copyright (c) 2021 Klaudijus Miseckas 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.

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

    [SCCommand("test223")]
    public static void OverloadedMethod(int a)
    {

    }

    [SCCommand("test113")]
    public static void OverloadedMethod(int a, int b)
    {

    }

    [SCCommand("test13")]
    public static void OutParamMethod(int a,string b)
    {
        b = "Output String";
    }

    [SCCommand("test12")]
    public static void OptionalParamMethod(int a, int b)
    {

    }

    [SCCommand("test1")]
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
