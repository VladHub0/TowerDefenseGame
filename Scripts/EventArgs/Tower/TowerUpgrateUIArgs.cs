using System;
using UnityEngine;

public class TowerUpgrateUIArgs : EventArgs
{
    public GameObject TowerObject { get; }

    public TowerUpgrateUIArgs(GameObject towerObject)
    {
        TowerObject = towerObject ?? throw new ArgumentNullException(nameof(towerObject));
    }
}