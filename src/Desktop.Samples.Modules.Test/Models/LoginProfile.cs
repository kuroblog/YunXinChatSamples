using Desktop.Samples.Common;
using Desktop.Samples.Modules.Test.Properties;
using System.Collections.Generic;
using System.Linq;

namespace Desktop.Samples.Modules.Test.Models
{
    public class LoginProfile
    {
        public List<UserProfile> Users { get; set; }

        public UserProfile Login { get; set; }
    }

    public static class LoginProfileExtensions
    {
        public static void FromSettings(this LoginProfile login)
        {
            var settingsLoginProfile = Settings.Default.LoginProfile.ParseTo<LoginProfile>();
            if (settingsLoginProfile == null)
            {
                settingsLoginProfile = new LoginProfile { };
            }

            if (settingsLoginProfile.Users == null)
            {
                settingsLoginProfile.Users = new List<UserProfile> { };
            }

            if (settingsLoginProfile.Login == null)
            {
                settingsLoginProfile.Login = new UserProfile
                {
#if DEBUG
                    Id = "wearebest",
                    Secret = "123456"
#endif
                };
            }

            login.Login = settingsLoginProfile.Login;
            login.Users = settingsLoginProfile.Users;
        }

        public static void SaveLogin(this LoginProfile login, string id, string secret)
        {
            login.Login = new UserProfile { Id = id, Secret = secret };

            var userProfiles = login.Users.Where(user => user.Id == login.Login.Id)?.ToList();
            if (userProfiles == null || !userProfiles.Any())
            {
                login.Users.Add(login.Login);
            }
            else
            {
                userProfiles.ForEach(user => user.Secret = login.Login.Secret);
            }

            Settings.Default.LoginProfile = login.ToJson();
            Settings.Default.Save();
        }

        public static void Clear(this LoginProfile login)
        {
            login = new LoginProfile
            {
                Login = new UserProfile { },
                Users = new List<UserProfile> { }
            };

            Settings.Default.LoginProfile = login.ToJson();
            Settings.Default.Save();
        }
    }
}
