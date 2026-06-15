using UnityEngine;

public class CameraDeps : IDependencyGroups
{
    public IFieldProvider FieldProvider { get; }
    public Camera Camera { get; }

    public CameraDeps(IFieldProvider fieldProvider, Camera camera)
    {
        FieldProvider = fieldProvider;
        Camera = camera;
    }
}
