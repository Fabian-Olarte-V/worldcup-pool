import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/apiResponse/apiResponse';
import { MatchDto } from '../../features/matches/models/match';
import { PredictionDto, PredictionSubmissionPayload } from '../models/prediction/prediction';

@Injectable({ providedIn: 'root' })
export class MatchesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/matches`;

  getMatches(): Observable<ApiResponse<MatchDto[]>> {
    return this.http.get<ApiResponse<MatchDto[]>>(this.baseUrl);
  }

  CreatePrediction(prediction: PredictionSubmissionPayload): Observable<ApiResponse<PredictionDto>> {
    return this.http.post<ApiResponse<PredictionDto>>(`${this.baseUrl}/predictions`, prediction);
  }
}
