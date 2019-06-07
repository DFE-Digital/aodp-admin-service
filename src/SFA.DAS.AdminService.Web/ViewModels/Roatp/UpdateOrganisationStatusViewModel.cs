﻿namespace SFA.DAS.AdminService.Web.ViewModels.Roatp
{
    using System;
    using System.Collections.Generic;
    using SFA.DAS.AssessorService.Api.Types.Models.Roatp;

    public class UpdateOrganisationStatusViewModel
    {
        public IEnumerable<OrganisationStatus> OrganisationStatuses { get; set; }
        public IEnumerable<RemovedReason> RemovedReasons { get; set; }
        public string LegalName { get; set; }
        public Guid OrganisationId { get; set; }
        public int OrganisationStatusId { get; set; }
        public int? RemovedReasonId { get; set; }
        public string UpdatedBy { get; set; }
        public int ProviderTypeId { get; set; }
    }
}
