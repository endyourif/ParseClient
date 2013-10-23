using System;

namespace ParseClient.Dtos
{
    public class DeserializableDto
    {
        public string encryptedData { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string objectId { get; set; }
    }
}