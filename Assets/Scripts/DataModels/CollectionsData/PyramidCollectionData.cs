using System;
using Newtonsoft.Json;

namespace DataModels.CollectionsData
{
    [Serializable]
    public class PyramidCollectionData
    {
        [JsonProperty("data")]
        public PyramidCollectionItemDTO[] PyramidCollectionItems { get; set; }
    }
}