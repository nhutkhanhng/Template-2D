using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class GameMenuEditor
{
    [MenuItem("GameObject/Pool/Update Client", false, 10)]
    static void UpdateClientPool(MenuCommand menuCommand)
    {
        string[] paths = new string[] { "_Runner/Prefabs/Game/",
        "Level Design/Prefabs/"};

        GameObject selectedObj = menuCommand.context as GameObject;
        if (selectedObj.GetComponent<SpawnPool>() == null)
        {
            Debug.LogWarning("GameObject not contain SpawnPool Component.");
            return;
        }
        SpawnPool sp = selectedObj.GetComponent<SpawnPool>();

        //clone for test
        //GameObject newObject = SGGameObjectUtils.LoadGameObject(selectedObj);
        //newObject.name = "Test Pool";
        //SpawnPool sp = newObject.GetComponent<SpawnPool>();
        //sp._perPrefabPoolOptions.Clear();//test

        List<GameObject> listPools = new List<GameObject>();

        //effects
        for (int i = 0; i < paths.Length; i++)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/" + paths[i]);
            if (dir.GetDirectories().Length > 0)
            {
                foreach (var d in dir.GetDirectories())
                {
                    //Debug.Log(Application.dataPath + "/" + paths[i] + d.Name);
                    DirectoryInfo childDir = new DirectoryInfo(Application.dataPath + "/" + paths[i] + d.Name);

                    if (childDir.GetDirectories().Length > 0)
                    {
                        foreach (var d2 in childDir.GetDirectories())
                        {
                            //Debug.Log(Application.dataPath + "/" + paths[i] + d.Name + "/" + d2.Name);
                            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/" + paths[i] + d.Name + "/" + d2.Name);
                            foreach (var s in dirInfo.GetFiles("*.prefab"))
                            {
                                string fullPath = s.FullName.Replace(@"\", "/");
                                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                                GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
                                listPools.Add(go);
                            }
                        }
                    }
                    else
                    {
                        foreach (var s in childDir.GetFiles("*.prefab"))
                        {
                            string fullPath = s.FullName.Replace(@"\", "/");
                            string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                            GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
                            listPools.Add(go);
                        }
                    }
                }
            }
            else
            {
                foreach (var s in dir.GetFiles("*.prefab"))
                {
                    string fullPath = s.FullName.Replace(@"\", "/");
                    string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");
                    GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
                    listPools.Add(go);
                }
            }
        }

        //TODO: add more later...

        //foreach (var p in listPools)
        //{
        //    if (sp._perPrefabPoolOptions.Find(r => r.prefab.gameObject.GetHashCode() == p.gameObject.GetHashCode()) != null)
        //        continue;//ignore existed

        //    sp._perPrefabPoolOptions.Add(new PrefabPool()
        //    {
        //        prefab = p.transform,
        //        preloadAmount = ((p.GetComponent<Unit>() != null) || p.name.Contains("Env") ? 2 : 0),
        //        preloadFrames = 0,
        //        limitAmount = 0,
        //        cullAbove = 0,
        //        cullDelay = 0,
        //        cullMaxPerPass = 0
        //    });
        //}
        PrefabUtility.ApplyPrefabInstance(selectedObj, InteractionMode.AutomatedAction);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
