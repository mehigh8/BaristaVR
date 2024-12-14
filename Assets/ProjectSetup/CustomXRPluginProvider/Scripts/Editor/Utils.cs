using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectSetup.CustomXRPluginProvider.Scripts.Editor
{
    /// <summary>
    /// XR plug-in management provider helpers.
    /// </summary>
    public static class Utils
    {
        public const string XR_ARCORE_LOADER = "UnityEngine.XR.ARCore.ARCoreLoader";
        public const string XR_OPENVR_LOADER = "Unity.XR.OpenVR.OpenVRLoader";
        public const string XR_OPENXR_LOADER = "UnityEngine.XR.OpenXR.OpenXRLoader";
        public const string XR_OCULUS_LOADER = "Unity.XR.Oculus.OculusLoader";
        public const string XR_CRDBRD_LOADER = "Google.XR.Cardboard.XRLoader";
        public const string XR_MOCK_LOADER   = "Unity.XR.MockHMD.MockHMDLoader";
        public const string XR_ARKIT_LOADER  = "UnityEngine.XR.ARKit.ARKitLoader";
        
        public static readonly Dictionary<TargetVR, string> StandaloneLoaderTargetDict = new()
        {
            { TargetVR.CardboardXR, XR_OCULUS_LOADER},  // Default to Oculus.
            { TargetVR.SteamVR    , XR_OPENVR_LOADER},
            { TargetVR.MetaXR     , XR_OCULUS_LOADER}
        };        
        public static readonly Dictionary<TargetVR, string> AndroidLoaderTargetDict = new()
        {
            { TargetVR.CardboardXR, XR_CRDBRD_LOADER}, 
            { TargetVR.SteamVR    , XR_OCULUS_LOADER},   // Default to Oculus.
            { TargetVR.MetaXR     , XR_OCULUS_LOADER}
        };
        public static readonly Dictionary<TargetVR, string> IOSLoaderTargetDict = new()
        {
            { TargetVR.CardboardXR, XR_CRDBRD_LOADER}, 
            { TargetVR.SteamVR    , XR_ARKIT_LOADER},   // Default to Oculus.
            { TargetVR.MetaXR     , XR_OCULUS_LOADER}
        };
    
        public enum TargetVR
        {
            None = 0,
            CardboardXR = 1,
            SteamVR = 2,
            MetaXR = 3,
        }
        
        public static T FindScriptableObject<T>(string filter) where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets(filter);
            if (guids.Length == 0)
            {
                // Debug.LogError($"[EnvironmentHandler] GUIDs not found for filter: {filter}");
                return null;
            }
            
            if (guids.Length > 1)
            {
                Debug.LogError($"[EnvironmentHandler] Too many GUIDs found for filter: {filter}");
                return null;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            
            return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
        }
    }
}
