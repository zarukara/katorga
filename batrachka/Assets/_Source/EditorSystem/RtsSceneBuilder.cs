using System.IO;
using CoreSystem;
using PlayerSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using ViewSystem;

namespace EditorSystem
{
    public static class RtsSceneBuilder
    {
        public const string ScenePath = "Assets/_Presentation/Scenes/RtsTraining.unity";
        private const string MaterialsPath = "Assets/_Presentation/Materials/";

        [MenuItem("ECS Practice/Rebuild Demo Scene")]
        public static void Rebuild()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            Directory.CreateDirectory(MaterialsPath);
            Directory.CreateDirectory("Assets/_Presentation/Prefabs");
            Directory.CreateDirectory("Assets/_Presentation/Scenes");
            AssetDatabase.Refresh();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            var layers = tagManager.FindProperty("layers");
            layers.GetArrayElementAtIndex(RtsPointerInput.GroundLayer).stringValue = "RTS Ground";
            layers.GetArrayElementAtIndex(RtsPointerInput.SoldierLayer).stringValue = "RTS Soldier";
            tagManager.ApplyModifiedPropertiesWithoutUndo();

            var groundMaterial = MakeMaterial("Ground", new Color(0.48f, 0.48f, 0.48f));
            var soldierMaterial = MakeMaterial("Soldier", new Color(0.85f, 0.85f, 0.85f));
            var selectedMaterial = MakeMaterial("Selected", new Color(0.2f, 0.85f, 0.25f));
            var selectionLine = MakeMaterial("SelectionLine", new Color(0.1f, 0.65f, 0.15f), true);
            var directionLine = MakeMaterial("DirectionLine", new Color(0.08f, 0.08f, 0.08f), true);

            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4.4f, 1, 3.2f);
            ground.layer = RtsPointerInput.GroundLayer;
            ground.GetComponent<Renderer>().sharedMaterial = groundMaterial;

            var soldier = new GameObject("Soldier");
            var view = soldier.AddComponent<SoldierView>();
            var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.name = "Capsule";
            capsule.transform.SetParent(soldier.transform, false);
            capsule.transform.localPosition = Vector3.up * 0.9f;
            capsule.transform.localScale = new Vector3(0.8f, 0.9f, 0.8f);
            capsule.layer = RtsPointerInput.SoldierLayer;
            capsule.GetComponent<Renderer>().sharedMaterial = soldierMaterial;
            var forward = MakeLine("Facing Direction", soldier.transform, directionLine, 5);
            forward.useWorldSpace = false;
            forward.SetPositions(new[]
            {
                new Vector3(0, 0.04f, 0.35f), new Vector3(0, 0.04f, 1.1f),
                new Vector3(-0.16f, 0.04f, 0.87f), new Vector3(0, 0.04f, 1.1f),
                new Vector3(0.16f, 0.04f, 0.87f)
            });
            view.Configure(capsule.GetComponent<Renderer>(), soldierMaterial, selectedMaterial, forward);
            var prefab = PrefabUtility.SaveAsPrefabAsset(soldier, "Assets/_Presentation/Prefabs/Soldier.prefab").GetComponent<SoldierView>();
            Object.DestroyImmediate(soldier);

            var camera = new GameObject("Main Camera", typeof(Camera), typeof(CameraFraming)).GetComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.rotation = Quaternion.Euler(35.26439f, 45, 0);
            camera.transform.position = -camera.transform.forward * 55;
            camera.orthographic = true;
            camera.orthographicSize = 17.5f;
            camera.nearClipPlane = 0.3f;
            camera.farClipPlane = 150;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.35f, 0.35f, 0.35f);
            camera.allowHDR = false;

            var light = new GameObject("Directional Light", typeof(Light)).GetComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
            light.intensity = 1;
            light.shadows = LightShadows.Hard;
            light.shadowStrength = 0.4f;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.65f, 0.65f, 0.65f);
            RenderSettings.skybox = null;
            RenderSettings.fog = false;

            var feedback = new GameObject("Selection Feedback").AddComponent<SelectionFeedback>();
            var rectangle = MakeLine("Selection Rectangle", feedback.transform, selectionLine, 4);
            rectangle.loop = true;
            var facingPreview = MakeLine("Facing Preview", feedback.transform, directionLine, 5);
            feedback.Configure(rectangle, facingPreview);
            var units = new GameObject("Units").transform;
            var bootstrap = new GameObject("ECS Bootstrap").AddComponent<RtsBootstrap>();
            bootstrap.Configure(prefab, units, camera, feedback);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath);
            RtsWebGlBuild.Configure();
            AssetDatabase.SaveAssets();
            Debug.Log("RTS_SCENE_READY: " + ScenePath);
        }

        private static Material MakeMaterial(string name, Color color, bool unlit = false)
        {
            string path = MaterialsPath + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find(unlit ? "Unlit/Color" : "Standard"));
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            if (!unlit) material.SetFloat("_Glossiness", 0);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static LineRenderer MakeLine(string name, Transform parent, Material material, int points)
        {
            var line = new GameObject(name).AddComponent<LineRenderer>();
            line.transform.SetParent(parent, false);
            line.sharedMaterial = material;
            line.useWorldSpace = true;
            line.widthMultiplier = 0.055f;
            line.positionCount = points;
            line.shadowCastingMode = ShadowCastingMode.Off;
            line.receiveShadows = false;
            line.enabled = false;
            return line;
        }
    }
}
