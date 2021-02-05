﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(in FlockAgent agent, in List<Transform> context);
}
