using System;

namespace LineWorldTotalWar {
    public enum PlayerNo { _1, _2 }

    public static class PlayerNoExtensions {
        public static int direction(this PlayerNo playerNo) =>
            playerNo switch {
                PlayerNo._1 => 1,
                PlayerNo._2 => -1,
                _ => 0
            };

        public static int spawnPosition(this PlayerNo playerNo, int size) =>
            playerNo switch {
                PlayerNo._1 => 0,
                PlayerNo._2 => size - 1,
                _ => throw new ArgumentOutOfRangeException($"No spawn position for player {playerNo}")
            };

        public static int targetPosition(this PlayerNo playerNo, int size) =>
            playerNo switch {
                PlayerNo._1 => size,
                PlayerNo._2 => -1,
                _ => throw new ArgumentOutOfRangeException($"No target position for player {playerNo}")
            };
    }
}