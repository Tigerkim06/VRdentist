using UnityEngine;

public class SceneAsset : MonoBehaviour
{
    public string assetName;
    public bool hideOnStart;

    private void Start()
    {
        SceneAssetManager.Instance.AddAsset(this);
        OnStart();
    }

    public void OnStart() {
        gameObject.SetActive(!hideOnStart);
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(assetName)) { assetName = this.name; }
    }
}
