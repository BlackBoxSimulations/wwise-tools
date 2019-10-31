
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


namespace Common.Wwise
{
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Wwise.Models;
using Common.Extensions;
using Newtonsoft.Json;
using UnityEngine;
using Event = Common.Wwise.Models.Event;

[System.Serializable] public class WiseEventDropDown : ISerializationCallbackReceiver
{
#if WWISE
    [SerializeField] private AK.Wwise.Event eventName;
 #endif

    public string wwiseEventName;
    public bool reload;
    public void OnAfterDeserialize() {}

    private string eventGuidString;
    private byte[] guid;

    public void OnBeforeSerialize()
    {
#if WWISE
#if UNITY_EDITOR
        if (eventName.valueGuid != null && (eventName.valueGuid.Length == 16 && eventName.IsValid()))
        {
            guid = eventName.valueGuid;
        }
  
        if (guid != null && string.IsNullOrEmpty(eventGuidString) && guid.Length == 16)
        {
            WiseEventDropDownEventStart.LoadData();
            eventGuidString = new Guid(guid).ToString().ToUpper();
                  if(WiseEventDropDownEventStart.wwiseEventNames != null)
            if (WiseEventDropDownEventStart.wwiseEventNames.ContainsKey(eventGuidString))
            {
                wwiseEventName = WiseEventDropDownEventStart.wwiseEventNames[eventGuidString];
            }

        }


        if (reload)
        {
            //  WwiseEventStart.Populate();
            eventGuidString = new Guid(guid).ToString().ToUpper();
            if(WiseEventDropDownEventStart.wwiseEventNames != null)
            if (WiseEventDropDownEventStart.wwiseEventNames.ContainsKey(eventGuidString))
            {
                wwiseEventName = WiseEventDropDownEventStart.wwiseEventNames[eventGuidString];
            }

        }
        #endif
#endif
    }
}
#if WWISE
#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]

public static class WiseEventDropDownEventStart
{

    public static string XmlInfoPath;
    public static Dictionary<string, string> wwiseEventNames = new Dictionary<string, string>();
    public static WwiseEventModel Model;
    public static void Populate()
    {

if (!string.IsNullOrEmpty(WiseEventDropDownEventStart.GetXMLPath())
    && UnityEditor.EditorPrefs.GetBool("RELOADWWISE", false) == false)
        {
            XmlDocument doc = new XmlDocument();
             var xmlPath = WiseEventDropDownEventStart.GetXMLPath();
               if (string.IsNullOrEmpty( xmlPath))
               return;
               
            string file = File.ReadAllText(WiseEventDropDownEventStart.GetXMLPath(), Encoding.UTF8);
            doc.LoadXml(file);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            Debug.Log("<color=blue><b>Wwise Path: </b></color> <i>" + WiseEventDropDownEventStart.GetXMLPath() + "</i>");
          
            Model = WwiseEventModel.FromJson(jsonText);

            if(Model?.WwiseDocument?.Events?.WorkUnit?.ChildrenList?.Event == null)
                return;

            wwiseEventNames = new Dictionary<string, string>();

            foreach (Event @event in Model.WwiseDocument.Events.WorkUnit.ChildrenList.Event)
            {
                string cleanID = @event.Id;
                cleanID = cleanID.Replace("}", "");
                cleanID = cleanID.Replace("{", "");
                wwiseEventNames.Add(cleanID.ToUpper(), @event.Name);
            }

            UnityEditor.EditorPrefs.SetBool("RELOADWWISE", true);
        }
    }

    public static void LoadData()
    {
        if (Model == null || Model.WwiseDocument == null || string.IsNullOrEmpty(Model.WwiseDocument.Id))
        {
            Populate();
        }
    }

    public static string GetXMLPath()
    {
        if (string.IsNullOrEmpty(XmlInfoPath))
        {
            var path = GetPath();
            if(path == null)
            return null;
            
            path = path.Replace("\\", "/");

            XmlInfoPath = path;
        }

        return XmlInfoPath;



    }
    static WiseEventDropDownEventStart()
    {

    
        XmlInfoPath = GetPath();
        Populate();
    }


    public static string GetPath()
    {
        var projectPath = Path.Combine(Application.dataPath , WwiseSettings.LoadSettings().WwiseProjectPath);
        var wwiseDirectory = Path.GetDirectoryName(Path.GetFullPath(projectPath));
        var wwiseEventsDirectory = "";
        var dirs = Directory.GetDirectories(wwiseDirectory);
        if(dirs !=null)
        for (int i = 0; i < dirs.Length; i++)
        {
            if (dirs[i].Contains("Events"))
            {
                wwiseEventsDirectory = dirs[i];
                break;
            }
        }


        string[] files = Directory.GetFiles(wwiseEventsDirectory, "Default Work Unit.wwu", SearchOption.AllDirectories);
if(files == null)
return null;

        return files[0];
    }
}
#endif
#endif
}
