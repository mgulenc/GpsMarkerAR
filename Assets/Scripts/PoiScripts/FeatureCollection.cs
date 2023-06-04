using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FeatureCollection
{
    public string type { get; set; }
    public List<double> bbox { get; set; }
    public int totalFeatures { get; set; }
    public List<Feature> features { get; set; }
    public Crs crs { get; set; }
}

[System.Serializable]
public class Crs
{
    public string type { get; set; }
    public Properties properties { get; set; }
}

[System.Serializable]
public class Feature
{
    public string type { get; set; }
    public string id { get; set; }
    public Geometry geometry { get; set; }
    public Properties properties { get; set; }
}


[System.Serializable]
public class Geometry
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}


[System.Serializable]
public class Properties
{
    public string adresse { get; set; }
    public string inst_leistung { get; set; }
    public string quelle { get; set; }
    public string name { get; set; }
    public string vnb_name { get; set; }
    public string ort_gemark { get; set; }
    public string plz { get; set; }
   // public DateTime inbetriebnahme { get; set; }
    public string strasse_flur_lang { get; set; }
    public string enh_anzahlmodule { get; set; }
    public string enh_hauptausrichtung { get; set; }
    public string enh_einspeisungsart { get; set; }
    public string inbetriebnahme { get; set; }
}




