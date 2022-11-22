﻿using moonbaboon.bingo.Core.Models;

namespace moonbaboon.bingo.WebApi.DTOs
{
    public class UserDtos
    {
        public class LoginDto
        {
            public LoginDto(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class NewUserDto
        {
            public NewUserDto(string username, string nickname, string? profilePicUrl, string password, string salt)
            {
                Username = username;
                Nickname = nickname;
                ProfilePicUrl = profilePicUrl;
                Password = password;
                Salt = salt;
            }

            public string Username { get; set; }
            public string Nickname { get; set; }
            public string? ProfilePicUrl { get; set; }
            public string Password { get; set; }
            public string Salt { get; set; }

            public User ToUser()
            {
                return new User(null, Username, Nickname, ProfilePicUrl);
            }
        }
    }
}