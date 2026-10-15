using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Builds a complete greybox scene for Mission 001 and wires the starter gameplay scripts.
/// Run from the Unity Editor menu: Last Survivor > Build Mission 001 Scene.
/// This is an Editor utility, not a runtime script.
/// </summary>
public static class InitialSceneBuilder
{
    private const string SceneFolder = "Assets/Scenes";
    private const string ScenePath = SceneFolder + "/Mission001_SecureTheOutpost.unity";

    private static readonly Color GroundColor = new Color(0.34f, 0.32f, 0.25f);
    private static readonly Color ConcreteColor = new Color(0.29f, 0.31f, 0.32f);
    private static readonly Color MetalColor = new Color(0.16f, 0.19f, 0.20f);
    private static readonly Color AccentColor = new Color(0.92f, 0.42f, 0.10f);
    private static readonly Color HealthColor = new Color(0.20f, 0.78f, 0.35f);
    private static readonly Color StaminaColor = new Color(0.22f, 0.67f, 0.95f);

    [MenuItem("Last Survivor/Build Mission 001 Scene")]
    public static void BuildMission001()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        EnsureFolder("Assets", "Scenes");

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Mission001_SecureTheOutpost";

        Material groundMaterial = CreateMaterial("MAT_OutpostGround", GroundColor);
        Material concreteMaterial = CreateMaterial("MAT_OutpostConcrete", ConcreteColor);
        Material metalMaterial = CreateMaterial("MAT_DarkMetal", MetalColor);
        Material accentMaterial = CreateMaterial("MAT_WarningOrange", AccentColor);
        Material enemyMaterial = CreateMaterial("MAT_EnemyCloth", new Color(0.38f, 0.12f, 0.10f));
        Material healthMaterial = CreateMaterial("MAT_HealthSupply", HealthColor);
        Material ammoMaterial = CreateMaterial("MAT_AmmoSupply", AccentColor);

        BuildEnvironment(groundMaterial, concreteMaterial, metalMaterial, accentMaterial);
        CreateLighting();

        GameObject player = BuildPlayer();
        Camera camera = BuildCamera(player.transform);
        WirePlayerAndWeapon(player, camera);

        List<SimpleEnemyAI> enemies = BuildEnemies(player.transform, enemyMaterial, concreteMaterial);
        BuildPickups(healthMaterial, ammoMaterial);

