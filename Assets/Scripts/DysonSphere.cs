using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DysonSphere : MonoBehaviour {

    public static DysonSphere instance;
    void Awake(){
        if (instance != null) { Debug.LogError("More than one DysonSphere in scene !"); return; } instance = this;
    }

    [Header("Settings")]
    public float baseEnergyProduction = 2000f;
    public float maxStructurePoints = 5000f;
    public float degradationRate = 1f;
    public List<ResourcesManager.ResourceAmount> resourceCostPerStructurePointRepair;
    public bool startAutoRepairState = false;

    [Header("Operation")]
    public bool isUnlocked = false;
    public bool isActivated = false;
    public float currentEnergyProduction = 2000f;
    // Structure points
    public bool isDegrading = true;
    public float currentStructurePoints = 5000f;
    public bool currentAutoRepairState = false;


    void Start()
    {
        Initialize();
    }

    public void SetSettings()
    {
        currentEnergyProduction = baseEnergyProduction;
        currentAutoRepairState = startAutoRepairState;

        resourceCostPerStructurePointRepair = new List<ResourcesManager.ResourceAmount>();
        resourceCostPerStructurePointRepair.Add(new ResourcesManager.ResourceAmount("steel", 5));
        resourceCostPerStructurePointRepair.Add(new ResourcesManager.ResourceAmount("carbon", 4));
        resourceCostPerStructurePointRepair.Add(new ResourcesManager.ResourceAmount("composite", 1));
    }

    public void Initialize()
    {
        SetSettings();
        isActivated = true;
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();

        InvokeRepeating("DegradeDysonSphere", 0f, 2f);
        InvokeRepeating("AutoRepairDysonSphere", 0f, 2f);
    }


    public void SendSettingsToControlPanel()
    {
        DysonSphereSettings dysonSphereSettings = GatherDysonSphereSettings();

        DysonSphereControlPanel.instance.ReceiveSettings(dysonSphereSettings);
    }

    public DysonSphereSettings GatherDysonSphereSettings()
    {
        DysonSphereSettings dysonSphereSettings = new DysonSphereSettings(currentEnergyProduction, maxStructurePoints, currentStructurePoints, currentAutoRepairState);
        return dysonSphereSettings;
    }

    public void ConfigureFromControlPanel(DysonSphereSettings dysonSphereSettings)
    {

    }

    public void RepairRequest()
    {
        Debug.Log("RepairRequest");

        int missingStructurePoints = Mathf.FloorToInt(maxStructurePoints - currentStructurePoints);
        Debug.Log("MissingStructurePoints [" + missingStructurePoints + "]");

        int maxStructurePointsPossibleToRepair = missingStructurePoints;     // Change to infinity ?

        foreach (ResourcesManager.ResourceAmount resAmount in resourceCostPerStructurePointRepair)
        {
            int neededAmountPerStructurePoint = resAmount.amount;
            //int totalNeededAmount = neededAmountPerStructurePoint * missingStructurePoints;
            int availableResourceAmount = ResourcesManager.instance.GetCurrentResourceAmount(resAmount.resourceType);

            int maxStructurePointsAbleToRepairWithResource = availableResourceAmount / neededAmountPerStructurePoint;
            Debug.Log("maxStructurePointsAbleToRepairWithResource with [" + resAmount.resourceType.resourceName + "] is [" + maxStructurePointsAbleToRepairWithResource + "]");

            if (maxStructurePointsAbleToRepairWithResource < maxStructurePointsPossibleToRepair)
            {
                maxStructurePointsPossibleToRepair = maxStructurePointsAbleToRepairWithResource;
            }
        }

        Debug.Log("We can repair [" + maxStructurePointsPossibleToRepair + "] Structure points");

        foreach (ResourcesManager.ResourceAmount resAmount in resourceCostPerStructurePointRepair)
        {
            ResourcesManager.ResourceAmount resAmountToPay = new ResourcesManager.ResourceAmount(resAmount.resourceType, resAmount.amount * maxStructurePointsPossibleToRepair);
            ResourcesManager.instance.PayResourceAmount(resAmountToPay);
        }

        RepairStructurePoints(maxStructurePointsPossibleToRepair);
    }

    public void RepairStructurePoints(int structurePointsNb)
    {
        currentStructurePoints = Mathf.Min(maxStructurePoints, currentStructurePoints + structurePointsNb);
        CalculateEnergyProduction();
        SendSettingsToControlPanel();
    }

    public void DegradeDysonSphere()
    {
        if(isDegrading && GameManager.instance.gameState == GameManager.GameState.Default)
        {
            if (!LevelManager.instance.currentLevelFinished)
            {
                DecreaseStructurePointsAtSetRate();
            }
        }
    }

    public void DecreaseStructurePointsAtSetRate()
    {
        currentStructurePoints = Mathf.Max(0f, currentStructurePoints - degradationRate);
        CalculateEnergyProduction();
        SendSettingsToControlPanel();
    }

    public void CalculateEnergyProduction()
    {
        currentEnergyProduction = ((baseEnergyProduction / maxStructurePoints) * currentStructurePoints);
        EnergyPanel.instance.UpdateEnergyProductionAndConsumption();
    }

    public void SwitchAutoRepairState()
    {
        currentAutoRepairState = !currentAutoRepairState;
        Debug.Log("AutoRepair [" + currentAutoRepairState + "]");
        SendSettingsToControlPanel();
    }

    public void AutoRepairDysonSphere()
    {
        if(currentAutoRepairState)
        {
            RepairRequest();
        }
    }

    public void AdaptLaserToPlanetMovement()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        Vector3 planetPos = GameManager.instance.mainPlanet.GetComponent<MainPlanet>().mainPlanetEmpty.transform.position;
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 calculatedPos = new Vector3(planetPos.x - cameraPos.x, planetPos.y - cameraPos.y, planetPos.z);
        lr.SetPosition(1, calculatedPos);
        Debug.Log("AdaptLaserToPlanetMovement | LaserPos [" + planetPos + "]");
    }

    public void Configure(float structurePointsSetting, bool autoRepairSetting)
    {
        currentStructurePoints = structurePointsSetting;
        currentAutoRepairState = autoRepairSetting;

        CalculateEnergyProduction();

        SendSettingsToControlPanel();
    }

    public class DysonSphereSettings
    {
        public float energyProduction;
        public float maxStructurePoint;
        public float currentStructurePoints;
        public bool currentAutoRepairState;

        public DysonSphereSettings(float energyProduction, float maxStructurePoint, float currentStructurePoints, bool currentAutoRepairState)
        {
            this.energyProduction = energyProduction;
            this.maxStructurePoint = maxStructurePoint;
            this.currentStructurePoints = currentStructurePoints;
            this.currentAutoRepairState = currentAutoRepairState;
        }
    }

}
