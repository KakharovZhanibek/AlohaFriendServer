using AlohaFriend.Server.Handlers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlohaFriend.Server.Services
{
    public class RoutingService
    {
        private readonly Dictionary<string, IHandler> _routing;
        public RoutingService(
            SignUpHandler signUpHandler, 
            SignInHandler signInHandler,
            ProfileImageHandler profileImageHandler,
            StatusHandler statusHandler,
            BanUserHandler banUserHandler,
            MuteNotifictionsHandler muteNotificationsHandler)
        {
            _routing = new Dictionary<string, IHandler>()
            {
                { "/app/sign-up",  signUpHandler },
                { "/app/sign-in", signInHandler },
                { "/app/profileImage", profileImageHandler },
                { "/app/status", statusHandler},
                { "/app/ban-user", banUserHandler },
                { "/app/mute-notifiction", muteNotificationsHandler}
            };
        }

        public IHandler GetHandlerByRoute(string rawRoute)
        {
            if (_routing.ContainsKey(rawRoute))
                return _routing[rawRoute];
            else return new NotFoundHandler();
        }
    }
}
