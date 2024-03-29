﻿using AlohaFriend.Server.Contexts;
using AlohaFriend.Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AlohaFriend.Server.Handlers
{
    public class SignUpHandler : IHandler
    {
        private readonly ApplicationDbContext _dbContext;
        public SignUpHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle(HttpListenerRequest request, HttpListenerResponse response)
        {
            using(var ms = new MemoryStream())
            {
                request.InputStream.CopyTo(ms);
                var content = Encoding.UTF8.GetString(ms.ToArray());
                var registrationModel = JObject.Parse(content);

                var phoneNumber = registrationModel["phoneNumber"].ToString();
                var password = registrationModel["password"].ToString();

                var user = _dbContext.ApplicationUsers.Add(new ApplicationUser()
                {
                    PhoneNumber = phoneNumber,
                    PasswordHash = password           
                });

                var mainChat = _dbContext.ChatRooms.Find(1);
                var junction = new ApplicationUserChatRoomJunction()
                {
                    ApplicationUserId = user.Entity.Id,
                    ChatRoomId = mainChat.Id,
                    ApplicationUserStatus = ApplicationUserInChatStatus.Active
                };

                _dbContext.ApplicationUserChatRooms.Add(junction);
                _dbContext.SaveChanges();

                response.StatusCode = 200;
                response.OutputStream.Write(Encoding.UTF8.GetBytes("OK!"));
                response.Close();
            }
        }
    }
}
