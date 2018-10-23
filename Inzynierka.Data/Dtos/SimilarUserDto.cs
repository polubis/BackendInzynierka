using System;
using System.Collections.Generic;
using System.Text;

namespace Inzynierka.Data.Dtos
{
    public class SimilarUserDto:BaseDto
    {
        public SimilarUserDto(string Username, bool? Sex, string PathToAvatar, int NumberOfPlayedGames, int PlaceInRank, double PointsForAllGames)
        {
            this.Username = Username;
            this.Sex = Sex;
            this.PathToAvatar = PathToAvatar;
            this.NumberOfPlayedGames = NumberOfPlayedGames;
            this.PlaceInRank = PlaceInRank;
            this.PointsForAllGames = PointsForAllGames;
        }
        public string Username { get; set; }
        public bool? Sex { get; set; }
        public string PathToAvatar { get; set; }
        public int NumberOfPlayedGames { get; set; }
        public int PlaceInRank { get; set; }
        public double PointsForAllGames { get; set; }
    }
}
