import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LeaderboardRowComponent } from './leaderboard-row.component';
import { LeaderboardUserViewModel } from '../../models/leaderboard.models';

describe('LeaderboardRowComponent', () => {
  let component: LeaderboardRowComponent;
  let fixture: ComponentFixture<LeaderboardRowComponent>;

  const user: LeaderboardUserViewModel = {
    userId: 'user-1',
    userName: 1,
    fullName: 'Ada Lovelace',
    points: 12,
    wonCount: 4,
    correctOutcomeCount: 3,
    lostCount: 1,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeaderboardRowComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(LeaderboardRowComponent);
    component = fixture.componentInstance;
    component.user = user;
    fixture.detectChanges();
  });

  it('should emit the user when the row is clicked', () => {
    const emitSpy = vi.spyOn(component.open, 'emit');
    const row = fixture.nativeElement.querySelector('.leaderboard-row');

    row.click();

    expect(emitSpy).toHaveBeenCalledWith(user);
  });

  it('should emit the user when pressing Enter on the row', () => {
    const emitSpy = vi.spyOn(component.open, 'emit');
    const row = fixture.nativeElement.querySelector('.leaderboard-row');

    row.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter' }));

    expect(emitSpy).toHaveBeenCalledWith(user);
  });

  it('should emit the user and prevent default when pressing Space on the row', () => {
    const emitSpy = vi.spyOn(component.open, 'emit');
    const row = fixture.nativeElement.querySelector('.leaderboard-row');
    const event = new KeyboardEvent('keydown', { key: ' ' });
    const preventDefaultSpy = vi.spyOn(event, 'preventDefault');

    row.dispatchEvent(event);

    expect(emitSpy).toHaveBeenCalledWith(user);
    expect(preventDefaultSpy).toHaveBeenCalled();
  });
});
