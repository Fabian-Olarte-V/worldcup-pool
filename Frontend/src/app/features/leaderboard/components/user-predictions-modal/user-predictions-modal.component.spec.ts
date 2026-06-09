import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserPredictionsModalComponent } from './user-predictions-modal.component';

describe('UserPredictionsModalComponent', () => {
  let component: UserPredictionsModalComponent;
  let fixture: ComponentFixture<UserPredictionsModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserPredictionsModalComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UserPredictionsModalComponent);
    component = fixture.componentInstance;
    component.fullName = 'Ada Lovelace';
    fixture.detectChanges();
  });

  it('should emit closed when clicking the close button', () => {
    const emitSpy = vi.spyOn(component.closed, 'emit');
    const button = fixture.nativeElement.querySelector('button');

    button.click();

    expect(emitSpy).toHaveBeenCalled();
  });
});
