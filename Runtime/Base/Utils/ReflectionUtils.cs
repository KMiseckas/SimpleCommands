using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;

namespace SimpleCommands.Runtime.Base
{
    internal static class ReflectionUtils
    {
        public static AttributeMethodInfo<TAttribute>[] FindAttributeMethodInfo<TAttribute>(BindingFlags flags) where TAttribute : Attribute
        {
            //Get all the assemblies to scan.
            Assembly[] targetAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<AttributeMethodInfo<TAttribute>> attributeMethodInfos = new List<AttributeMethodInfo<TAttribute>>();

            CompareInfo comparer = new CultureInfo("en-US").CompareInfo;

            List<string> assembliesToIgnore = new List<string>();
            PopulateAssembliesToIgnoreByPrefix(assembliesToIgnore);

            //For every assembly that is not ignored, get every possible class type.
            for (int i = 0; i < targetAssemblies.Length; i++)
            {
                Assembly assembly = targetAssemblies[i];

                string assemblyName = assembly.GetName().Name;
                bool isIgnoredAssembly = false;

                foreach (string name in assembliesToIgnore)
                {
                    if (comparer.IsPrefix(assemblyName, name))
                    {
                        isIgnoredAssembly = true;
                        break;
                    }
                }

                if (isIgnoredAssembly) continue;

                Type[] types = assembly.GetTypes();

                //For every class type found, get every method.
                for (int j = 0; j < types.Length; j++)
                {
                    try
                    {
                        MethodInfo[] methods = types[j].GetMethods(flags);

                        //For every method found, check if the method has a attribute defined for it.
                        for (int k = 0; k < methods.Length; k++)
                        {
                            IEnumerable<TAttribute> attributes = methods[k].GetCustomAttributes<TAttribute>();

                            foreach (TAttribute attr in attributes)
                            {
                                if (attr != null)
                                {
                                    attributeMethodInfos.Add(new AttributeMethodInfo<TAttribute>(methods[k], attr));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e);
                    }
                }
            }

            return attributeMethodInfos.ToArray();
        }

        public static void PopulateAssembliesToIgnoreByPrefix(List<string> assembliesToIgnore)
        {

            assembliesToIgnore.Add("Unity");
            assembliesToIgnore.Add("System");
            assembliesToIgnore.Add("Mono.");
            assembliesToIgnore.Add("mscorlib");
            assembliesToIgnore.Add("netstandard");
            assembliesToIgnore.Add("TextMeshPro");
            assembliesToIgnore.Add("Microsoft.GeneratedCode");
            assembliesToIgnore.Add("I18N");
            assembliesToIgnore.Add("Boo.");
            assembliesToIgnore.Add("UnityScript.");
            assembliesToIgnore.Add("ICSharpCode.");
            assembliesToIgnore.Add("ExCSS.Unity");
            assembliesToIgnore.Add("Assembly-CSharp-Editor");
            assembliesToIgnore.Add("Assembly-UnityScript-Editor");
            assembliesToIgnore.Add("nunit.");
            assembliesToIgnore.Add("SyntaxTree.");
            assembliesToIgnore.Add("AssetStoreTools");
        }

        internal struct AttributeMethodInfo<TAttribute> where TAttribute : Attribute
        {
            internal readonly MethodInfo MethodInfo;

            internal readonly TAttribute Attribute;

            internal AttributeMethodInfo(MethodInfo methodInfo, TAttribute attribute)
            {
                MethodInfo = methodInfo;
                Attribute = attribute;
            }
        }
    }
}