﻿using Newtonsoft.Json;

namespace SFA.DAS.AdminService.Settings
{
    public class CertificateDetails : ICertificateDetails
    {
        [JsonRequired]
        public string ChairName{ get; set; }
        [JsonRequired]
        public string ChairTitle { get; set; }
    }
}
