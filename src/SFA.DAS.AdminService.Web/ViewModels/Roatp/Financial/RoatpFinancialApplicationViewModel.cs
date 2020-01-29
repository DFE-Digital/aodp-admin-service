using System;
using SFA.DAS.AssessorService.ApplyTypes;
using SFA.DAS.QnA.Api.Types;

namespace SFA.DAS.AdminService.Web.ViewModels.Apply.Financial
{
    public class RoatpFinancialApplicationViewModel
    {
        public string ApplicationReference { get; }
        public string LegalName { get; }
        public string TradingName { get; }
        public string ProviderName { get; }
        public int? Ukprn { get; }
        public string CompanyNumber { get; }

        public Section Section { get; }
        public FinancialGrade Grade { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid Id { get; set; }
        public Guid OrgId { get; set; }

        public FinancialDueDate OutstandingFinancialDueDate { get; set; }
        public FinancialDueDate GoodFinancialDueDate { get; set; }
        public FinancialDueDate SatisfactoryFinancialDueDate { get; set; }

        public RoatpFinancialApplicationViewModel() { }

        public RoatpFinancialApplicationViewModel(Guid id, Guid applicationId, Section section, FinancialGrade grade, AssessorService.ApplyTypes.Roatp.Apply application)
        {
            Id = id;
            if (section != null)
            {
                Section = section;
                ApplicationId = section.ApplicationId;
            }
            else
            {
                ApplicationId = applicationId;
            }

            SetupGradeAndFinancialDueDate(grade);
            OrgId = application.OrganisationId;

            //if (application.ApplicationData != null)
            //{
            //    ApplicationReference = application.ApplicationData.ReferenceNumber;
            //}


            //if (application.ApplyingOrganisation?.OrganisationData != null)
            //{
            //    Ukprn = application.ApplyingOrganisation.EndPointAssessorUkprn;
            //    LegalName = application.ApplyingOrganisation.OrganisationData.LegalName;
            //    TradingName = application.ApplyingOrganisation.OrganisationData.TradingName;
            //    ProviderName = application.ApplyingOrganisation.OrganisationData.ProviderName;
            //    CompanyNumber = application.ApplyingOrganisation.OrganisationData.CompanyNumber;
            //}
        }

        private void SetupGradeAndFinancialDueDate(FinancialGrade grade)
        {
            Grade = grade ?? new FinancialGrade();

            OutstandingFinancialDueDate = new FinancialDueDate();
            GoodFinancialDueDate = new FinancialDueDate();
            SatisfactoryFinancialDueDate = new FinancialDueDate();

            if(Grade.FinancialDueDate.HasValue)
            {
                var day = Grade.FinancialDueDate.Value.Day.ToString();
                var month = Grade.FinancialDueDate.Value.Month.ToString();
                var year = Grade.FinancialDueDate.Value.Year.ToString();

                switch (Grade.SelectedGrade)
                {
                    case FinancialApplicationSelectedGrade.Outstanding:
                        OutstandingFinancialDueDate = new FinancialDueDate { Day = day, Month = month, Year = year };
                        break;
                    case FinancialApplicationSelectedGrade.Good:
                        GoodFinancialDueDate = new FinancialDueDate { Day = day, Month = month, Year = year };
                        break;
                    case FinancialApplicationSelectedGrade.Satisfactory:
                        SatisfactoryFinancialDueDate = new FinancialDueDate { Day = day, Month = month, Year = year };
                        break;
                    default:
                        break;
                }
            }
        }
    }

}