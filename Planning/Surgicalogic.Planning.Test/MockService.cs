using Surgicalogic.Planning.Model.InputModel;
using System.Collections.Generic;

namespace Surgicalogic.Planning.Test
{
    public static class MockService
    {
        public static List<RoomInputModel> Rooms(int count)
        {
            var rooms = new List<RoomInputModel>();

            for (int i = 1; i <= count; i++)
            {
                rooms.Add(new RoomInputModel
                {
                    Id = i,
                    Name = string.Format("Room {0}", i)
                });
            }

            return rooms;
        }
    }
}