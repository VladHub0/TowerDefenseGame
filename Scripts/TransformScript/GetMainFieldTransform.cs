using UnityEngine;
using System;

public class GetMainFieldTransform : MonoBehaviour, IFieldProvider
{
    [SerializeField] private Transform fieldTransform;
    [SerializeField] private string fieldTag = "MainField";

    private void Start()
    {
        if (fieldTransform == null)
        {
            GameObject go = GameObject.FindWithTag(fieldTag);
            if (go != null) fieldTransform = go.transform;
        }
    }

    public Transform GetTransform()
    {
        if (fieldTransform != null) return fieldTransform;
        throw new InvalidOperationException($"Main field not found. Assign fieldTransform or add object with tag '{fieldTag}'.");
    }

    public bool TryGetTransform(out Transform result)
    {
        result = fieldTransform;
        return result != null;
    }
}
