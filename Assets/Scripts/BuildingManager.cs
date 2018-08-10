using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one BuildingManager in scene !"); return; } instance = this;
    }

    public enum BuildingState { Default, BuildingSelected, LocationSelected, Building }

    public BuildingState buildingState;

    public GameObject buildingPreviewIndicator;
    public GameObject mainPlanet;
    public GameObject chosenBuildingSlot;
    public List<GameObject> buildingList = new List<GameObject>();
    public BuildingType selectedBuilding = null;

    public List<BuildingType> availableBuildings = new List<BuildingType>();

    [Header("Building Prefabs")]
    public GameObject laserTurretPrefab;
    public GameObject bulletTurretPrefab;
    public GameObject freezingTurretPrefab;
    public GameObject powerPlantPrefab;
    public GameObject mineBuildingPrefab;
    public GameObject laserSatellitePrefab;
    public GameObject shockSatellitePrefab;
    public GameObject debrisCollectorStationPrefab;

    void Start()
    {   

    }


    public void SetAvailableBuildings()
    {
        availableBuildings.Add(new BuildingType("Laser Turret", laserTurretPrefab, 25f, new List<ResourcesManager.ResourceAmount>(){
                                                                                            new ResourcesManager.ResourceAmount(ResourcesManager.instance.GetResourceFromCurrentListFromName("steel"), 100)
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet, "laser_turret"));
        availableBuildings.Add(new BuildingType("Bullet Turret", bulletTurretPrefab, 20f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet));
        availableBuildings.Add(new BuildingType("Freezing Turret", freezingTurretPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet));
        availableBuildings.Add(new BuildingType("Power Plant", powerPlantPrefab, 0f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet));
        availableBuildings.Add(new BuildingType("Mine Building", mineBuildingPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet));
        availableBuildings.Add(new BuildingType("Laser Satellite", laserSatellitePrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks));
        availableBuildings.Add(new BuildingType("Shock Satellite", shockSatellitePrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks));
        availableBuildings.Add(new BuildingType("Debris Collector Station", debrisCollectorStationPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks));
    }

    public void SelectBuilding(BuildingType bType)
    {
        if(buildingState == BuildingState.Default)
        {
            selectedBuilding = bType;
            buildingState = BuildingState.BuildingSelected;
            DebugManager.instance.DisplayBuildingState();
            ShowCancelButton();
        }
    }

    public void CancelButton()
    {
        if(buildingState == BuildingState.BuildingSelected || buildingState == BuildingState.LocationSelected)
        {
            buildingState = BuildingState.Default;
            DebugManager.instance.DisplayBuildingState();
            HideCancelButton();
            ShopPanel.instance.HideBuildButton();
            mainPlanet.GetComponent<MainPlanet>().ResetAllBuildingSlotsColor();
            Debug.Log("Leaving Building State.");
        }
    }

    public void BuildButton()
    {
        if (buildingState == BuildingState.LocationSelected)
        {
            if (chosenBuildingSlot.GetComponent<BuildingSlot>().CanBuildHere())
            {
                if (ResourcesManager.instance.CanPay(selectedBuilding))
                {
                    ResourcesManager.instance.Pay(selectedBuilding);

                    BuildBuilding();
                    buildingState = BuildingState.Default;
                    HideCancelButton();
                    HideBuildButton();
                    mainPlanet.GetComponent<MainPlanet>().ResetAllBuildingSlotsColor();
                    Debug.Log("Building Placed | Leaving Building State.");
                }
            }
        }
    }

    public void ShowCancelButton(){ ShopPanel.instance.ShowCancelButton(); }
    public void HideCancelButton(){ ShopPanel.instance.HideCancelButton(); }
    public void ShowBuildButton(){ ShopPanel.instance.ShowBuildButton(); }
    public void HideBuildButton(){ ShopPanel.instance.HideBuildButton(); }

    public void DisplayBuildingPreview()
    {
        buildingPreviewIndicator.transform.position = chosenBuildingSlot.transform.position;

        chosenBuildingSlot.GetComponent<BuildingSlot>().SetSelectionColor();
    }

    public Vector3 GetBuildingLocationFromTouchPoint()
    {
        Vector3 lastTouchPos = TouchManager.instance.lastTouch;
        float touchX = lastTouchPos.x;
        float touchY = lastTouchPos.y;
        Vector2 normalizedTouchXY = new Vector2(touchX, touchY).normalized;

        // TODO : Change
        Vector3 pos = new Vector3(touchX - Screen.width / 2, touchY - Screen.height / 2, GameManager.instance.mainPlanet.transform.position.z);

        Vector2 posV2 = new Vector2(touchX - Screen.width / 2, touchY - Screen.height / 2);
        Vector2 normalizedPos2 = posV2.normalized;
        Vector2 scaledPos2 = normalizedPos2 * GameManager.instance.mainPlanet.transform.localScale.x / 2;

        return new Vector3(scaledPos2.x, scaledPos2.y, pos.z);
    }

    public Vector3 GetLocationFromTouchPointOnPlanetPlane(Vector3 touchPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        Plane planetPlane = new Plane(Vector3.forward, mainPlanet.transform.position);
        float distance = 0;
        Vector3 intersectPointPos = new Vector3(0f, 0f, 0f);

        if (planetPlane.Raycast(ray, out distance)){
            intersectPointPos = ray.GetPoint(distance); 
        }

        return intersectPointPos;
    }

    public void SelectBuildingLocation()
    {   
        // Chose Building Spot
        chosenBuildingSlot = SelectBuildingSpot();

        // Preview
        DisplayBuildingPreview();

        buildingState = BuildingState.LocationSelected;
        DebugManager.instance.DisplayBuildingState();
        ShopPanel.instance.ShowBuildButton();
    }

    public GameObject SelectBuildingSpot()
    {
        GameObject buildingSpot = null;
        Vector3 touchPos = TouchManager.instance.lastTouch;
        Vector3 planeLoc =  GetLocationFromTouchPointOnPlanetPlane(touchPos);

        GameManager.instance.mainPlanet.GetComponent<MainPlanet>().ResetAllBuildingSlotsColor();
        SurroundingAreasManager.instance.ResetAllSatelliteBuildingSlotsColor();

        if(selectedBuilding.buildingLocationType == BuildingType.BuildingLocationType.Planet)
        {
            buildingSpot = GameManager.instance.mainPlanet.GetComponent<MainPlanet>().FindClosestBuildingSlot(planeLoc);
        }
        else if(selectedBuilding.buildingLocationType == BuildingType.BuildingLocationType.Disks)
        {
            buildingSpot = SurroundingAreasManager.instance.FindClosestBuildingSlotInUnlockedDisks(planeLoc);
        }

        buildingSpot.GetComponent<BuildingSlot>().SetSelectionColor();

        return buildingSpot;
    }

    public void BuildBuilding()
    {
        GameObject instantiatedBuilding = Instantiate(selectedBuilding.prefab, chosenBuildingSlot.transform.position, Quaternion.Euler(0f,0f, GeometryManager.instance.RadiansToDegrees(chosenBuildingSlot.GetComponent<BuildingSlot>().angle)));
        buildingList.Add(instantiatedBuilding);
        instantiatedBuilding.GetComponent<Building>().buildingType = selectedBuilding;
        chosenBuildingSlot.GetComponent<BuildingSlot>().SetBuilding(laserTurretPrefab.GetComponent<LaserTurret>());
        instantiatedBuilding.transform.SetParent(chosenBuildingSlot.transform);
        Debug.Log("New building instantiated !");

        EnergyPanel.instance.UpdateEnergyNeedsDisplay();

        // Distribute the available energy across all buildings
        EnergyPanel.instance.DistributeEnergy();
    }


    // TODO : Only testing purpose
    public void TestBuildButton()
    {
        Debug.Log("Test Build Button.");
        if(buildingState == BuildingState.Default)
        {
            buildingState = BuildingState.Building;
            Debug.Log("Entering Building State.");
        }
    }

    [System.Serializable]
    public class BuildingType {

        public string name;
        public GameObject prefab;

        // TODO : Maybe these things have nothing to do here ? Put these info in the gameObject script instead ?
        public List<ResourcesManager.ResourceAmount> resourceCosts;
        public float requiredEnergy;

        public enum BuildingLocationType {Planet, Disks};
        public BuildingLocationType buildingLocationType;

        public Sprite buildingImage;


        //public Building buildingScript;
        // Its in the prefab !

        public BuildingType(string name, GameObject prefab, float requiredEnergy)
        {
            this.name = name;
            this.prefab = prefab;
            this.requiredEnergy = requiredEnergy;
        }

        public BuildingType(string name, GameObject prefab, float requiredEnergy, List<ResourcesManager.ResourceAmount> cost)
        {
            this.name = name;
            this.prefab = prefab;
            this.requiredEnergy = requiredEnergy;
            this.resourceCosts = cost;
        }

        public BuildingType(string name, GameObject prefab, float requiredEnergy, List<ResourcesManager.ResourceAmount> cost, BuildingLocationType buildingLocationType)
        {
            this.name = name;
            this.prefab = prefab;
            this.requiredEnergy = requiredEnergy;
            this.resourceCosts = cost;
            this.buildingLocationType = buildingLocationType;
        }

        public BuildingType(string name, GameObject prefab, float requiredEnergy, List<ResourcesManager.ResourceAmount> cost, BuildingLocationType buildingLocationType, string imageName)
        {
            this.name = name;
            this.prefab = prefab;
            this.requiredEnergy = requiredEnergy;
            this.resourceCosts = cost;
            this.buildingLocationType = buildingLocationType;
            this.buildingImage = Resources.Load<Sprite>("Images/Buildings/Turrets/" + imageName);
        }

    }

}
