﻿using System;
using moonbaboon.bingo.Core.IServices;
using moonbaboon.bingo.Core.Models;
using moonbaboon.bingo.Domain.IRepositories;

namespace moonbaboon.bingo.Domain.Services
{
    public class BoardService: IBoardService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IGameRepository _gameRepository;
        public BoardService(IBoardRepository boardRepository, IGameRepository gameRepository)
        {
            _boardRepository = boardRepository;
            _gameRepository = gameRepository;
        }

        public Board? GetById(string id)
        {
            return _boardRepository.FindById(id).Result;
        }

        public Board? CreateBoard(string userId, string gameId)
        {
            var game = _gameRepository.FindById(gameId).Result;

            if (game is null || !string.IsNullOrEmpty(game.WinnerId))
            {
                return null;
            }
            
            return _boardRepository.Create(userId, gameId).Result;
        }
    }
}