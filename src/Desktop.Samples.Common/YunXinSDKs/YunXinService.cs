using Desktop.Samples.Common.Events;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.ViewModel;
using Newtonsoft.Json.Linq;
using NIM;
using NIM.DataSync;
using NIM.Friend;
using NIM.SysMessage;
using NIM.User;
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

        private const string YunXinSDKSettingsFile = "config.json";

        protected NIMVChatSessionStatus _vChatSessionStatus;

        public YunXinService(
            IEventAggregator @event,
            ILoggerFacade logger)
        {
            _event = @event ?? throw new ArgumentNullException(nameof(@event));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger?.Debug($"{GetType().FullName} ... ctor.");
        }

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

            var initState = ClientAPI.Init(sdkConfig.AppKey, appDataDir, appInstallDir, sdkConfig);
            if (!initState)
            {
                throw new ArgumentException($"init {nameof(ClientAPI)} from {nameof(YunXinSDKSettingsFile)} failed.");
            }

            initState = VChatAPI.Init("");
            if (!initState)
            {
                throw new ArgumentException($"init {nameof(VChatAPI)} failed.");
            }

            return initState;
        }

        public virtual void Clean(bool needToLogout = false)
        {
            //DeviceAPI.EndDevice(NIMDeviceType.kNIMDeviceTypeAudioIn);
            //DeviceAPI.EndDevice(NIMDeviceType.kNIMDeviceTypeAudioOutChat);
            //DeviceAPI.EndDevice(NIMDeviceType.kNIMDeviceTypeVideo);
            //DeviceAPI.EndDevice(NIMDeviceType.kNIMDeviceTypeSoundcardCapturer);
            //DeviceAPI.EndDevice(NIMDeviceType.kNIMDeviceTypeAudioHook);

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

        public virtual void SetProxy(NIMProxyType type, string ip, int port, string user, string secret)
        {
            GlobalAPI.SetProxy(type, ip, port, user, secret);
        }

        #region callbacks
        protected virtual void OnRegDisconnected() { }

        protected virtual void OnRegKickout(NIMKickoutResult result) { }

        protected virtual void OnRegAutoRelogin(NIMLoginResult result) { }

        protected virtual void OnRegMultiSpotLoginNotify(NIMMultiSpotLoginNotifyResult result) { }

        protected virtual void OnRegKickOtherClient(NIMKickOtherResult result) { }

        protected virtual void OnRegComplete(NIMDataSyncType syncType, NIMDataSyncStatus status, string jsonAttachment)
        {
            _logger.Debug($"... result:{new { syncType, status, jsonAttachment }.ToJson(true)}");
        }

        class YunXinUserOnlineConfig
        {
            public List<NIMClientType> online { get; set; }
        }

        protected virtual void OnRegPushEvent(ResponseCode code, NIMEventInfo info)
        {
            if (code == ResponseCode.kNIMResSuccess)
            {
                if (info.Type == 1)
                {
                    var config = info.NimConfig.ParseTo<YunXinUserOnlineConfig>();
                    var userId = info.PublisherID;
                    var isOnline = config == null ? false : config.online == null ? false : config.online.Count > 0;

                    _event.PublishYunXinUserOnlineEvent(new YunXinUserOnlineEventArgs { UserId = userId, IsOnline = isOnline });
                }
            }

            _logger.Debug($"... result:{new { code, info }.ToJson(true)}");
        }

        protected virtual void OnRegBatchPushEvent(ResponseCode code, List<NIMEventInfo> infoList)
        {
            _logger.Debug($"... result:{new { code, infoList }.ToJson(true)}");
        }

        protected virtual void OnFriendProfileChanged(object sender, NIMFriendProfileChangedArgs e) { }

        protected virtual void OnUserRelationshipListSync(object sender, UserRelationshipSyncArgs e) { }

        protected virtual void OnUserRelationshipChanged(object sender, UserRelationshipChangedArgs e) { }

        protected virtual void OnUserNameCardChanged(object sender, UserNameCardChangedArgs e) { }

        protected virtual void OnReceiveMessage(object sender, NIMReceiveMessageEventArgs e) { }

        protected virtual void OnReceiveSysMsg(object sender, NIMSysMsgEventArgs e) { }

        protected virtual void OnRegMulitiportPushEnableChanged(ResponseCode response, ConfigMultiportPushParam param) { }

        protected virtual void OnRegRecallMessage(ResponseCode result, RecallNotification[] notify) { }

        protected virtual void OnRegReceiveBroadcast(NIMBroadcastMessage msg) { }

        protected virtual void OnRegReceiveBroadcastMsgs(List<NIMBroadcastMessage> msg) { }

        protected virtual void OnIsMultiportPushEnabled(ResponseCode response, ConfigMultiportPushParam param) { }

        protected virtual void OnSetStartNotify(string sessionId, int channelType, string uid, string customInfo)
        {
            // NIMSDK_Demo.Rts.RtsHandler.OnReceiveSessionRequest
        }

        protected virtual void OnRegReceiveBatchMessages(List<NIMReceivedMessage> msgsList) { }

        protected virtual void OnSessionStartRes(long channel_id, int code) { }

        protected virtual void OnSessionInviteNotify(long channel_id, string uid, int mode, long time, string customInfo) { }

        protected virtual void OnSessionCalleeAckRes(long channel_id, int code) { }

        protected virtual void OnSessionCalleeAckNotify(long channel_id, string uid, int mode, bool accept) { }

        protected virtual void OnSessionControlRes(long channel_id, int code, int type) { }

        protected virtual void OnSessionControlNotify(long channel_id, string uid, int type) { }

        protected virtual void OnSessionConnectNotify(long channel_id, int code, string record_file, string video_record_file, long chat_time, ulong chat_rx, ulong chat_tx) { }

        protected virtual void OnSessionMp4InfoStateNotify(long channel_id, int code, NIMVChatMP4State mp4_info) { }

        protected virtual void OnSessionPeopleStatus(long channel_id, string uid, int status) { }

        protected virtual void OnSessionNetStatus(long channel_id, int status, string uid) { }

        protected virtual void OnSessionHangupRes(long channel_id, int code) { }

        protected virtual void OnSessionHangupNotify(long channel_id, int code) { }

        protected virtual void OnSessionSyncAckNotify(long channel_id, int code, string uid, int mode, bool accept, long time, int client) { }

        protected virtual void OnSetAudioReceiveData(ulong time, IntPtr data, uint size, int rate) { }

        protected virtual void OnSetVideoReceiveData(ulong time, IntPtr data, uint size, uint width, uint height, string json_extension) { }

        protected virtual void OnSetVideoCaptureData(ulong time, IntPtr data, uint size, uint width, uint height, string json_extension) { }

        protected virtual void OnAddDeviceStatus(NIMDeviceType type, uint status, string devicePath) { }
        #endregion

        public virtual void Login(string user, string secret, Action<NIMLoginResult> loginCallback)
        {
            if (loginCallback == null)
            {
                throw new ArgumentException($"{nameof(loginCallback)} is not null.");
            }

            var token = ToolsAPI.GetMd5(secret);
            var sdkConfig = GetSDKConfig();
            ClientAPI.Login(sdkConfig.AppKey, user, token, result => loginCallback(result));
        }

        public virtual void Logout(Action<NIMLogoutResult> logoutCallback)
        {
            if (logoutCallback == null)
            {
                throw new ArgumentException($"{nameof(logoutCallback)} is not null.");
            }

            ClientAPI.Logout(NIMLogoutType.kNIMLogoutChangeAccout, result => logoutCallback(result));
        }

        public virtual void ReleaseEventCallbacks()
        {
            FriendAPI.FriendProfileChangedHandler -= OnFriendProfileChanged;
            UserAPI.UserRelationshipListSyncHander -= OnUserRelationshipListSync;
            UserAPI.UserRelationshipChangedHandler -= OnUserRelationshipChanged;
            UserAPI.UserNameCardChangedHandler -= OnUserNameCardChanged;
            TalkAPI.OnReceiveMessageHandler -= OnReceiveMessage;
            SysMsgAPI.ReceiveSysMsgHandler -= OnReceiveSysMsg;
        }

        public virtual void RegisterEventCallbacks()
        {
            FriendAPI.FriendProfileChangedHandler += OnFriendProfileChanged;
            UserAPI.UserRelationshipListSyncHander += OnUserRelationshipListSync;
            UserAPI.UserRelationshipChangedHandler += OnUserRelationshipChanged;
            UserAPI.UserNameCardChangedHandler += OnUserNameCardChanged;
            TalkAPI.OnReceiveMessageHandler += OnReceiveMessage;
            SysMsgAPI.ReceiveSysMsgHandler += OnReceiveSysMsg;
        }

        public virtual void RegisterCallbacks()
        {
            ClientAPI.RegDisconnectedCb(OnRegDisconnected);
            ClientAPI.RegKickoutCb(OnRegKickout);
            ClientAPI.RegAutoReloginCb(OnRegAutoRelogin);

            ClientAPI.RegMultiSpotLoginNotifyCb(OnRegMultiSpotLoginNotify);
            ClientAPI.RegKickOtherClientCb(OnRegKickOtherClient);
            DataSyncAPI.RegCompleteCb(OnRegComplete);

            NIMSubscribeApi.RegPushEventCb(OnRegPushEvent);
            NIMSubscribeApi.RegBatchPushEventCb(OnRegBatchPushEvent);

            //RegisterEventCallbacks();

            ClientAPI.RegMulitiportPushEnableChangedCb(OnRegMulitiportPushEnableChanged);
            TalkAPI.RegRecallMessageCallback(OnRegRecallMessage);
            TalkAPI.RegReceiveBroadcastCb(OnRegReceiveBroadcast);
            TalkAPI.RegReceiveBroadcastMsgsCb(OnRegReceiveBroadcastMsgs);

            ClientAPI.IsMultiportPushEnabled(OnIsMultiportPushEnabled);

            RtsAPI.SetStartNotifyCallback(OnSetStartNotify);

            TalkAPI.RegReceiveBatchMessagesCb(OnRegReceiveBatchMessages);

            _vChatSessionStatus.onSessionStartRes = OnSessionStartRes;
            _vChatSessionStatus.onSessionInviteNotify = OnSessionInviteNotify;
            _vChatSessionStatus.onSessionCalleeAckRes = OnSessionCalleeAckRes;
            _vChatSessionStatus.onSessionCalleeAckNotify = OnSessionCalleeAckNotify;
            _vChatSessionStatus.onSessionControlRes = OnSessionControlRes;
            _vChatSessionStatus.onSessionControlNotify = OnSessionControlNotify;
            _vChatSessionStatus.onSessionConnectNotify = OnSessionConnectNotify;
            _vChatSessionStatus.onSessionMp4InfoStateNotify = OnSessionMp4InfoStateNotify;
            _vChatSessionStatus.onSessionPeopleStatus = OnSessionPeopleStatus;
            _vChatSessionStatus.onSessionNetStatus = OnSessionNetStatus;
            _vChatSessionStatus.onSessionHangupRes = OnSessionHangupRes;
            _vChatSessionStatus.onSessionHangupNotify = OnSessionHangupNotify;
            _vChatSessionStatus.onSessionSyncAckNotify = OnSessionSyncAckNotify;

            VChatAPI.SetSessionStatusCb(_vChatSessionStatus);
            //DeviceAPI.SetAudioReceiveDataCb(OnSetAudioReceiveData, null);
            DeviceAPI.SetAudioReceiveDataCb((time, data, size, rate) => { }, null);
            DeviceAPI.SetVideoReceiveDataCb(OnSetVideoReceiveData, null);
            DeviceAPI.SetVideoCaptureDataCb(OnSetVideoCaptureData, null);

            DeviceAPI.AddDeviceStatusCb(NIMDeviceType.kNIMDeviceTypeVideo, OnAddDeviceStatus);
        }

        protected NIMFriendProfile[] _nimFriends;

        public virtual void GetFriends(Action<NIMFriends> getFriendsCallback)
        {
            if (getFriendsCallback == null)
            {
                throw new ArgumentException($"{nameof(getFriendsCallback)} is not null.");
            }

            FriendAPI.GetFriendsList(result =>
            {
                if (result != null && result.ProfileList != null && result.ProfileList.Any())
                {
                    _nimFriends = result.ProfileList.ToArray();
                }

                getFriendsCallback(result);
            });
        }

        protected UserNameCard[] _nimUserCards;

        public virtual void GetUserProfiles(List<string> users, Action<UserNameCard[]> getUserProfilesCallback)
        {
            if (getUserProfilesCallback == null)
            {
                throw new ArgumentException($"{nameof(getUserProfilesCallback)} is not null.");
            }

            UserAPI.GetUserNameCard(
                users,
                result =>
                {
                    if (result != null)
                    {
                        _nimUserCards = result;
                    }

                    getUserProfilesCallback(result);
                });
        }
    }
}
