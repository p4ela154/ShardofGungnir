using BepInEx;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.GUI;
using Jotunn.Managers;
using Jotunn.Utils;
//using JotunnModExample.ConsoleCommands;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Jotunn.Logger;
using static EffectList;

//Thanks to MarcoPogo!

namespace ShardofGungnir
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    [BepInDependency("cinnabun.backpacks-v1.0.0", BepInDependency.DependencyFlags.SoftDependency)]
    internal class ShardofGungnir : BaseUnityPlugin
    {
        // BepInEx' plugin metadata
        public const string PluginGUID = "com.jotunn.ShardofGungnir";
        public const string PluginName = "ShardofGungnir";
        public const string PluginVersion = "1.0.0";


        private void Awake()
        {  
            AddShardofGungnir(); // Load, create and init your custom mod stuff
            PrefabManager.OnVanillaPrefabsAvailable += AddShardofGungnir; // Add custom items cloned from vanilla items
        }
        // Implementation of cloned items
        private void AddShardofGungnir()
        {
            try
            {
                CustomItem CI = new CustomItem("ShardofGungnir", "SpearElderbark");
                CustomItem CIP = new CustomItem("ShardofGungnir_projectile", "ancientbarkspear_projectile");
                //  Try using TargetParentPath = "attach"
                KitbashManager.Instance.AddKitbash(CI.ItemDrop.gameObject, new KitbashConfig
                {
                    KitbashSources = new List<KitbashSourceConfig>
                    {
                        new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "Sparcs",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(0.3f, 0.3f, 0.3f)
                        }
                        ,new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "sfx",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(0.2f, 0.2f, 0.2f)

                        }
                        ,new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "Point light (1)",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(0.4f, 0.4f, 0.4f)
                        }
                    }

                }
                ) ;
                KitbashSourceConfig CIK = ;
                KitbashManager.Instance.AddKitbash(CIP.ItemPrefab, new KitbashConfig
                {
                    Layer = "piece",
                    KitbashSources = new List<KitbashSourceConfig>
                    {
                        new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "Sparcs",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(1f, 1f, 1f)
                        }
                        ,new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "sfx",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(0.7f, 0.7f, 0.7f)

                        }
                        ,new KitbashSourceConfig
                        {
                            Name = "SE",
                            SourcePrefab = "fx_Lightning",
                            SourcePath = "Point light (1)",
                            Position = new Vector3(0, 0, 0.7f),
                            Rotation = Quaternion.Euler(-0, 0, 0),
                            Scale = new Vector3(0.4f, 0.4f, 0.4f)
                        }
                    }
                });
                ItemManager.Instance.AddItem(CI);
                var itemDrop_ = CI.ItemDrop;
                var itemDrop_Projectile = CIP.ItemPrefab;
                //GameObject beech = PrefabManager.Instance.GetPrefab("SpearBronze");
                itemDrop_.m_itemData.m_shared.m_name = "$item_ShardofGungnir";
                itemDrop_.m_itemData.m_shared.m_description = "$item_ShardofGungnir_desc";
                itemDrop_.m_itemData.m_shared.m_secondaryAttack.m_attackProjectile = itemDrop_Projectile;
                //CI.ItemPrefab.gameObject.GetComponent<"attach"> = itemDrop_Projectile;
                CIP.ItemDrop.m_autoPickup = true;
                itemDrop_.m_itemData.m_shared.m_damages.m_lightning = 40f;
                RecipeShardofGungnir(itemDrop_);
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while adding cloned item: {ex.Message}");
            }
            finally
            {
                // You want that to run only once, Jotunn has the item cached for the game session
                PrefabManager.OnVanillaPrefabsAvailable -= AddShardofGungnir; 
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Implementation of assets via using manual recipe creation and prefab cache's
        private void RecipeShardofGungnir(ItemDrop itemDrop_)
        {
            // Create and add a recipe for the copied item
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.name = "Recipe_ShardofGungnir";
            recipe.m_item = itemDrop_;
            recipe.m_craftingStation = PrefabManager.Cache.GetPrefab<CraftingStation>("piece_workbench"); //forge
            recipe.m_repairStation = PrefabManager.Cache.GetPrefab<CraftingStation>("piece_workbench"); //forge
            recipe.m_minStationLevel = 2;
            recipe.m_resources = new Piece.Requirement[]
            {
                new Piece.Requirement()
                {
                    //m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("FineWood"),
                    m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Wood"),
                    m_amount = 1
                },
                new Piece.Requirement()
                {
                    //m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("SurtlingCore"),
                    m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Stone"),
                    m_amount = 1
                }
            };
            // Since we got the vanilla prefabs from the cache, no referencing is needed
            CustomRecipe CR = new CustomRecipe(recipe, fixReference: false, fixRequirementReferences: false);
            ItemManager.Instance.AddRecipe(CR);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
    }
}
