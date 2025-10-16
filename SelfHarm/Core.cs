using BoneLib;
using HarmonyLib;
using Il2CppJetBrains.Annotations;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Warehouse;
using LabFusion.Data;
using LabFusion.Player;
using LabFusion.Utilities;
using MelonLoader;
using SelfHarm.Fusion;
using System;
using UnityEngine;
using Page = BoneLib.BoneMenu.Page;


[assembly: MelonInfo(typeof(SelfHarm.Core), "SelfHarm", "1.0.1", "notnotnotswipez", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace SelfHarm
{

    public class Core : MelonMod
    {
        public RigManager fusionrig;

        public override void OnInitializeMelon()
        {
            LoadModule();
            
            base.LoggerInstance.Msg("Initialized.");
            SetupMelonPrefs();
            CreateBonemenu();
        }

        private static void LoadModule()
        {
            LabFusion.SDK.Modules.ModuleManager.RegisterModule<MyModule>();
        }

        // full credits to notnotnotswipez for the code, credits to me (MajedCT) for spending an hour adding a toggle button >:D
        public static void AddImpactProperties(RigManager rigManager)
        {
            
            PhysicsRig physicsRig = rigManager.physicsRig;
            rigManager.health._testVisualDamage = true;
            DataCardReference<SurfaceDataCard> surfaceDataCard = new DataCardReference<SurfaceDataCard>("SLZ.Backlot.SurfaceDataCard.Blood");
            foreach (Rigidbody rigidbody in physicsRig.GetComponentsInChildren<Rigidbody>())
            {
                bool flag = rigidbody.GetComponent<ImpactProperties>();
                if (!flag)
                {
                    GameObject gameObject = rigidbody.gameObject;
                    bool flag2 = gameObject.GetInstanceID() == physicsRig.feet.GetInstanceID();
                    if (!flag2)
                    {
                        bool flag3 = gameObject.GetInstanceID() == physicsRig.knee.GetInstanceID();
                        if (!flag3)
                        {
                            ImpactProperties impactProperties = rigidbody.gameObject.AddComponent<ImpactProperties>();
                            impactProperties.SurfaceDataCard = surfaceDataCard;
                            impactProperties.decalType = (ImpactProperties.DecalType)(-1);
                        }
                    }
                }
            }
        }
        public static void DestroyImpactProperties(RigManager rigManager)
        {
            
            PhysicsRig physicsRig = rigManager.physicsRig;
            rigManager.health._testVisualDamage = true;
            DataCardReference<SurfaceDataCard> surfaceDataCard = new DataCardReference<SurfaceDataCard>("SLZ.Backlot.SurfaceDataCard.Blood");
            foreach (Rigidbody rigidbody in physicsRig.GetComponentsInChildren<Rigidbody>())
            {
                bool flag = rigidbody.GetComponent<ImpactProperties>();
                if (flag)
                {
                    // DESTROY THEM DESTROY THE IMPACT PROPERTIES DIE
                    ImpactProperties.Destroy(rigidbody.GetComponent<ImpactProperties>());

                }
            }
        }


        [HarmonyPatch(typeof(UIRig), "Awake")]
        public class UIRigAwakePatch
        {
            
            public static void Postfix()
            {
                var playerRig = PlayerRefs.Instance.PlayerRigManager;
                var localRig = RigData.Refs.RigManager;
                if (isEnabled)
                {
                    MelonLogger.Msg("[Self Harm] Mod is already enabled, Adding impact properties");
                    AddImpactProperties(playerRig);
                }
            }
        }

        

        
        
        public static MelonPreferences_Category MelonPrefCategory
        {
            get; set;
        }
        public static MelonPreferences_Entry<bool> ModEnabledPref
        {
            get; set;
        }
        public static bool isEnabled
        {
            get; set;
        }


        public static void SetupMelonPrefs()
        {
            MelonPrefCategory = MelonPreferences.CreateCategory("Self Harm");
            ModEnabledPref = MelonPrefCategory.CreateEntry("isEnabled", true);
            isEnabled = ModEnabledPref.Value;


        }
        public static void CreateBonemenu()
        {
            Page mainPage = Page.Root.CreatePage("Self Harm", Color.red);
            mainPage.CreateBool("Mod Toggle", Color.yellow, ModEnabledPref.Value, OnToggle);
        }

        public static void OnToggle(bool value)
        {
            var playerRig = PlayerRefs.Instance.PlayerRigManager;
            var localRig = RigData.Refs.RigManager;
            isEnabled = value;

            if (isEnabled)
            {
                MelonLogger.Msg("[Self Harm] Mod Enabled, Adding Impact Properties.");
                AddImpactProperties(playerRig);
            }
            else if(!isEnabled)
            {
                MelonLogger.Msg("[Self Harm] Mod Disabled, Removing Impact Properties.");
                DestroyImpactProperties(playerRig);
            }

            
            ModEnabledPref.Value = value;
            MelonPrefCategory.SaveToFile(true);
        }
    }
}
