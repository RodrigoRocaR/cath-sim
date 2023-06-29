using System.Reflection;
using UnityEngine;

namespace Tests.EditMode
{
    public static class MethodGetter
    {
        public static MethodInfo GetPrivateMethod(object instance, string methodName)
        {
            var m = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (m == null)
            {
                Debug.LogError("Can not find private method \"" + methodName + "\"");
            }
            
            return m;
        }
        
    }
}