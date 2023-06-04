using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wgs84Coordinates
{
    /*
    public string x { get; set; }
    public string y { get; set; }
    public string z { get; set; }
}
    */
    private string _x;
    private string _y;
    private string _z = "0"; // Setze z auf den Wert 0 als Standard

    public string x
    {
        get { return _x; }
        set { _x = value.Replace(",", "."); } // Ersetze Komma mit Punkt
    }

    public string y
    {   
        get { return _y; }
        set { _y = value.Replace(",", "."); } // Ersetze Komma mit Punkt
    }

    public string z
    {
        get { return _z; }
        set { _z = value; }
    }

}