/**
 * MusicMod - A Valheim Mod
 * Copyright (C) 2022 Joey Parrish
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

[assembly: AssemblyTitle("MusicMod")]
[assembly: AssemblyProduct("MusicMod")]
[assembly: AssemblyCopyright("Copyright © 2022 Joey Parrish")]
[assembly: AssemblyVersion(MusicMod.ModVersion.String + ".0")]

// Sample usage:
//
// using System.IO;
// using System.Reflection;
// using MusicMod;
//
// public class MyMod : BaseUnityPlugin {
//   private void Awake() {
//     string assemblyFolder = Path.GetDirectoryName(
//         Assembly.GetExecutingAssembly().Location);
//     string absolutePath = Path.Combine(
//         assemblyFolder, "Main-Menu.mp3");
//     MusicMod.Mod.OverrideMusic["menu"] = absolutePath;
//   }
// }

namespace MusicMod {
#if DEBUG
  // This is generated at build time for releases.
  public static class ModVersion {
    public const string String = "0.0.1";
  }
#endif

  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class Mod : BaseUnityPlugin {
    // BepInEx' plugin metadata
    public const string PluginGUID = "io.github.joeyparrish.MusicMod";
    public const string PluginName = "MusicMod";
    public const string PluginVersion = ModVersion.String;

    private static readonly Harmony harmony = new Harmony(PluginName);

    private static new BepInEx.Logging.ManualLogSource Logger;

    private void Awake() {
      Logger = BepInEx.Logging.Logger.CreateLogSource("MusicMod");

      try {
        harmony.PatchAll();
      } catch (Exception ex) {
        Logger.LogError($"Exception installing patches for {PluginName}: {ex}");
      }
    }

    public static Dictionary<string, string> OverrideMusic =
        new Dictionary<string, string> {};

    internal static AudioClip LoadAudioClip(string absolutePath) {
      var pathUrl = "file:///" + absolutePath.Replace("\\", "/");
      var request = UnityWebRequestMultimedia.GetAudioClip(
          pathUrl, AudioType.MPEG);

      request.SendWebRequest();
      while (!request.isDone) {}

      if (request.error != null) {
        Logger.LogError($"Failed to load clip from {absolutePath}: {request.error}");
        return null;
      }

      var downloadHandler = request.downloadHandler as DownloadHandlerAudioClip;
      return downloadHandler?.audioClip;
    }

    // Patch in our custom music.
    [HarmonyPatch(typeof(MusicMan), nameof(MusicMan.Awake))]
    class CustomMusic_Patch {
      static void Postfix() {
        foreach (var music in MusicMan.instance.m_music) {
          if (OverrideMusic.ContainsKey(music.m_name)) {
            AudioClip audioClip = LoadAudioClip(OverrideMusic[music.m_name]);
            if (audioClip == null) {
              Logger.LogError($"Failed to load music override: {music.m_name}");
            } else {
              Logger.LogDebug($"Overriding music: {music.m_name}, orig. volume {music.m_volume}");
              music.m_clips = new AudioClip[]{ audioClip };
              music.m_volume = 1f;
            }
          } else {
            Logger.LogDebug($"Not overriding music: {music.m_name}, volume {music.m_volume}");
          }
        }
      }
    }
  }  // class Mod
}  // namespace MusicMod
