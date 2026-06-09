import { MatchDto, TeamViewModel } from '../../features/matches/models/match';
import { ResultRowMatchViewModel } from '../../shared/components/result-row/result-row.component';
import { TeamResultCardViewModel } from '../../shared/components/team-result-card/team-result-card.component';

export function formatDateLabel(value: string): string | null {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return null;
  return new Intl.DateTimeFormat('en-GB', { day: '2-digit', month: 'long' }).format(date);
}

export function formatTimeLabel(value: string): string | null {
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return null;
  return new Intl.DateTimeFormat('en-GB', {
    hour: '2-digit',
    minute: '2-digit',
    hour12: false,
  }).format(date);
}

export function teamFromApi(teamName: string, teamCode: string): TeamViewModel {
  const normalizedName = teamName.trim();
  const normalizedCode = teamCode.trim().toUpperCase();

  return {
    id: normalizedCode || normalizedName.toLowerCase().replace(/\s+/g, '-'),
    name: normalizedName,
    code: normalizedCode || normalizedName,
  };
}

export function mapTeam(vm: TeamViewModel): TeamResultCardViewModel {
  return {
    id: vm.id,
    name: vm.name,
    code: vm.code,
  };
}

export function mapMatches(matches: MatchDto[]): ResultRowMatchViewModel[] {
  return matches.map((match) => ({
    id: match.id,
    dateLabel: formatDateLabel(match.startTimeUtc) ?? 'TBD',
    timeLabel: formatTimeLabel(match.startTimeUtc) ?? 'TBD',
    homeTeam: mapTeam(teamFromApi(match.homeTeam, match.homeTeamCode)),
    awayTeam: mapTeam(teamFromApi(match.awayTeam, match.awayTeamCode)),
    result: {
      homeGoals:
        match.status === 'Finished' || match.hasFinalResult ? (match.homeGoals ?? null) : null,
      awayGoals:
        match.status === 'Finished' || match.hasFinalResult ? (match.awayGoals ?? null) : null,
    },
    status: match.status === 'Finished' || match.hasFinalResult ? 'Finalizado' : 'Pendiente',
  }));
}
