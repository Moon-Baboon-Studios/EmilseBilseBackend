﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using moonbaboon.bingo.Core.IServices;
using moonbaboon.bingo.Core.Models;

namespace moonbaboon.bingo.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardTileController: ControllerBase
    {
        private readonly IBoardTileService _boardTileService;

        public BoardTileController(IBoardTileService boardTileService)
        {
            _boardTileService = boardTileService;
        }

        [HttpGet("{id}")]
        public ActionResult<BoardTile?> GetById(string id)
        {
            return _boardTileService.GetById(id);
        }
        
        [HttpGet(nameof(GetByBoardId) + "{id}")]
        public ActionResult<List<BoardTile?>> GetByBoardId(string id)
        {
            return _boardTileService.GetByBoardId(id);
        }

        [HttpPost(nameof(Create))]
        public ActionResult<BoardTile?> Create(BoardTile boardTile)
        {
            return _boardTileService.Create(boardTile);
        }
    }
}