/**
 * MIT License
 *
 * Copyright (c) 2019 Vis'Eyth (viseyth#3934)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;

namespace UnmoddedClients {
    [BepInPlugin("com.viseyth.ror2.unmoddedclients", "UnmoddedClients", "1.1.2")]
    public class UnmoddedClients : BaseUnityPlugin
    {
        private static string _buildId;
        private static ConfigWrapper<string> _buildIdPreset;
        
        private void Awake() {
            On.RoR2.SteamworksLobbyManager.OnLobbyChanged += orig => {
                orig();
                
                RoR2Application.instance.steamworksClient.Lobby.CurrentLobbyData.SetData("build_id", _buildId);
            };
            On.RoR2.Console.Awake += (orig, self) => {
                CommandHelper.RegisterCommands(self);
                orig(self);
            };
            
            _buildIdPreset = Config.Wrap("Game"
                , "BuildIdPreset"
                , "The preset Build-Id for the server at launch. Use mod, for modded"
                + ", or steam for the real id. You can set any other value to set your server buildid to that value, too."
                , "steam");

            On.RoR2.Networking.GameNetworkManager.CreateP2PConnectionWithPeer += (orig, self, peer) =>
            {
                RoR2Application.instance.steamworksClient.Lobby.CurrentLobbyData.SetData("build_id", _buildId);
                orig(self, peer);
            };
            
            On.RoR2.RoR2Application.Awake += (orig, self) =>
            {
                orig(self);
                switch (_buildIdPreset.Value)
                {
                    case "mod":
                        SetBuildIdMod(new ConCommandArgs());
                        break;
                    case "steam":
                        SetBuildIdSteam(new ConCommandArgs());
                        break;
                    default:
                        _buildId = _buildIdPreset.Value;
                        break;
                }
            };
        }
        
        // ReSharper disable UnusedMember.Local UnusedParameter.Local
        [ConCommand(commandName = "build_id_mod", flags = ConVarFlags.SenderMustBeServer, helpText = "Sets the buildid to MOD.")]
        private static void SetBuildIdMod(ConCommandArgs args)
        {
            _buildId = "MOD";
            
            var instance = RoR2Application.instance;
            if (instance == null)
                throw new ConCommandException("RoR2Application is null.");
            var client = instance.steamworksClient;
            if (client == null)
                throw new ConCommandException("SteamworksClient is null");
            
            client.Lobby?.CurrentLobbyData?.SetData("build_id", _buildId);
            Debug.Log($"BuildId set to {_buildId}");
        }
        [ConCommand(commandName = "build_id_steam", flags = ConVarFlags.SenderMustBeServer, helpText = "Sets buildid to the steam build id, as if your client was unmodded.")]
        private static void SetBuildIdSteam(ConCommandArgs args)
        {
            var instance = RoR2Application.instance;
            if (instance == null)
                throw new ConCommandException("RoR2Application is null.");
            var client = instance.steamworksClient;
            if (client == null)
                throw new ConCommandException("SteamworksClient is null");
            
            _buildId = TextSerialization.ToStringInvariant(client.BuildId);
            client.Lobby?.CurrentLobbyData?.SetData("build_id", _buildId);
            Debug.Log($"BuildId set to {_buildId}");
        }
        [ConCommand(commandName = "build_id_custom", flags = ConVarFlags.SenderMustBeServer, helpText = "Sets the buildid to a custom value. One argument.")]
        private static void SetBuildIdCustom(ConCommandArgs args)
        {
            args.CheckArgumentCount(1);
            _buildId = args[0];
            
            var instance = RoR2Application.instance;
            if (instance == null)
                throw new ConCommandException("RoR2Application is null.");
            var client = instance.steamworksClient;
            if (client == null)
                throw new ConCommandException("SteamworksClient is null");

            client.Lobby?.CurrentLobbyData?.SetData("build_id", _buildId);
            Debug.Log($"BuildId set to {_buildId}");
        }
        // ReSharper restore UnusedMember.Local UnusedParameter.Local
    }
}