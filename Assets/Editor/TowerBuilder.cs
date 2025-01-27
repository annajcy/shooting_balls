using System.Collections.Generic;
using MaterialApplier;
using UnityEditor;
using UnityEngine;

namespace EditorScript
{
    public enum BuildType
    {
        Sampler,
        Tower,
    }

    public class TowerBuilder : EditorWindow
    {
        private Vector3 worldPosition = new Vector3(0.0f, 0.5f, 0.0f);
        private Vector3 towerScale = new Vector3(10.0f, 10.0f, 10.0f);
        private Vector3 cubeScale = Vector3.one;

        private bool isSampling = false;
        private bool isSaveToPrefab = false;
        private bool isRandomizeColor = true;
        private string prefabSavingPath = $"Assets/Resources/Prefabs";

        private GameObject sampler;
        private GameObject tower;

        [MenuItem("Tools/Tower Builder")]
        public static void ShowWindow()
        {
            GetWindow<TowerBuilder>("TowerBuilder");
        }

        private bool SaveToPrefab()
        {
            if (tower == null) return false;
            string prefabPath = prefabSavingPath + $"/tower.prefab";
            PrefabUtility.SaveAsPrefabAsset(tower, prefabPath);
            return true;
        }

        private void AlignGround(GameObject gameObject)
        {
            var x = gameObject.transform.position.x;
            var z = gameObject.transform.position.z;
            var y = cubeScale.y * towerScale.y * 0.5f;
            gameObject.transform.position = new Vector3(x, y, z);
        }

        private void OnGUI()
        {
            GUILayout.Label("Tower Parameters", EditorStyles.boldLabel);
            cubeScale = EditorGUILayout.Vector3Field("Cube Scale", cubeScale);
            towerScale = EditorGUILayout.Vector3Field("Tower Scale", towerScale);
            worldPosition = EditorGUILayout.Vector3Field("World Position", worldPosition);
            isSaveToPrefab = EditorGUILayout.Toggle("Save To Prefab", isSaveToPrefab);
            isRandomizeColor = EditorGUILayout.Toggle("Randomize Color", isRandomizeColor);

            if (isSaveToPrefab)
                prefabSavingPath = EditorGUILayout.TextField("Prefab Saving Path", prefabSavingPath);


            if (!isSampling)
            {
                if (GUILayout.Button("Sample Position"))
                {
                    if (sampler) DestroyImmediate(sampler);
                    sampler = Build(BuildType.Sampler);
                    sampler.transform.position = worldPosition;
                    isSampling = true;
                }

                if (GUILayout.Button("Build"))
                {
                    if (sampler != null) DestroyImmediate(sampler);
                    tower = Build(BuildType.Tower);
                    if (isSaveToPrefab) SaveToPrefab();
                }

            }
            else
            {
                if (GUILayout.Button("AlignGround"))
                {
                    if (sampler) AlignGround(sampler);
                    else Debug.LogError("No position sampler");
                }

                if (GUILayout.Button("Accept"))
                {
                    worldPosition = sampler.transform.position;
                    isSampling = false;
                }

                if (GUILayout.Button("Cancel"))
                {
                    DestroyImmediate(sampler);
                    isSampling = false;
                }
            }

        }

        private void OnDestroy()
        {
            if (sampler != null)
                DestroyImmediate(sampler);
        }

        private List<float> GeneratePointList(float center, float length, int count)
        {
            List<float> result = new List<float>();
            if (count % 2 == 1)
            {
                result.Add(center);
                int half = count / 2;
                for (int i = 1; i <= half; i++)
                {
                    result.Add(center - i * length);
                    result.Add(center + i * length);
                }
            }
            else
            {
                int half = count / 2;
                float left = center - length * 0.5f;
                float right = center + length * 0.5f;

                for (int i = 0; i < half; i++)
                {
                    result.Add(left - i * length);
                    result.Add(right + i * length);
                }
            }

            return result;
        }

        private List<Vector3> GeneratePositionList()
        {
            List<Vector3> result = new List<Vector3>();
            List<float> listX = GeneratePointList(0f, cubeScale.x, (int)towerScale.x);
            List<float> listY = GeneratePointList(0f, cubeScale.y, (int)towerScale.y);
            List<float> listZ = GeneratePointList(0f, cubeScale.z, (int)towerScale.z);

            for (int i = 0; i < listX.Count; i ++)
            for (int j = 0; j < listY.Count; j ++)
            for (int k = 0; k < listZ.Count; k ++)
                result.Add(new Vector3(listX[i], listY[j], listZ[k]));

            return result;
        }

        private GameObject Build(BuildType buildType)
        {
            string gameObjectName;
            GameObject prefab;
            bool randomPickColor;
            if (buildType == BuildType.Sampler)
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/TransparentCube.prefab");
                gameObjectName = "Sampler";
                randomPickColor = false;
            }
            else
            {
                prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/OpaqueCube.prefab");
                gameObjectName = "Tower";
                randomPickColor = isRandomizeColor;
            }

            GameObject parent = new GameObject(gameObjectName);
            parent.transform.position = worldPosition;

            List<Vector3> positionList = GeneratePositionList();

            for (int i = 0; i < positionList.Count; i++)
            {
                GameObject cube = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (cube != null)
                {
                    cube.transform.SetParent(parent.transform);
                    cube.transform.localPosition = positionList[i];
                    cube.transform.localScale = cubeScale;
                }
            }

            if (randomPickColor)
            {
                RandomizeColor randomizeColor = parent.AddComponent<RandomizeColor>();
                randomizeColor.Work();
            }

            return parent;
        }
    }
}