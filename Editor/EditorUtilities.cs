/* Copyright (C) 2019 Adrian Babilinski
* You may use, distribute and modify this code under the
* terms of the MIT License
*
*Permission is hereby granted, free of charge, to any person obtaining a copy
*of this software and associated documentation files (the "Software"), to deal
*in the Software without restriction, including without limitation the rights
*to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
*copies of the Software, and to permit persons to whom the Software is
*furnished to do so, subject to the following conditions:
*
*The above copyright notice and this permission notice shall be included in all
*copies or substantial portions of the Software.
*
*THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
*IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
*FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
*AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
*LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
*OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
*SOFTWARE.
*
*For more information contact adrian@blackboxrealities.com or visit blackboxrealities.com
*/




namespace Common.EditorUtilities
{
    using System.IO;
    using UnityEditor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine; 

    /// <summary>
    /// Use to search the root of the project. This allows you to go into the Packages and Library folders
    /// </summary>
    public class EditorFileUtility
    {
        /// <summary>
        /// &lt;path to project folder&gt;
        /// </summary>
        public static string AbsolutePath
        {
            get { return string.Concat(Application.dataPath.Reverse().Skip(7).Reverse()); }
        }

        /// <summary>
        /// Determines whether the specified file exists relative to the root project
        /// </summary>
        /// <returns></returns>
        public static bool FileExistsInAbsolutePath(string path)
        {
            return File.Exists(AbsolutePath + path);
        }

        public static bool ValidFolder(string path)
        {
            return AssetDatabase.IsValidFolder(path);
        }

    }

    public class DefineSymbolsUtility
    { 

    /// <summary>
    /// Add define symbols as soon as Unity gets done compiling.
    /// </summary>
    public static void AddDefineSymbols(string[] Symbols)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.AddRange(Symbols.Except(allDefines));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                                                             string.Join(";", allDefines.ToArray()));
        }

        /// <summary>
        /// Remove define symbols as soon as Unity gets done compiling.
        /// </summary>
        public static void RemoveDefineSymbols(string[] Symbols)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();

            for (int i = 0; i < Symbols.Length; i++)
            {
                if (!allDefines.Contains(Symbols[i]))
                {
                    Debug.LogWarning("Remove Defines Ignored. Symbol does not exists.");
                  
                }
                else
                {
                    allDefines.Remove(Symbols[i]);
                }
               
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                                                             string.Join(";", allDefines.ToArray()));
        }

        /// <summary>
        /// Add define symbol as soon as Unity gets done compiling.
        /// </summary>
        public static void AddDefineSymbol(string Symbol)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            if (allDefines.Contains(Symbol))
            {
                Debug.LogWarning("Add Defines Ignored. Symbol already exists.");
                return;
            }

            allDefines.Add(Symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                                                             string.Join(";", allDefines.ToArray()));
        }

        /// <summary>
        /// Remove define symbol as soon as Unity gets done compiling.
        /// </summary>
        public static void RemoveDefineSymbol(string Symbol)
        {
            string definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();
            allDefines.Remove(Symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                                                             string.Join(";", allDefines.ToArray()));
        }
    }
}
