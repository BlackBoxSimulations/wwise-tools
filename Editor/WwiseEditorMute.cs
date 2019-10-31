
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


#if WWISE
namespace Common.Wwise
{
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// Works out of the box. Allows for wwise to be muted with the editor mute button
[InitializeOnLoad]
public static class WwiseEditorMute 
{
    [SerializeField]
    private static bool isSuspended;

    static WwiseEditorMute()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
        EditorApplication.update += Update;
    }

     private static void LogPlayModeState(PlayModeStateChange state)
     {
         if (state == PlayModeStateChange.EnteredEditMode)
             isSuspended = false;

     }



    static void Update()
    {
        if(!EditorApplication.isPlaying || !AkSoundEngine.IsInitialized())
            return;
      
        if (isSuspended != UnityEditor.EditorUtility.audioMasterMute)
        {
            SuspendEngine(UnityEditor.EditorUtility.audioMasterMute);
        }
    }

    private static void SuspendEngine(bool isMuted)
    {
        if (isMuted)
        {
            AkSoundEngine.Suspend(true);
            isSuspended = true;
        }
        else
        {
            isSuspended = false;
            AkSoundEngine.WakeupFromSuspend();
            AkSoundEngine.RenderAudio(true);
        }
    }

}
}
#endif
