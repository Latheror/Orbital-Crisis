using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager instance;
    void Awake(){ 
        if (instance != null){ Debug.LogError("More than one BuildingManager in scene !"); return; } instance = this;
    }

    public enum BuildingState { Default, BuildingSelected, LocationSelected, BuildingAndLocationSelected, Building }

    [Header("World")]
    public GameObject mainPlanet;

    [Header("Building Stage")]
    public BuildingState buildingState;
    public GameObject buildingPreviewIndicator;
    public GameObject chosenBuildingSlot;
    public BuildingType selectedBuilding = null;

    [Header("Buildings")]
    public List<BuildingType> availableBuildings = new List<BuildingType>();
    public List<GameObject> buildingList = new List<GameObject>();

    [Header("Building Prefabs")]
    public GameObject laserTurretPrefab;
    public GameObject bulletTurretPrefab;
    public GameObject freezingTurretPrefab;
    public GameObject powerPlantPrefab;
    public GameObject mineBuildingPrefab;
    public GameObject laserSatellitePrefab;
    public GameObject shockSatellitePrefab;
    public GameObject debrisCollectorStationPrefab;
    public GameObject satelliteSolarStationPrefab;
    public GameObject healingTurretPrefab;

    void Start()
    {   

    }


    public void SetAvailableBuildings()
    {
        availableBuildings.Add(new BuildingType("Laser Turret", laserTurretPrefab, 25f, new List<ResourcesManager.ResourceAmount>(){
                new ResourcesManager.ResourceAmount("carbon", 50),
                new ResourcesManager.ResourceAmount("steel", 50),
                new ResourcesManager.ResourceAmount("carbon", 50)},
                                                BuildingType.BuildingLocationType.Planet, "laser_turret", 3, 0,
                                                "Powerful turret firing a laser beam at incoming ennemies.",
                new List<ResourcesManager.UpgradeCost>(){
                    new ResourcesManager.UpgradeCost(2, new List<ResourcesManager.ResourceAmount>(){
                        new ResourcesManager.ResourceAmount("carbon", 50),
                        new ResourcesManager.ResourceAmount("steel", 30),
                    }),
                    new ResourcesManager.UpgradeCost(3, new List<ResourcesManager.ResourceAmount>(){
                        new ResourcesManager.ResourceAmount("steel", 50)
                    })
                },
                true
                ));

        availableBuildings.Add(new BuildingType("Missile Turret", bulletTurretPrefab, 20f, new List<ResourcesManager.ResourceAmount>(){
                new ResourcesManager.ResourceAmount("steel", 50)},
                                                BuildingType.BuildingLocationType.Planet, "bullet_turret", 3, 0,
                                                "Shoots missiles at incoming ennemies.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));

        availableBuildings.Add(new BuildingType("Freezing Turret", freezingTurretPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                new ResourcesManager.ResourceAmount("silver", 40)},
                                                BuildingType.BuildingLocationType.Planet, "freezing_turret", 3, 2,
                                                "Freezes nearby ennemies and slow them down.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));

        availableBuildings.Add(new BuildingType("Power Plant", powerPlantPrefab, 0f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet, "power_plant", 3, 0,
                                                "Provides energy to your infrastructures.",
                new List<ResourcesManager.UpgradeCost>() { },
                false
                ));

        availableBuildings.Add(new BuildingType("Mine Building", mineBuildingPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet, "", 3, 0,
                                                "Gather resources needed to build infrastructures.",
                new List<ResourcesManager.UpgradeCost>() { },
                false
                ));

        availableBuildings.Add(new BuildingType("Laser Satellite", laserSatellitePrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks, "", 3, 3,
                                                "Satellite firing at nearby ennemies.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));

        availableBuildings.Add(new BuildingType("Shock Satellite", shockSatellitePrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks, "shock_satellite", 3, 4,
                                                "Satellite building dealing damage salves in a circle around it.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));

        availableBuildings.Add(new BuildingType("Recycling Station", debrisCollectorStationPrefab, 10f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks, "recycling_station", 3, 5,
                                                "Satellite base of recycling shuttles, able to recycle meteor debris and ennemy spaceship wrecks.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));

        availableBuildings.Add(new BuildingType("Solar Station", satelliteSolarStationPrefab, 0f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Disks, "solar_station", 3, 6,
                                                "A satellite covered by solar panels, providing energy to your infrastructures.",
                new List<ResourcesManager.UpgradeCost>() { },
                false
                ));

        availableBuildings.Add(new BuildingType("Healing Turret", healingTurretPrefab, 15f, new List<ResourcesManager.ResourceAmount>(){
                                                                                        },
                                                BuildingType.BuildingLocationType.Planet, "healing_turret", 3, 7,
                                                "Turret able to restore your spaceships health.",
                new List<ResourcesManager.UpgradeCost>() { },
                true
                ));
    }

    public void SelectBuilding(BuildingType bType)
    {
        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.ShopItemSelected);

        if (buildingState == BuildingState.Default || buildingState == BuildingState.BuildingSelected || buildingState == BuildingState.LocationSelected || buildingState == BuildingState.BuildingAndLocationSelected)
        {
            selectedBuilding = bType;
            if (buildingState == BuildingState.Default)
            {
                buildingState = BuildingState.BuildingSelected;
            }
            else if (buildingState == BuildingState.LocationSelected)
            {

                buildingState = BuildingState.BuildingAndLocationSelected;
            } else if (buildingState == BuildingState.BuildingAndLocationSelected)
            {
                if (bType.buildingLocationType != chosenBuildingSlot.GetComponent<BuildingSlot>().locationType)
                {
                    DeselectSelectedBuildingSlot();
                }
                if ((ResourcesManager.instance.CanPay(selectedBuilding)))
                {
                    ShopPanel.instance.ShowBuildButton();
                }
            }
            //DebugManager.instance.DisplayBuildingState();
            ShowCancelButton();
        }
    }

    public void DeselectBuilding()
    {
        selectedBuilding = null;
    }

    public void CancelButton()
    {
        if(buildingState == BuildingState.BuildingSelected || buildingState == BuildingState.LocationSelected
            || buildingState == BuildingState.BuildingAndLocationSelected)
        {
            buildingState = BuildingState.Default;

            DebugManager.instance.DisplayBuildingState();
            ShopPanel.instance.ResetLastShopItemSelected();
            DeselectBuilding();
            ShopPanel.instance.HideBuildButton();
            mainPlanet.GetComponent<MainPlanet>().ResetAllBuildingSlotsColor();
            HideCancelButton();

            Debug.Log("Leaving Building State.");
        }
    }

    public void BuildButton()
    {
        if (buildingState == BuildingState.LocationSelected || buildingState == BuildingState.BuildingAndLocationSelected)
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
                    SurroundingAreasManager.instance.ResetAllSatelliteBuildingSlotsColor();
                    ShopPanel.instance.ResetLastShopItemSelected();
                    Debug.Log("Building Placed | Leaving Building State.");
                }
            }
        }

        GameManager.instance.ChangeSelectionState(GameManager.SelectionState.Default);
    }

    public void ShowCancelButton(){ ShopPanel.instance.ShowCancelButton(); }
    public void HideCancelButton(){ ShopPanel.instance.HideCancelButton(); }
    public void ShowBuildButton(){ ShopPanel.instance.ShowBuildButton(); }
    public void HideBuildButton(){ ShopPanel.instance.HideBuildButton(); }

    public void DisplayBuildingPreview(){
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

        if(buildingState == BuildingState.BuildingSelected)
        {
            buildingState = BuildingState.BuildingAndLocationSelected;
        }
        else
        {
            //Debug.Log("Building State error...");
        }

        //DebugManager.instance.DisplayBuildingState();
        if((ResourcesManager.instance.CanPay(selectedBuilding)))
        {
            ShopPanel.instance.ShowBuildButton();
        }
        
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
        if(selectedBuilding != null)
        {
            float buildingSpotAngle = GeometryManager.instance.RadiansToDegrees(chosenBuildingSlot.GetComponent<BuildingSlot>().angle);
            Vector3 instantiationPosition = chosenBuildingSlot.transform.position;
            // Instantiate satellite slighly in front of building slot
            if (selectedBuilding.buildingLocationType == BuildingType.BuildingLocationType.Disks)
            {
                instantiationPosition += new Vector3(0f, 0f, -10f);
            }
            GameObject instantiatedBuilding = Instantiate(selectedBuilding.prefab, instantiationPosition, Quaternion.Euler(0f, 0f, buildingSpotAngle));
            buildingList.Add(instantiatedBuilding);
            instantiatedBuilding.GetComponent<Building>().buildingType = selectedBuilding;
            instantiatedBuilding.GetComponent<Building>().buildingSpotAngle = buildingSpotAngle;
            instantiatedBuilding.GetComponent<Building>().currentTier = 1;
            chosenBuildingSlot.GetComponent<BuildingSlot>().SetBuilding(laserTurretPrefab.GetComponent<LaserTurret>());
            instantiatedBuilding.transform.SetParent(chosenBuildingSlot.transform);
            Debug.Log("New building instantiated !");

            EnergyPanel.instance.UpdateEnergyNeedsDisplay();

            // Distribute the available energy across all buildings
            EnergyPanel.instance.DistributeEnergy();
        }     
    }

    public void DeselectSelectedBuildingSlot()
    {
        if(chosenBuildingSlot.GetComponent<BuildingSlot>().locationType == BuildingType.BuildingLocationType.Planet)    // Planet slot
        {
            GameManager.instance.mainPlanet.GetComponent<MainPlanet>().ResetAllBuildingSlotsColor();
        }
        else    // Satellite slot
        {
            SurroundingAreasManager.instance.ResetAllSatelliteBuildingSlotsColor();
        }

        chosenBuildingSlot = null;

        if(buildingState == BuildingState.BuildingAndLocationSelected)
        {
            buildingState = BuildingState.BuildingSelected;
        }else if(buildingState == BuildingState.BuildingSelected)
        {
            buildingState = BuildingState.Default;
        }

    }

    public void UnlockBuildingType(BuildingManager.BuildingType bType)
    {
        if (!bType.isUnlocked)
        {
            ShopPanel.instance.AddBuildingShopItem(bType);
            bType.isUnlocked = true;
            Debug.Log("Building \"" + bType.name + "\" unlocked.");
        }
        else
        {
            Debug.Log("Building : " + bType.name + " is already unlocked !");
        }
    }


    // TODO : Only testing purpose
    public void TestBuildButton()
    {
        //Debug.Log("Test Build Button.");
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
        public List<ResourcesManager.ResourceAmount> resourceCosts;
        public float requiredEnergy = 0;
        public enum BuildingLocationType {Planet, Disks};
        public BuildingLocationType buildingLocationType;
        public Sprite buildingImage;
        public int maxTier = 3;
        public bool isUnlocked = true;
        public int unlockedAtLevelNb = 0;
        public string description;
        public List<ResourcesManager.UpgradeCost> upgradeCosts;
        public bool hasRange;

        public BuildingType(string name, GameObject prefab, float requiredEnergy, List<ResourcesManager.ResourceAmount> cost, BuildingLocationType buildingLocationType, string imageName, int maxTier, int unlockedAtLevelNb, string description, List<ResourcesManager.UpgradeCost> upgradeCosts, bool hasRange)
        {
            this.name = name;
            this.prefab = prefab;
            this.requiredEnergy = requiredEnergy;
            this.resourceCosts = cost;
            this.buildingLocationType = buildingLocationType;
            this.buildingImage = Resources.Load<Sprite>("Images/Buildings/" + imageName);
            this.maxTier = maxTier;
            this.isUnlocked = (unlockedAtLevelNb == 0) ? true : false;
            this.unlockedAtLevelNb = unlockedAtLevelNb;
            this.description = description;
            this.upgradeCosts = upgradeCosts;
            this.hasRange = hasRange;
        }

        public List<ResourcesManager.ResourceAmount> GetUpgradeCostsForTierNb(int tierNb)
        {
            List<ResourcesManager.ResourceAmount> costs = new List<ResourcesManager.ResourceAmount>();
            foreach (ResourcesManager.UpgradeCost upgradeCost in upgradeCosts)
            {
                if(upgradeCost.tierIndex == tierNb) // Matching tier nb
                {
                    Debug.Log("Found matching UpgradeCostList");
                    costs = upgradeCost.resourceCosts;
                    break;
                }
            }
            return costs;
        }
    }

}
