using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using static ARLocation.WebMapLoader;

namespace ARLocation
{
    public class WebMapLoader : MonoBehaviour
    {
        public static WebMapLoader Instance;
        public string[] locationStrings;
       // public List<DataEntry> _dataEntries;

        public void Awake()
        {
            Instance = this;

            // dummy daten zum Testen
            // lat und lon Werte können beliebig ausgewählt und geschrieben werden(zum Testen!!)
            /*
            _dataEntries = new List<DataEntry>()
        {
            new DataEntry
            {
                inst_leistung = "280",
                adresse = "Am Vorwerk 7, 13127 Berlin",
                id = 1,
                lat = 123456789,
                lng = 123456789,
                altitude = 0,
                altitudeMode = "GroundRelative",
                name = "Windrad",
                meshId = "Logo",
                movementSmoothing = 0.05f,
                maxNumberOfLocationUpdates = 0,
                useMovingAverage = false,
                hideObjectUtilItIsPlaced = true
            },
            new DataEntry
            {
                inst_leistung = "2300",
                adresse = "Bernauer straße 38, 10435 Berlin",
                id = 2,
                lat = 123456789, 
                lng = 123456789,
                altitude = 0,
                altitudeMode = "GroundRelative",
                name = "Biomasse",
                meshId = "Logo",
                movementSmoothing = 0.05f,
                maxNumberOfLocationUpdates = 0,
                useMovingAverage = false,
                hideObjectUtilItIsPlaced = true
            },
            new DataEntry
            {
                inst_leistung = "1800",
                adresse = "Daimlerstr. 123, 12277 Berlin",
                id = 3,
                lat = 123456789,
                lng = 123456789,
                altitude = 0,
                altitudeMode = "GroundRelative",
                name = "Photovoltaik",
                meshId = "Logo",
                movementSmoothing = 0.05f,
                maxNumberOfLocationUpdates = 0,
                useMovingAverage = false,
                hideObjectUtilItIsPlaced = true
            }
        };
            //
            */

        }

        public class DataEntry
        {
            public string inst_leistung;
            public string adresse;
            public int id;
            public double lat;
            public double lng;
            public double altitude;
            public string altitudeMode;
            public string name;
            public string meshId;
            public float movementSmoothing;
            public int maxNumberOfLocationUpdates;
            public bool useMovingAverage;
            public bool hideObjectUtilItIsPlaced;

            public AltitudeMode getAltitudeMode()
            {
                if (altitudeMode == "GroundRelative")
                {
                    return AltitudeMode.GroundRelative;
                }
                else if (altitudeMode == "DeviceRelative")
                {
                    return AltitudeMode.DeviceRelative;
                }
                else if (altitudeMode == "Absolute")
                {
                    return AltitudeMode.Absolute;
                }
                else
                {
                    return AltitudeMode.Ignore;
                }
            }
        }

        /// <summary>
        ///   The `PrefabDatabase` ScriptableObject, containing a dictionary of Prefabs with a string ID.
        /// </summary>
        public PrefabDatabase PrefabDatabase;

        /// <summary>
        ///   The XML data file download from the Web Map Editor (htttps://editor.unity-ar-gps-location.com)
        /// </summary>
        public TextAsset XmlDataFile;

        /// <summary>
        ///   If true, enable DebugMode on the `PlaceAtLocation` generated instances.
        /// </summary>
        public bool DebugMode;

        /// <summary>
        /// Returns a list of the `PlaceAtLocation` instances created by this compoonent.
        /// >/summary>
        public List<PlaceAtLocation> Instances
        {
            get => _placeAtComponents;
        }

        public List<DataEntry> _dataEntries = new List<DataEntry>();

        private List<PlaceAtLocation> _placeAtComponents = new List<PlaceAtLocation>();

        // Start is called before the first frame update
        void Start()
        {
            IntegrateEnergyDataToObjects();
            BuildGameObjects();
        }

        /// <summary>
        ///
        /// Calls `SetActive(value)` for each of the gameObjects created by this component.
        ///
        /// </summary>
        public void SetActiveGameObjects(bool value)
        {
            foreach (var i in _placeAtComponents)
            {
                i.gameObject.SetActive(value);
            }
        }

        /// <summary>
        ///
        /// Hides all the meshes contained on each of the gameObjects created
        /// by this component, but does not disable the gameObjects.
        ///
        /// </summary>
        public void HideMeshes()
        {
            foreach (var i in _placeAtComponents)
            {
                Utils.Misc.HideGameObject(i.gameObject);
            }
        }

        /// <summary>
        ///
        /// Makes all the gameObjects visible after calling `HideMeshes`.
        ///
        /// </summary>
        public void ShowMeshes()
        {
            foreach (var i in _placeAtComponents)
            {
                Utils.Misc.ShowGameObject(i.gameObject);
            }
        }

