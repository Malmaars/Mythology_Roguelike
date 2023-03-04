using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    public Vector3 position { get; set; }
    public GameObject GameReference { get; set; }
}
