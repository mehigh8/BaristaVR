#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace ProjectSetup.CustomXRPluginProvider.Scripts.Editor
{
    /// <summary>
    /// Script which adds or removes `UNITY_XR_MANAGEMENT` from the project's Scripting defines symbols if
    /// the XR Plug-In Manager is installed or not.
    /// </summary>
    [InitializeOnLoad]
    public class XRPluginProviderScriptingDefineSymbolManager
    {
        private static List<BuildTargetGroup> _buildTargetGroups = new() { BuildTargetGroup.Standalone, BuildTargetGroup.Android, BuildTargetGroup.iOS };
        
        static XRPluginProviderScriptingDefineSymbolManager()
        {
            bool isXRManagementInstalled = Type.GetType("UnityEngine.XR.Management.XRGeneralSettings, Unity.XR.Management") != null;

            if (isXRManagementInstalled) AddDefineIfNeeded("UNITY_XR_MANAGEMENT");
            else RemoveDefineIfNeeded("UNITY_XR_MANAGEMENT");
        }

        private static void AddDefineIfNeeded(string define)
        {
            _buildTargetGroups.ForEach(btg =>
            {
                var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
                if (!definesString.Contains(define))
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, definesString + ";" + define);
                }
            });
        }

        private static void RemoveDefineIfNeeded(string define)
        {
            _buildTargetGroups.ForEach(btg =>
            {
                var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
                if (definesString.Contains(define))
                {
                    definesString = definesString.Replace(define, "");
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, definesString);
                }
            });
        }
    }
}

#endif