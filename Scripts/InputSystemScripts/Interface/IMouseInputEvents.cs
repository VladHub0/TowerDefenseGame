using System;

public interface IMouseInputEvents
{
    event EventHandler<BuildUITowerEventArgs> OnBuildUIRequested;
    event EventHandler<TowerUpgrateUIArgs> OnTowerUIUpgrateRequested;
    event EventHandler OnClickEmptyPlace;
}