﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using moonbaboon.bingo.Core.Models;
using moonbaboon.bingo.Domain.IRepositories;
using MySqlConnector;

namespace moonbaboon.bingo.DataAccess.Repositories
{
    public class PackTileRepository : IPackTileRepository
    {
        private readonly MySqlConnection _connection;

        public PackTileRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<PackTile>> GetByPackId(string packId)
        {
            List<PackTile> list = new();
            await using var con = _connection;
            {
                con.Open();

                await using MySqlCommand command =
                    new(
                        @"SELECT PackTile.Id As PackTileId, T.Id AS TileId, T.Action AS TileAction, 
                        TP.Id AS TilePackId, TP.Name AS TilePackName, TP.PicUrl AS TilePackPic, TP.Stripe_PRICE As TilePackPrice 
                        FROM PackTile 
                            JOIN Tile T on PackTile.TileId = T.Id 
                            JOIN TilePack TP on TP.Id = PackTile.PackId 
                        WHERE PackTile.PackId = @packId",
                        con);
                {
                    command.Parameters.Add("@packId", MySqlDbType.VarChar).Value = packId;
                }

                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) list.Add(new PackTile(reader));

                await con.CloseAsync();
            }
            return list;
        }

        public async Task<PackTile> GetById(string id)
        {
            await using var con = _connection;
            {
                con.Open();

                await using MySqlCommand command =
                    new(
                        @"SELECT PackTile.Id As PackTileId, T.Id AS TileId, T.Action AS TileAction, 
                        TP.Id AS TilePackId, TP.Name AS TilePackName, TP.PicUrl AS TilePackPic, TP.Stripe_PRICE As TilePackPrice 
                        FROM PackTile 
                            JOIN Tile T on PackTile.TileId = T.Id 
                            JOIN TilePack TP on TP.Id = PackTile.PackId 
                        WHERE PackTile.Id = @Id",
                        con);
                {
                    command.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
                }

                await using var reader = await command.ExecuteReaderAsync();
                while (reader.Read()) return new PackTile(reader);
                await con.CloseAsync();
            }

            throw new Exception($"No {nameof(PackTile)} with id: {id}");
        }

        public async Task<List<Tile>> GetTilesUsedInPacks()
        {
            var list = new List<Tile>();
            await _connection.OpenAsync();

            await using MySqlCommand command = new(
                "SELECT * FROM Tile RIGHT JOIN PackTile on PackTile.TileId = Tile.Id"
                , _connection);
            await using MySqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                list.Add(new Tile(reader.GetString(0), reader.GetString(1), null, TileType.PackTile));

            await _connection.CloseAsync();
            return list;
        }

        public async Task<PackTileEntity> Create(PackTileEntity pt)
        {
            pt.Id = Guid.NewGuid().ToString();
            await using var con = _connection;
            {
                con.Open();
                await using MySqlCommand command =
                    new("INSERT INTO PackTile(Id, TileId, PackId) VALUES (@Id,@tileId,@packId);", con);
                {
                    command.Parameters.Add("@Id", MySqlDbType.VarChar).Value = pt.Id;
                    command.Parameters.Add("@tileId", MySqlDbType.VarChar).Value = pt.TileId;
                    command.Parameters.Add("@packId", MySqlDbType.VarChar).Value = pt.PackId;
                }
                command.ExecuteNonQuery();
            }
            return pt;
        }

        public async Task<bool> Clear(string id)
        {
            await using var con = _connection;
            {
                con.Open();
                await using MySqlCommand command =
                    new("DELETE FROM PackTile WHERE PackId = @packId;", con);
                {
                    command.Parameters.Add("@packId", MySqlDbType.VarChar).Value = id;
                }
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}