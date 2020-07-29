using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssetManager : MonoBehaviour
{
    private Dictionary<string, SceneAsset> assetDictionary;
    public Dictionary<string, SceneAsset> AssetDictionary { get { return assetDictionary; } }

    private static SceneAssetManager instance;
    public static SceneAssetManager Instance { get { return instance; } }
    public void Awake()
    {
        instance = this;
        assetDictionary = new Dictionary<string, SceneAsset>();
    }

    public void AddAsset(SceneAsset asset) {
        if (!assetDictionary.ContainsKey(asset.assetName))
        {
            assetDictionary.Add(asset.assetName, asset);
        }
        else {
            Debug.Log(asset.assetName+" has already existed.");
        }
    }

    public static bool GetAsset(string name, out SceneAsset sceneAsset)
    {
        sceneAsset = new SceneAsset();
        if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
        {
            bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
            return found;
        }
        return false;
    }

    public static bool GetGameObjectAsset(string name, out GameObject retGameObject)
    {
        retGameObject = default;
        if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
        {
            if ( SceneAssetManager.instance.assetDictionary.TryGetValue(name, out SceneAsset sceneAsset)) {
                retGameObject = sceneAsset.gameObject;
                return retGameObject != null;
            }
        }
        return false;
    }

    public static bool GetAssetComponent<T>(string name, out T asset) {
        asset = default;
        if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null) {
            SceneAsset sceneAsset = new SceneAsset();
            bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
            Debug.Log("TryGetValue["+name+"]: " + found);
            if (found && sceneAsset.gameObject != null) {
                asset = sceneAsset.gameObject.GetComponent<T>();
                return asset!=null;
            }
        }
        return false;
    }

    public static bool GetAssetComponentInChildren<T>(string name, out T asset)
    {
        asset = default;
        if (SceneAssetManager.instance != null && SceneAssetManager.instance.assetDictionary != null)
        {
            SceneAsset sceneAsset = new SceneAsset();
            bool found = SceneAssetManager.instance.assetDictionary.TryGetValue(name, out sceneAsset);
            if (found && sceneAsset.gameObject != null)
            {
                asset = sceneAsset.gameObject.GetComponentInChildren<T>();
                return asset != null;
            }
        }
        return false;
    }
}
