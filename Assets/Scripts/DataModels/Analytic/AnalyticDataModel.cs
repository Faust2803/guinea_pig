using SO.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataModels.CollectionsData
{
    [Serializable]
    public class AnalyticDataModel
    {
        public string event_type;
        public Dictionary<string, object> event_properties = new Dictionary<string, object>();
    }
}