using System;
using UnityEngine;

public class BuildUITowerEventArgs : EventArgs
{
    public GameObject PlaceObject { get; }

    public BuildUITowerEventArgs(GameObject placeObject)
    {
        PlaceObject = placeObject ?? throw new ArgumentNullException(nameof(placeObject));
    }
}
