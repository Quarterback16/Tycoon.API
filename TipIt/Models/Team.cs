using TipIt.Events;

namespace TipIt.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string League { get; set; }

        public Team(AddTeamEvent team)
        {
            Id = team.TeamId;
            Name = team.TeamName;
            League = team.LeagueCode;
        }

        public override string ToString()
        {
            return $"{Id} : {Name} ({League})";
        }
    }
}
