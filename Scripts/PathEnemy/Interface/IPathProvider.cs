using UnityEngine;

public interface IPathProvider
{
    IPath GetPathForSpawn(GameObject spawnPoint);
}