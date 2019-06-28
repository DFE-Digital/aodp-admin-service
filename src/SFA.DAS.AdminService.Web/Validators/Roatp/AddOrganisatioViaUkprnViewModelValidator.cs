﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SFA.DAS.AdminService.Web.Infrastructure;
using SFA.DAS.AdminService.Web.Resources;
using SFA.DAS.AdminService.Web.ViewModels.Roatp;
using SFA.DAS.AssessorService.Api.Types.Models.Validation;


namespace SFA.DAS.AdminService.Web.Validators.Roatp
{
    public class AddOrganisatioViaUkprnViewModelValidator : AbstractValidator<AddOrganisationViaUkprnViewModel>
    {
        private readonly IRoatpOrganisationValidator _validator;
        private readonly IRoatpApiClient _apiClient;

        public AddOrganisatioViaUkprnViewModelValidator(IRoatpOrganisationValidator validator, IRoatpApiClient apiClient)
        {
            _validator = validator;
            _apiClient = apiClient;

            RuleFor(vm => vm).Custom((vm, context) =>
            {
                var validationResult = IsValidUkprn(vm);
                if (validationResult.IsValid) return;
                foreach (var error in validationResult.Errors)
                {
                    context.AddFailure(error.Field, error.ErrorMessage);
                }
            });
        }

        private ValidationResponse IsValidUkprn(AddOrganisationViaUkprnViewModel vm)
        {
            var validationResponse = new ValidationResponse
            {
                Errors = new List<ValidationErrorDetail>()
            };


            //REMOVEREMOVE this should be removed once APR-471 has been tested, probably before merging into master
            if (vm.UKPRN == "111111111111") return validationResponse;

            var fieldValidationErrors = _validator.IsValidUKPRN(vm.UKPRN);
            if (fieldValidationErrors.Any())
            {
                validationResponse.Errors.AddRange(fieldValidationErrors);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(vm.UKPRN))
                {
                    fieldValidationErrors = IsDuplicateUkprn(vm.OrganisationId, vm.UKPRN);
                    if (fieldValidationErrors.Any())
                    {
                        validationResponse.Errors.AddRange(fieldValidationErrors);
                    }
                }
            }

            return validationResponse;
        }


        private List<ValidationErrorDetail> IsDuplicateUkprn(Guid organisationId, string ukprn)
        {
            var errorMessages = new List<ValidationErrorDetail>();

            long ukprnValue = 0;
            var isParsed = long.TryParse(ukprn, out ukprnValue);

            var duplicateCheckResponse = _apiClient.DuplicateUKPRNCheck(organisationId, ukprnValue).Result;

            if (duplicateCheckResponse != null && duplicateCheckResponse.DuplicateFound)
            {
                var duplicateErrorMessage = string.Format(RoatpOrganisationValidation.UKPRNDuplicateMatch,
                    duplicateCheckResponse.DuplicateOrganisationName);
                errorMessages.Add(new ValidationErrorDetail("UKPRN", duplicateErrorMessage));
            }

            return errorMessages;
        }
    }
}
