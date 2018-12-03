using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStructure : MonoBehaviour {

    public int id;
    public string megaStructureName;
    public GameObject go;
    public bool isBuilt;

    public MegaStructure(int id, string megaStructureName, GameObject go)
    {
        this.id = id;
        this.megaStructureName = megaStructureName;
        this.go = go;
        this.isBuilt = false;
    }
}
