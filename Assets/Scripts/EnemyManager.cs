using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public List<IEnemy> AllEnemies;

    public EnemyManager()
    {
        AllEnemies = new List<IEnemy>();
    }
    // Update is called once per frame
    public void LogicUpdate()
    {
        foreach(IEnemy enemy in AllEnemies)
        {
            enemy.LogicUpdate();
        }
    }

    public void SpawnEnemy(Vector3 _location, IEnemy _enemyType)
    {
        IEnemy newEnemy = _enemyType;
        _enemyType.Initialize(_location);

        AllEnemies.Add(newEnemy);
    }
}