        Canvas canvas = BuildCanvas();
        GameObject missionProgress = CreateText(canvas.transform, "MissionProgress",
            "MISSION 001 — SECURE THE OUTPOST\nEliminate enemies: 0/3", 22,
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -28f),
            new Vector2(560f, 76f), TextAnchor.UpperLeft, Color.white);
        AddOutline(missionProgress.GetComponent<Text>(), new Color(0f, 0f, 0f, 0.8f));

        GameObject healthBar = CreateSlider(canvas.transform, "HealthBar",
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -118f),
            new Vector2(260f, 24f), HealthColor);
        GameObject staminaBar = CreateSlider(canvas.transform, "StaminaBar",
            new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -150f),
            new Vector2(260f, 18f), StaminaColor);
        GameObject healthLabel = CreateText(canvas.transform, "HealthLabel", "HEALTH 100/100",
            15, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -92f),
            new Vector2(260f, 22f), TextAnchor.MiddleLeft, Color.white);
        GameObject staminaLabel = CreateText(canvas.transform, "StaminaLabel", "STAMINA 100/100",
            13, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(24f, -171f),
            new Vector2(260f, 20f), TextAnchor.MiddleLeft, Color.white);

        GameObject ammoLabel = CreateText(canvas.transform, "AmmoLabel", "AMMO 12 | 60 RESERVE",
            20, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -28f),
            new Vector2(330f, 38f), TextAnchor.UpperRight, Color.white);
        GameObject crosshair = CreateText(canvas.transform, "Crosshair", "+", 34,
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero,
            new Vector2(50f, 50f), TextAnchor.MiddleCenter, Color.white);

        GameObject enemyHealthBar = CreateSlider(canvas.transform, "EnemyHealthBar",
            new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -48f),
            new Vector2(240f, 18f), new Color(0.85f, 0.20f, 0.16f));
        GameObject enemyHealthLabel = CreateText(canvas.transform, "EnemyHealthLabel", "ENEMY 60/60",
            15, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -70f),
            new Vector2(260f, 24f), TextAnchor.MiddleCenter, Color.white);

        BuildMobileControls(canvas.transform);

        GameObject missionPanel = CreatePanel(canvas.transform, "MissionCompletePanel",
            new Color(0.025f, 0.04f, 0.035f, 0.94f));
        GameObject winTitle = CreateText(missionPanel.transform, "VictoryMessage",
            "MISSION COMPLETE\nOUTPOST SECURED!", 38,
            new Vector2(0.5f, 0.63f), new Vector2(0.5f, 0.63f), Vector2.zero,
            new Vector2(680f, 170f), TextAnchor.MiddleCenter, new Color(0.45f, 1f, 0.55f));
        GameObject winRestart = CreateButton(missionPanel.transform, "VictoryRestartButton",
            "PLAY AGAIN", new Vector2(0.5f, 0.36f), new Vector2(260f, 64f), AccentColor);

        GameObject defeatPanel = CreatePanel(canvas.transform, "DefeatPanel",
            new Color(0.07f, 0.015f, 0.02f, 0.95f));
        GameObject defeatTitle = CreateText(defeatPanel.transform, "DefeatMessage",
            "MISSION FAILED\nYou were eliminated.", 36,
            new Vector2(0.5f, 0.63f), new Vector2(0.5f, 0.63f), Vector2.zero,
            new Vector2(680f, 160f), TextAnchor.MiddleCenter, new Color(1f, 0.42f, 0.36f));
        GameObject defeatRestart = CreateButton(defeatPanel.transform, "DefeatRestartButton",
            "RETRY MISSION", new Vector2(0.5f, 0.36f), new Vector2(280f, 64f), AccentColor);

        GameObject hudControllerObject = new GameObject("HUDControllers");
        hudControllerObject.transform.SetParent(canvas.transform, false);
        MobileHUDController mobileHud = hudControllerObject.AddComponent<MobileHUDController>();
        SetObjectReference(mobileHud, "playerStats", player.GetComponent<SurvivalStats>());
        SetObjectReference(mobileHud, "healthBar", healthBar.GetComponent<Slider>());
        SetObjectReference(mobileHud, "staminaBar", staminaBar.GetComponent<Slider>());
        SetObjectReference(mobileHud, "healthLabel", healthLabel.GetComponent<Text>());
        SetObjectReference(mobileHud, "staminaLabel", staminaLabel.GetComponent<Text>());

        CombatHUDController combatHud = hudControllerObject.AddComponent<CombatHUDController>();
        SetObjectReference(combatHud, "weapon", player.GetComponent<SimpleWeapon>());
        SetObjectReference(combatHud, "ammoLabel", ammoLabel.GetComponent<Text>());
        SetObjectReference(combatHud, "crosshairLabel", crosshair.GetComponent<Text>());

        EnemyHealthHUDController enemyHud = hudControllerObject.AddComponent<EnemyHealthHUDController>();
        SetObjectReference(enemyHud, "player", player.transform);
        SetObjectReference(enemyHud, "healthBar", enemyHealthBar.GetComponent<Slider>());
        SetObjectReference(enemyHud, "healthLabel", enemyHealthLabel.GetComponent<Text>());

        MissionObjectiveController missionController = hudControllerObject.AddComponent<MissionObjectiveController>();
        SetObjectReference(missionController, "objectiveText", missionProgress.GetComponent<Text>());
        SetObjectReference(missionController, "missionCompletePanel", missionPanel);
        SetObjectReference(missionController, "completionMessage", winTitle.GetComponent<Text>());
        SetEnemyArray(missionController, enemies.ToArray());

        GameOverController gameOverController = hudControllerObject.AddComponent<GameOverController>();
        SetObjectReference(gameOverController, "playerStats", player.GetComponent<SurvivalStats>());
        SetObjectReference(gameOverController, "defeatPanel", defeatPanel);
        SetObjectReference(gameOverController, "defeatMessage", defeatTitle.GetComponent<Text>());

        AddPersistentRestart(winRestart.GetComponent<Button>(), missionController);
        AddPersistentRestart(defeatRestart.GetComponent<Button>(), gameOverController);

        // The panels start hidden; their controller scripts activate the correct one.
        missionPanel.SetActive(false);
        defeatPanel.SetActive(false);

        CreateEventSystem();
        AddSceneToBuildSettings(scene.path);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);
        Selection.activeGameObject = player;

        Debug.Log("Mission 001 scene created at " + ScenePath +
            ". Open it and press Play to begin Editor testing. Scripts still need a real compile/test pass.");
        EditorUtility.DisplayDialog("Mission 001 Scene Created",
            "The greybox scene and UI have been created.\n\nNext: open the scene, press Play, and inspect the Console. This builder does not replace Unity compile and gameplay testing.",
            "OK");
    }

    private static void BuildEnvironment(Material ground, Material concrete, Material metal, Material accent)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Outpost_Ground";
        floor.transform.position = Vector3.zero;
        floor.transform.localScale = new Vector3(6f, 1f, 6f);
        floor.GetComponent<Renderer>().sharedMaterial = ground;

        // Outer perimeter walls create a recognizable, simple outpost courtyard.
        CreateBlock("North_Wall_Left", new Vector3(-7f, 1.4f, 13f), new Vector3(7f, 2.8f, 0.7f), concrete);
        CreateBlock("North_Wall_Right", new Vector3(7f, 1.4f, 13f), new Vector3(7f, 2.8f, 0.7f), concrete);
        CreateBlock("West_Wall", new Vector3(-13f, 1.4f, 3f), new Vector3(0.7f, 2.8f, 20f), concrete);
        CreateBlock("East_Wall", new Vector3(13f, 1.4f, 3f), new Vector3(0.7f, 2.8f, 20f), concrete);
        CreateBlock("South_Wall_Left", new Vector3(-7f, 1.4f, -7f), new Vector3(7f, 2.8f, 0.7f), concrete);
        CreateBlock("South_Wall_Right", new Vector3(7f, 1.4f, -7f), new Vector3(7f, 2.8f, 0.7f), concrete);

        // Entry lane, cover, supply area and a small watch platform.
        CreateBlock("Entry_Barrier_Left", new Vector3(-4.5f, 0.65f, -3.2f), new Vector3(3f, 1.3f, 0.7f), metal);
        CreateBlock("Entry_Barrier_Right", new Vector3(4.5f, 0.65f, -3.2f), new Vector3(3f, 1.3f, 0.7f), metal);
        CreateBlock("Supply_Shed", new Vector3(-8.5f, 1.5f, 7.8f), new Vector3(4f, 3f, 4f), concrete);
        CreateBlock("Watchtower_Base", new Vector3(9f, 1.1f, 9f), new Vector3(3f, 2.2f, 3f), metal);
        CreateBlock("Watchtower_Platform", new Vector3(9f, 3.5f, 9f), new Vector3(4f, 0.3f, 4f), accent);
        CreateBlock("Central_Cover", new Vector3(0f, 0.8f, 3f), new Vector3(4f, 1.6f, 0.8f), metal);
        CreateBlock("West_Cover", new Vector3(-5.5f, 0.7f, 1.5f), new Vector3(2.2f, 1.4f, 0.8f), metal);
        CreateBlock("East_Cover", new Vector3(5.5f, 0.7f, 1.5f), new Vector3(2.2f, 1.4f, 0.8f), metal);

        // Small orange warning markers make the entry readable.
        CreateBlock("Entry_Marker_Left", new Vector3(-2.6f, 0.35f, -2.7f), new Vector3(0.18f, 0.7f, 0.18f), accent);
        CreateBlock("Entry_Marker_Right", new Vector3(2.6f, 0.35f, -2.7f), new Vector3(0.18f, 0.7f, 0.18f), accent);
    }

    private static GameObject BuildPlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, -10f);
        player.tag = "Player";
        player.layer = 2; // Ignore Raycast: prevents the weapon ray from hitting the player's own capsule.
        Collider oldCollider = player.GetComponent<Collider>();
        Object.DestroyImmediate(oldCollider);

        CharacterController characterController = player.AddComponent<CharacterController>();
        characterController.height = 2f;
        characterController.radius = 0.45f;
        characterController.center = new Vector3(0f, 0f, 0f);
        player.AddComponent<SurvivalStats>();
        player.AddComponent<ThirdPersonPlayerController>();
        return player;
    }

    private static Camera BuildCamera(Transform target)
    {
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0f, 3.8f, -14.5f);
        cameraObject.transform.rotation = Quaternion.Euler(12f, 0f, 0f);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 65f;
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 250f;
        cameraObject.AddComponent<AudioListener>();

        ThirdPersonCameraFollow follow = cameraObject.AddComponent<ThirdPersonCameraFollow>();
        SetObjectReference(follow, "target", target);
        return camera;
    }

    private static void WirePlayerAndWeapon(GameObject player, Camera camera)
    {
        ThirdPersonPlayerController movement = player.GetComponent<ThirdPersonPlayerController>();
        SetObjectReference(movement, "cameraTransform", camera.transform);
        SetObjectReference(movement, "survivalStats", player.GetComponent<SurvivalStats>());

        SimpleWeapon weapon = player.AddComponent<SimpleWeapon>();
        SetObjectReference(weapon, "aimCamera", camera);
        SetObjectReference(weapon, "playerStats", player.GetComponent<SurvivalStats>());
    }

    private static List<SimpleEnemyAI> BuildEnemies(Transform player, Material enemyMaterial, Material concreteMaterial)
    {
        List<SimpleEnemyAI> enemies = new List<SimpleEnemyAI>();
        Vector3[] positions =
        {
            new Vector3(-6f, 1f, 7f),
            new Vector3(0f, 1f, 10f),
            new Vector3(6f, 1f, 7f)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy_" + (i + 1).ToString("00");
            enemy.transform.position = positions[i];
            enemy.GetComponent<Renderer>().sharedMaterial = enemyMaterial;
            SimpleEnemyAI ai = enemy.AddComponent<SimpleEnemyAI>();
            SetObjectReference(ai, "player", player);
            enemies.Add(ai);
        }

        return enemies;
    }

    private static void BuildPickups(Material healthMaterial, Material ammoMaterial)
    {
        GameObject health = GameObject.CreatePrimitive(PrimitiveType.Cube);
        health.name = "Health_Supply";
        health.transform.position = new Vector3(-5.5f, 0.55f, -0.3f);
        health.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        health.GetComponent<Renderer>().sharedMaterial = healthMaterial;
        SupplyPickup healthPickup = health.AddComponent<SupplyPickup>();
        SetEnumValue(healthPickup, "supplyType", 0);
        SetFloatValue(healthPickup, "healthAmount", 35f);

        GameObject ammo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ammo.name = "Ammo_Supply";
        ammo.transform.position = new Vector3(5.5f, 0.55f, -0.3f);
        ammo.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        ammo.GetComponent<Renderer>().sharedMaterial = ammoMaterial;
        SupplyPickup ammoPickup = ammo.AddComponent<SupplyPickup>();
        SetEnumValue(ammoPickup, "supplyType", 1);
        SetIntValue(ammoPickup, "ammoAmount", 24);
    }

    private static Canvas BuildCanvas()
    {
        GameObject canvasObject = new GameObject("GameCanvas", typeof(RectTransform), typeof(Canvas),
            typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;
        return canvas;
    }

    private static void BuildMobileControls(Transform canvas)
    {
        // Transparent look area is behind all action buttons in the hierarchy.
        GameObject lookArea = CreateImage(canvas, "MobileLookArea", new Color(1f, 1f, 1f, 0f),
            new Vector2(0.48f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
        lookArea.GetComponent<Image>().raycastTarget = true;
        lookArea.AddComponent<MobileLookAreaControl>();

        GameObject joystick = CreateImage(canvas, "MovementJoystick", new Color(0.08f, 0.10f, 0.11f, 0.55f),
            new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(175f, 160f), new Vector2(210f, 210f));
        Image joystickImage = joystick.GetComponent<Image>();
        joystickImage.raycastTarget = true;

        GameObject handle = CreateImage(joystick.transform, "JoystickHandle", new Color(0.88f, 0.91f, 0.92f, 0.72f),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(88f, 88f));
        MobileJoystickControl joystickControl = joystick.AddComponent<MobileJoystickControl>();
        SetObjectReference(joystickControl, "handle", handle.GetComponent<RectTransform>());

        CreateActionButton(canvas, "FireButton", "FIRE", new Vector2(1f, 0f),
            new Vector2(-150f, 175f), new Vector2(128f, 128f), MobileActionButton.ActionType.Fire, new Color(0.78f, 0.20f, 0.14f, 0.88f));
        CreateActionButton(canvas, "ReloadButton", "RELOAD", new Vector2(1f, 0f),
            new Vector2(-290f, 115f), new Vector2(105f, 72f), MobileActionButton.ActionType.Reload, new Color(0.22f, 0.27f, 0.30f, 0.9f));
        CreateActionButton(canvas, "JumpButton", "JUMP", new Vector2(1f, 0f),
            new Vector2(-300f, 235f), new Vector2(100f, 72f), MobileActionButton.ActionType.Jump, new Color(0.22f, 0.35f, 0.45f, 0.9f));
        CreateActionButton(canvas, "SprintButton", "SPRINT", new Vector2(0f, 0f),
            new Vector2(330f, 135f), new Vector2(110f, 68f), MobileActionButton.ActionType.Sprint, new Color(0.22f, 0.35f, 0.45f, 0.9f));
    }

    private static void CreateActionButton(Transform parent, string name, string label, Vector2 anchor,
        Vector2 position, Vector2 size, MobileActionButton.ActionType action, Color color)
    {
        GameObject buttonObject = CreateButton(parent, name, label, anchor, size, color);
        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        MobileActionButton actionButton = buttonObject.AddComponent<MobileActionButton>();
        SetEnumValue(actionButton, "action", (int)action);
    }

    private static GameObject CreatePanel(Transform parent, string name, Color color)
    {
        GameObject panel = CreateImage(parent, name, color, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        panel.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        panel.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        return panel;
    }

    private static GameObject CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 size, Color color)
    {
        GameObject buttonObject = CreateImage(parent, name, color, anchor, anchor, Vector2.zero, size);
        Button button = buttonObject.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = Color.Lerp(color, Color.white, 0.18f);
        colors.pressedColor = Color.Lerp(color, Color.black, 0.18f);
        button.colors = colors;

        GameObject textObject = CreateText(buttonObject.transform, "Label", label, 22,
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero,
            TextAnchor.MiddleCenter, Color.white);
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        textObject.GetComponent<Text>().fontStyle = FontStyle.Bold;
        return buttonObject;
    }

    private static GameObject CreateSlider(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 position, Vector2 size, Color fillColor)
    {
        GameObject root = new GameObject(name, typeof(RectTransform), typeof(Slider));
        root.transform.SetParent(parent, false);
        RectTransform rootRect = root.GetComponent<RectTransform>();
        rootRect.anchorMin = anchorMin;
        rootRect.anchorMax = anchorMax;
        rootRect.anchoredPosition = position;
        rootRect.sizeDelta = size;

        GameObject background = CreateImage(root.transform, "Background", new Color(0.04f, 0.05f, 0.06f, 0.9f),
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        RectTransform backgroundRect = background.GetComponent<RectTransform>();
        backgroundRect.offsetMin = Vector2.zero;
        backgroundRect.offsetMax = Vector2.zero;

        GameObject fillArea = new GameObject("Fill Area", typeof(RectTransform));
        fillArea.transform.SetParent(root.transform, false);
        RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(3f, 3f);
        fillAreaRect.offsetMax = new Vector2(-3f, -3f);

        GameObject fill = CreateImage(fillArea.transform, "Fill", fillColor,
            Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        Slider slider = root.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;
        slider.wholeNumbers = false;
        slider.interactable = false;
        slider.fillRect = fillRect;
        slider.targetGraphic = fill.GetComponent<Image>();
        slider.direction = Slider.Direction.LeftToRight;
        return root;
    }

    private static GameObject CreateText(Transform parent, string name, string value, int fontSize,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size, TextAnchor alignment, Color color)
    {
        GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        textObject.transform.SetParent(parent, false);
        RectTransform rect = textObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Text text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = color;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return textObject;
    }

    private static GameObject CreateImage(Transform parent, string name, Color color,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 position, Vector2 size)
    {
        GameObject imageObject = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        imageObject.transform.SetParent(parent, false);
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        Image image = imageObject.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = true;
        return imageObject;
    }

    private static void CreateEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() != null)
            return;

        GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        Undo.RegisterCreatedObjectUndo(eventSystem, "Create EventSystem");
    }

    private static void CreateLighting()
    {
        GameObject lightObject = new GameObject("Directional Light", typeof(Light));
        Light light = lightObject.GetComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.15f;
        light.color = new Color(1f, 0.91f, 0.78f);
        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.38f, 0.40f, 0.43f);
    }

    private static GameObject CreateBlock(string name, Vector3 position, Vector3 scale, Material material)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = name;
        block.transform.position = position;
        block.transform.localScale = scale;
        block.GetComponent<Renderer>().sharedMaterial = material;
        return block;
    }

    private static Material CreateMaterial(string name, Color color)
    {
        string path = "Assets/Scenes/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Standard");
            if (shader == null)
                shader = Shader.Find("Universal Render Pipeline/Lit");
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        material.name = name;
        material.color = color;
        EditorUtility.SetDirty(material);
        return material;
    }

    private static void EnsureFolder(string parent, string folderName)
    {
        string path = parent + "/" + folderName;
        if (!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder(parent, folderName);
    }

    private static void AddSceneToBuildSettings(string path)
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].path == path)
            {
                scenes[i] = new EditorBuildSettingsScene(path, true);
                EditorBuildSettings.scenes = scenes.ToArray();
                return;
            }
        }

        scenes.Add(new EditorBuildSettingsScene(path, true));
        EditorBuildSettings.scenes = scenes.ToArray();
    }

    private static void AddOutline(Text text, Color color)
    {
        if (text == null)
            return;

        Outline outline = text.gameObject.AddComponent<Outline>();
        outline.effectColor = color;
        outline.effectDistance = new Vector2(1.5f, -1.5f);
    }

    private static void SetObjectReference(Object target, string propertyName, Object value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(propertyName);
        if (property == null)
        {
            Debug.LogWarning("InitialSceneBuilder: Property not found: " + propertyName + " on " + target.GetType().Name);
            return;
        }

        property.objectReferenceValue = value;
        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void SetEnemyArray(MissionObjectiveController target, SimpleEnemyAI[] enemies)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty("objectiveEnemies");
        property.arraySize = enemies.Length;
        for (int i = 0; i < enemies.Length; i++)
            property.GetArrayElementAtIndex(i).objectReferenceValue = enemies[i];
        serialized.ApplyModifiedPropertiesWithoutUndo();

        SetIntValue(target, "requiredKills", enemies.Length);
    }

    private static void SetEnumValue(Object target, string propertyName, int value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(propertyName);
        if (property != null)
        {
            property.enumValueIndex = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    private static void SetFloatValue(Object target, string propertyName, float value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(propertyName);
        if (property != null)
        {
            property.floatValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    private static void SetIntValue(Object target, string propertyName, int value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty property = serialized.FindProperty(propertyName);
        if (property != null)
        {
            property.intValue = value;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    private static void AddPersistentRestart(Button button, MissionObjectiveController controller)
    {
        if (button == null || controller == null)
            return;

        UnityEventTools.AddPersistentListener(button.onClick, controller.RestartMission);
        UnityEventTools.SetPersistentListenerState(button.onClick, 0, UnityEventCallState.RuntimeOnly);
    }

    private static void AddPersistentRestart(Button button, GameOverController controller)
    {
        if (button == null || controller == null)
            return;

        UnityEventTools.AddPersistentListener(button.onClick, controller.RestartMission);
        UnityEventTools.SetPersistentListenerState(button.onClick, 0, UnityEventCallState.RuntimeOnly);
    }
}
