using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHeroHelper.Templates
{
    public class UITemplate
    {
        public const string AddEditModalRazor = @"@inject Microsoft.Extensions.Localization.IStringLocalizer<AddEdit$_ENTITY_$Modal> _localizer

<EditForm Model=""@AddEdit$_ENTITY_$Model"" OnValidSubmit=""SaveAsync"">
    <FluentValidationValidator @ref=""_fluentValidationValidator"" />
    <MudDialog>
        <TitleContent>
            @{
                if (AddEdit$_ENTITY_$Model.Id == 0)
                {
                    <MudText Typo=""Typo.h6"">
                        <MudIcon Icon=""@Icons.Material.Filled.Add"" Class=""mr-3 mb-n1"" />
                        @_localizer[""Add $_ENTITY_$""]
                    </MudText>
                }
                else
                {
                    <MudText Typo=""Typo.h6"">
                        <MudIcon Icon=""@Icons.Material.Filled.Update"" Class=""mr-3 mb-n1"" />
                        @_localizer[""Update $_ENTITY_$""]
                    </MudText>
                }
            }
        </TitleContent>
        <DialogContent>
            <MudGrid>
                @if (AddEdit$_ENTITY_$Model.Id != 0)
                {
                    <MudItem xs=""12"" md=""6"">
                        <MudTextField Disabled For=""@(() => AddEdit$_ENTITY_$Model.Id)"" @bind-Value=""AddEdit$_ENTITY_$Model.Id"" Label=""@_localizer[""Id""]"" />
                    </MudItem>
                }
                @* TODO: Insert Data Member Here *@
                @*<MudItem xs=""12"" md=""6"">
                    <MudTextField For=""@(() => AddEdit$_ENTITY_$Model.Name)"" @bind-Value=""AddEdit$_ENTITY_$Model.Name"" Label=""@_localizer[""Name""]"" />
                </MudItem>
                <MudItem xs=""12"" md=""6"">
                    <MudTextField For=""@(() => AddEdit$_ENTITY_$Model.Description)"" @bind-Value=""AddEdit$_ENTITY_$Model.Description"" Label=""@_localizer[""Description""]"" />
                </MudItem>
                <MudItem xs=""12"" md=""6"">
                    <MudAutocomplete T=""int"" Label=""@_localizer[""Brand""]"" For=""@(() => AddEdit$_ENTITY_$Model.BrandId)"" @bind-Value=""AddEdit$_ENTITY_$Model.BrandId"" ResetValueOnEmptyText=""true"" SearchFunc=""@SearchBrands"" Variant=""Variant.Filled"" ToStringFunc=""@(i => _brands.FirstOrDefault(b => b.Id == i)?.Name ?? string.Empty)"" OffsetY=""true"" />
                </MudItem>
                <MudItem xs=""12"" md=""6"">
                    <MudTextField For=""@(() => AddEdit$_ENTITY_$Model.Rate)"" @bind-Value=""AddEdit$_ENTITY_$Model.Rate"" Label=""@_localizer[""Rate""]"" />
                </MudItem>
                *@
            </MudGrid>
        </DialogContent>
        <DialogActions>
            <MudButton DisableElevation Variant=""Variant.Filled"" OnClick=""Cancel"">@_localizer[""Cancel""]</MudButton>
            @if (AddEdit$_ENTITY_$Model.Id != 0)
            {
                <MudButton DisableElevation Variant=""Variant.Filled"" ButtonType=""ButtonType.Submit"" Disabled=""@(!Validated)"" Color=""Color.Secondary"">@_localizer[""Update""]</MudButton>
            }
            else
            {
                <MudButton DisableElevation Variant=""Variant.Filled"" ButtonType=""ButtonType.Submit"" Disabled=""@(!Validated)"" Color=""Color.Success"">@_localizer[""Save""]</MudButton>
            }
        </DialogActions>
    </MudDialog>
</EditForm>";
        public const string AddEditModalCS = @"using $_ADD_EDIT_CQRS_NAMESPACE_$;
using $_REQUEST_NAMESPACE_$;
using $_CLIENT_EXTENSIONS_NAMESPACE_$;
using $_SHARED_CONSTANTS_APPLICATION_NAMESPACE_$;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using $_MANAGER_NAMESPACE_$;

namespace $_NAMESPACE_$
{
    public partial class AddEdit$_ENTITY_$Modal
    {
        [Inject] private I$_ENTITY_$Manager $_ENTITY_$Manager { get; set; }

        [Parameter] public AddEdit$_ENTITY_$Command AddEdit$_ENTITY_$Model { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await $_ENTITY_$Manager.SaveAsync(AddEdit$_ENTITY_$Model);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
    }
}";

        public const string ListRazor = @"@page ""$_PAGE_PATH_$""
@attribute [Authorize(Policy = Permissions.$_ENTITY_$s.View)]
@inject Microsoft.Extensions.Localization.IStringLocalizer<$_ENTITY_$s> _localizer
@using $_GET_ALL_PAGED_NAMESPACE_$;

<style>
    .mud-table-container {
        overflow: auto;
    }
</style>

<HeroTitle Title=""@_localizer[""$_ENTITY_$s""]"" Description=""@_localizer[""Manage $_ENTITY_$s.""]"" />
@if (!_loaded)
{
    <MudProgressCircular Color=""Color.Primary"" Indeterminate=""true"" />
}
else
{
    <MudTable Elevation=""25"" ServerData=""@(new Func<TableState, Task<TableData<GetAllPaged$_ENTITY_$sResponse>>>(ServerReload))"" Hover=""true"" Dense=""@_dense"" Bordered=""@_bordered"" Striped=""@_striped"" @ref=""_table"">
        <ToolBarContent>
            <div class=""justify-center mud-text-align-center"">
                @if (_canCreate$_ENTITY_$s)
                {
                    <MudButton DisableElevation Variant=""Variant.Filled"" Color=""Color.Primary"" @onclick=""@(() => InvokeModal(0))"" StartIcon=""@Icons.Material.Filled.Add"" IconColor=""Color.Surface"">@_localizer[""Create""]</MudButton>
                    <MudButton DisableElevation Variant=""Variant.Filled"" OnClick=""@(() => OnSearch(""""))"" StartIcon=""@Icons.Material.Filled.Refresh"" IconColor=""Color.Surface"" Color=""Color.Secondary"">@_localizer[""Reload""]</MudButton>
                }
                else
                {
                    <MudButton DisableElevation Variant=""Variant.Filled"" OnClick=""@(() => OnSearch(""""))"" StartIcon=""@Icons.Material.Filled.Refresh"" IconColor=""Color.Surface"" Color=""Color.Secondary"">@_localizer[""Reload""]</MudButton>
                }
            </div>
            <MudSpacer />
            @if (_canSearch$_ENTITY_$s)
            {
                <MudTextField T=""string"" ValueChanged=""@(s=>OnSearch(s))"" Placeholder=""@_localizer[""Search""]"" Adornment=""Adornment.Start"" AdornmentIcon=""@Icons.Material.Filled.Search"" IconSize=""Size.Medium"" Class=""mt-0""></MudTextField>
            }
        </ToolBarContent>
        <HeaderContent>
            <MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Id"">@_localizer[""Id""]</MudTableSortLabel></MudTh>
             @* TODO: Insert Data Member Here *@
            @*<MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Name"">@_localizer[""Name""]</MudTableSortLabel></MudTh>
            <MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Brand"">@_localizer[""Brand""]</MudTableSortLabel></MudTh>
            <MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Description"">@_localizer[""Description""]</MudTableSortLabel></MudTh>
            <MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Barcode"">@_localizer[""Barcode""]</MudTableSortLabel></MudTh>
            <MudTh><MudTableSortLabel T=""GetAllPaged$_ENTITY_$sResponse"" SortLabel=""Rate"">@_localizer[""Rate""]</MudTableSortLabel></MudTh>*@
            <MudTh Style=""text-align:right"">@_localizer[""Actions""]</MudTh>
        </HeaderContent>
        <RowTemplate>
            <MudTd DataLabel=""Id"">@context.Id</MudTd>
             @* TODO: Insert Data Member Here *@
            @*<MudTd DataLabel=""Name"">
                <MudHighlighter Text=""@context.Name"" HighlightedText=""@_searchString"" />
            </MudTd>
            <MudTd DataLabel=""Brand"">
                <MudHighlighter Text=""@context.Brand"" HighlightedText=""@_searchString"" />
            </MudTd>
            <MudTd DataLabel=""Description"">
                <MudHighlighter Text=""@context.Description"" HighlightedText=""@_searchString"" />
            </MudTd>
            <MudTd DataLabel=""Barcode"">
                <MudHighlighter Text=""@context.Barcode"" HighlightedText=""@_searchString"" />
            </MudTd>
            <MudTd DataLabel=""Rate"">@context.Rate</MudTd>*@
            <MudTd DataLabel=""Actions"" Style=""text-align: right"">
                @if (_canEdit$_ENTITY_$s || _canDelete$_ENTITY_$s)
                {
                    <MudMenu Label=""@_localizer[""Actions""]"" Variant=""Variant.Filled"" DisableElevation=""true"" EndIcon=""@Icons.Filled.KeyboardArrowDown"" IconColor=""Color.Secondary"" Direction=""Direction.Left"" OffsetX=""true"">
                        @if (_canEdit$_ENTITY_$s)
                            {
                            <MudMenuItem @onclick=""@(() => InvokeModal(@context.Id))"">@_localizer[""Edit""]</MudMenuItem>
                            }
                        @if (_canDelete$_ENTITY_$s)
                            {
                            <MudMenuItem @onclick=""@(() => Delete(@context.Id))"">@_localizer[""Delete""]</MudMenuItem>
                            }
                    </MudMenu>
                    }
                    else
                    {
                    <MudButton Variant=""Variant.Filled""
                               DisableElevation=""true""
                               StartIcon=""@Icons.Material.Filled.DoNotTouch""
                               IconColor=""Color.Secondary""
                               Size=""Size.Small""
                               Color=""Color.Surface"">
                        @_localizer[""No Allowed Actions""]
                    </MudButton>
                }
            </MudTd>
        </RowTemplate>
        <FooterContent>
            <MudSwitch @bind-Checked=""@_dense"" Color=""Color.Secondary"" Style=""margin-left: 5px;"">@_localizer[""Dense""]</MudSwitch>
            <MudSwitch @bind-Checked=""@_striped"" Color=""Color.Tertiary"" Style=""margin-left: 5px;"">@_localizer[""Striped""]</MudSwitch>
            <MudSwitch @bind-Checked=""@_bordered"" Color=""Color.Warning"" Style=""margin-left: 5px;"">@_localizer[""Bordered""]</MudSwitch>
        </FooterContent>
        <PagerContent>
            <TablePager />
        </PagerContent>
    </MudTable>
}";
        public const string ListCS = @"using $_GET_ALL_PAGED_CQRS_NAMESPACE_$;
using $_REQUEST_NAMESPACE_$;
using $_CLIENT_EXTENSIONS_NAMESPACE_$;
using $_SHARED_CONSTANTS_APPLICATION_NAMESPACE_$;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using $_ADD_EDIT_CQRS_NAMESPACE_$;
using $_MANAGER_NAMESPACE_$;
using $_SHARED_CONSTANTS_PERMISSION_NAMESPACE_$;
using Microsoft.AspNetCore.Authorization;

namespace $_NAMESPACE_$
{
    public partial class $_ENTITY_$s
    {
        [Inject] private I$_ENTITY_$Manager $_ENTITY_$Manager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPaged$_ENTITY_$sResponse> _pagedData;
        private MudTable<GetAllPaged$_ENTITY_$sResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = """";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreate$_ENTITY_$s;
        private bool _canEdit$_ENTITY_$s;
        private bool _canDelete$_ENTITY_$s;
        private bool _canSearch$_ENTITY_$s;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreate$_ENTITY_$s = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.$_ENTITY_$s.Create)).Succeeded;
            _canEdit$_ENTITY_$s = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.$_ENTITY_$s.Edit)).Succeeded;
            _canDelete$_ENTITY_$s = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.$_ENTITY_$s.Delete)).Succeeded;
            _canSearch$_ENTITY_$s = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.$_ENTITY_$s.Search)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPaged$_ENTITY_$sResponse>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPaged$_ENTITY_$sResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] {$""{state.SortLabel} {state.SortDirection}""} : new[] {$""{state.SortLabel}""};
            }

            var request = new GetAllPaged$_ENTITY_$sRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await $_ENTITY_$Manager.Get$_ENTITY_$sAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task InvokeModal($_DEFAULT_ID_DATATYPE_$ id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var $_ENTITY_LOWER_$ = _pagedData.FirstOrDefault(c => c.Id == id);
                if ($_ENTITY_LOWER_$ != null)
                {
                    parameters.Add(nameof(AddEdit$_ENTITY_$Modal.AddEdit$_ENTITY_$Model), new AddEdit$_ENTITY_$Command
                    {
                        Id = $_ENTITY_LOWER_$.Id,
                        //TODO: Insert Data Member Here

                        //Name = product.Name,
                        //Description = product.Description,
                        //Rate = product.Rate,
                        //BrandId = product.BrandId,
                        //Barcode = product.Barcode
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEdit$_ENTITY_$Modal>(id == 0 ? _localizer[""Create""] : _localizer[""Edit""], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("""");
            }
        }

        private async Task Delete($_DEFAULT_ID_DATATYPE_$ id)
        {
            string deleteContent = _localizer[""Delete Content""];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer[""Delete""], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await $_ENTITY_$Manager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("""");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("""");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}";
    }
}
