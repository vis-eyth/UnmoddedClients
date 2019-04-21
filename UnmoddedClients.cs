using BepInEx;

namespace UnmoddedClients {
    [BepInPlugin("com.viseyth.ror2.unmoddedclients", "UnmoddedClient", "1.0.0")]
    public class UnmoddedClients : BaseUnityPlugin {
        private void Awake() {
            On.RoR2.SteamworksLobbyManager.OnLobbyChanged += orig => {
                orig();

                var instance = RoR2.RoR2Application.instance;
                if (instance == null)
                    return;

                var client = instance.steamworksClient;
                if (client == null)
                    return;
                
                RoR2.RoR2Application.instance.steamworksClient.Lobby.CurrentLobbyData.SetData("build_id", TextSerialization.ToStringInvariant(client.BuildId));
            };
        }
    }
}