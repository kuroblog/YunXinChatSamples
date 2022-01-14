using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json.Linq;
using NIM;
using NimUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Desktop.Samples.Common.YunXinSDKs
{
    public class YunXinService : NotificationObject
    {
        private readonly ILoggerFacade _logger;
        private readonly IEventAggregator _event;

        public YunXinService(
            IEventAggregator @event,
            ILoggerFacade logger)
        {
            _event = @event ?? throw new ArgumentNullException(nameof(@event));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

        private const string YunXinSDKSettingsFile = "config.json";

        public virtual NimConfig GetSDKConfig()
        {
            JObject GetConfigRoot()
            {
                if (!File.Exists(YunXinSDKSettingsFile))
                {
                    throw new ArgumentException($"{YunXinSDKSettingsFile} is not found.");
                }

                using (var stream = File.OpenRead(YunXinSDKSettingsFile))
                {
                    var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    reader.Close();
                    return JObject.Parse(content);
                }
            }

            JToken GetConfigToken()
            {
                var root = GetConfigRoot();
                if (root == null)
                {
                    throw new ArgumentException($"{YunXinSDKSettingsFile} is empty.");
                }

                var indexToken = root.SelectToken("configs.index");
                int indexTokenResult = indexToken == null ? 0 : indexToken.ToObject<int>();

                var listToken = root.SelectToken("configs.list");
                if (listToken == null)
                {
                    throw new ArgumentNullException($"{YunXinSDKSettingsFile} format error, configs:list is empty.");
                }

                var listTokenResult = listToken.ToArray();
                var configResult = listTokenResult.Where((item, index) => index == indexTokenResult)?.FirstOrDefault();
                if (configResult == null)
                {
                    throw new ArgumentNullException($"{YunXinSDKSettingsFile} format error, configs:list[{indexTokenResult}] is empty.");
                }

                return configResult;
            }

            var configToken = GetConfigToken();
            var result = configToken.ToObject<NimConfig>();
            if (result == null)
            {
                throw new ArgumentException($"{YunXinSDKSettingsFile} format error.");
            }

            return result;
        }

        public virtual bool Init(string appDataDir, string appInstallDir = "")
        {
            var sdkConfig = GetSDKConfig();
            if (string.IsNullOrEmpty(sdkConfig.AppKey))
            {
                throw new ArgumentException($"{YunXinSDKSettingsFile} format error, {nameof(sdkConfig.AppKey)} is empty.");
            }

            if (sdkConfig.CommonSetting == null)
            {
                sdkConfig.CommonSetting = new SdkCommonSetting
                {
                    TeamMsgAckEnabled = true
                };
            }

            return ClientAPI.Init(sdkConfig.AppKey, appDataDir, appInstallDir, sdkConfig);
        }

        public virtual void Clean(bool needToLogout = false)
        {
            VChatAPI.Cleanup();

            if (!needToLogout)
            {
                ClientAPI.Cleanup();
            }
            else
            {
                ClientAPI.Logout(
                    NIMLogoutType.kNIMLogoutAppExit,
                    state =>
                    {
                        ClientAPI.Cleanup();
                    });
            }
        }
    }
}
