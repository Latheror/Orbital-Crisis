using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaStructureManager : MonoBehaviour {

    public static MegaStructureManager instance;
    void Awake()
    {
        if (instance != null) { Debug.LogError("More than one MegaStructureManager in scene !"); return; }
        instance = this;
    }


    public List<MegaStructure> availableMegaStructures;

    public GameObject planetaryShield;


    public void DefineAvailableMegastructures()
    {
        availableMegaStructures.Add(new MegaStructure(1, "Planetary Shield", planetaryShield));
    }



    public void EnableMegaStructure(MegaStructure megaStructure, bool enable)
    {
        megaStructure.go.SetActive(enable);
    }

}
