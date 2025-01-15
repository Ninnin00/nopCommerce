using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.NopMobileApp;

/// <summary>
/// Represents the nopCommerce api application helper plugin
/// </summary>
public class HorizonCommercePlugin : BasePlugin, IAdminMenuPlugin
{

    #region Fields

    private readonly ISettingService _settingService;
    private readonly IWorkContext _workContext;
    private readonly IWebHelper _webHelper;
    private readonly ILocalizationService _localizationService;
    private readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public HorizonCommercePlugin(
        IWebHelper webHelper, ISettingService settingService, IWorkContext workContext, ILocalizationService localizationService, IPermissionService permissionService)
    {

        _webHelper = webHelper;
        _settingService = settingService;
        _workContext = workContext;
        _localizationService = localizationService;
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return $"{_webHelper.GetStoreLocation()}Admin/HorizonCommerce/Configure";
    }

    /// <summary>
    /// Install the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        await base.InstallAsync();
    }

    public async Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
            return;

        var config = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Configuration"));
        if (config == null)
            return;

        var plugins = config.ChildNodes.FirstOrDefault(node => node.SystemName.Equals("Local plugins"));

        if (plugins == null)
            return;

        var index = config.ChildNodes.IndexOf(plugins);

        if (index < 0)
            return;

        config.ChildNodes.Insert(index, new SiteMapNode
        {
            SystemName = "nopCommerce api application",
            Title = "Horizon Commerce",
            ControllerName = "HorizonCommerce",
            ActionName = "Configure",
            IconClass = "far fa-dot-circle",
            Visible = true,
            RouteValues = new RouteValueDictionary { { "area", AreaNames.ADMIN } }
        });
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }

    #endregion

}