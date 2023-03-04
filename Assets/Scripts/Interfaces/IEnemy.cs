
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy : ILockable
{
    public int health { get; set; }

    public void Initialize(Vector3 _spawnLocation);

    public void LogicUpdate();
}
