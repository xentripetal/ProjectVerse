using Verse.API.Events;
using Verse.API.Events.EventBus;

namespace Verse.API {
    public static class World {
        public static IEventBus EventBus = new DictEventBus();
        //public static RoomsContainer Rooms;
        //public static ModPackagesContainer ModPackages;
        //public static 
    }
}