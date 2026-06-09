using WorldCupPool.Application.Features.Matches.DTOs;
using WorldCupPool.Application.Exceptions;
using WorldCupPool.Domain.Entities;

namespace WorldCupPool.Application.Features.Matches
{
    public sealed class MatchesService : IMatchesService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchResultRepository _matchResultRepository;

        public MatchesService(IMatchRepository matchRepository, IMatchResultRepository matchResultRepository)
        {
            _matchRepository = matchRepository;
            _matchResultRepository = matchResultRepository;
        }


        public async Task<IReadOnlyList<MatchListItemResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var matches = await _matchRepository.GetAllListItemsAsync(cancellationToken);
            return matches.Select(MapToResponse).ToList();
        }

        public async Task<MatchListItemResponseDto> UpdateResultAsync(UpdateMatchResultRequestDto request, CancellationToken cancellationToken = default)
        {
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken)
                ?? throw new NotFoundException("Match was not found.");

            await ApplyResultAsync(match, request, cancellationToken);

            await _matchRepository.SaveChangesAsync(cancellationToken);
            return MapToResponse(match);
        }

        public async Task<IReadOnlyList<MatchListItemResponseDto>> UpdateResultsBulkAsync(BulkUpdateMatchResultRequestDto request, CancellationToken cancellationToken = default)
        {
            var matchIds = request.Results.Select(x => x.MatchId).ToList();
            var matches = await _matchRepository.GetByIdsAsync(matchIds, cancellationToken);
            var matchesById = matches.ToDictionary(x => x.Id);

            foreach (var result in request.Results)
            {
                if (!matchesById.TryGetValue(result.MatchId, out var match))
                {
                    continue;
                }

                await ApplyResultAsync(match, result, cancellationToken);
            }

            await _matchRepository.SaveChangesAsync(cancellationToken);
            return matches
                .OrderBy(x => x.StartTimeUtc)
                .Select(MapToResponse)
                .ToList();
        }


        private async Task ApplyResultAsync(Match match, UpdateMatchResultRequestDto request, CancellationToken cancellationToken)
        {
            if (match.Status == Domain.Enums.MatchStatus.Finished || match.HasFinalResult)
            {
                throw new BusinessRuleViolationException("Results cannot be updated for finished matches.");
            }

            if (match.Result is null)
            {
                var result = new MatchResult(match.Id, request.HomeGoals, request.AwayGoals);
                match.SetResult(result);
                await _matchResultRepository.AddAsync(result, cancellationToken);
            }
            else
            {
                match.Result.Update(request.HomeGoals, request.AwayGoals);
            }

            foreach (var prediction in match.Predictions)
            {
                prediction.CalculatePoints(request.HomeGoals, request.AwayGoals);
            }
        }

        private static MatchListItemResponseDto MapToResponse(Match match)
        {
            return new MatchListItemResponseDto
            {
                Id = match.Id,
                GroupName = match.GroupName,
                HomeTeam = match.HomeTeam.Name,
                HomeTeamCode = match.HomeTeam.Code,
                AwayTeam = match.AwayTeam.Name,
                AwayTeamCode = match.AwayTeam.Code,
                Status = match.Status.ToString(),
                HasFinalResult = match.HasFinalResult,
                HomeGoals = match.Result?.HomeGoals,
                AwayGoals = match.Result?.AwayGoals,
                StartTimeUtc = match.StartTimeUtc
            };
        }

        private static MatchListItemResponseDto MapToResponse(MatchListItemData match)
        {
            return new MatchListItemResponseDto
            {
                Id = match.Id,
                GroupName = match.GroupName,
                HomeTeam = match.HomeTeamName,
                HomeTeamCode = match.HomeTeamCode,
                AwayTeam = match.AwayTeamName,
                AwayTeamCode = match.AwayTeamCode,
                Status = match.Status,
                HasFinalResult = match.HasFinalResult,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                StartTimeUtc = match.StartTimeUtc
            };
        }
    }
}
