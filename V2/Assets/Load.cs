using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using GLTFast;

public class Load : MonoBehaviour
{
    private Transform model;
    byte[] data;

    public void ReceiveData(byte[] d)
    {
        data = d;
    }

    public async Task LoadModel()
    {
        if (model != null)
        {
            Destroy(model.gameObject);
            model = null;
        }
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data);
        if (success)
        {
            success = await gltf.InstantiateMainSceneAsync(transform);
            model = transform.GetChild(transform.childCount - 1);
        }
    }
}
