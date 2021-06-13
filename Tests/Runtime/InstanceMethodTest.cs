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

public class InstanceMethodTest : MonoBehaviour
{
    [SCCommand]
    public void EmptyMethod()
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand(commandDescription: "Return method")]
    public int ReturnMethod()
    {
        Debug.LogWarning("COMMAND WORKS");
        return 10;
    }

    [SCCommand(commandDescription: "Param method")]
    public void ParamMethod(int a)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand(commandDescription: "Overloaded method")]
    public void OverloadedMethod(int a)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    [SCCommand(commandKey:"OverloadedMethod_Two", commandDescription:"Overloaded method")]
    public void OverloadedMethod(int a, int b)
    {
        Debug.LogWarning("COMMAND WORKS");
    }

    //[SCCommand(commandDescription: "Out method")]
    public void OutParamMethod(int a, out string b)
    {
        Debug.LogWarning("COMMAND WORKS");
        b = "Output String";
    }

    [SCCommand(commandDescription: "Optional method")]
    public void OptionalParamMethod(int a, int b = 0, string c = "")
    {
        Debug.LogWarning("COMMAND WORKS");
    }
}
