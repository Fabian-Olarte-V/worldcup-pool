using System.ComponentModel.DataAnnotations;

namespace WorldCupPool.Application.Features.Predictions.DTOs
{
    public sealed class CreatePredictionRequestDto
    {
        public Guid MatchId { get; set; }
        
        [Range(0, 20, ErrorMessage = "The number of goals exceeded the allowed limit.")]
        public int HomeGoals { get; set; }
        
        [Range(0, 20, ErrorMessage = "The number of goals exceeded the allowed limit.")]
        public int AwayGoals { get; set; }
    }
}

