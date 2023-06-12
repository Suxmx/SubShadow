using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Services
{
    public static class MenuTool
    {
        [MenuItem("Tools/Scene/Scene0 _`")]
        //用于便捷地返回0场景
        public static void OpenScene0()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.OpenScene("Assets/Scenes/0.unity");
                SceneAsset asset = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/0.unity");
                ProjectWindowUtil.ShowCreatedAsset(asset);
            }
#endif
        }

        [MenuItem("Tools/优化Texture")]
        private static void OptimizeTexture()
        {
            Queue<string> dirPaths = new Queue<string>();
            dirPaths.Enqueue("Assets/Images");
            int doCount = 0;
            while (dirPaths.Count > 0)
            {
                string dirPath = dirPaths.Dequeue();
                if (Directory.Exists(dirPath))
                {
                    //1. 遍历文件夹
                    DirectoryInfo direction = new DirectoryInfo(dirPath);
                    foreach (var dir in direction.GetDirectories())
                    {
                        dirPaths.Enqueue(dir.FullName);
                    }
                    FileInfo[] files = direction.GetFiles();
                    //2. 收集所有的图片
                    for (int i = 0; i < files.Length; ++i)
                    {
                        var path = files[i].FullName;
                        //Debug.Log(path);
                        if (path.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase))
                        {
                            path = path.Substring(32);
                            Debug.Log(path);
                            //3. 设置图片的格式 
                            if (SetTextureFormat(path))
                            {
                                ++doCount;
                            }
                        }
                    }
                }
            }
            Debug.LogFormat("优化完成，共优化{0}张图片", doCount);
        }

        private static bool SetTextureFormat(string texPath)
        {
            //Debug.Log(texPath);
            AssetImporter importer = AssetImporter.GetAtPath(texPath);
            bool isChanged = false;
            //Debug.Log(importer == null);
            if (importer != null && importer is UnityEditor.TextureImporter)
            {
                Debug.Log(texPath);
                //TODO if (!_whiteNames.Contains(fileName))
                TextureImporter textureImporter = (TextureImporter)importer;
                if (textureImporter.maxTextureSize != 512)
                {
                    //textureImporter.maxTextureSize = 512;
                    //isChanged = true;
                }
        }
            return isChanged;
        }
    }
}