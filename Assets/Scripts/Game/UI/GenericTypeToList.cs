using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;
public class GenericTypeToList
{
    public static List<string> Set<T>() {
        return Enum.GetNames(typeof(T)).ToList(); 
    }
}
