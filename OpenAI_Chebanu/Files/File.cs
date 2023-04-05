﻿using Newtonsoft.Json;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Client.Files
{
    public class File : ApiResultBase
    {
        /// <summary>
        /// Unique id for this file, so that it can be referenced in other operations
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        [JsonProperty("filename")]
        public string Name { get; set; }

        /// <summary>
        /// What is the purpose of this file, fine-tune, search, etc
        /// </summary>
        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// The size of the file in bytes
        /// </summary>
        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        /// <summary>
        /// Timestamp for the creation time of this file
        /// </summary>
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        /// <summary>
        /// When the object is deleted, this attribute is used in the Delete file operation
        /// </summary>
        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// The status of the File (ie when an upload operation was done: "uploaded")
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The status details, it could be null
        /// </summary>
        [JsonProperty("status_details")]
        public string StatusDetails { get; set; }
    }
}
