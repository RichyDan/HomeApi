﻿using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            Room roomFromEntity = _context.Rooms.Where(r => r.Id == room.Id).FirstOrDefault();
            roomFromEntity.Name = room.Name;
            roomFromEntity.Area = room.Area;
            roomFromEntity.GasConnected = room.GasConnected;
            roomFromEntity.Voltage = room.Voltage;

            var entry = _context.Entry(roomFromEntity);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(roomFromEntity);
            await _context.SaveChangesAsync();
        }
    }
}