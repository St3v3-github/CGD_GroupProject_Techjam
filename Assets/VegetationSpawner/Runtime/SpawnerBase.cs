﻿// Vegetation Spawner by Staggart Creations http://staggart.xyz
// Copyright protected under Unity Asset Store EULA

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace sc.terrain.vegetationspawner
{
    [AddComponentMenu("")] //Hide
    public class SpawnerBase : MonoBehaviour
    {
        private void OnValidate()
        {
            if (terrainSettings == null) terrainSettings = new TerrainSettings();
        }
        /// <summary>
        /// Global seed that affects all vegetation spawning
        /// </summary>
        public int seed = 0;

        public List<Terrain> terrains = new List<Terrain>();
        [Tooltip("[Editor only] Opt to only affect terrains that are visible in the scene-view. Use this to speed up the spawning process." +
                 "\n\nNote that the visibility check is not completely accurate, as it is based on bounding boxes.")]
        public bool visibleTerrainsOnly;
        
        [Serializable]
        public class VegetationPrefab
        {
            public bool enabled = true;
            public string name = "VegetationItem";

            public int seed;
            [Range(0f, 100f)]
            public float probability;
            public bool collisionCheck;
            public bool rejectUnderwater;
            public Vector2 heightRange = new Vector2(0f, 1000f);
            public Vector2 slopeRange = new Vector2(0f, 60f);
            public Vector2 curvatureRange = new Vector2(0f, 1f);

            public List<TerrainLayerMask> layerMasks = new List<TerrainLayerMask>();
            public int instanceCount;
        }

        [Serializable]
        public class TreeType : VegetationPrefab
        {
            [NonSerialized]
            public List<Vector3> spawnPoints = new List<Vector3>();

            [SerializeField]
            public List<TreePrefab> prefabs = new List<TreePrefab>();

            [Range(1f, 25f)]
            public float distance = 5f;
           
            public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
            public float sinkAmount = 0f;

            public static TreeType New()
            {
                TreeType t = new TreeType();

                //Constructor for inherent variables
                t.probability = 10f;

                return t;
            }
            
            public static TreeType New(GameObject initialPrefab)
            {
                TreeType t = New();
                t.seed = Random.Range(0, 9999);

                //Add an initial prefab on construction
                TreePrefab p = new TreePrefab();
                p.prefab = initialPrefab;
                t.prefabs.Add(p);

                if(initialPrefab) t.name = initialPrefab.name;

                return t;
            }

        }
        [SerializeField]
        public List<TreeType> treeTypes = new List<TreeType>();

        [Serializable]
        public class TreePrefab
        {
            //Index of tree prototype in TerrainData
            public int index;
            [Range(0f, 100f)]
            public float probability = 100f;
            public GameObject prefab;
        }

        public enum GrassType
        {
            Mesh,
            Texture
        }

        [Serializable]
        public class GrassPrefab : VegetationPrefab
        {
            public int index;
            public GrassType type = GrassType.Texture;
            [Tooltip("When enabled, orients the grass towards the camera on the Y axis")]
            public bool renderAsBillboard;
            public GameObject prefab;
            
            #if UNITY_2021_2_OR_NEWER
            public bool gpuInstancing = true;
            [Range(0f, 100f)]
            public float holeEdgePadding = 2f;
            #endif
            
            #if UNITY_2022_2_OR_NEWER
            [Range(0f, 100f)]
            public float alignToGround = 100f;
            [Range(0f,100f)]
            public float positionJitter;
            public bool useDensityScaling = true;
            public float density;
            #endif
            
            public Texture2D billboard;

            public Color mainColor = Color.white;
            //Typo! Has nothing to do with dairy :)
            [FormerlySerializedAs("secondairyColor")]
            public Color secondaryColor = Color.white;
            public bool linkColors = true;
            
            public Vector2 minMaxHeight;
            public Vector2 minMaxWidth;
            [Range(0.01f, 0.5f)]
            [Tooltip("Grass size and color variation is controlled by an internal noise value. This controls the tiling size of the noise")]
            public float noiseSize = 0.1f;

            public GrassPrefab()
            {
                probability = 25f;
                heightRange = new Vector2(0f, 1000f);
                slopeRange = new Vector2(0f, 60f);
                minMaxHeight = new Vector2(0.5f, 1f);
                minMaxWidth = new Vector2(0.8f, 1.2f);
                noiseSize = 0.1f;
                #if UNITY_2022_2_OR_NEWER
                alignToGround = 100f;
                #endif
            }
            
            public static GrassPrefab Duplicate(GrassPrefab source)
            {
                GrassPrefab dest = new GrassPrefab();

                dest.enabled = source.enabled;
                dest.name = source.name;
                
                dest.type = source.type;
                dest.prefab = source.prefab;
                #if UNITY_2021_2_OR_NEWER
                dest.gpuInstancing = source.gpuInstancing;
                dest.holeEdgePadding = source.holeEdgePadding;
                #endif
                dest.billboard = source.billboard;
                dest.renderAsBillboard = source.renderAsBillboard;
                dest.noiseSize = source.noiseSize;

                dest.mainColor = source.mainColor;
                dest.secondaryColor = source.secondaryColor;
                dest.linkColors = source.linkColors;

                dest.minMaxHeight = source.minMaxHeight;
                dest.minMaxWidth = source.minMaxWidth;

                //Base
                dest.seed = source.seed;
                dest.probability = source.probability;
                dest.heightRange = source.heightRange;
                dest.slopeRange = source.slopeRange;
                dest.curvatureRange = source.curvatureRange;
                
                #if UNITY_2022_2_OR_NEWER
                dest.useDensityScaling = source.useDensityScaling;
                dest.alignToGround = source.alignToGround;
                dest.positionJitter = source.positionJitter;
                #endif

                dest.layerMasks = new List<TerrainLayerMask>(source.layerMasks);
                for (int i = 0; i < dest.layerMasks.Count; i++)
                {
                    dest.layerMasks[i] = new TerrainLayerMask(source.layerMasks[i].name, source.layerMasks[i].layerID, source.layerMasks[i].threshold);
                }
            
                dest.collisionCheck = source.collisionCheck;
                dest.rejectUnderwater = source.rejectUnderwater;

                return dest;
            }
        }
        public List<GrassPrefab> grassPrefabs = new List<GrassPrefab>();

        [Serializable]
        public class TerrainLayerMask
        {
            public string name;
            public int layerID;
            [Range(0f, 1f)]
            public float threshold = 0f;

            public TerrainLayerMask() {}

            //Cloning
            public TerrainLayerMask(string name, int layerID, float threshold)
            {
                this.name = name;
                this.layerID = layerID;
                this.threshold = threshold;
            }
        }

        [Serializable]
        public class TerrainSettings
        {
            [Header("Rendering")]
            public bool drawTreesAndFoliage = true;

            [Header("Trees")]
            [FormerlySerializedAs("perservePrefabLayer")]
            public bool preservePrefabLayer = true;
            public bool treeLightProbes = false;
            [Range(0f, 5000f)]
            public float treeDistance = 1000f;
            [Range(5f, 2000f)]
            public float billboardStart = 300f;
            public int maxMeshTrees = 50;

            [Header("Grass")]
            #if UNITY_2022_2_OR_NEWER
            public DetailScatterMode detailScattermode = DetailScatterMode.CoverageMode;
            #endif
            public float grassDistance = 200f;
            [Range(0f, 1f)]
            public float grassDensity = 1f;
            
            [Header("Grass wind")]
            [Range(0f, 1f)]
            public float windStrength = 0.15f;
            [Range(0f, 5f)]
            public float windSpeed = 1f;
            [Range(0.1f, 10f)]
            public float windFrequency = 2f;
            public Color wintTint = Color.white;
        }
        public TerrainSettings terrainSettings = new TerrainSettings();

        [ContextMenu("Randomize seed")]
        public void RandomizeSeed()
        {
            seed = Random.Range(0, 9999);
        }

        public void InitializeSeed(int start = 0)
        {
            Random.InitState(start + seed);
        }

        private static int recursionCounter;

        public static TreePrefab GetProbableTree(TreeType treeType)
        {
            recursionCounter = 0;

            return PickTreeRecursive(treeType);
        }

        //Chooses a prefab based on probability, recursively executed until succesful
        private static TreePrefab PickTreeRecursive(TreeType treeType)
        {
            if (treeType.prefabs.Count == 0) return null;

            TreePrefab p = treeType.prefabs[Random.Range(0, treeType.prefabs.Count)];

            //If prefabs have an extremely low probabilty, give up after 4 attempts
            if (recursionCounter >= 4) return null;

            if ((Random.value * 100f) <= p.probability)
            {
                //Debug.Log("<color=green>" + p.prefab.name + " passed probability check..</color>");
                return p;
            }

            //Debug.Log("<color=red>" + p.prefab.name + " failed probability check, trying another...</color>");

            recursionCounter++;

            //Note: It's possible for the next candidate to be the one that just failed
            return PickTreeRecursive(treeType);
        }

        public void CopySettingsToTerrains()
        {
            foreach (Terrain t in terrains)
            {
                t.drawTreesAndFoliage = terrainSettings.drawTreesAndFoliage;
                t.treeMaximumFullLODCount = terrainSettings.maxMeshTrees;
                
                t.preserveTreePrototypeLayers = terrainSettings.preservePrefabLayer;
#if UNITY_EDITOR
                t.bakeLightProbesForTrees = terrainSettings.treeLightProbes;
#endif
                t.treeBillboardDistance = terrainSettings.billboardStart;
                t.treeDistance = terrainSettings.treeDistance;

                #if UNITY_2022_2_OR_NEWER
                t.terrainData.SetDetailScatterMode(terrainSettings.detailScattermode);
                #endif
                
                t.detailObjectDistance = terrainSettings.grassDistance;
                t.detailObjectDensity = terrainSettings.grassDensity;
                
                t.terrainData.wavingGrassAmount = terrainSettings.windStrength;
                t.terrainData.wavingGrassStrength = terrainSettings.windSpeed;
                t.terrainData.wavingGrassSpeed = terrainSettings.windFrequency;
                t.terrainData.wavingGrassTint = terrainSettings.wintTint;
            }
        }
        
        public static bool HasMissingTerrain(List<Terrain> terrains)
        {
            bool isMissing = false;

            for (int i = 0; i < terrains.Count; i++)
            {
                if (terrains[i] == null) isMissing = true;
            }

            return isMissing;
        }
        
        protected List<Terrain> GetTargetTerrains()
        {
            if (visibleTerrainsOnly == false) return terrains;
            
            Plane[] frustumPlanes = new Plane[6];
            List<Terrain> targetTerrains = new List<Terrain>();
            
            #if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView.camera) GeometryUtility.CalculateFrustumPlanes(UnityEditor.SceneView.lastActiveSceneView.camera, frustumPlanes);
            else return null;
            #else
            if (Camera.main) GeometryUtility.CalculateFrustumPlanes(Camera.main, frustumPlanes);
            else return null;
            #endif
            
            foreach (Terrain terrain in terrains)
            {
                if(terrain == null) continue;

                Bounds bounds = terrain.terrainData.bounds;
                //Bounds is in local-space, shift it to virtually place it in world-space
                bounds.center += terrain.GetPosition();

                if (GeometryUtility.TestPlanesAABB(frustumPlanes, bounds))
                {
                    //Debug.Log($"{terrain.name} visible");
                    targetTerrains.Add(terrain);
                }
            }

            return targetTerrains;
        }

        //Returns the splatmap index for a given terrain layer
        protected static int GetSplatmapID(int layerID)
        {
            if (layerID > 11) return 3;
            if (layerID > 7) return 2;
            if (layerID > 3) return 1;

            return 0;
        }
    }
}