
using UnityEngine;

public interface ITowerBuilder
{
    GameObject BuildTower(GameObject place, int towerIndex);
}

public interface IBuildAction
{
    void ExecuteBuild(int towerIndex);
}

public interface IUpgradeAction
{
    void ExecuteUpgrade();
}

public interface ICancelAction
{
    void ExecuteCancel();
}


public interface IDemolishAction
{
    void ExecuteDemolish();
}