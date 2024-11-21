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

    public async Task LoadModel(string path)
    {
        if (model != null)
        {
            Destroy(model.gameObject);
            model = null;
        }
        byte[] data = File.ReadAllBytes(path);
        var gltf = new GltfImport();
        bool success = await gltf.LoadGltfBinary(data, new Uri(path));
        if (success)
        {
            success = await gltf.InstantiateMainSceneAsync(transform);
            model = transform.GetChild(transform.childCount - 1);
        }
    }
}
