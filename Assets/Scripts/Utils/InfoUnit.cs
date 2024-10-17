using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InfoUnit<T>{
    public T value;
    public bool isAvailable;
    public bool isViewed;
}
