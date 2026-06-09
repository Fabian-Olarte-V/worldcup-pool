import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';

export interface EditableMatchState {
  matchId: string;
  homeGoals: number;
  awayGoals: number;
  isEditing: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class MatchEditService {
  private editingMatches = new Map<string, BehaviorSubject<EditableMatchState>>();

  getMatchEditState$(matchId: string): Observable<EditableMatchState> {
    if (!this.editingMatches.has(matchId)) {
      this.editingMatches.set(
        matchId,
        new BehaviorSubject<EditableMatchState>({
          matchId,
          homeGoals: 0,
          awayGoals: 0,
          isEditing: false,
        })
      );
    }
    return this.editingMatches.get(matchId)!.asObservable();
  }

  getEditingState$(matchId: string): Observable<boolean> {
    return this.getMatchEditState$(matchId).pipe(
      map(state => state.isEditing)
    );
  }

  startEditing(matchId: string): void {
    const subject = this.getSubject(matchId);
    subject.next({
      ...subject.value,
      isEditing: true,
    });
  }

  cancelEditing(matchId: string): void {
    const subject = this.getSubject(matchId);
    subject.next({
      ...subject.value,
      isEditing: false,
      homeGoals: 0,
      awayGoals: 0,
    });
  }

  setHomeGoals(matchId: string, goals: number): void {
    const subject = this.getSubject(matchId);
    subject.next({
      ...subject.value,
      homeGoals: goals,
    });
  }

  setAwayGoals(matchId: string, goals: number): void {
    const subject = this.getSubject(matchId);
    subject.next({
      ...subject.value,
      awayGoals: goals,
    });
  }

  getPayload(matchId: string) {
    const state = this.getSubject(matchId).value;
    return {
      MatchId: matchId,
      HomeGoals: state.homeGoals,
      AwayGoals: state.awayGoals,
    };
  }

  private getSubject(matchId: string): BehaviorSubject<EditableMatchState> {
    if (!this.editingMatches.has(matchId)) {
      this.editingMatches.set(
        matchId,
        new BehaviorSubject<EditableMatchState>({
          matchId,
          homeGoals: 0,
          awayGoals: 0,
          isEditing: false,
        })
      );
    }
    return this.editingMatches.get(matchId)!;
  }
}