        void BuildGameObjects()
        {

            foreach (var entry in _dataEntries)
            {
                var Prefab = PrefabDatabase.GetEntryById(entry.meshId);

                if (!Prefab)
                {
                    Debug.LogWarning($"[ARLocation#WebMapLoader]: Prefab {entry.meshId} not found.");
                    continue;
                }

                var PlacementOptions = new PlaceAtLocation.PlaceAtOptions()
                {
                    MovementSmoothing = entry.movementSmoothing,
                    MaxNumberOfLocationUpdates = entry.maxNumberOfLocationUpdates,
                    UseMovingAverage = entry.useMovingAverage,
                    HideObjectUntilItIsPlaced = entry.hideObjectUtilItIsPlaced
                };

                var location = new Location()
                {
                    Latitude = entry.lat,
                    Longitude = entry.lng,
                    Altitude = entry.altitude,
                    AltitudeMode = entry.getAltitudeMode(),
                    Label = entry.name
                };

                var instance = PlaceAtLocation.CreatePlacedInstance(Prefab,
                                                                    location,
                                                                    PlacementOptions,
                                                                    DebugMode);

                _placeAtComponents.Add(instance.GetComponent<PlaceAtLocation>());
            }

        }

        void IntegrateEnergyDataToObjects()
        {
            /*
            var xmlString = XmlDataFile.text;

            Debug.Log(xmlString);

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(xmlString);
            }
            catch (XmlException e)
            {
                Debug.LogError("[ARLocation#WebMapLoader]: Failed to parse XML file: " + e.Message);
            }

            var root = xmlDoc.FirstChild;
            var nodes = root.ChildNodes;


            foreach (XmlNode node in nodes)
            {

                Debug.Log(node.InnerXml);
                Debug.Log(node["id"].InnerText);

                int id = int.Parse(node["id"].InnerText);
                double lat = double.Parse(node["lat"].InnerText, CultureInfo.InvariantCulture);
                double lng = double.Parse(node["lng"].InnerText, CultureInfo.InvariantCulture);
                double altitude = double.Parse(node["altitude"].InnerText, CultureInfo.InvariantCulture);
                string altitudeMode = node["altitudeMode"].InnerText;
                string name = node["name"].InnerText;
                string meshId = node["meshId"].InnerText;
                float movementSmoothing = float.Parse(node["movementSmoothing"].InnerText, CultureInfo.InvariantCulture);
                int maxNumberOfLocationUpdates = int.Parse(node["maxNumberOfLocationUpdates"].InnerText);
                bool useMovingAverage = bool.Parse(node["useMovingAverage"].InnerText);
                bool hideObjectUtilItIsPlaced = bool.Parse(node["hideObjectUtilItIsPlaced"].InnerText);

                DataEntry entry = new DataEntry()
                {
                    id = id,
                    lat = lat,
                    lng = lng,
                    altitudeMode = altitudeMode,
                    altitude = altitude,
                    name = name,
                    meshId = meshId,
                    movementSmoothing = movementSmoothing,
                    maxNumberOfLocationUpdates = maxNumberOfLocationUpdates,
                    useMovingAverage = useMovingAverage,
                    hideObjectUtilItIsPlaced = hideObjectUtilItIsPlaced
                };

                _dataEntries.Add(entry);

                Debug.Log($"{id}, {lat}, {lng}, {altitude}, {altitudeMode}, {name}, {meshId}, {movementSmoothing}, {maxNumberOfLocationUpdates}, {useMovingAverage}, {hideObjectUtilItIsPlaced}");
            }
            */

            string json = DownloadManager.Instance.jsonWgsFormat; //json data der umgewandelten Koordinaten

            List<Wgs84Coordinates> list = JsonConvert.DeserializeObject<List<Wgs84Coordinates>>(json); //geparste List von umgewandelten Koordinaten

            locationStrings = new string[DownloadManager.Instance.featureCollection.totalFeatures];

            //um die Koordinaten als Lat und Lon einzuordnen
            for (int i = 0; i < list.Count; i++)
            {
                locationStrings[i] = list[i].y + " , " + list[i].x;
            }

            int index = 1;
            int id = 0;
            foreach (var feature in DownloadManager.Instance.featureCollection.features)
            {
                string[] coordinates = locationStrings[id].Split(',');

                DataEntry entry = new DataEntry()
                {
                    id = id,
                    lat = double.Parse(coordinates[0]),
                    lng = double.Parse(coordinates[1]),
                    altitudeMode = "GroundRelative",
                    altitude = 0,
                    //  name = feature.properties.name,
                    meshId = "Logo",
                    movementSmoothing = 0.05f,
                    maxNumberOfLocationUpdates = 0,
                    useMovingAverage = false,
                    hideObjectUtilItIsPlaced = true,
                    inst_leistung = feature.properties.inst_leistung
                };

                if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Wind)
                {
                    entry.adresse = feature.properties.adresse;
                    entry.name = "Windrad";
                }
                else if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Bio)
                {
                    entry.adresse = feature.properties.strasse_flur_lang + " , " + feature.properties.plz;
                    entry.name = "Biomasse";
                }
                else
                {
                    entry.adresse = feature.properties.strasse_flur_lang + " , " + feature.properties.plz;
                    entry.name = "Photovoltaik";
                }

                _dataEntries.Add(entry);

                //um zu testen
                Debug.Log("Energieart : " + _dataEntries[id].name + " / " + index + ". AR Objekt: / " + _dataEntries[id].lat + " , " + _dataEntries[id].lng + " / instalierte Leistung : " + _dataEntries[id].inst_leistung + " / Adresse: " + _dataEntries[id].adresse);
                id++;
                index++;

                //locText.text = "Your current location: " + locationProvider.CurrentLocation.latitude + " + " + locationProvider.CurrentLocation.longitude;
            }

        }
    }
}

