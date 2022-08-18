﻿using System.Collections.Generic;
using moonbaboon.bingo.Core.IServices;
using moonbaboon.bingo.Core.Models;
using moonbaboon.bingo.Domain.IRepositories;

namespace moonbaboon.bingo.Domain.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;

        public FriendshipService(IFriendshipRepository friendshipRepository, IUserRepository userRepository)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
        }
        
        public List<Friendship> GetAll()
        {
            return _friendshipRepository.FindAll().Result;  
        }

        public List<Friend> GetFriendsByUserId(string userId)
        {
            return _friendshipRepository.FindAcceptedFriendshipsByUserId(userId).Result;
        }

        public Friendship? SendFriendRequest(string fromUserId, string toUserId)
        {
            //check is users er identical
            if (fromUserId == toUserId)
            {
                //Todo feedback "you cant send yourself a friendRequest"
                return null;
            }
            //check if user exists
            if (_userRepository.ReadById(toUserId).Result is null)
            {
                //Todo feedback "the user you are requesting does not exist"
                return null;
            }
            //checks if already friends
            if (_friendshipRepository.ValidateFriendship(fromUserId, toUserId).Result)
            {
                //Todo feedback "is already Friends"
                return null;
            }
            return _friendshipRepository.Create(fromUserId, toUserId, false).Result;
        }

        public List<Friendship> GetFriendRequestsByUserId(string userId)
        {
            return _friendshipRepository.FindFriendRequests_ByUserId(userId).Result;
        }

        public Friendship? AcceptFriendRequest(string friendshipId, string acceptingUserId)
        {
            var friendship = _friendshipRepository.FindById(friendshipId).Result;
            if (friendship is {Accepted: true})
            {
                //Todo feedback "This friendship is already accepted"
                return null;
            }
            if (friendship?.FriendId2.Id != acceptingUserId)
            {
                //Todo feedback "This is not a request for you to accept"
                return null;
            }
            friendship.Accepted = true;
            return _friendshipRepository.AcceptFriendship(friendshipId).Result;
        }
    };
}