using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AssessorService.Domain.Paging;
using SFA.DAS.AdminService.Web.Domain;
using SFA.DAS.AdminService.Web.Infrastructure;
using SFA.DAS.AdminService.Web.ViewModels.Apply.Applications;
using SFA.DAS.AssessorService.ApplyTypes;
using SFA.DAS.AssessorService.Api.Types.Models.Apply.Review;
using SFA.DAS.AdminService.Web.Helpers;
using SFA.DAS.AdminService.Web.Domain.Apply;
using SFA.DAS.AdminService.Web.Extensions;
using SFA.DAS.AdminService.Common.Extensions.TagHelpers;

namespace SFA.DAS.AdminService.Web.Controllers.Apply
{
    [Authorize(Roles = Roles.AssessmentDeliveryTeam + "," + Roles.CertificationTeam)]
    [CheckSession(nameof(OrganisationWithdrawalApplicationController), nameof(ResetSession), nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid))]
    public class OrganisationWithdrawalApplicationController : Controller
    {
        private readonly IControllerSession _controllerSession;
        private readonly IApplicationApiClient _applyApiClient;
        private readonly ILogger<OrganisationWithdrawalApplicationController> _logger;

        private const int DefaultPageIndex = 1;
        private const int DefaultApplicationsPerPage = 10;
        private const int DefaultPageSetSize = 6;

        public OrganisationWithdrawalApplicationController(IControllerSession controllerSession, IApplicationApiClient applyApiClient, ILogger<OrganisationWithdrawalApplicationController> logger)
        {
            _controllerSession = controllerSession;
            _applyApiClient = applyApiClient;
            _logger = logger;
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Ignore)]
        public IActionResult ResetSession()
        {
            // reset all the session paging settings
            SetDefaultSession();
            
            return RedirectToAction(nameof(OrganisationWithdrawalApplications));
        }

        [HttpGet]
        public IActionResult Index()
        {
            // do not reset items per page settings
            ResetPageIndex();
            ResetSortColumn();
            ResetSortDirection();

            return RedirectToAction(nameof(OrganisationWithdrawalApplications));
        }

        [HttpGet]
        public async Task<IActionResult> OrganisationWithdrawalApplications()
        {
            var vm = await MapViewModelFromSession();
            return View(nameof(OrganisationWithdrawalApplications), vm);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePageNewApplications(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplications(ApplicationReviewStatus.New, pageIndex);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangePageNewApplicationsPartial(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplicationsPartial(ApplicationReviewStatus.New, pageIndex);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeApplicationsPerPageNewApplications(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplications(ApplicationReviewStatus.New, itemsPerPage);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangeApplicationsPerPageNewApplicationsPartial(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplicationsPartial(ApplicationReviewStatus.New, itemsPerPage);
        }

        [HttpGet]
        public async Task<IActionResult> SortNewApplications(string sortColumn, string sortDirection)
        {
            return await SortApplications(ApplicationReviewStatus.New, sortColumn, sortDirection);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> SortNewApplicationsPartial(string sortColumn, string sortDirection)
        {
            return await SortApplicationsPartial(ApplicationReviewStatus.New, sortColumn, sortDirection);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePageInProgressApplications(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplications(ApplicationReviewStatus.InProgress, pageIndex);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangePageInProgressApplicationsPartial(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplicationsPartial(ApplicationReviewStatus.InProgress, pageIndex);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeApplicationsPerPageInProgressApplications(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplications(ApplicationReviewStatus.InProgress, itemsPerPage);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangeApplicationsPerPageInProgressApplicationsPartial(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplicationsPartial(ApplicationReviewStatus.InProgress, itemsPerPage);
        }

        [HttpGet]
        public async Task<IActionResult> SortInProgressApplications(string sortColumn, string sortDirection)
        {
            return await SortApplications(ApplicationReviewStatus.InProgress, sortColumn, sortDirection);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> SortInProgressApplicationsPartial(string sortColumn, string sortDirection)
        {
            return await SortApplicationsPartial(ApplicationReviewStatus.InProgress, sortColumn, sortDirection);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePageFeedbackApplications(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplications(ApplicationReviewStatus.HasFeedback, pageIndex);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangePageFeedbackApplicationsPartial(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplicationsPartial(ApplicationReviewStatus.HasFeedback, pageIndex);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeApplicationsPerPageFeedbackApplications(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplications(ApplicationReviewStatus.HasFeedback, itemsPerPage);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangeApplicationsPerPageFeedbackApplicationsPartial(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplicationsPartial(ApplicationReviewStatus.HasFeedback, itemsPerPage);
        }

        [HttpGet]
        public async Task<IActionResult> SortFeedbackApplications(string sortColumn, string sortDirection)
        {
            return await SortApplications(ApplicationReviewStatus.HasFeedback, sortColumn, sortDirection);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> SortFeedbackApplicationsPartial(string sortColumn, string sortDirection)
        {
            return await SortApplicationsPartial(ApplicationReviewStatus.HasFeedback, sortColumn, sortDirection);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePageApprovedApplications(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplications(ApplicationReviewStatus.Approved, pageIndex);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangePageApprovedApplicationsPartial(int pageIndex = DefaultPageIndex)
        {
            return await ChangePageApplicationsPartial(ApplicationReviewStatus.Approved, pageIndex);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeApplicationsPerPageApprovedApplications(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplications(ApplicationReviewStatus.Approved, itemsPerPage);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> ChangeApplicationsPerPageApprovedApplicationsPartial(int itemsPerPage = DefaultApplicationsPerPage)
        {
            return await ChangeApplicationsPerPageApplicationsPartial(ApplicationReviewStatus.Approved, itemsPerPage);
        }

        [HttpGet]
        public async Task<IActionResult> SortApprovedApplications(string sortColumn, string sortDirection)
        {
            return await SortApplications(ApplicationReviewStatus.Approved, sortColumn, sortDirection);
        }

        [HttpGet]
        [CheckSession(nameof(IControllerSession.OrganisationWithdrawalApplication_SessionValid), CheckSession.Error)]
        public async Task<IActionResult> SortApprovedApplicationsPartial(string sortColumn, string sortDirection)
        {
            return await SortApplicationsPartial(ApplicationReviewStatus.Approved, sortColumn, sortDirection);
        }

        private IPagingState GetPagingState(string reviewStatus)
        {
            switch (reviewStatus)
            {
                case ApplicationReviewStatus.New:
                    return _controllerSession.OrganisationWithdrawalApplication_NewApplications;
                case ApplicationReviewStatus.InProgress:
                    return _controllerSession.OrganisationWithdrawalApplication_InProgressApplications;
                case ApplicationReviewStatus.HasFeedback:
                    return _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications;
                case ApplicationReviewStatus.Approved:
                    return _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications;
            }

            return null;
        }

        private async Task<ApplicationsDashboardViewModel> MapViewModelFromSession()
        {
            var viewModel = new ApplicationsDashboardViewModel(nameof(OrganisationWithdrawalApplicationController).RemoveController());

            viewModel.NewApplications = await AddApplicationsViewModelValues(viewModel.NewApplications, ApplicationReviewStatus.New, _controllerSession.OrganisationWithdrawalApplication_NewApplications);
            viewModel.InProgressApplications = await AddApplicationsViewModelValues(viewModel.InProgressApplications, ApplicationReviewStatus.InProgress, _controllerSession.OrganisationWithdrawalApplication_InProgressApplications);
            viewModel.FeedbackApplications = await AddApplicationsViewModelValues(viewModel.FeedbackApplications, ApplicationReviewStatus.HasFeedback, _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications);
            viewModel.ApprovedApplications = await AddApplicationsViewModelValues(viewModel.ApprovedApplications, ApplicationReviewStatus.Approved, _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications);

            return viewModel;
        }

        private async Task<IActionResult> ChangePageApplications(string reviewStatus, int pageIndex)
        {
            var pagingState = GetPagingState(reviewStatus);
            pagingState.PageIndex = pageIndex;

            var vm = await MapViewModelFromSession();
            return View(nameof(OrganisationWithdrawalApplications), vm);
        }

        private async Task<IActionResult> ChangePageApplicationsPartial(string reviewStatus, int pageIndex)
        {
            var pagingState = this.GetPagingState(reviewStatus);
            pagingState.PageIndex = pageIndex;

            var viewModel = await AddApplicationsViewModelValues(new ApplicationsViewModel(), reviewStatus, pagingState);
            return PartialView("_OrganisationApplicationsPartial", viewModel);
        }

        private async Task<IActionResult> ChangeApplicationsPerPageApplications(string reviewStatus, int itemsPerPage)
        {
            var pageingState = this.GetPagingState(reviewStatus);
            pageingState.ItemsPerPage = itemsPerPage;

            return await ChangePageApplications(reviewStatus, 1);
        }

        private async Task<IActionResult> ChangeApplicationsPerPageApplicationsPartial(string reviewStatus, int itemsPerPage)
        {
            var pagingState = GetPagingState(reviewStatus);
            pagingState.ItemsPerPage = itemsPerPage;

            return await ChangePageApplicationsPartial(reviewStatus, 1);
        }

        private async Task<IActionResult> SortApplications(string reviewStatus, string sortColumn, string sortDirection)
        {
            UpdateSortDirection(sortColumn, sortDirection, GetPagingState(reviewStatus));
            return await ChangePageApplications(reviewStatus, 1);
        }
        
        private async Task<IActionResult> SortApplicationsPartial(string reviewStatus, string sortColumn, string sortDirection)
        {
            UpdateSortDirection(sortColumn, sortDirection, GetPagingState(reviewStatus));
            return await ChangePageApplicationsPartial(reviewStatus, 1);
        }

        private void UpdateSortDirection(string sortColumn, string sortDirection, IPagingState pagingState)
        {
            if (pagingState.SortColumn == sortColumn)
            {
                pagingState.SortDirection = sortDirection;
            }
            else
            {
                pagingState.SortColumn = sortColumn;
                pagingState.SortDirection = SortOrder.Asc;
            }
        }

        private async Task<ApplicationsViewModel> AddApplicationsViewModelValues(ApplicationsViewModel viewModel, string reviewStatus, IPagingState pagingState)
        {
            viewModel.ReviewStatus = reviewStatus;
            viewModel.ItemsPerPage = pagingState.ItemsPerPage;
            viewModel.SortColumn = pagingState.SortColumn;
            viewModel.SortDirection = pagingState.SortDirection;
            viewModel.PaginatedList = await GetPageApplications(viewModel.ReviewStatus, pagingState);

            switch (reviewStatus)
            {
                case ApplicationReviewStatus.New:
                    viewModel.ChangePageAction = nameof(ChangePageNewApplications);
                    viewModel.ChangeItemsPerPageAction = nameof(ChangeApplicationsPerPageNewApplications);
                    viewModel.SortColumnAction = nameof(SortNewApplications);
                    viewModel.Title = "New";
                    break;
                case ApplicationReviewStatus.InProgress:
                    viewModel.ChangePageAction = nameof(ChangePageInProgressApplications);
                    viewModel.ChangeItemsPerPageAction = nameof(ChangeApplicationsPerPageInProgressApplications);
                    viewModel.SortColumnAction = nameof(SortInProgressApplications);
                    viewModel.Title = "In progress";
                    break;
                case ApplicationReviewStatus.HasFeedback:
                    viewModel.ChangePageAction = nameof(ChangePageFeedbackApplications);
                    viewModel.ChangeItemsPerPageAction = nameof(ChangeApplicationsPerPageFeedbackApplications);
                    viewModel.SortColumnAction = nameof(SortFeedbackApplications);
                    viewModel.Title = "Feedback";
                    break;
                case ApplicationReviewStatus.Approved:
                    viewModel.ChangePageAction = nameof(ChangePageApprovedApplications);
                    viewModel.ChangeItemsPerPageAction = nameof(ChangeApplicationsPerPageApprovedApplications);
                    viewModel.SortColumnAction = nameof(SortApprovedApplications);
                    viewModel.Title = "Approved";
                    break;
            }

            viewModel.Fragment = ApplicationReviewHelpers.ApplicationFragment(reviewStatus);

            return viewModel;
        }

        private async Task<PaginatedList<ApplicationSummaryItem>> GetPageApplications(string reviewStatus, IPagingState pagingState)
        {
            var organisationWithdrawalApplicationsRequest = new OrganisationWithdrawalApplicationsRequest
            (
                reviewStatus,
                pagingState.SortColumn.ToString(),
                pagingState.SortDirection == SortOrder.Asc ? 1 : 0,
                pagingState.ItemsPerPage,
                pagingState.PageIndex,
                DefaultPageSetSize
            );

            var response = await _applyApiClient.GetOrganisationWithdrawalApplications(organisationWithdrawalApplicationsRequest);
            return response;
        }

        private void SetDefaultSession()
        {
            _controllerSession.OrganisationWithdrawalApplication_SessionValid = true;

            ResetPageIndex();
            ResetItemsPerPage();
            ResetSortColumn();
            ResetSortDirection();
        }

        private void ResetPageIndex()
        {
            _controllerSession.OrganisationWithdrawalApplication_NewApplications.PageIndex = DefaultPageIndex;
            _controllerSession.OrganisationWithdrawalApplication_InProgressApplications.PageIndex = DefaultPageIndex;
            _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications.PageIndex = DefaultPageIndex;
            _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications.PageIndex = DefaultPageIndex;
        }

        private void ResetItemsPerPage()
        {
            _controllerSession.OrganisationWithdrawalApplication_NewApplications.ItemsPerPage = DefaultApplicationsPerPage;
            _controllerSession.OrganisationWithdrawalApplication_InProgressApplications.ItemsPerPage = DefaultApplicationsPerPage;
            _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications.ItemsPerPage = DefaultApplicationsPerPage;
            _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications.ItemsPerPage = DefaultApplicationsPerPage;
        }

        private void ResetSortColumn()
        {
            _controllerSession.OrganisationWithdrawalApplication_NewApplications.SortColumn = OrganisationWithdrawalApplicationsSortColumn.SubmittedDate;
            _controllerSession.OrganisationWithdrawalApplication_InProgressApplications.SortColumn = OrganisationWithdrawalApplicationsSortColumn.SubmittedDate;
            _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications.SortColumn = OrganisationWithdrawalApplicationsSortColumn.FeedbackAddedDate;
            _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications.SortColumn = OrganisationWithdrawalApplicationsSortColumn.ClosedDate;
        }

        private void ResetSortDirection()
        {
            _controllerSession.OrganisationWithdrawalApplication_NewApplications.SortDirection = SortOrder.Desc;
            _controllerSession.OrganisationWithdrawalApplication_InProgressApplications.SortDirection = SortOrder.Desc;
            _controllerSession.OrganisationWithdrawalApplication_FeedbackApplications.SortDirection = SortOrder.Desc;
            _controllerSession.OrganisationWithdrawalApplication_ApprovedApplications.SortDirection = SortOrder.Desc;
        }
    }
}