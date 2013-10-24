using System;

namespace ParseClient.Dtos
{
    public class SerializableDto
    {
        public string encryptedData { get; set; }
        public DateTime lastSyncedDate { get; set; }
    }
}