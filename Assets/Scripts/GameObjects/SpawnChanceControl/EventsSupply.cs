using System;
using System.Linq;
namespace GameObjectsControllingNS
{
    public enum EventsSupplyLevel
    {
        VeryLow, Low, MediumLow, Medium, MediumHigh, High, VeryHigh
    }
    public static class EventsSupply
    {
        static float EventsSupplyLevelPercentage(EventsSupplyLevel level)
        {
            switch (level)
            {
                case EventsSupplyLevel.VeryLow:
                    return 1.6f;
                case EventsSupplyLevel.Low:
                    return 1.4f;
                case EventsSupplyLevel.MediumLow:
                    return 1.2f;
                case EventsSupplyLevel.Medium:
                    return 1f;
                case EventsSupplyLevel.MediumHigh:
                    return 0.8f;
                case EventsSupplyLevel.High:
                    return 0.6f;
                case EventsSupplyLevel.VeryHigh:
                    return 0.4f;
                default:
                    return 0;
            }
        }
        private static EventsSupplyLevel _eventsSupplyLevel;
        public static EventsSupplyLevel EventSupplyLevel { get { return _eventsSupplyLevel; } set { _eventsSupplyLevel = value; } }
        public static void IncreaseEventsSupplyLevel() =>
            _eventsSupplyLevel = _eventsSupplyLevel == Enum.GetValues(typeof(EventsSupplyLevel)).Cast<EventsSupplyLevel>().Last() ? _eventsSupplyLevel : _eventsSupplyLevel + 1;
        public static void DecreaseEventsSupplyLevel() =>
             _eventsSupplyLevel = _eventsSupplyLevel == Enum.GetValues(typeof(EventsSupplyLevel)).Cast<EventsSupplyLevel>().First() ? _eventsSupplyLevel : _eventsSupplyLevel - 1;
        public static bool CalculateEventChances(float eventChance)
        {
            if (eventChance == 100f)
                return true;
            if (eventChance == 0f)
                return false;
            eventChance *= EventsSupplyLevelPercentage(EventSupplyLevel);
            if (new System.Random().Next() % 100 >= eventChance)
            {
                IncreaseEventsSupplyLevel();
                return false;
            }
            else
            {
                DecreaseEventsSupplyLevel();
                return true;
            }
        }
    }
}