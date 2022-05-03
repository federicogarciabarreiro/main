using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlKey
{
    First,
    Second,
    Third,
    Forth,
    Jump,
    Boost,
    Right,
    Left,
    Up,
    Down
}

public enum Prefabs
{
    BreakableWall,
    Enemy,
    TelekinesisPowerUp
}

public class Config : MonoBehaviour {

    public static Config me;
    public Transform laser00;
    public Material heroOriginalMaterial;
    public Material heroChangeSkillMaterial;
    public Transform energyShield;
    public Transform labStartPos;
    public Transform sandBoxStartPos;
    public Transform shockWave;
    public List<GameObject> listOfPrefabs;
    public Transform hero;
    public Transform timeBubble;
    public Transform timeStopBubble;
    public Transform handPowerEffect;
    public Transform handPowerEffect2;
    public Transform handPowerEffect3;
    public Material hologramMaterial;
    public Material ghostMaterial;
    public Material timeSlowMaterial;
    public GameObject dialogBubblePrefab;
    public LayerMask floorLayerMask;
    public LayerMask objectsMask;
    public LayerMask bulletMask;
    public LayerMask climbableMask;
    public LayerMask enemyLayer;
    public LayerMask repelableMask;
    public LayerMask freezableMask;
    public LayerMask sliderMask;
    public LayerMask dropableMask;
    public LayerMask waterBlocker;
    public LayerMask breakableMask;
    public LayerMask anyLayer;
    public LayerMask trasnparentFXMask;
    public LayerMask wallSideLayer;
    public LayerMask floorAndEnemies;
    public LayerMask objectsAndEnemies;
    public List<GameObject> labRoomObjects;
    public List<IControlKey> controlKeys = new List<IControlKey>();
    public Transform brokenDoor_interruptor;
    public Transform topPipes_first;
    public LayerMask heroAndWallsMask;
    public LayerMask heroAndObjectsMask;
    public LayerMask objectsAndHeroAndWalls;
    public LayerMask objectsHeroShield;
    public LayerMask objectsHeroShieldWalls;
    public LayerMask EnergyWallMask;
    public Transform targetBoxLearnShieldOnObject;
    public Transform doorToOpenFrom01to02;
    public Transform doorToCloseFrom01to02;
    public Transform portallvl02;
    public Transform portallvl03;
    public Transform portallvl02OutStartPos;
    public Transform crystalPortalOutlvl2;
    public LayerMask cameraLayerEverythingButCinematic;
    public LayerMask cameraLayerHeroAndCinematic;
    public Transform proyector;
    public List<Transform> platformsToDropDown;
    public Transform noLiftablePowerwall;
    public Transform platformMovable;
    public Transform cubeLevel03;
    public GameObject dyingEnemyPrefab;

    void Awake()
    {
        me = this;
        controlKeys.Add(new GenericControlKey(KeyCode.Mouse0, "LEFT CLICK"));
        controlKeys.Add(new GenericControlKey(KeyCode.Mouse1, "RIGHT CLICK"));
        controlKeys.Add(new GenericControlKey(KeyCode.Q, "Q"));
        controlKeys.Add(new GenericControlKey(KeyCode.F, "F"));
        controlKeys.Add(new GenericControlKey(KeyCode.Space, "SPACE BAR"));
        controlKeys.Add(new GenericControlKey(KeyCode.LeftShift, "LEFT SHIFT"));
        controlKeys.Add(new GenericControlKey(KeyCode.D, "D"));
        controlKeys.Add(new GenericControlKey(KeyCode.A, "A"));
        controlKeys.Add(new GenericControlKey(KeyCode.W, "W"));
        controlKeys.Add(new GenericControlKey(KeyCode.S, "S"));
    }
}
