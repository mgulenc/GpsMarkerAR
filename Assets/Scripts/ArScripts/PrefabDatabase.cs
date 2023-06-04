using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
    [CreateAssetMenu(fileName = "PrefabDb", menuName = "AR+GPS/PrefabDatabase")]
    public class PrefabDatabase : ScriptableObject
    {
        [System.Serializable]
        public class PrefabDatabaseEntry
        {
            /// <summary>
            ///   The `MeshId` associated with the prefab. Should match a `MeshId` from the data created
            ///   the Web Map Editor (https://editor.unity-ar-gps-location.com).
            /// </summary>
            public string MeshId;

            /// <summary>
            ///   The prefab you want to associate with the `MeshId`.
            /// </summary>
            //  public GameObject Prefab;
            public GameObject pvPrefab;
            public GameObject windradPrefab;
            public GameObject biomassePrefab;
        }

        public List<PrefabDatabaseEntry> Entries;

        public GameObject GetEntryById(string Id)
        {
            GameObject result = null;

            foreach (var entry in Entries)
            {
                if (entry.MeshId == Id)
                {
                    if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Bio)
                    {
                        result = entry.biomassePrefab;
                    }
                    else if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Wind)
                    {
                        result = entry.windradPrefab;
                    }
                    else
                    {
                        result = entry.pvPrefab;

                    }

                   // result = entry.Prefab;
                    break;
                }
            }

            return result;
        }
    }
}
