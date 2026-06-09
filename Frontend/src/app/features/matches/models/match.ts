import { ResultRowMatchViewModel } from "../../../shared/components/result-row/result-row.component";

export interface MatchDto {
  id: string;
  groupName: string;
  homeTeam: string;
  homeTeamCode: string;
  awayTeam: string;
  awayTeamCode: string;
  status: string;
  startTimeUtc: string;
  homeGoals?: number | null;
  awayGoals?: number | null;
  hasFinalResult?: boolean;
}

export interface TeamViewModel {
  id: string;
  name: string;
  code: string;
}

export interface MatchViewModel {
  id: string;
  dateLabel: string;
  timeLabel: string;
  homeTeam: TeamViewModel;
  awayTeam: TeamViewModel;
}

export interface MatchdayViewModel {
  id: string;
  title: string;
  matches: MatchViewModel[];
}

export interface MatchesPageViewModel {
  matches: ResultRowMatchViewModel[];
  loading: boolean;
  errorMessage: string | null;
}
