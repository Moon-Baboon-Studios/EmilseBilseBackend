﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moonbaboon.bingo.Core.IServices;
using moonbaboon.bingo.Core.Models;
using moonbaboon.bingo.WebApi.DTOs;

namespace moonbaboon.bingo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardTileController: ControllerBase
    {
        private readonly IBoardTileService _boardTileService;
        private readonly IUserTileService _userTileService;
        private readonly IBoardService _boardService;

        public BoardTileController(IBoardTileService boardTileService, IUserTileService userTileService, IBoardService boardService)
        {
            _boardTileService = boardTileService;
            _userTileService = userTileService;
            _boardService = boardService;
        }

        [HttpGet("{id}")]
        public ActionResult<BoardTile?> GetById(string id)
        {
            return _boardTileService.GetById(id);
        }
        
        [HttpGet(nameof(GetByBoardId) + "/{id}")]
        public ActionResult<List<BoardTileDto>> GetByBoardId(string id)
        {
            var boardTiles = _boardTileService.GetByBoardId(id);
            List<BoardTileDto> list = new();

            foreach (var boardTile in boardTiles)
            {
                var tile = _userTileService.GetById(boardTile.Tile.Id);
                list.Add(new BoardTileDto(boardTile.Id, boardTile.Board.Id, tile, boardTile.Position, boardTile.IsActivated));
            }

            return list;
        }
        
        [Authorize]
        [HttpGet(nameof(GetByGameId) + "/{gameId}")]
        public ActionResult<List<BoardTile>> GetByGameId(string gameId)
        {
            var board = _boardService.GetByUserAndGameId(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                gameId);
            var boardTiles = _boardTileService.GetByBoardId(board.Id);

            return boardTiles;
        }

        [HttpPost(nameof(Create))]
        public ActionResult<BoardTile?> Create(BoardTile boardTile)
        {
            return _boardTileService.Create(boardTile);
        }
    }
}