namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;
    using Newtonsoft.Json;


    public class SpawnOnMap : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        [Geocode]
        public string[] locationStrings;
        public Vector2d[] locations;

        [SerializeField]
        float _spawnScale = 100f;

        [SerializeField]
        GameObject _windradPrefab;
        [SerializeField]
        GameObject _bioPrefab;
        [SerializeField]
        GameObject _pvPrefab;

        List<GameObject> spawnedObjects;

        void Start()
        {
            _spawnScale = 5f;

            spawnedObjects = new List<GameObject>();

            FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(DownloadManager.Instance.jsonFromWeb);

            locationStrings = new string[featureCollection.totalFeatures];

            locations = new Vector2d[locationStrings.Length];


            string json = DownloadManager.Instance.jsonWgsFormat; //json data der umgewandelten Koordinaten

            List<Wgs84Coordinates> list = JsonConvert.DeserializeObject<List<Wgs84Coordinates>>(json); //geparste List von umgewandelten Koordinaten

            //um die Koordinaten als Lat und Lon einzuordnen
            for (int i = 0; i < list.Count; i++)
            {
                locationStrings[i] = list[i].y + " , " + list[i].x;
            }


            for (int i = 0; i < locationStrings.Length; i++)
            {
                GameObject instance = null;
                var locationString = locationStrings[i];
                locations[i] = Conversions.StringToLatLon(locationString);

                // überprüft den ausgewählten Energietyp und instanziert das entsprechende Prefab
                if (DownloadManager.Instance.SelectedEnergyType == EnergyType.PV)
                {
                    instance = Instantiate(_pvPrefab);
                    Debug.Log("Objekt erstellt.");
                }
                else if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Wind)
                {
                    instance = Instantiate(_windradPrefab);
                    Debug.Log("Objekt erstellt.");
                }
                else
                {
                    instance = Instantiate(_bioPrefab);
                    Debug.Log("Objekt erstellt.");
                }

                // setzt die Eigenschaften des an der Instanz angehängten PoiPointer-Komponenten
                instance.GetComponent<PoiPointer>().PointPosition = locations[i];
                instance.GetComponent<PoiPointer>().poiID = i + 1;
                instance.GetComponent<PoiPointer>().feature = featureCollection.features[i];

                // konvertiert die geografischen Koordinaten in eine Weltposition
                Vector3 vector = _map.GeoToWorldPosition(locations[i], true);

                // setzt die Position und Skalierung der Instanz
                instance.transform.localPosition = _map.GeoToWorldPosition(locations[i], true);
                instance.transform.localPosition = vector;
                vector.z = 0;
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

                // fügt die Instanz der spawnedObjects-Liste hinzu
                spawnedObjects.Add(instance);
            }
        }

        private void Update()
        {
            int count = spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = spawnedObjects[i];
                var location = locations[i];
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                Vector3 vector = _map.GeoToWorldPosition(location, true);
                vector.z = 0;
            }
        }

    }
}