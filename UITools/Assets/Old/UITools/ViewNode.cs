using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class ViewNode : IComparable
{
    public int Index;
    public JToken Data;

    public ViewNode(int index, JToken data)
    {
        this.Index = index;
        this.Data = data;
    }

    public int CompareTo(object obj)
    {
        var target = obj as ViewNode;
        if (target == null) return -1;
        return Index.CompareTo(target.Index);
    }
}