using System.ComponentModel.DataAnnotations;

namespace WorldCupPool.Application.Features.Matches.DTOs
{
    public sealed class UpdateMatchResultRequestDto
    {
        public Guid MatchId { get; set; }
        
        [Range(0, 20, ErrorMessage = "The number of goals exceeded the allowed limit.")]
        public int HomeGoals { get; set; }
        
        [Range(0, 20, ErrorMessage = "The number of goals exceeded the allowed limit.")]
        public int AwayGoals { get; set; }
    }
}

