using LabFusion.Data;
using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.Player;
using LabFusion.SDK.Modules;
using LabFusion.Utilities;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;

namespace SelfHarm.Fusion
{
    public class MyModule : Module
    {
        public override string Name => "Self Harm";

        public override string Author => "MajedCT";

        public override Version Version => new(1, 0, 0);

        public NetworkLayer CurrentLayer => NetworkLayerManager.Layer;

        public override ConsoleColor Color => ConsoleColor.Red;

        

        protected override void OnModuleRegistered()
        {
            MelonEvents.OnUpdate.Subscribe(Update);

            MultiplayerHooking.OnPlayerJoined += MultiplayerHooking_OnPlayerJoined;
        }

        private void MultiplayerHooking_OnPlayerJoined(PlayerID playerId)
        {
            
        }

        private void Update()
        {
            if (CurrentLayer == null)
                return;

            Core.AddImpactProperties(RigData.Refs.RigManager);
            
        }
    }
}
