using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneAssetManager))]
public class SceneAssetManagerEditor : Editor
{
    SceneAssetManager assetManager;
    List<SceneAsset> sceneAssetList;
    bool showAssetList = true;

    private void OnEnable()
    {
        assetManager = target as SceneAssetManager;
        if (assetManager && assetManager.AssetDictionary!=null && assetManager.AssetDictionary.Count > 0)
        {
            sceneAssetList = assetManager.AssetDictionary.Values.ToList();
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        this.DrawDefaultInspector();

        showAssetList = EditorGUILayout.BeginFoldoutHeaderGroup(showAssetList, "Scene Assets");
        if (showAssetList) {
            if (sceneAssetList != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                for (int i = 0; i < sceneAssetList.Count; i++)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    EditorGUILayout.ObjectField(i + " : " + sceneAssetList[i].assetName,
                        sceneAssetList[i], typeof(SceneAsset));
#pragma warning restore CS0618 // Type or member is obsolete
                }
                EditorGUI.BeginDisabledGroup(false);

            }
            else
            {
                EditorGUILayout.LabelField("No scene assets");
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (EditorGUI.EndChangeCheck())
        {
            if (assetManager && assetManager.AssetDictionary != null && assetManager.AssetDictionary.Count > 0)
            {
                sceneAssetList = assetManager.AssetDictionary.Values.ToList();
            }
            else
            {
                sceneAssetList.Clear();
            }
            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.Update();

    }
}
