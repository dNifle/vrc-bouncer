using VRChat.API.Api;

namespace VrChatBouncerBot.AutoMode
{
    internal class InstanceCapacityChecker : IInstanceCapacityChecker
    {
        private readonly IUsersApi USER_API;
        private readonly IInstancesApi INSTANCE_API;
        private readonly IAuthenticationApi AUTH_API;

        public InstanceCapacityChecker(IUsersApi userApi, IInstancesApi instanceApi, IAuthenticationApi authApi)
        {
            USER_API = userApi ?? throw new ArgumentNullException(nameof(userApi));
            INSTANCE_API = instanceApi ?? throw new ArgumentNullException(nameof(instanceApi));
            AUTH_API = authApi ?? throw new ArgumentNullException(nameof(authApi));
        }

        public async Task<int> GetOpenSlotsForCurrentInstanceAsync()
        {
            var user = await AUTH_API.GetCurrentUserAsync();
            var userInfo = await USER_API.GetUserAsync(user.Id);
            var instance = await INSTANCE_API.GetInstanceAsync(userInfo.WorldId, userInfo.InstanceId);
            return instance.Capacity - instance.NUsers;
        }
    }
}
