import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse } from '../../../shared/models/apiResponse/apiResponse';
import {
  MatchResultsBulkRequest,
  PredictionSubmissionPayload,
} from '../../../shared/models/prediction/prediction';
import { MatchDto } from '../../../features/matches/models/match';

@Injectable({ providedIn: 'root' })
export class MatchResultService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/matches/results/bulk`;

  setMatchesResult(payload: PredictionSubmissionPayload[]): Observable<ApiResponse<MatchDto[]>> {
    return this.http.post<ApiResponse<MatchDto[]>>(this.baseUrl, {
      results: payload,
    } satisfies MatchResultsBulkRequest);
  }
}
