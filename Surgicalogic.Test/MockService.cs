using Smartiks.Teydeb.Surgicalogic.ConsoleApp.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surgicalogic.Test
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
