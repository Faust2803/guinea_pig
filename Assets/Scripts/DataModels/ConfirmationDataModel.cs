using System;

namespace DataModels
{
    [Serializable]
    public class ConfirmationDataModel
    {
        public bool success;
        public string message;
    }
    
    [Serializable]
    public class AnalyticsConfirmationDataModel
    {
        public bool success;
        public AnalyticsRequest message;
    }
    
    [Serializable]
    public class AnalyticsRequest
    {
        public int code;
        public int payload_size_bytes;
        public int events_ingested;
        public double server_upload_time;
    }
}